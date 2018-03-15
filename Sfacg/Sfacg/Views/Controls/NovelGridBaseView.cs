using Sfacg.Model.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Sfacg.Views.Controls
{
    public abstract class NovelGridBaseView : Page
    {
        /// <summary>
        /// 获取列表对象
        /// </summary>
        /// <returns></returns>
        protected abstract GridView getPage();

        /// <summary>
        /// 设置项高
        /// </summary>
        /// <param name="width"></param>
        protected abstract void setItemWidth(double width);

        /// <summary>
        /// 列表对象被点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void page_ItemClick(object sender, ItemClickEventArgs e)
        {

            Novel item = (Novel)e.ClickedItem;
            try
            {
                getPage().PrepareConnectedAnimation("novelCover", item, "NovelCoverImage");
                getPage().PrepareConnectedAnimation("novelName", item, "NovelName");
            }
            catch (Exception)
            {

            }
            MainPage.secondFrame.Navigate(typeof(NovelDetail), item);
        }

        /// <summary>
        /// 页面大小改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            int d = Convert.ToInt32(this.ActualWidth / 120);
            if (d > 10)
            {
                d = 10;
            }

            setItemWidth((this.ActualWidth - 10 * d) / d - 3);
        }
    }


}
