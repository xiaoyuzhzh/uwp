using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using Windows.Foundation.Metadata;
using Windows.ApplicationModel.Core;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Sfacg
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SplashPage : Page
    {
        StatusBar statusBar;

        public SplashPage()
        {
            this.InitializeComponent();

            // 判断是否存在 StatusBar
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                statusBar = StatusBar.GetForCurrentView();
            }

            if (statusBar != null)
            {
                statusBar.BackgroundColor = Color.FromArgb(255, 233, 233, 233);
                statusBar.BackgroundOpacity = 100;
                statusBar.ForegroundColor = Colors.Black;
            }

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;

            var appTitleBar = ApplicationView.GetForCurrentView().TitleBar;

            if (coreTitleBar != null)
            {
                coreTitleBar.ExtendViewIntoTitleBar = false;
            }

            if (appTitleBar != null)
            {
                appTitleBar.BackgroundColor = Color.FromArgb(255, 233, 233, 233);
                appTitleBar.ButtonBackgroundColor = Color.FromArgb(255, 233, 233, 233);
            }
        }

        DispatcherTimer timer;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {

            load_img.Height = this.ActualHeight;

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();
            
        }

        int i = 1;
        int maxnum = 3;
        private void Timer_Tick(object sender, object e)
        {
            if (i != maxnum)
            {
                i++;
            }
            else
            {
                this.Frame.Navigate(typeof(MainPage));

            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            timer.Stop();
            timer = null;
        }

        private void grid_Load_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            load_img.Height = this.ActualHeight;
        }
    }
}
