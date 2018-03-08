using Sfacg.Model;
using Sfacg.Model.ApiVO;
using Sfacg.Model.StoreModel;
using Sfacg.Utils;
using Sfacg.Views.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace Sfacg.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NovelReadV2 : Page
    {
        private Chapter chapter;

        private List<Chapter> chapters;

        StatusBar statusBar;

        private ObservableCollection<NovelContentVO> novelContents;

        private List<NovelContentVO> tempList = new List<NovelContentVO>();

        private NovelContentVO[] tempArray;

        private int topOffset;

        private int endOffset;

        private bool statusBarVisible = true;//是否展示statusBar

        private bool loadContent = false;//是否正在加载内容

        private bool hasLoadPosition = false;//是否已经跳跃到指定位置，主要用在ListView的contentChanging方法中，跳转之后，不再需要监听事件

        private static double itemContainerHeight = 0;
        private static int itemId = 0;
        private static string _persistedPosition = "";

        private ScrollViewer scrollViewer = null;

        private int progress = 0;


        public NovelReadV2()
        {
            this.InitializeComponent();

            // 判断是否存在 StatusBar
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                statusBar = StatusBar.GetForCurrentView();
            }
            else
            {
                btn_StatusBar.Visibility = Visibility.Collapsed;
            }

            novelContents = new ObservableCollection<NovelContentVO>();
            novelList.ItemsSource = novelContents;

        }

        /// <summary>
        /// 进入页面
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Chapter)
            {
                chapter = (Chapter)e.Parameter;
                ChapterName.Text = chapter.title;
                process.IsActive = true;
                _persistedPosition = chapter.listPosition;
                if (!string.IsNullOrEmpty(_persistedPosition)) {
                    itemId = chapter.itemId;
                    itemContainerHeight = chapter.itemContainerHeight;
                    hasLoadPosition = false;//刚进入页面，需要跳转
                }
                else
                {
                    itemId = 0;
                    itemContainerHeight = 0;
                    hasLoadPosition = true;//没有阅读断点，标记为已跳转
                }

                var catalog = await NovelApiUtil.getNovelCatalog(chapter.novelId, true);
                if (catalog != null && catalog.Count > 0)
                {
                    catalog.ForEach(v =>
                    {
                        if(chapters== null || chapters.Count == 0)
                        {
                            chapters = v.chapters;
                        }
                        else
                        {
                            chapters = chapters.Concat(v.chapters).ToList();
                        }
                        
                    });
                }
                
                try
                {
                    var titleAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("chapterName");
                    if (titleAnimation != null)
                    {
                        titleAnimation.TryStart(ChapterName);
                    }
                }
                catch (Exception)
                {
                    
                }
                
            }
            else
            {
                //content.Text = "wrong param";
            }

            showStatusBar(false);
        }

        /// <summary>
        /// 打开新的章节
        /// </summary>
        /// <param name="nChapter"></param>
        private void openNew(Chapter nChapter)
        {
            ResetParam(nChapter);

            GetAndShowNovel();

        }

        /// <summary>
        /// 充值页面参数
        /// </summary>
        /// <param name="nChapter"></param>
        private void ResetParam(Chapter nChapter)
        {
            chapter = nChapter;
            ChapterName.Text = chapter.title;
            process.IsActive = true;
            itemId = 0;
            itemContainerHeight = 0;
            hasLoadPosition = true;//新开章节，不需要跳转
            novelContents = new ObservableCollection<NovelContentVO>();
            novelList.ItemsSource = novelContents;
            tempList = new List<NovelContentVO>();
            tempArray = null;
            topOffset = 0;
            endOffset = 0;
            progress = 0;
        }

        /// <summary>
        /// 从页面浏览出去
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {

            showStatusBar(true);
        }

        /// <summary>
        /// 展示小说
        /// </summary>
        /// <param name="novelStr"></param>
        private void ShowNovel(string novelStr)
        {
            if (string.IsNullOrEmpty(novelStr))
            {
                return;
            }

            //将内容保存到临时列表
            setTempList(novelStr);

            
            int start = itemId - 10;
            if (start < 0)
            {
                start = 0;
            }
            showSomeContent(start);
            loadPosition();
            ProgressText.Text = getProgress() + "%";
            updateReadPoint();
        }

        /// <summary>
        /// 显示一些内容
        /// </summary>
        /// <param name="offset"></param>
        private void showSomeContent(int offset)
        {
            topOffset = offset;
            endOffset = offset + 20;
            if (endOffset > tempArray.Length)
            {
                endOffset = tempArray.Length;
            }
            addAsc(offset,endOffset);
        }

        /// <summary>
        /// 正序添加
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void addAsc(int start,int end)
        {
            if (start > end)
            {
                return;
            }
            for (int i = start; i < end; i++)
            {
                novelContents.Add(tempArray[i]);
            }
        }

        /// <summary>
        /// 倒序添加
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void addDesc(int start, int end)
        {
            if (start < end)
            {
                return;
            }
            for (int i = start-1; i >= end; i--)
            {
                novelContents.Insert(0, tempArray[i]);
            }
        }

        /// <summary>
        /// 文章段落的临时保存列表，因为一口气放到listview中的时候，会出现加载很慢，并且有闪退的可能。
        /// </summary>
        /// <param name="novelStr"></param>
        private void setTempList(string novelStr)
        {
            if (string.IsNullOrEmpty(novelStr))
            {
                return;
            }
            int length = novelStr.Length;
            var imageUrls = getImageUrls(novelStr);
            var paragraphs = getParagraphs(novelStr);


            int i = 0;
            int count = 0;
            string imageUrl = null;
            bool hasImage;
            bool stopAdd2View = false;
            hasImage = imageUrls.Count > 0;
            int paraIndex = 0;
            paragraphs.ForEach(p =>
            {
                paragraphSlices(p, ref paraIndex).ForEach(n =>
                {
                    stopAdd2View = true;
                    tempList.Add(n);
                });

                if (hasImage)
                {

                    try
                    {
                        imageUrl = imageUrls[i];
                    }
                    catch (Exception)
                    {
                        imageUrl = null;
                    }
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        tempList.Add(new NovelContentVO() { imageUrl = imageUrl, id = paraIndex++ });
                        i++;
                    }
                    else
                    {
                        hasImage = false;
                    }
                }

            });


            tempArray = tempList.ToArray();
        }

        /// <summary>
        /// 切割文章段落
        /// </summary>
        /// <param name="paragraph"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private List<NovelContentVO> paragraphSlices(string paragraph,ref int index)
        {
            var paragraphSlices = new List<NovelContentVO>();
            string[] novelParagraph = Regex.Split(paragraph, @"\r\n");
            for (int i = 0; i< novelParagraph.Length; i++)
            {
                if (!string.IsNullOrEmpty(novelParagraph[i]) && !string.IsNullOrWhiteSpace(novelParagraph[i]))
                {
                    paragraphSlices.Add(new NovelContentVO() { paragraph = novelParagraph[i], id = index++ });
                }
                
            }
            return paragraphSlices;
        }

        private List<string> getParagraphs(string novelStr)
        {
            string[] novelParagraph = Regex.Split(novelStr, @"\[img=[0-9,]+\][^\[]+\[/img\]");
            return novelParagraph.ToList();
        }

        private static List<string> getImageUrls(string novelStr)
        {
            Regex pattenOfImage = new Regex(@"(\[img=[0-9,]+\])([^\[]+)(\[/img\])");
            List<string> imageUrl = new List<string>();
            

            foreach (Match mch in pattenOfImage.Matches(novelStr))
            {
                GroupCollection groups = mch.Groups;
                Group group = groups[2];
                imageUrl.Add(group.Value);
            }
            return imageUrl;
        }

        private void showStatusBar(bool show)
        {
            if (statusBar != null)
            {
                if (show)
                {
                    statusBar.ShowAsync();
                }else
                {
                    statusBar.HideAsync();
                }
                statusBarVisible = show;
            }
        }

        /// <summary>
        /// 阅读页面被点击了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void content_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (Down_ComBar.Visibility == Visibility.Collapsed)
            {
                Down_ComBar.Visibility = Visibility.Visible;
            }
            else
            {
                Down_ComBar.Visibility = Visibility.Collapsed;
            }

        }

        private int getProgress()
        {
            markPosition();
            return Convert.ToInt32(itemId / (double)tempArray.Length * 100);
        }

        private void sv_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (loadContent)
            {
                return;
            }

            try
            {
                loadContent = true;//标记处理中


                //加载上下文
                if (scrollViewer.ScrollableHeight - scrollViewer.VerticalOffset < 200)
                {
                    loadRestContent();
                }
                if (scrollViewer.VerticalOffset < 200)
                {
                    loadTopContent();
                }

                //刷新进度
                if (scrollViewer.ScrollableHeight == scrollViewer.VerticalOffset)
                {
                    ProgressText.Text = "100%";
                    messShow.Show("已经到底了o(*￣▽￣*)o", 1000);
                }
                else
                {
                    progress = getProgress();
                    ProgressText.Text = progress + "%";
                }

                //缓存阅读进度
                updateReadPoint();
            }
            catch (Exception)
            {

            }
            finally
            {
                loadContent = false;
            }
            
        }

        /// <summary>
        /// 更新阅读进度
        /// </summary>
        private void updateReadPoint()
        {
            markPosition();
            var bookmark = new Bookmark() { novelId = chapter.novelId, chapId = chapter.chapId, chapName = chapter.title, itemId = itemId, itemContainerHeight = itemContainerHeight, listPosition = _persistedPosition, progress = getProgress() };
            BaseUtil.setSetting<Bookmark>(BaseUtil.READ_POINT_PREFIX + chapter.novelId, bookmark, true);
        }

        /// <summary>
        /// 加载阅读断点
        /// </summary>
        private async void loadPosition()
        {
            if (!hasLoadPosition&&!string.IsNullOrEmpty(_persistedPosition))
            {
                // Here we kick off the async function to use the saved string _persistedPosition and the function GetItem to restore the scroll posistion
                await ListViewPersistenceHelper.SetRelativeScrollPositionAsync(this.novelList, _persistedPosition, this.GetItem);

                hasLoadPosition = true;
            }
        }

        private IAsyncOperation<object> GetItem(string key)
        {
            // This function takes a key that ListViewPersistenceHelper parsed out of the _persistedPosition string
            // It returns an item that corresponds to the given key
            // The implementation of this function is dependent on the data model you are using. Every item in your list should have
            // a unique key this function returns
            return Task.Run(() =>
            {
                if (novelContents.Count <= 0||string.IsNullOrEmpty(key))
                {
                    return null;
                }
                else
                {
                    return (object)novelContents.FirstOrDefault(i => i.id == Convert.ToInt32(key));
                }
            }).AsAsyncOperation();
        }

        /// <summary>
        /// 标记阅读断点
        /// </summary>
        private void markPosition()
        {
            _persistedPosition = ListViewPersistenceHelper.GetRelativeScrollPosition(this.novelList, this.GetKey);
        }

        private string GetKey(object item)
        {
            var singleItem = item as NovelContentVO;
            if (singleItem != null)
            {
                itemContainerHeight = (novelList.ContainerFromItem(item) as ListViewItem).ActualHeight;
                itemId = singleItem.id;
                return itemId+"";
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// 加载上面的内容
        /// </summary>
        private void loadTopContent()
        {
            if (topOffset > 0)
            {
                int start = topOffset;
                topOffset = topOffset - 20;
                if (topOffset < 0)
                {
                    topOffset = 0;
                }
                addDesc(start, topOffset);
            }
        }

        /// <summary>
        /// 加载剩余内容
        /// </summary>
        private void loadRestContent()
        {
            if (endOffset<=tempArray.Length)
            {
                int startOffset = endOffset;
                endOffset = endOffset + 20;
                if (endOffset > tempArray.Length)
                {
                    endOffset = tempArray.Length;
                }

                addAsc(startOffset, endOffset);
            }
        }


        /// <summary>
        /// 跳转之前，根据item项高预设项的高度，保证列表的位置跳转准确，跳转完成之后，删除这个事件的监听方法。 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ItemsListView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var singleItem = args.Item as NovelContentVO;

            if (singleItem != null && singleItem.id == itemId)
            {
                if (!args.InRecycleQueue&&!string.IsNullOrEmpty(_persistedPosition)&&!hasLoadPosition)
                {
                    // Here we set the container's height equal to the fully rendered container height we had saved before navigating away. If all the items in 
                    // your list have the same fixed height, you can replace this variable with a hardcoded height value. 
                    args.ItemContainer.Height = itemContainerHeight;
                }
                else
                {
                    // Containers in a list are recycled when they are scrolled out of view. However, if those containers have their Height property set and the content
                    // changes, that set Height is still applied. This creates an incorect UI if the items in your list are supposed to be of variable height. 
                    // If all the items in your list have the same fixed height, you do not have to do this. 
                    args.ItemContainer.ClearValue(HeightProperty);
                }
               
            }

            if (hasLoadPosition)
            {
                novelList.ContainerContentChanging -= ItemsListView_ContainerContentChanging;
            }
        }


        /// <summary>
        /// 页面加载完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            scrollViewer = BaseUtil.FindFirstChild<ScrollViewer>(novelList);
            scrollViewer.ViewChanged += sv_ViewChanged;

            //展示内容
            await GetAndShowNovel();
        }

        /// <summary>
        /// 展示新的小说
        /// </summary>
        /// <returns></returns>
        private async Task GetAndShowNovel()
        {
            try
            {
                string novelStr;
                try
                {
                    novelStr = await NovelApiUtil.getNovel(chapter.novelId, chapter.chapId);
                }
                catch (Exception)
                {
                    messShow.Show("网络异常", 3000);
                    return;
                }
                ShowNovel(novelStr);
            }
            catch (Exception)
            {
            }
            finally
            {
                process.IsActive = false;
            }
        }

        /// <summary>
        /// 状态栏按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_StatusBar_Clicked(object sender, RoutedEventArgs e)
        {
            showStatusBar(!statusBarVisible);
        }

        /// <summary>
        /// 书签按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_BookMark_Clicked(object sender, RoutedEventArgs e)
        {
            markPosition();
            Bookmark bookmark = new Bookmark() { novelId = chapter.novelId, chapId = chapter.chapId, chapName = chapter.title, itemId = itemId, itemContainerHeight = itemContainerHeight, listPosition = _persistedPosition, progress = getProgress() };
            BookmarkRepositoryUtil.save(bookmark);
            messShow.Show("书签添加成功", 1000);
        }

        /// <summary>
        /// 下一章按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_NextChapter_Click(object sender, RoutedEventArgs e)
        {
            Chapter nextChapter = GetNextChapter();
            if(nextChapter != null)
            {
                openNew(nextChapter);
            }
            else
            {
                messShow.Show("已经到了最后一章", 1000);
            }
        }

        /// <summary>
        /// 获取下一章
        /// </summary>
        /// <param name="chapterArr"></param>
        /// <returns></returns>
        private Chapter GetNextChapter()
        {
            if (chapters == null || chapters.Count == 0)
            {
                return null;
            }
            var chapterArr = chapters.ToArray();
            Chapter nextChapter = null;
            for (int i = 0; i < chapterArr.Length; i++)
            {
                if (chapterArr[i].chapId.Equals(chapter.chapId) && i < (chapterArr.Length - 1))
                {
                    nextChapter = chapterArr[i + 1];
                    break;
                }
            }
            return nextChapter;
        }

        /// <summary>
        /// 获取前一章
        /// </summary>
        /// <param name="chapterArr"></param>
        /// <returns></returns>
        private Chapter GetPrevChapter()
        {
            if (chapters == null || chapters.Count == 0)
            {
                return null;
            }
            var chapterArr = chapters.ToArray();
            Chapter nextChapter = null;
            for (int i = 0; i < chapterArr.Length; i++)
            {
                if (chapterArr[i].chapId.Equals(chapter.chapId) && i>0)
                {
                    nextChapter = chapterArr[i-1];
                    break;
                }
            }
            return nextChapter;
        }

        private void btn_PrevChapter_Click(object sender, RoutedEventArgs e)
        {
            Chapter nextChapter = GetPrevChapter();
            if (nextChapter != null)
            {
                openNew(nextChapter);
            }
            else
            {
                messShow.Show("已经到了第一章", 1000);
            }
        }
    }
}
