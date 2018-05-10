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

namespace Tool.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class TimestampConvert : Page
    {
        public TimestampConvert()
        {
            this.InitializeComponent();
        }

        private void time_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch (e.OriginalKey)
            {
                case Windows.System.VirtualKey.Enter:
                    convertValue();
                    break;
                default:
                    break;
            }
        }

        private void convertValue()
        {
            var timeStr = time.Text;
            if (string.IsNullOrEmpty(timeStr) || string.IsNullOrWhiteSpace(timeStr))
            {
                return;
            }

            try
            {
                var lTime = convertTimeStamp2DateTime(long.Parse(timeStr));

                result.Text = "转换结果：" +  lTime.ToString();
            }
            catch (Exception)
            {
                result.Text = "时间戳格式不对";
            }
        }


        /**
         * 入参为毫秒数
         */ 
        private DateTime convertTimeStamp2DateTime(long timestamp)
        {
            timestamp = timestamp * TimeSpan.TicksPerMillisecond;

            DateTime lTime = new DateTime(1970, 1, 1).ToLocalTime();

            TimeSpan span = new TimeSpan(timestamp);

            return lTime.Add(span);

        }
    }
}
