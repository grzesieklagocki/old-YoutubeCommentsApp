using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using YouTube;

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

        public string VideoID { get; private set; } = "EyWKea6IyuA";

        private string videoUrl;
        public string VideoURL
        {
            get => videoUrl;
            set
            {
                try
                {
                    string id = YouTubeHelper.GetVideoIDFromUrl(value);

                    videoUrl = value;
                    VideoID = id;
                    IsVideoURLValid = YouTubeHelper.ValidateVideoId(id);
                }
                catch
                {
                    IsVideoURLValid = false;
                }
            }
        }

        public bool IsVideoURLValid { get; private set; }

        private RelayCommand openChannelPage;
        public RelayCommand OpenChannelPage { get => openChannelPage ?? (openChannelPage = new RelayCommand(() => Process.Start(YouTubeHelper.GetChannelUrl(VideoVievModel.ChannelID)))); }

        public VideoVievModel VideoVievModel { get; private set; }
        public ObservableCollection<CommentViewModel> Comments { get; private set; }

        private readonly string key = "AIzaSyAqJ9rdOYJEkqcKsZRg5ANYBY3m2vDZCgg";
        private readonly YouTube.ApiClient api;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            //if (IsInDesignMode)
            //{
            //    // Code runs in Blend --> create design time data.
            //    var videoResponse = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(@"/text.html"));
            //    VideoVievModel = new VideoVievModel(videoResponse.items[0]);
            //}
            //else
            //{
            //    // Code runs "for real"
            //    api = new ApiService(key);
            //    VideoVievModel = new VideoVievModel(api.GetVideoInfo(VideoID).items[0]);
            //}

            //api = new ApiClient(key);
            //VideoVievModel = new VideoVievModel(api.GetVideosById(VideoID).Items[0]);
            //var comments = api.GetAllComments(VideoID, new CommentThreadsParameters() { MaxResults = 100, Parts = CommentThreadsParameters.CommentPartsFlags.snippet, Format = CommentThreadsParameters.TextFormat.plainText });
            //api.get
            //var request = WebRequest.Create("https://www.googleapis.com/youtube/v3/commentThreads?key=AIzaSyAqJ9rdOYJEkqcKsZRg5ANYBY3m2vDZCgg&textFormat=plainText&order=time&part=snippet&videoId=MK6TXMsvgQg&maxResults=100");

            //api = new ApiService(key);

            //var comments = api.GetAllComments(VideoID, (a, b) => System.Diagnostics.Debug.WriteLine($"wszystkich: {a}, odpowiedzi: {b}"));

            //foreach (var comment in comments)
            //{
            //    Response += ">> " + comment.topLevelComment.snippet.textDisplay + Environment.NewLine;
            //}

            //var first = comments.First(c => c.totalReplyCount > 2);

            //string commentWithRepliesId = first.topLevelComment.id;
            ////string commentWithRepliesId = "UgipDzYkt3T003gCoAEC";

            //var replies = api.GetAllCommentReplies(VideoID, commentWithRepliesId);

            //foreach (var reply in replies)
            //{
            //    Response += ">>> " + reply.textDisplay + Environment.NewLine;
            //}

            //VideoVievModel = new VideoVievModel(api.GetVideoInfo(VideoID).items[0]);
        }
    }
}