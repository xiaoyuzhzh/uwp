using Sfacg.Model;
using Sfacg.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Sfacg
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MainPage Current;

        public MainPage()
        {
            this.InitializeComponent();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Current = this;
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://api.sfacg.com/novels?size=8&filter=newpush";
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic YW5kcm9pZHVzZXI6MWEjJDUxLXl0Njk7KkFjdkBxeHE=");
            System.Uri uri = new Uri(url);
            HttpResponseMessage x = await httpClient.GetAsync(uri);
            resultTextBlock.Text = await x.Content.ReadAsStringAsync();

        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string url = "https://api.sfacg.com/Chaps/629050?expand=content,needFireMoney";
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic YW5kcm9pZHVzZXI6MWEjJDUxLXl0Njk7KkFjdkBxeHE=");
            System.Uri uri = new Uri(url);
            HttpResponseMessage x = await httpClient.GetAsync(uri);
            var result = await x.Content.ReadAsStringAsync();

            var serializer = new DataContractJsonSerializer(typeof(NovelJSON));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));

            NovelJSON data = (NovelJSON)serializer.ReadObject(ms);

            StorageFolder folder;
            folder = ApplicationData.Current.LocalFolder;

            StorageFolder novelFolder = await folder.CreateFolderAsync(data.data.novelId, CreationCollisionOption.OpenIfExists);
            fileDir.Text = novelFolder.Path;

            StorageFile file = await novelFolder.CreateFileAsync(data.data.chapId, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, data.data.expand.content, Windows.Storage.Streams.UnicodeEncoding.Utf8);


        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            StorageFolder folder;
            folder = ApplicationData.Current.LocalFolder;

            StorageFolder novelf = await folder.GetFolderAsync("18488");

            StorageFile file = await novelf.GetFileAsync("629050");

            var str = await FileIO.ReadTextAsync(file, Windows.Storage.Streams.UnicodeEncoding.Utf8);

            //fileContent.Text = str;

            var rootFrame = (Frame)Window.Current.Content;

            ReadInfo readInfo = new ReadInfo();
            readInfo.novalId = "18488";
            readInfo.chapId = "629050";
            rootFrame.Navigate(typeof(NovelReadView), readInfo);

        }

    }
}
