using Sfacg.Model;
using Sfacg.Model.ApiVO;
using Sfacg.Model.StoreModel;
using Sfacg.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace Sfacg.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NovelDetail : Page
    {

        //private PushNovelApiVOData novelInfo;
        private string novelId;
        private string novelCover = "ms-appx:///Assets/defaultCover.jpg";

        ApplicationData applicationData = null;
        ApplicationDataContainer roamingSettings = null;

        public NovelDetail()
        {
            this.InitializeComponent();
            //NavigationCacheMode = NavigationCacheMode.Enabled;
            process.IsActive = true;

            applicationData = ApplicationData.Current;
            roamingSettings = applicationData.RoamingSettings;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string)
            {
                novelId = (string)e.Parameter;
            }
            else if (e.Parameter is Novel)
            {
                var novel = (Novel)e.Parameter;
                novelId = novel.novelId;
                NovelName.Text = novel.novelName;
                novelCover = novel.novelCover;
            }

            StartAnimation();//开始切换动画

            Novel novelDetail;
            try
            {
                novelDetail = await NovelApiUtil.getNovelDetail(novelId);
            }
            catch (Exception)
            {
                messShow.Show("网络异常", 3000);
                process.IsActive = false;
                return;
            }
            if (string.IsNullOrEmpty(novelDetail.intro))
            {
                novelDetail.intro = "连个简介都没有，😔";
            }
            if (novelDetail.tags == null || novelDetail.tags.Count == 0)
            {
                novelDetail.tags = new List<string>();
                novelDetail.tags.Add("没有标签");
            }

            setKeepReadButtionStyle();


            this.DataContext = novelDetail;
            process.IsActive = false;


        }

        /// <summary>
        /// 启动切换动画
        /// </summary>
        private void StartAnimation()
        {
            try
            {
                ConnectedAnimation imageAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("novelCover");
                if (imageAnimation != null)
                {
                    imageAnimation.TryStart(NovelCover);
                }

                var testAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("novelName");
                if (testAnimation != null)
                {
                    testAnimation.TryStart(NovelName);
                }
            }
            catch (Exception)
            {
            }
        }

        private void Catalog_Button_Click(object sender, RoutedEventArgs e)
        {
            //var rootFrame = (Frame)Window.Current.Content;
            this.Frame.Navigate(typeof(NovelCatalogView), novelId);
        }

        private void txt_desc_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (txt_desc.MaxLines == 3)
            {
                txt_desc.MaxLines = 0;
            }
            else
            {
                txt_desc.MaxLines = 3;
            }
        }

        private void Collect_btn_Click(object sender, RoutedEventArgs e)
        {
            Novel novel = (Novel)this.DataContext;
            try
            {
                NovelRepositoryUtil.save(novel);
            }
            catch (Exception)
            {
                messShow.Show("收藏失败，书籍信息可能有误，请反馈开发", 1000);
            }

            messShow.Show("收藏成功", 1000);
        }

        private void setKeepReadButtionStyle()
        {
            var bookmark = BaseUtil.getSetting<Bookmark>(BaseUtil.READ_POINT_PREFIX + novelId, true);
            if (bookmark == null)
            {
                //KeepReadingButton.IsEnabled = false;
                //KeepReadingButton.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(0xFF, 0xE9, 0xE9, 0xE9));
                KeepReadingButton.Content = "开始阅读";
            }
        }

        private async void Read_btn_Click(object sender, RoutedEventArgs e)
        {
            
            var bookmark = BaseUtil.getSetting<Bookmark>(BaseUtil.READ_POINT_PREFIX + novelId,true);
            if (bookmark!=null)
            {
                var charpter = new Chapter() { novelId = bookmark.novelId, chapId = bookmark.chapId, title = bookmark.chapName, itemId = bookmark.itemId, listPosition = bookmark.listPosition, itemContainerHeight = bookmark.itemContainerHeight };
                this.Frame.Navigate(typeof(NovelReadV2), charpter);
            }else
            {
                //messShow.Show("没有阅读记录", 1000);
                await ReadFirstChapter();
            }
        }

        /// <summary>
        /// 直接阅读第一个章节
        /// </summary>
        /// <returns></returns>
        private async System.Threading.Tasks.Task ReadFirstChapter()
        {
            process.IsActive = true;
            var catalog = await NovelApiUtil.getNovelCatalog(novelId,false);
            if (catalog != null && catalog.Count > 0)
            {
                var volume = catalog[0];
                if (volume != null && volume.chapters != null && volume.chapters.Count > 0)
                {
                    var chapter = volume.chapters[0];
                    if (chapter != null)
                    {
                        process.IsActive = false;
                        this.Frame.Navigate(typeof(NovelReadV2), chapter);
                    }
                    else
                    {
                        process.IsActive = false;
                        messShow.Show("没有可以阅读的章节", 1000);
                    }

                }
            }
        }
    }
}
