using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace YouTubeComments.Model.Api
{
    public class ApiService
    {
        public int RequestDelay { get; set; }

        public int QuotaUsage { get; private set; }

        private readonly string ytApiUrl = "https://www.googleapis.com/youtube/v3";
        private readonly string key;


        private readonly Dictionary<string, string> videosParameters = new Dictionary<string, string>
        {
            { "part", "statistics,snippet" },
        };


        public ApiService(string key, int requestDelay = 50)
        {
            this.key = key;
            RequestDelay = requestDelay;
        }

        public List<Snippet> GetAllComments(string videoId, CommentListParameters parameters, Action<int, int> progressChanged = null)
        {
            CommentThreadListResponse response = GetComments(videoId, parameters);
            List<Snippet> comments = new List<Snippet>(response.Items.Select(i => i.Snippet));

            string token = response.NextPageToken;
            int doneTopLevels = response.Items.Count;
            int doneReplies = GetRepliesCount(response);
            progressChanged?.Invoke(doneTopLevels, doneReplies);

            Thread.Sleep(RequestDelay);

            while (!string.IsNullOrWhiteSpace(token))
            {
                response = GetComments(videoId, parameters);
                comments.AddRange(response.Items.Select(i => i.Snippet));
                token = response.NextPageToken;

                if (progressChanged != null)
                {
                    doneTopLevels += response.Items.Count;
                    doneReplies += GetRepliesCount(response);

                    progressChanged.Invoke(doneTopLevels, doneReplies);
                }

                Thread.Sleep(RequestDelay);
            }

            return comments;
        }

        public CommentThreadListResponse GetComments(string videoId, CommentListParameters parameters)
        {
            var id = new Dictionary<string, string>
            {
                { "videoId", videoId },
            };

            QuotaUsage += parameters.QuotaCost;

            return ApiRequest<CommentThreadListResponse>("commentThreads", id, parameters.ToDictionary());
        }

        //public CommentListResponse GetCommentReplies(string videoId, string commentId, string nextPageToken = null)
        //{
        //    var parameters = new Dictionary<string, string>
        //    {
        //        { "videoId", videoId },
        //        { "parentId", commentId }
        //    };

        //    if (nextPageToken != null)
        //    {
        //        parameters.Add("pageToken", nextPageToken);
        //    }

        //    return ApiRequest<CommentListResponse>("comments", parameters, commentsParameters);
        //}

        //public List<Comment> GetAllCommentReplies(string videoId, string commentId)
        //{
        //    CommentListResponse response = GetCommentReplies(videoId, commentId);
        //    List<Comment> replies = new List<Comment>(response.Items.Select(i => i.snippet));

        //    string token = response.NextPageToken;

        //    while (!string.IsNullOrWhiteSpace(token))
        //    {
        //        response = GetCommentReplies(videoId, commentId, token);
        //        replies.AddRange(response.Items.Select(i => i.snippet));
        //        token = response.NextPageToken;

        //        Thread.Sleep(RequestDelay);
        //    }

        //    return replies;
        //}

        public RootObject GetVideoInfo(string videoId)
        {
            return ApiRequest<RootObject>("videos", videosParameters, new Dictionary<string, string> {{ "id", videoId }});
        }

        #region Helpers

        private int GetRepliesCount(CommentThreadListResponse response)
        {
            int replyCount = 0;

            foreach (Item item in response.Items)
            {
                replyCount += item.Snippet.TotalReplyCount;
            }

            return replyCount;
        }

        private T ApiRequest<T>(string path, params Dictionary<string, string>[] parameters)
        {
            string url = CreateURL(path, parameters);
            var request = (HttpWebRequest)WebRequest.Create(url);
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

            return JsonConvert.DeserializeObject<T>(responseString, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
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

        #endregion
    }
}
