using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            GetAllComments("LiQcVSPkT6M");
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

        private void GetAllComments(string videoId)
        {
            List<Comment> snippets = new List<Comment>();
            var parameters = new Dictionary<string, string>
            {
                { "videoId", videoId },
                { "order", "time" },
                { "part", "snippet" },
                { "textFormat", "plainText" },
                { "maxResults", "100" }
            };

            CommentThreadListResponse commentsThreads = ApiRequest<CommentThreadListResponse>("commentThreads", parameters);
            snippets.AddRange(commentsThreads.items.Select(i => i.snippet.topLevelComment.snippet));

            string token = commentsThreads.nextPageToken;

            var tokenDictionary = new Dictionary<string, string> { { "pageToken", $"{token}" } };

            while (!string.IsNullOrWhiteSpace(token))
            {
                commentsThreads = ApiRequest<CommentThreadListResponse>("commentThreads", parameters, tokenDictionary);
                snippets.AddRange(commentsThreads.items.Select(i => i.snippet.topLevelComment.snippet));
                tokenDictionary["pageToken"] = token = commentsThreads.nextPageToken;
                System.Diagnostics.Debug.WriteLine(token);
            }

            foreach (var snippet in snippets)
            {
                Response += snippet.textDisplay + Environment.NewLine;
            }
        }

        private T ApiRequest<T>(string path, params Dictionary<string, string>[] parameters)
        {
            var request = WebRequest.Create(CreateURL(path, parameters));
            string responseString;


            using (var response = request.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    using (var streamReader = new StreamReader(responseStream))
                    {
                        responseString = streamReader.ReadToEnd();
                    }
                }
            }

            return JsonConvert.DeserializeObject<T>(responseString);
        }
    }
}