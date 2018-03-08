using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Web.Http;

namespace Sfacg.Utils
{
    class HttpUtil
    {

        public static StorageFolder folder = ApplicationData.Current.LocalFolder;

        private static string ApiCacheFolder = "apicache";

        /// <summary>
        /// get获取数据，（请求的header目前写死，后期封装header的对象）
        /// </summary>
        /// <param name="url">连接</param>
        /// <returns>返回的请求体</returns>
        public static async Task<string> get(string url)
        {
            return await get(url, false);
        }

        /// <summary>
        /// get获取数据，使用etag标记缓存
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> getWithCache(string url)
        {
            return await get(url, true);
        }

        /// <summary>get获取数据，（请求的header目前写死，后期封装header的对象）
        /// 
        /// </summary>
        /// <param name="url">连接</param>
        /// <returns>返回的请求体</returns>
        private static async Task<string> get(string url,bool withCache)
        {
            
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic YW5kcm9pZHVzZXI6MWEjJDUxLXl0Njk7KkFjdkBxeHE=");
            if (withCache)
            {
                sendWithEtag(url, httpClient);
            }

            System.Uri uri = new Uri(url);
            try
            {
                HttpResponseMessage x = await httpClient.GetAsync(uri);
                if (x.StatusCode.Equals(HttpStatusCode.NotModified))//没有改动
                {
                    return await getCache(url);
                }
                var result = await x.Content.ReadAsStringAsync();
                if (withCache)
                {
                    saveEtag(url, x);
                    await cacheResult(url, result);
                }
                return result;
            }
            catch (Exception)
            {
                return await getCache(url);//网络请求失败，尝试使用缓存
            }
        }

        /// <summary>
        /// 保存返回的etag
        /// </summary>
        /// <param name="url"></param>
        /// <param name="x"></param>
        private static void saveEtag(string url, HttpResponseMessage x)
        {
            string etag = null;
            if (x.Headers.ContainsKey("ETag"))
            {
                etag = x.Headers["ETag"];
            }
            if (!string.IsNullOrEmpty(etag))
            {
                Debug.WriteLine("get with etag :" + etag);
                BaseUtil.setLocalSetting<string>(url, etag, false);
            }
        }

        /// <summary>
        /// 使用etag请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="httpClient"></param>
        private static void sendWithEtag(string url, HttpClient httpClient)
        {
            var etag = BaseUtil.getLocalSetting<string>(url, false);
            if (!string.IsNullOrEmpty(etag))
            {
                Debug.WriteLine("send with etag :" + etag);
                httpClient.DefaultRequestHeaders.Add("If-None-Match", etag);
            }
        }

        private static void deleteEtag(string url)
        {
            BaseUtil.deleteLocalSetting(url);
        }


        /// <summary>
        /// 缓存url的值
        /// </summary>
        /// <param name="url"></param>
        /// <param name="result"></param>
        private static async Task cacheResult(string url, string result)
        {
            var cacheHash = getFileName(GetMD5Code(url));

            var cacheFolder = await folder.CreateFolderAsync(ApiCacheFolder, CreationCollisionOption.OpenIfExists);

            if (cacheFolder != null)
            {
                StorageFile apiCacheFile = null;
                try
                {
                    apiCacheFile = await cacheFolder.CreateFileAsync(cacheHash, CreationCollisionOption.ReplaceExisting);
                }
                catch (Exception)
                {
                }

                if (apiCacheFile != null)
                {
                    await FileIO.WriteTextAsync(apiCacheFile, result, Windows.Storage.Streams.UnicodeEncoding.Utf8);
                }
            }
        }

        /// <summary>
        /// 获取url缓存的值
        /// </summary>
        /// <param name="url"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static async Task<string> getCache(string url)
        {
            var cacheHash = getFileName(GetMD5Code(url));

            var cacheFolder = await folder.CreateFolderAsync(ApiCacheFolder, CreationCollisionOption.OpenIfExists);

            if (cacheFolder != null)
            {
                StorageFile apiCacheFile = null;
                try
                {
                    apiCacheFile = await cacheFolder.GetFileAsync(cacheHash);
                }
                catch (Exception)
                {
                    //没有缓存的删除Etag
                    deleteEtag(url);
                }

                if (apiCacheFile != null)
                {
                    return await FileIO.ReadTextAsync(apiCacheFile, Windows.Storage.Streams.UnicodeEncoding.Utf8);
                }
            }

            return null;
        }

        public static string GetMD5Code(string msg)
        {
            CryptographicHash objHash = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5).CreateHash();
            objHash.Append(CryptographicBuffer.ConvertStringToBinary(msg, BinaryStringEncoding.Utf16BE));
            return CryptographicBuffer.EncodeToHexString(objHash.GetValueAndReset());
        }

        public static string getFileName(string sourceName)
        {
            sourceName = sourceName.Replace("/", "1")
                                    .Replace("?", "2")
                                    .Replace("、", "3")
                                    .Replace("\\", "4")
                                    .Replace("*", "5")
                                    .Replace(" ", "6")
                                    .Replace("<", "7")
                                    .Replace(">", "8")
                                    .Replace("|", "9");
            return sourceName;
        }
    }
}
