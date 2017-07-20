using Sfacg.Model;
using Sfacg.Model.StoreModel;
using Sfacg.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class QueryPage : Page
    {
        private string keyword = "";

        public QueryPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private async void autoSug_Box_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (sender.Text.Length != 0)
            {
                sender.ItemsSource = await GetSugges(sender.Text);
            }
            else
            {
                sender.ItemsSource = null;
            }
        }

        private async void autoSug_Box_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var text = autoSug_Box.Text;
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            keyword = Uri.EscapeDataString(text);
            List<Novel> novels = await NovelApiUtil.queryNovels(keyword);
            result.ItemsSource = novels;
        }

        private void autoSug_Box_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            autoSug_Box.Text = args.SelectedItem as string;
        }


        public async Task<List<String>> GetSugges(string text)
        {
            try
            {
                return await NovelApiUtil.getSugges(text);
            }
            catch (Exception)
            {
                return new List<string>();
            }

        }

        private void page_ItemClick(object sender, ItemClickEventArgs e)
        {
            Novel novel = (Novel)e.ClickedItem;
            result.PrepareConnectedAnimation("novelCover", novel, "NovelCoverImage");
            result.PrepareConnectedAnimation("novelName", novel, "NovelName");
            this.Frame.Navigate(typeof(NovelDetail), novel);
        }
    }
}
