using EasyAccount.Model;
using EasyAccount.Pojo;
using EasyAccount.Services.Respository;
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
    public sealed partial class LatelyBills : Page
    {

        public LatelyBills()
        {
            this.InitializeComponent();
            GetTransactionList();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private void GetTransactionList()
        {
            List<Transaction> trans = TransactionRepository.getList(new TransactionQO());
            trans.ForEach(t =>
            {
                TransactionGrid.Items.Add(t);
            });
        }

        private void btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            List<Transaction> trans = TransactionRepository.getList(new TransactionQO());
            TransactionGrid.Items.Clear();
            GetTransactionList();
            
        }
    }
}
