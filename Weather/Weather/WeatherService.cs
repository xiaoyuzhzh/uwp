using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Weather
{
    class WeatherService
    {

        public async Task<WeatherInfo> getWeather(String cityName)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("apikey", "ccbc700e317f71a13aa2700bc91dd594");
            var url = String.Format("http://apis.baidu.com/apistore/weatherservice/cityname?cityname={0}", cityName);
            var response = await client.GetAsync(url);

            var result = await response.Content.ReadAsStringAsync();

            var weatherInfo = new WeatherInfo();
            weatherInfo.jsonInfo = result;
            return weatherInfo;
        }

    }

    class WeatherInfo
    {
        public String jsonInfo { get; set; }
    }
}
