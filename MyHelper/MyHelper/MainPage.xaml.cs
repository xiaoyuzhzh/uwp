using MyHelper.Pages;
using Tool.Pages;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyHelper
{
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();

            //SystemNavigationManager.GetForCurrentView().BackRequested += SystemNavigationManager_BackRequested;

            //SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;

            //this.frame.Navigated += OnNavigatedToPage;

            //使用没有宽度的block，替换原来的TitleBar，这样可以保证扩展到顶部的按钮不会被阻挡
            //Window.Current.SetTitleBar(AppTitle);


            //var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            //Window.Current.CoreWindow.SizeChanged += (s, e) => UpdateAppTitle();
            //coreTitleBar.LayoutMetricsChanged += (s, e) => UpdateAppTitle();
        }

        void UpdateAppTitle()
        {
            var full = (ApplicationView.GetForCurrentView().IsFullScreenMode);
            var left = 12 + (full ? 0 : CoreApplication.GetCurrentView().TitleBar.SystemOverlayLeftInset);
            AppTitle.Margin = new Thickness(left, 8, 0, 0);
        }


        private void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {

            }
            else
            {
                var menu = args.SelectedItem as NavigationViewItem;
                switch (menu.Tag.ToString())
                {
                    case "codeChange":
                        contentFrame.Navigate(typeof(CodeConvert));
                        ContentHeader.Text = "编码转换";
                        break;
                    case "timestampTranslate":
                        contentFrame.Navigate(typeof(TimestampConvert));
                        ContentHeader.Text = "时间戳转换";
                        break;
                    case "jsonFormat":
                        contentFrame.Navigate(typeof(JSONFormat));
                        ContentHeader.Text = "json转换";
                        break;
                    case "calculator":
                        contentFrame.Navigate(typeof(Calculator));
                        ContentHeader.Text = "sb计数器";
                        break;
                }
            }
        }

        private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {

        }
    }
}
