using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Weather
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private LocationService locationService = new LocationService();

        private WeatherService weatherService = new WeatherService();

        public MainPage()
        {
            this.InitializeComponent();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                locationText.Text = "loading";
                var location = await locationService.getLocation();

                var locationString = location.Coordinate.Latitude + "\n" + location.Coordinate.Longitude;

                locationText.Text = locationString;

                var weatherInfo = await weatherService.getWeather("杭州");

                weatherInfoText.Text = weatherInfo.jsonInfo;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                locationText.Text = message;
                //throw;
            }


        }
    }
}
