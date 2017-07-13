using Sfacg.Model;
using Sfacg.Model.StoreModel;
using Sfacg.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
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
    public sealed partial class NovelReadV2 : Page
    {
        private ChapterList chapter;

        private LinkedList<ChapterList> chapters;

        StatusBar statusBar;

        private ObservableCollection<NovelContentVO> novelContents;

        private List<NovelContentVO> tempList = new List<NovelContentVO>();

        public NovelReadV2()
        {
            this.InitializeComponent();

            // 判断是否存在 StatusBar
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                statusBar = StatusBar.GetForCurrentView();
            }

            novelContents = new ObservableCollection<NovelContentVO>();
            novelList.ItemsSource = novelContents;

            chapters = new LinkedList<ChapterList>();
        }

        /**
         * 进入页面
         */
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is ChapterList)
            {
                chapter = (ChapterList)e.Parameter;
                process.IsActive = true;
                string novelStr;
                tempList.Clear();
                novelContents.Clear();
                try
                {
                    novelStr = await NovelUtil.getNovel(chapter.novelId, chapter.chapId);
                }
                catch (Exception)
                {
                    messShow.Show("网络异常", 3000);
                    process.IsActive = false;
                    return;
                }
                ShowNovel(novelStr);
                process.IsActive = false;

            }
            else
            {
                //content.Text = "wrong param";
            }

            showStatusBar(false);
        }

        /**
         * 从页面浏览出去
         */ 
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            showStatusBar(true);
        }

        /**
         * 展示小说
         */
        private void ShowNovel(string novelStr)
        {
            if (string.IsNullOrEmpty(novelStr))
            {
                return;
            }
            int length = novelStr.Length;
            var imageUrls = getImageUrls(novelStr);
            var paragraphs = getParagraphs(novelStr);
            

            int i = 0;
            int count = 0;
            string imageUrl = null;
            bool hasImage;
            bool stopAdd2View = false;
            hasImage = imageUrls.Count > 0;
            paragraphs.ForEach(p =>
            {

                paragraphSlices2(p).ForEach(n => {
                    if (count < 20&&!stopAdd2View)
                    {
                        novelContents.Add(n);
                        count++;
                    }
                    else
                    {
                        stopAdd2View = true;
                        tempList.Add(n);
                    }    
                });

                if (hasImage)
                {

                    try
                    {
                        imageUrl = imageUrls[i];
                    }
                    catch (Exception)
                    {
                        imageUrl = null;
                    }
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        if (!stopAdd2View)
                        {
                            novelContents.Add(new NovelContentVO() { imageUrl = imageUrl });
                        }else
                        {
                            tempList.Add(new NovelContentVO() { imageUrl = imageUrl });
                        }
                        i++;
                    }
                    else
                    {
                        hasImage = false;
                    }
                }

            });

        }

        //private List<NovelContentVO> paragraphSlices(string paragraph)
        //{
        //    var paragraphSlices = new List<NovelContentVO>();
        //    int length = paragraph.Length;
        //    int endlength = 0;
        //    int sliceLength = 1000;
        //    if (length > sliceLength)
        //    {
        //        for (int i = 0; i < length; i += sliceLength)
        //        {
        //            if (length - endlength < sliceLength)
        //            {
        //                endlength = length - i;
        //            }else
        //            {
        //                endlength = i+ sliceLength;
        //            }
        //            try
        //            {
        //                paragraphSlices.Add(new NovelContentVO() { paragraph = paragraph.Substring(i, endlength) });
        //            }
        //            catch (Exception)
        //            {

        //                break;
        //            }
        //        }
        //    }else
        //    {
        //        paragraphSlices.Add(new NovelContentVO() { paragraph = paragraph });
        //    }

        //    return paragraphSlices;
        //}

        private List<NovelContentVO> paragraphSlices2(string paragraph)
        {
            var paragraphSlices = new List<NovelContentVO>();
            string[] novelParagraph = Regex.Split(paragraph, @"\n");
            novelParagraph.ToList().ForEach(p => paragraphSlices.Add(new NovelContentVO() { paragraph = p }));

            return paragraphSlices;
        }

        private List<string> getParagraphs(string novelStr)
        {
            string[] novelParagraph = Regex.Split(novelStr, @"\[img=[0-9,]+\][^\[]+\[/img\]");
            return novelParagraph.ToList();
        }

        private static List<string> getImageUrls(string novelStr)
        {
            Regex pattenOfImage = new Regex(@"(\[img=[0-9,]+\])([^\[]+)(\[/img\])");
            List<string> imageUrl = new List<string>();
            

            foreach (Match mch in pattenOfImage.Matches(novelStr))
            {
                GroupCollection groups = mch.Groups;
                Group group = groups[2];
                imageUrl.Add(group.Value);
            }
            return imageUrl;
        }

        private void showStatusBar(bool show)
        {
            if (statusBar != null)
            {
                if (show)
                {
                    statusBar.ShowAsync();
                }else
                {
                    statusBar.HideAsync();
                }
            }
        }

        /**
         * 阅读页面被点击了
         */
        private void content_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (Down_ComBar.Visibility == Visibility.Collapsed)
            {
                Down_ComBar.Visibility = Visibility.Visible;
            }
            else
            {
                Down_ComBar.Visibility = Visibility.Collapsed;
            }
        }

        private void btn_BookMark_Clicked(object sender, RoutedEventArgs e)
        {
            Bookmark bookmark = new Bookmark() { novelId = chapter.novelId, chapId = chapter.chapId, chapName = chapter.title };
            BookmarkRepositoryUtil.save(bookmark);
            messShow.Show("书签添加成功", 1000);
        }

        private void sv_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (sv.ScrollableHeight - sv.VerticalOffset <200 )
            {
                loadRestContent();
            }
        }

        private void loadRestContent()
        {
            if (tempList.Count > 0)
            {
                if (tempList.Count >= 20)
                {
                    tempList.GetRange(0, 20).ForEach(n => novelContents.Add(n));
                    tempList.RemoveRange(0, 20);
                }
                else
                {
                    tempList.GetRange(0, tempList.Count - 1).ForEach(n => novelContents.Add(n));
                    tempList.Clear();
                }
                
                
            }
        }
    }
}
