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

            StorageFile novel = await novelFolder.GetFileAsync(chapId);

            if(novel == null)
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
    }
}
