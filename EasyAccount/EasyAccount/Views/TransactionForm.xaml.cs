using EasyAccount.Model;
using EasyAccount.Pojo;
using EasyAccount.Services;
using EasyAccount.Services.Respository;
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

        private CategoryRepository categoryRepo = CategoryRepository.getInstance();

        private Dictionary<string,List<Category>> classTwoMap { get; set; }

        public TransactionForm()
        {
            this.InitializeComponent();

            CategoryQO categoryQO = new CategoryQO();
            categoryQO.level = 1;
            classOne.ItemsSource = CategoryRepository.getList(categoryQO);
            
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

            transaction.classOne = classOne.SelectedValue == null ? null : ((Category)classOne.SelectedValue).value;
            transaction.classTwo = classTwo.SelectedValue == null ? null : ((Category)classTwo.SelectedValue).value;
            transaction.classThree = classThree.SelectedValue == null ? null : ((Category)classThree.SelectedValue).value;
            return transaction;
        }


        /**
        * 一级分类下拉框变化事件
        */
        private void classOne_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Category category = (Category)classOne.SelectedValue;
            if (category == null) return;
            List<Category> categories;
            CategoryQO qo = new CategoryQO();
            qo.parentId = category.id;
            categories = CategoryRepository.getList(qo);
            classTwo.ItemsSource = categories;
        }


        /**
        * 二级分类下拉框变化事件
        */
        private void classTwo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Category category = (Category)classTwo.SelectedValue;
            if (category == null) return;
            List<Category> categories;
            CategoryQO qo = new CategoryQO();
            qo.parentId = category.id;
            categories = CategoryRepository.getList(qo);
            classThree.ItemsSource = categories;
        }
    }
}
