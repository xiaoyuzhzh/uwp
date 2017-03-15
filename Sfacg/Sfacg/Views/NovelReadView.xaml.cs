using Sfacg.Model;
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
    public sealed partial class NovelReadView : Page
    {
        MainPage rootPage;

        public NovelReadView()
        {
            this.InitializeComponent();
            rootPage = MainPage.Current;
            
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is ReadInfo)
            {
                ReadInfo readInfo = (ReadInfo)e.Parameter;
                var novelStr = await NovelUtil.getNovel(readInfo.novalId, readInfo.chapId);
                content.Text = novelStr;
            }
            else
            {
                content.Text = "wrong param";
            }
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
            content.Text = "left area tapped";
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
            content.Text = "right area tapped";
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
            content.Text = "middle area tapped";
        }


    }
}
