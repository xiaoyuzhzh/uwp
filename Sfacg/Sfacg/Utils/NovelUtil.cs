using Sfacg.Model;
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
    class NovelUtil
    {
        private static StorageFolder folder = ApplicationData.Current.LocalFolder;

        private static string NovelAccessUrl = "https://api.sfacg.com/Chaps/#chapId#?expand=content,needFireMoney";

        private static string PushNovelsUrl = "https://api.sfacg.com/novels?size=#size#&filter=newpush";

        private static string NovelCatalogUrl = "https://api.sfacg.com/novels/#novelId#/dirs";

        private static string JPNovelsUrl = "https://api.sfacg.com/APP/API/HTML5.ashx?op=jpnovels&index=#index#&listype=#listType# ";

        private static string NovelDetailUrl = "https://api.sfacg.com/APP/API/HTML5.ashx?op=noveldetailnew&nid=#novelId#";

        private static string QuerySuggestionUrl = "https://api.sfacg.com/search/novels/reffers?q=#text#";

        private static string QueryNovelsUrl = "https://api.sfacg.com/APP/API/HTML5.ashx?op=search&keyword=#keyword#";

        /**
         *  获取小说章节数据
         */
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
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic YW5kcm9pZHVzZXI6MWEjJDUxLXl0Njk7KkFjdkBxeHE=");
                System.Uri uri = new Uri(NovelAccessUrl.Replace("#chapId#",chapId));
                HttpResponseMessage x = await httpClient.GetAsync(uri);
                var result = await x.Content.ReadAsStringAsync();

                var serializer = new DataContractJsonSerializer(typeof(NovelJSON));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));

                NovelJSON data = (NovelJSON)serializer.ReadObject(ms);


                StorageFile file = await novelFolder.CreateFileAsync(data.data.chapId, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, data.data.expand.content, Windows.Storage.Streams.UnicodeEncoding.Utf8);

                return data.data.expand.content;
            }else
            {
                return await FileIO.ReadTextAsync(novel, Windows.Storage.Streams.UnicodeEncoding.Utf8);
            }
        }

        public static async Task<List<NovelsVOData>> getPushNovels(int size)
        {
            //调用接口获取文本
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic YW5kcm9pZHVzZXI6MWEjJDUxLXl0Njk7KkFjdkBxeHE=");
            System.Uri uri = new Uri(PushNovelsUrl.Replace("#size#", size.ToString()));
            HttpResponseMessage x = await httpClient.GetAsync(uri);
            var result = await x.Content.ReadAsStringAsync();

            var serializer = new DataContractJsonSerializer(typeof(NovelsVO));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));

            NovelsVO data = (NovelsVO)serializer.ReadObject(ms);


            return data.data;
        }

        public static async Task<List<JPNovelData>> getJPNovels(int index, ListType listType)
        {
            //调用接口获取文本
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic YW5kcm9pZHVzZXI6MWEjJDUxLXl0Njk7KkFjdkBxeHE=");
            System.Uri uri = new Uri(JPNovelsUrl.Replace("#index#", index.ToString())
                                                .Replace("#listType#", listType.ToString()));
            HttpResponseMessage x = await httpClient.GetAsync(uri);
            var result = await x.Content.ReadAsStringAsync();
            result = "{\"novels\":" + result + "}";

            var serializer = new DataContractJsonSerializer(typeof(JPNovelVO));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));

            JPNovelVO response = (JPNovelVO)serializer.ReadObject(ms);
            List<JPNovelData> list = response.novels;

            list.ForEach(n => {
                n.NovelCover = "http://rs.sfacg.com/web/novel/images/NovelCover/Big/" + n.NovelCover;
            });
            return list;
        }

        

        public static async Task<List<VolumeList>> getNovelCatalog(string novelId)
        {
            //调用接口获取文本
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic YW5kcm9pZHVzZXI6MWEjJDUxLXl0Njk7KkFjdkBxeHE=");
            System.Uri uri = new Uri(NovelCatalogUrl.Replace("#novelId#", novelId));
            HttpResponseMessage x = await httpClient.GetAsync(uri);
            var result = await x.Content.ReadAsStringAsync();

            var serializer = new DataContractJsonSerializer(typeof(NovelCatalogVO));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));

            NovelCatalogVO response = (NovelCatalogVO)serializer.ReadObject(ms);

            return response.data.volumeList;

        }

        public static async Task<NovelDetailVO> getNovelDetail(string novelId)
        {
            //调用接口获取文本
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic YW5kcm9pZHVzZXI6MWEjJDUxLXl0Njk7KkFjdkBxeHE=");
            System.Uri uri = new Uri(NovelDetailUrl.Replace("#novelId#", novelId));
            HttpResponseMessage x = await httpClient.GetAsync(uri);
            var result = await x.Content.ReadAsStringAsync();

            var serializer = new DataContractJsonSerializer(typeof(NovelDetailVO));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));

            NovelDetailVO response = (NovelDetailVO)serializer.ReadObject(ms);

            return response;

        }

        public static async Task<List<string>> getSugges(string text)
        {
            //调用接口获取文本
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic YW5kcm9pZHVzZXI6MWEjJDUxLXl0Njk7KkFjdkBxeHE=");
            System.Uri uri = new Uri(QuerySuggestionUrl.Replace("#text#", text));
            HttpResponseMessage x = await httpClient.GetAsync(uri);
            var result = await x.Content.ReadAsStringAsync();

            var serializer = new DataContractJsonSerializer(typeof(SuggestionVO));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));

            SuggestionVO response = (SuggestionVO)serializer.ReadObject(ms);

            return response.data;
        }

        public static async Task<List<Novels>> queryNovels(string keyword)
        {
            //调用接口获取文本
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic YW5kcm9pZHVzZXI6MWEjJDUxLXl0Njk7KkFjdkBxeHE=");
            System.Uri uri = new Uri(QueryNovelsUrl.Replace("#keyword#", keyword));
            HttpResponseMessage x = await httpClient.GetAsync(uri);
            var result = await x.Content.ReadAsStringAsync();

            var serializer = new DataContractJsonSerializer(typeof(QueryNovelsVo));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));

            QueryNovelsVo response = (QueryNovelsVo)serializer.ReadObject(ms);

            response.Novels.ForEach(n => {
                n.NovelCover = "http://rs.sfacg.com/web/novel/images/NovelCover/Big/" + n.NovelCover;
            });
            return response.Novels;
        }
    }
}
