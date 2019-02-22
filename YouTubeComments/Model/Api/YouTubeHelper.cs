using System.Linq;
using System.Text.RegularExpressions;

namespace YouTubeComments.Model.Api
{
    public static class YouTubeHelper
    {
        public static string YouTubeUrl => "https://www.youtube.com";


        public static string GetVideoIDFromUrl(string url)
        {
            return Regex.Match(url, @"v=\w*").Value.Substring(2);
        }

        public static string GetVideoUrl(string videoId)
        {
            return $"{YouTubeUrl}/watch?v={videoId}";
        }

        public static string GetCommentUrl(string videoId, string commentId)
        {
            return $"{GetVideoUrl(videoId)}&lc={commentId}";
        }

        public static string GetChannelUrl(string channelId)
        {
            return $"{YouTubeUrl}/channel/{channelId}";
        }

        public static bool ValidateVideoId(string id)
        {
            return !string.IsNullOrWhiteSpace(id) && id.Length == 11 && !id.Any(c => !char.IsLetterOrDigit(c));
        }
    }
}
