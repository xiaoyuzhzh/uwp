using EasyAccount.Model;
using EasyAccount.Services;
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

namespace EasyAccount.Views
{
    public sealed partial class TransactionForm : ContentDialog
    {
        public TransactionForm()
        {
            this.InitializeComponent();

            classOne.ItemsSource = new List<string> { "支出很长支出很长支出很长支出很长", "收入" };
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var transaction = getTransactionFromForm();

            if (transaction != null)
            {
                save(transaction);
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
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
    }
}
