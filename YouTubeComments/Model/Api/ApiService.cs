using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace YouTubeComments.Model.Api
{
    public class ApiService
    {
        public int RequestDelay { get; set; }

        private readonly string ytApiUrl = "https://www.googleapis.com/youtube/v3";
        private readonly string key;

        private readonly Dictionary<string, string> commentsParameters = new Dictionary<string, string>
        {
            { "order", "time" },
            { "part", "snippet" },
            { "textFormat", "plainText" },
            { "maxResults", "100" }
        };


        public ApiService(string key, int requestDelay = 50)
        {
            this.key = key;
            RequestDelay = requestDelay;
        }


        public string GetVideoIDFromUrl(string url)
        {
            return Regex.Match(url, @"v=\w*").Value.Substring(2);
        }

        public bool ValidateVideoId(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || id.Length != 11 || id.Any(c => !char.IsLetterOrDigit(c)))
                return false;

            return true;
        }

        public List<Snippet> GetAllComments(string videoId, Action<int, int> progressChanged = null)
        {
            CommentThreadListResponse response = GetComments(videoId);
            List<Snippet> comments = new List<Snippet>(response.items.Select(i => i.snippet));

            string token = response.nextPageToken;
            int doneTopLevels = response.pageInfo.totalResults;
            int doneReplies = GetRepliesCount(response);
            progressChanged?.Invoke(doneTopLevels, doneReplies);

            Thread.Sleep(RequestDelay);

            while (!string.IsNullOrWhiteSpace(token))
            {
                response = GetComments(videoId, token);
                comments.AddRange(response.items.Select(i => i.snippet));
                token = response.nextPageToken;

                if (progressChanged != null)
                {
                    doneTopLevels += response.pageInfo.totalResults;
                    doneReplies += GetRepliesCount(response);

                    progressChanged.Invoke(doneTopLevels, doneReplies);
                }

                Thread.Sleep(RequestDelay);
            }

            return comments;
        }

        public CommentThreadListResponse GetComments(string videoId, string nextPageToken = null)
        {
            var parameters = new Dictionary<string, string>
            {
                { "videoId", videoId },
            };

            if (nextPageToken != null)
            {
                parameters.Add("pageToken", nextPageToken);
            }

            return ApiRequest<CommentThreadListResponse>("commentThreads", parameters, commentsParameters);
        }

        public CommentListResponse GetCommentReplies(string videoId, string commentId, string nextPageToken = null)
        {
            var parameters = new Dictionary<string, string>
            {
                { "videoId", videoId },
                { "parentId", commentId }
            };

            if (nextPageToken != null)
            {
                parameters.Add("pageToken", nextPageToken);
            }

            return ApiRequest<CommentListResponse>("comments", parameters, commentsParameters);
        }

        public List<CommentReply> GetAllCommentReplies(string videoId, string commentId)
        {
            CommentListResponse response = GetCommentReplies(videoId, commentId);
            List<CommentReply> replies = new List<CommentReply>(response.items);

            string token = response.nextPageToken;

            while (!string.IsNullOrWhiteSpace(token))
            {
                response = GetCommentReplies(videoId, commentId, token);
                replies.AddRange(response.items);
                token = response.nextPageToken;

                Thread.Sleep(RequestDelay);
            }

            return replies;
        }

        #region Helpers

        private int GetRepliesCount(CommentThreadListResponse response)
        {
            int replyCount = 0;

            foreach (Item item in response.items)
            {
                replyCount += item.snippet.totalReplyCount;
            }

            return replyCount;
        }

        private T ApiRequest<T>(string path, params Dictionary<string, string>[] parameters)
        {
            var request = WebRequest.Create(CreateURL(path, parameters)) as HttpWebRequest;
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
