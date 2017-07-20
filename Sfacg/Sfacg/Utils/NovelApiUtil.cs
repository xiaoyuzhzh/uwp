using Sfacg.Model;
using Sfacg.Model.ApiVO;
using Sfacg.Model.StoreModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Web.Http;

namespace Sfacg.Utils
{
    class NovelApiUtil
    {
        private static StorageFolder folder = ApplicationData.Current.LocalFolder;

        private static string NovelContentAccessurl = "https://api.sfacg.com/Chaps/#chapId#?expand=content,needFireMoney";//小说内容

        private static string PushNovelsUrl = "https://api.sfacg.com/novels?size=#size#&filter=newpush";//推荐小说

        private static string NovelCatalogUrl = "https://api.sfacg.com/novels/#novelId#/dirs";//小说目录

        private static string NovelDetailUrl = "https://api.sfacg.com/novels/#novelId#?expand=intro,ticket,fav,typeName,tags,sysTags,pointCount,signLevel";//小说详情接口

        private static string JPNovelsUrl = "https://api.sfacg.com/novels?expand=tags,typeName&fields=novelId,novelName,novelCover,typeId,authorName,signStatus,point&filter=latest&ntype=jp&page=#pageNo#&size=#pageSize#";//日轻小说接口

        private static string QuerySuggestionUrl = "https://api.sfacg.com/search/all/reffers?q=#text#";//搜索提示

        private static string QueryNovelsUrl = "https://api.sfacg.com/search/all/result?expand=typeName,tags,intro,latestchaptitle,latestchapintro,authorname,authorName&q=#keyword#";//搜索小说

        //private static string CategoryNovelsUrl = "https://api.sfacg.com/APP/API/HTML5.ashx?op=typenovels&index=#index#&listype=#listType#&tid=#tid#";//分类小说

        private static string CategoryNovelsUrl = "https://api.sfacg.com/novels/#tid#/sysTags/0/novels?expand=typeName&filter=#filter#&page=#pageNo#&size=#pageSize#&sort=Lastest";//分类小说,sort有四种枚举，后面再做

        

        /// <summary>
        /// 获取小说章节内容数据
        /// </summary>
        /// <param name="novelId"></param>
        /// <param name="chapId"></param>
        /// <returns></returns>
        public static async Task<string> getNovel(string novelId, string chapId)
        {
            if (string.IsNullOrEmpty(novelId) || string.IsNullOrEmpty(chapId))
            {
                return "";
            }

            StorageFolder novelFolder = await folder.CreateFolderAsync(novelId, CreationCollisionOption.OpenIfExists);

            StorageFile novel = null;

            try
            {
                novel = await novelFolder.GetFileAsync(chapId);
            }
            catch (Exception)
            {
            }


            if (novel == null)
            {
                //调用接口获取文本
                var result = await HttpUtil.get(NovelContentAccessurl.Replace("#chapId#", chapId));
                NovelJSON data = JSONUtil.deSerialize<NovelJSON>(result);

                //保存内容到文件
                StorageFile file = await novelFolder.CreateFileAsync(data.data.chapId, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, data.data.expand.content, Windows.Storage.Streams.UnicodeEncoding.Utf8);

                //返回内容
                return data.data.expand.content;
            }
            else
            {
                return await FileIO.ReadTextAsync(novel, Windows.Storage.Streams.UnicodeEncoding.Utf8);
            }
        }



        /// <summary> 
        /// 获取推荐小说
        /// </summary>
        /// <param name="size">获取的数量</param>
        /// <returns></returns>
        public static async Task<List<Novel>> getPushNovels(int size)
        {
            //调用接口
            var result = await HttpUtil.get(PushNovelsUrl.Replace("#size#", size.ToString()));
            PushNovelApiVO response = JSONUtil.deSerialize<PushNovelApiVO>(result);
            var novels = new List<Novel>();
            if (response != null)
            {
                novels = response.getNovel();
            }
            return novels;
        }

        /// <summary>
        /// 获取日轻小说接口
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="listType"></param>
        /// <returns></returns>
        public static async Task<List<Novel>> getJPNovels(int pageNo, int pageSize, ListType listType)
        {
            var result = await HttpUtil.get(JPNovelsUrl.Replace("#pageNo#", pageNo + "").Replace("#pageSize#", pageSize+""));
            JPNovelApiVO response = JSONUtil.deSerialize<JPNovelApiVO>(result);

            var novels = new List<Novel>();
            if (response != null)
            {
                novels = response.getNovel();
            }

            return novels;
        }

        /// <summary>
        /// 获取目录列表
        /// </summary>
        /// <param name="novelId"></param>
        /// <returns></returns>
        public static async Task<List<Volume>> getNovelCatalog(string novelId)
        {
            //调用接口获取文本
            var result = await HttpUtil.get(NovelCatalogUrl.Replace("#novelId#", novelId));
            var response = JSONUtil.deSerialize<NovelCatalogApiVO>(result);

            var volumes = new List<Volume>();
            if (response != null)
            {
                volumes = response.getVolumes();
            }
            return volumes;
        }

        /// <summary>
        /// 获取小说详情
        /// </summary>
        /// <param name="novelId"></param>
        /// <returns></returns>
        public static async Task<Novel> getNovelDetail(string novelId)
        {
            var result = await HttpUtil.get(NovelDetailUrl.Replace("#novelId#", novelId));
            var response = JSONUtil.deSerialize<NovelDetailApiVO>(result);



            var novel = new Novel();
            if (response != null)
            {
                novel = response.getNovel();
            }
            

            return novel;
        }

        /// <summary>
        /// 获取搜索提示
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static async Task<List<string>> getSugges(string text)
        {
            var result = await HttpUtil.get(QuerySuggestionUrl.Replace("#text#", text));
            var response = JSONUtil.deSerialize<SearchSuggessApiVO>(result);
            return response.data;
        }

        /// <summary>
        /// 搜索小说
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static async Task<List<Novel>> queryNovels(string keyword)
        {
            var result = await HttpUtil.get(QueryNovelsUrl.Replace("#keyword#", keyword));
            var response = JSONUtil.deSerialize<SearchNovelsApiVO>(result);
            var novels = new List<Novel>();
            if (response != null)
            {
                novels = response.getNovel();
            }
            return novels;
        }


        public static async Task<List<Novel>> queryCategoryNovels(int tid,int pageNo,int pageSize, FilterType filterType)
        {

            var result = await HttpUtil.get(CategoryNovelsUrl.Replace("#tid#", tid.ToString()).Replace("#pageNo#", pageNo.ToString())
                                                .Replace("#pageSize#", pageSize.ToString()).Replace("#filter#", filterType.ToString()));

            var response = JSONUtil.deSerialize<CategoryNovelApiVO>(result);
            var novels = new List<Novel>();
            if (response != null)
            {
                novels = response.getNovel();
            }
            return novels;
        }



    }
}
