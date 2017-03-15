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

namespace EasyAccount.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CategoryView : Page
    {

        private ObservableCollection<Category> categories = new ObservableCollection<Category>();

        private int level;//当前页面分类等级

        private CategoryQO categoryQO = new CategoryQO();//当前页面查询对象

        public static bool changed { get; set; }//当前页面值是否变化了

        private bool showFilters = false;

        public CategoryView()
        {
            this.InitializeComponent();
            CategoryQO categoryQO = new CategoryQO();
            categoryQO.level = 1;
            classOne.ItemsSource = CategoryRepository.getList(categoryQO);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if(e.Parameter != null)
            {
                categoryQO.level = (int)e.Parameter;
                level = categoryQO.level;
                findCategory();
            }
        }

        /**
         * 刷新分类列表
         */
        private void findCategory()
        {
            List<Category> category = CategoryRepository.getList(categoryQO);
            categories.Clear();
            category.ForEach(c => categories.Add(c));
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var form = new CategoryForm(this.level) ;
            await form.ShowAsync();
            if (changed)
            {
                changed = !changed;
                findCategory();
            }
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            this.showFilters = !this.showFilters;

            switch (this.level)
            {
                case 2: {
                        if (showFilters)
                        {
                            classOne.Visibility = Visibility.Visible;
                        }else
                        {
                            classOne.Visibility = Visibility.Collapsed;
                        }
                        
                        break;
                    }
                case 3: {
                        if (showFilters)
                        {
                            classOne.Visibility = Visibility.Visible;
                            classTwo.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            classOne.Visibility = Visibility.Collapsed;
                            classTwo.Visibility = Visibility.Collapsed;
                        }
                        break;
                    }
                default:
                    break;
            }

            if (!showFilters)
            {
                categoryQO.parentId = null;
                findCategory();
            }
            
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
                categoryQO.level = level;
                categoryQO.parentId = category.id;
                findCategory();
            }
            else
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
                categoryQO.level = level;
                categoryQO.parentId = category.id;
                findCategory();
            }
        }
    }
}
