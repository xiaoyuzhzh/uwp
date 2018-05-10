using System;
using System.Collections.Generic;
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

namespace Tool.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CodeConvert : Page
    {
        public CodeConvert()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        /**
         * 转换
         */
        private async void ConvertStr(object sender, RoutedEventArgs e)
        {

            var source = sourceTextBox.Text;
            if (string.IsNullOrEmpty(source))
            {
                return;
            }

            var format = (string)Switch.SelectedItem;

            resultTextBox.Text = String2Unicode(source, format);

        }

        private async void Reverse(object sender, RoutedEventArgs e)
        {
            var source = resultTextBox.Text;
            if (string.IsNullOrEmpty(source))
            {
                return;
            }

            sourceTextBox.Text = Unicode2String(source);
        }



        /// <summary>
        /// 将Unicon字符串转成汉字String
        /// </summary>
        /// <param name="str">Unicon字符串</param>
        /// <returns>汉字字符串</returns>
        public string Unicode2String(string str)
        {
            string outStr = "";
            if (!string.IsNullOrEmpty(str))
            {
                string[] strlist = str.Replace("\\", "").Split('u','U');
                try
                {
                    for (int i = 1; i < strlist.Length; i++)
                    {
                        //将unicode字符转为10进制整数，然后转为char中文字符
                        outStr += (char)int.Parse(strlist[i], System.Globalization.NumberStyles.HexNumber);
                    }
                }
                catch (FormatException ex)
                {
                    outStr = ex.Message;
                }
            }
            return outStr;
        }

        public string String2Unicode(string source,string format)
        {
            string result = "";
            if (!string.IsNullOrEmpty(source))
            {
                var chararr = source.ToCharArray();
                for (int i = 0; i < chararr.Length; i++)
                {
                    if (format.Equals("Native/Unicode"))
                    {
                        result += getUnicodeStr(((int)chararr[i]));
                        continue;
                    }
                    if (format.Equals(@"Native/UTF-8"))
                    {

                    }

                }
            }

            return result;
        }


        private string getUnicodeStr(int v)
        {
            if(v == null){
                return "";
            }
            return @"\u"+Convert.ToString(v, 16);
        }


    }
}
