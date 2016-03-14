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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace EasyAccount.Views
{


    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AddTransaction : Page
    {

        public List<string> classOnes { get; set; }

        public List<string> classTwos { get; set; }

        public List<string> classThrees { get; set; }




        public AddTransaction()
        {
            this.InitializeComponent();

            classOnes = new List<string> { "支出很长支出很长支出很长支出很长", "收入" };

            classOne.ItemsSource = classOnes;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var transaction = getTransactionFromForm();

            if (transaction != null)
            {
                save(transaction);
            }
        }

        private void save(Transaction transaction)
        {
            if (transaction.id == null)
            {
                transaction.id = Guid.NewGuid().ToString();
            }
            AppDatabase.saveTransaction(transaction);
        }





        private Transaction getTransactionFromForm()
        {
            var transaction = new Transaction();
            try
            {
                transaction.amount = Decimal.Parse(amount.Text);
            }
            catch (Exception)
            {
                return null;
            }

            transaction.tradeTime = new DateTime(date.Date.Year, date.Date.Month, date.Date.Day,
                                                    time.Time.Hours, time.Time.Minutes, time.Time.Seconds);

            transaction.classOne = classOne.SelectedValue == null ? null : classOne.SelectedValue.ToString();
            transaction.classTwo = classTwo.SelectedValue == null ? null : classTwo.SelectedValue.ToString();
            transaction.classThree = classThree.SelectedValue == null ? null : classThree.SelectedValue.ToString();
            return transaction;
        }

        private void jumpToStatistics(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Statistics));
        }
    }
}
