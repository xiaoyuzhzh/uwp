using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LogCenterReader.Util.httpUtil
{
    public class CHttpClient
    {
        public async static Task<String> get(String url) {
            return null;
        }

        public async static Task<String> post(String url,String content,MediaTypeHeaderValue contentType,params MediaTypeWithQualityHeaderValue[] accepts)
        {
            var client = new HttpClient();

            if (accepts != null&&accepts.Length>0)
            {
                foreach (var accept in accepts)
                {
                    client.DefaultRequestHeaders.Accept.Add(accept);
                }
            }

            var httpContent = new ByteArrayContent(Encoding.GetEncoding("utf-8").GetBytes(content));
            httpContent.Headers.ContentType = contentType;

            var response = await client.PostAsync(url, httpContent);

            return await response.Content.ReadAsStringAsync();
        }



        public async static Task<String> postJson(String url, String content)
        {
            return await post(url, content, new MediaTypeHeaderValue("application/json"), new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
