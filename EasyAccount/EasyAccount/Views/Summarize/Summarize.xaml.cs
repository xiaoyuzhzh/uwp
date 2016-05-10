using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace EasyAccount.Views.Summarize
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Summarize : Page
    {
        private List<TextBlock> navigationText;

        // 未选中的label样式
        private Style baseTextBlockStyle;

        // 已选中的label样式
        private Style selectedTextBlockStyle;

        public Summarize()
        {
            this.InitializeComponent();
            this.SummarizeFrame.Navigate(typeof(Weekly));
            navigationText = new List<TextBlock>();
            navigationText.Add(weekTextBlock);
            navigationText.Add(monthTextBlock);
            navigationText.Add(yearTextBlock);

            initParam();
        }

        private void initParam()
        {
            var resouce = this.Resources;
            
            baseTextBlockStyle = (Style)resouce.ToList().Where(s => s.Key.ToString().Equals("TextBlockBase")).ToList()[0].Value;
            selectedTextBlockStyle = (Style)resouce.ToList().Where(s => s.Key.ToString().Equals("Selected")).ToList()[0].Value;
        }

        private void Week_Tapped(object sender, TappedRoutedEventArgs e)
        {
            selectText(weekTextBlock);
            this.SummarizeFrame.Navigate(typeof(Weekly));
        }

        //改变选中的TextBlock的样式
        private void selectText(TextBlock block)
        {
            navigationText.ForEach(t => t.Style = baseTextBlockStyle);
            block.Style = selectedTextBlockStyle;
        }

        private void Month_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.SummarizeFrame.Navigate(typeof(Month));
            selectText(monthTextBlock);
        }

        private void Year_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.SummarizeFrame.Navigate(typeof(Month));
            selectText(yearTextBlock);
        }
    }
}
