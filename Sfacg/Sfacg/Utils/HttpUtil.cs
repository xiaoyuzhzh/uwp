using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace Sfacg.Utils
{
    class HttpUtil
    {

        /// <summary>get获取数据，（请求的header目前写死，后期封装header的对象）
        /// 
        /// </summary>
        /// <param name="url">连接</param>
        /// <returns>返回的请求体</returns>
        public static async Task<string> get(string url)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic YW5kcm9pZHVzZXI6MWEjJDUxLXl0Njk7KkFjdkBxeHE=");
            System.Uri uri = new Uri(url);
            HttpResponseMessage x = await httpClient.GetAsync(uri);
            var result = await x.Content.ReadAsStringAsync();
            return result;
        }
    }
}
