using LogCenterReader.Model;
using LogCenterReader.Pojo;
using LogCenterReader.Pojo.QO;
using LogCenterReader.Service;
using LogCenterReader.Util.logUtil;
using LogCenterReader.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace LogCenterReader
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private ObservableCollection<LogMessage> logs;

        private QueryForm queryForm = new QueryForm();

        private int pageNo = 1;

        private int pageSize = 20;

        private int pageCount = 1;

        private static LogCenterUtil logger = new LogCenterUtil("dev", "logCenterReaderApp");

        private const string LOGQUERYURL = "LOGQUERYURL";

        private const string LOGADDURL = "LOGADDURL";


        public MainPage()
        {
            this.InitializeComponent();

            logs = new ObservableCollection<LogMessage>();

            goToPage();
        }

        /**
        * 初始化日志查询
        */
        private async void initLogs()
        {
            try
            {
                LogMessageQO qo = new LogMessageQO();
                var page = await LogFinder.getLogByPage(1,5,qo);
                var logList = page.list;
                if (logList != null)
                {
                    logs.Clear();
                    logList.ForEach(p => {
                        logs.Add(p);
                    });
                }
            }
            catch (Exception ex)
            {
               var s =  ex.Message;
            }

        }

        private void goPrePage(object sender, RoutedEventArgs e)
        {
            if (this.pageNo > 1)
            {
                this.pageNo--;
                goToPage();
            }
        }

        private void goNextPage(object sender, RoutedEventArgs e)
        {
            if(this.pageNo < this.pageCount)
            {
                this.pageNo++;
                goToPage();
            }
        }

        private async void ChangeQuery_Click(object sender, RoutedEventArgs e)
        {
            await queryForm.ShowAsync();
            
            if (queryForm.changed)
            {
                queryForm.changed = false;
                resetPage();
            }
        }

        private async void resetPage()
        {
            this.pageNo = 1;
            goToPage();
        }

        private async void goToPage()
        {
            try
            {
                this.pageNoText.Text = "" + this.pageNo;
                LogMessageQO qo = queryForm.qo;
                titleTextBox.Text = qo.title == null?"":qo.title;
                contentTextBox.Text = qo.content == null?"":qo.content;
                var page = await LogFinder.getLogByPage(this.pageNo, this.pageSize, qo);
                this.pageCountText.Text = "" + page.pageCount;
                this.pageCount = page.pageCount;
                var logList = page.list;
                if (logList != null)
                {
                    logs.Clear();
                    logList.ForEach(p => {
                        logs.Add(p);
                    });
                }
            }
            catch (Exception ex)
            {
                logger.error("查询日志出错", LogCenterUtil.getStackTrace(ex));
            }

        }

        private void quickSearchButton_Click(object sender, RoutedEventArgs e)
        {

            queryForm.setContent(contentTextBox.Text);
            queryForm.setTitle(titleTextBox.Text);
            queryForm.refreshQO();
            resetPage();

        }

        private void titleTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case VirtualKey.Enter:
                    {
                        quickSearchButton_Click(sender,e);
                        break;
                    }
                default: break;
            }
        }
    }
}
