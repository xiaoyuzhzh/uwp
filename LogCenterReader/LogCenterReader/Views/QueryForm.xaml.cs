using LogCenterReader.Model;
using LogCenterReader.Pojo.QO;
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

// “内容对话框”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上进行了说明

namespace LogCenterReader.Views
{
    public sealed partial class QueryForm : ContentDialog
    {
        public LogMessageQO qo { get; set; }
        /**
        * 是否有变化
        */
        public bool changed { get; set; }

        public QueryForm()
        {
            this.InitializeComponent();
            qo = new LogMessageQO();
            changed = false;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            changed = true;
            refreshQO();
        }

        public void refreshQO()
        {
            if (!string.IsNullOrEmpty(envId.Text))
            {
                qo.envId = envId.Text;
            }
            else
            {
                qo.envId = null;
            }
            if (!string.IsNullOrEmpty(projectId.Text))
            {
                qo.projectId = projectId.Text;
            }
            else
            {
                qo.projectId = null;
            }
            var levelStr = (String)level.SelectedItem;
            if (!string.IsNullOrEmpty(levelStr))
            {
                qo.level = levelStr;
            }
            else
            {
                qo.level = null;
            }
            if (!string.IsNullOrEmpty(title.Text))
            {
                qo.title = title.Text;
            }
            else
            {
                qo.title = null;
            }
            if (!string.IsNullOrEmpty(content.Text))
            {
                qo.content = content.Text;
            }
            else
            {
                qo.content = null;
            }
        }

        public void setContent(string contentStr)
        {
            content.Text = contentStr;

        }

        public void setTitle(string titleStr) {
            title.Text = titleStr;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private void level_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
