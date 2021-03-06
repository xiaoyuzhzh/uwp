﻿using EasyAccount.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace EasyAccount.Views.Summarize
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Weekly : Page
    {
        private ObservableCollection<DailySummarize> summarizes;

        public Weekly()
        {
            this.InitializeComponent();
            summarizes = new ObservableCollection<DailySummarize>();

            mockData();
        }

        private void mockData()
        {
            for(var i = 0; i < 30; i++)
            {
                DailySummarize summarize = new DailySummarize();
                summarize.income = 10;
                summarize.payout = 20;
                summarize.day = 1;
                summarize.weekName = "Monday";
                summarizes.Add(summarize);
            }

        }
    }
}
