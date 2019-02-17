using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using PropertyChanged;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace YouTubeComments.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class MainViewModel : ViewModelBase
    {
        public string Response { get; private set; }


        private readonly string ytApiUrl = "https://www.googleapis.com/youtube/v3";
        private readonly string key = "AIzaSyAqJ9rdOYJEkqcKsZRg5ANYBY3m2vDZCgg";

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            //var request = WebRequest.Create("https://www.googleapis.com/youtube/v3/commentThreads?key=AIzaSyAqJ9rdOYJEkqcKsZRg5ANYBY3m2vDZCgg&textFormat=plainText&order=time&part=snippet&videoId=MK6TXMsvgQg&maxResults=100");

            var parameters = new Dictionary<string, string>
            {
                { "videoId", "MK6TXMsvgQg" },
                { "order", "time" },
                { "part", "snippet" },
                { "textFormat", "plainText" },
                { "maxResults", "100" }
            };

            var request = WebRequest.Create(CreateURL("commentThreads", parameters));
            var responseStream = request.GetResponse().GetResponseStream();

            using (var streamReader = new StreamReader(responseStream))
            {
                Response = streamReader.ReadToEnd();
            }

            var obj = JsonConvert.DeserializeObject<RootObject>(Response);
        }

        private string CreateURL(string path, params Dictionary<string, string>[] parameters)
        {

            string url = $"{ytApiUrl}/{path}?key={key}";

            foreach (Dictionary<string, string> dictionary in parameters)
            {
                foreach (KeyValuePair<string, string> item in dictionary)
                {
                    url += $"&{item.Key}={item.Value}";
                }
            }

            return url;
        }

        private string GetVideoIDFromUrl(string url)
        {
            return Regex.Match(url, @"v=\w*").Value.Substring(2);
        }
    }
}