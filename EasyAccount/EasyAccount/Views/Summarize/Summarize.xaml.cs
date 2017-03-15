using EasyAccount.Model;
using EasyAccount.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private SummarizeService summarizeServices = new SummarizeService();

        private ObservableCollection<DailySummarize> dailySummarizes;

        // 未选中的label样式
        private Style baseTextBlockStyle;

        // 已选中的label样式
        private Style selectedTextBlockStyle;

        public Summarize()
        {
            this.InitializeComponent();
            navigationText = new List<TextBlock>();
            dailySummarizes = new ObservableCollection<DailySummarize>();
            //initParam();
            summarizeServices.getWeeklySummarize(dailySummarizes);

        }

        private void initParam()
        {
            var resouce = this.Resources;
            
            baseTextBlockStyle = (Style)resouce.ToList().Where(s => s.Key.ToString().Equals("TextBlockBase")).ToList()[0].Value;
            selectedTextBlockStyle = (Style)resouce.ToList().Where(s => s.Key.ToString().Equals("Selected")).ToList()[0].Value;
        }


    }
}
