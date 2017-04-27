using Sfacg.Model;
using Sfacg.Model.StoreModel;
using Sfacg.Utils;
using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace Sfacg.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NovelReadView : Page
    {
        MainPage rootPage;

        private ChapterList chapter;

        StatusBar statusBar;

        public NovelReadView()
        {
            this.InitializeComponent();
            rootPage = MainPage.Current;
            //Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TryEnterFullScreenMode();//全屏显示

            // 判断是否存在 StatusBar
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                statusBar = StatusBar.GetForCurrentView();
            }
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is ChapterList)
            {
                chapter = (ChapterList)e.Parameter;
                process.IsActive = true;
                var novelStr = await NovelUtil.getNovel(chapter.novelId, chapter.chapId);
                ShowNovel(novelStr);
                process.IsActive = false;
            }
            else
            {
                //content.Text = "wrong param";
            }

            if (statusBar != null)
            {
                statusBar.HideAsync();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (statusBar != null)
            {
                statusBar.ShowAsync();
            }
        }

        private void ShowNovel(string novelStr)
        {
            //content.Text = novelStr;

            List<Image> images = getImages(novelStr);

            addContent(novelStr,images);

            

        }

        private static List<Image> getImages(string novelStr)
        {
            Regex pattenOfImage = new Regex(@"(\[img=[0-9,]+\])([^\[]+)(\[/img\])");
            List<string> imageStr = new List<string>();
            List<string> imageUrl = new List<string>();

            foreach (Match mch in pattenOfImage.Matches(novelStr))
            {
                imageStr.Add(mch.Value.Trim());
                GroupCollection groups = mch.Groups;
                Group group = groups[2];
                imageUrl.Add(group.Value);

            }

            List<Image> images = new List<Image>();
            imageUrl.ForEach(str =>
            {
                Image image = new Image();
                BitmapImage btimage = new BitmapImage();
                btimage.UriSource = new Uri(str, UriKind.RelativeOrAbsolute);
                image.Source = btimage;
                images.Add(image);
            });
            return images;
        }

        private void addContent(string novelStr,List<Image> images)
        {
            string[] novelParagraph = Regex.Split(novelStr,@"\[img=[0-9,]+\][^\[]+\[/img\]");

            int i = 0;
            bool hasImage = false;
            if (images != null && images.Count > 0)
            {
                hasImage = true;
            }
            novelParagraph.ToList().ForEach(para =>
            {
                Paragraph paragraph = new Paragraph();
                Run run = new Run();
                run.Text = para;
                paragraph.Inlines.Add(run);
                paragraph.Inlines.Add(new LineBreak());
                if (hasImage)
                {
                    Image image = null;
                    try
                    {
                        image = images[i];
                    }
                    catch (Exception )
                    {
                    }
                    if (image != null)
                    {
                        InlineUIContainer iuc = new InlineUIContainer();
                        iuc.Child = image;
                        paragraph.Inlines.Add(iuc);
                        i++;
                    }else
                    {
                        hasImage = false;
                    }
                }
                content.Blocks.Add(paragraph);
            });
        }

        private void Left_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Left_Page();
        }

        private void Left_Tapped(object sender, RoutedEventArgs e)
        {
            Left_Page();
        }

        private void Left_Page()
        {
            //content.Text = "left area tapped";
        }




        private void Right_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Right_Page();
        }

        private void Right_Tapped(object sender, RoutedEventArgs e)
        {
            Right_Page();
        }

        private void Right_Page()
        {
            //content.Text = "right area tapped";
        }




        private void Middle_Tapped(object sender, RoutedEventArgs e)
        {
            Middle_Page();
        }

        private void Middle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Middle_Page();
        }

        private void Middle_Page()
        {
            //content.Text = "middle area tapped";
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
            Bookmark bookmark = new Bookmark() { novelId = chapter.novelId, chapId = chapter.chapId, chapName = chapter.title};
            BookmarkRepositoryUtil.save(bookmark);
            messShow.Show("书签添加成功", 1000);
        }
    }
}
