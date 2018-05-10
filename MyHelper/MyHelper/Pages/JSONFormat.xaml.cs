using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Tool.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class JSONFormat : Page
    {
        public JSONFormat()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private void ConvertStr(object sender, RoutedEventArgs e)
        {
            Content.Text = JsonTree(Content.Text);
        }


        /// <summary>
        /// JSON字符串格式化
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string JsonTree(string json)
        {
            int level = 0;
            var jsonArr = json.ToArray();  // Using System.Linq;
            string jsonTree = string.Empty;
            for (int i = 0; i < json.Length; i++)
            {
                char c = jsonArr[i];
                if (level > 0 && '\n' == jsonTree.ToArray()[jsonTree.Length - 1])
                {
                    jsonTree += TreeLevel(level);
                }
                switch (c)
                {
                    case '[':
                        tabOne(ref level, ref jsonTree, c);
                        break;
                    case ',':
                        jsonTree += c + "\n";
                        break;
                    case ']':
                        inTabOne(ref level, ref jsonTree, c);
                        break;
                    case '{':
                        tabOne(ref level, ref jsonTree, c);
                        break;
                    case '}':
                        inTabOne(ref level, ref jsonTree, c);
                        break;
                    default:
                        jsonTree += c;
                        break;
                }
            }

            //移除空数组中的制表符和换行符
            jsonTree = Regex.Replace(jsonTree, @"\[[\t\n\r]+\]", @"[]");
            return jsonTree;
        }

        private static void inTabOne(ref int level, ref string jsonTree, char c)
        {
            jsonTree += "\n";
            level--;
            jsonTree += TreeLevel(level);
            jsonTree += c;
        }

        /// <summary>
        /// 缩进一位
        /// </summary>
        /// <param name="level"></param>
        /// <param name="jsonTree"></param>
        /// <param name="c"></param>
        private static void tabOne(ref int level, ref string jsonTree, char c)
        {
            jsonTree += c + "\n";
            level++;
        }

        /// <summary>
        /// 树等级
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private static string TreeLevel(int level)
        {
            string leaf = string.Empty;
            for (int t = 0; t < level; t++)
            {
                leaf += "\t";
            }
            return leaf;
        }
    }
}
