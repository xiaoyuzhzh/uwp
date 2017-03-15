using Sfacg.Model;
using Sfacg.Utils;
using Sfacg.Views;
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
    public sealed partial class PushNovelView : Page
    {
        private List<NovelsVOData> novels;

        public PushNovelView()
        {
            this.InitializeComponent();
            
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            novels = await NovelUtil.getPushNovels(10);

            page.ItemsSource = novels;
        }

        private void page_ItemClick(object sender, ItemClickEventArgs e)
        {
            NovelsVOData item = (NovelsVOData)e.ClickedItem;
            var rootFrame = (Frame)Window.Current.Content;

            rootFrame.Navigate(typeof(NovelDetail), item);
        }
    }
}
