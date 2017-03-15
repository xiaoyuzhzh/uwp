using EasyAccount.Model;
using EasyAccount.Pojo;
using EasyAccount.Services.Respository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
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
    public sealed partial class CategoryForm : ContentDialog
    {

        public int level { get; set; }

        public string parentId { get; set; }

        public CategoryForm(int level)
        {
            this.level = level;
            this.InitializeComponent();
            switch (level)
            {               
                case 1: {
                        this.Title = "添加一级分类";
                        break;
                    }
                case 2: {
                        this.Title = "添加二级分类";
                        break;
                    }
                case 3: {
                        this.Title = "添加三级分类";
                        classTwo.Visibility = Visibility.Visible;
                        break;
                    }
                default:
                    break;
            }

            CategoryQO categoryQO = new CategoryQO();
            categoryQO.level = 1;
            classOne.ItemsSource = CategoryRepository.getList(categoryQO);
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            //保存分类
            if (String.IsNullOrEmpty(name.Text))
            {
                return;
            }
            if (String.IsNullOrEmpty(this.parentId))
            {
                return;
            }
            
            Category category = new Category();
            category.level = this.level;
            category.name = name.Text;
            category.value = name.Text;
            category.parentId = this.parentId;
            

            try
            {
                CategoryRepository.save(category);
                CategoryView.changed = true;
            }
            catch (Exception e)
            {
                ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;
                XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
                XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
                toastTextElements[0].AppendChild(toastXml.CreateTextNode(e.Message));
                IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
                ((XmlElement)toastNode).SetAttribute("duration", "short");

                // 5. audio
                XmlElement audio = toastXml.CreateElement("audio");
                audio.SetAttribute("src", $"ms-winsoundevent:Notification.Default");
                toastNode.AppendChild(audio);

                ToastNotification toast = new ToastNotification(toastXml);
                ToastNotificationManager.CreateToastNotifier().Show(toast);
            }
        }



        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }


        /**
        * 一级分类下拉框变化事件
        */
        private void classOne_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Category category = (Category)classOne.SelectedValue;
            if (category == null) return;
            if (level == 2)
            {
                this.parentId = category.id;
            }else
            {
                List<Category> categories;
                CategoryQO qo = new CategoryQO();
                qo.parentId = category.id;
                categories = CategoryRepository.getList(qo);
                classTwo.ItemsSource = categories;
            }

        }

        private void classTwo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Category category = (Category)classTwo.SelectedValue;
            if (category == null) return;
            if (level == 3)
            {
                this.parentId = category.id;
            }
        }
    }
}
