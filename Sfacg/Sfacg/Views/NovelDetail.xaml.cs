using Sfacg.Model;
using Sfacg.Model.StoreModel;
using Sfacg.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace Sfacg.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NovelDetail : Page
    {

        private NovelsVOData novelInfo;
        private string novelId;

        public NovelDetail()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            process.IsActive = true;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string)
            {
                var newNovelId = (string)e.Parameter;
                if (novelId == newNovelId) return;
                novelId = newNovelId;
                NovelDetailVO novelDetail;
                try
                {
                    novelDetail = await NovelUtil.getNovelDetail(newNovelId);
                }
                catch (Exception)
                {
                    messShow.Show("网络异常", 3000);
                    process.IsActive = false;
                    return;
                }
                novelDetail.NovelCover = "http://rs.sfacg.com/web/novel/images/NovelCover/Big/" + novelDetail.NovelCover;

                NovelDetailModel data = new NovelDetailModel();
                data.novelDetail = novelDetail;
                this.DataContext = data;
                process.IsActive = false;
            }
            else
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
            var data = (NovelDetailModel)this.DataContext;
            Novel novel = new Novel() { novelId = data.novelDetail.NovelID,
                                        novelCover = data.novelDetail.NovelCover,
                                        novelName = data.novelDetail.NovelName};
            NovelRepositoryUtil.save(novel);

            messShow.Show("收藏成功", 1000);
        }
    }
}
