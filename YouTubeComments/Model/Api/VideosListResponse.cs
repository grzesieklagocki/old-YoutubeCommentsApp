using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouTubeComments.Model.Api
{
    public class Thumbnail
    {
        public string url;
        public int width;
        public int height;
    }

    public class Thumbnails
    {
        public Thumbnail @default;
        public Thumbnail medium;
        public Thumbnail high;
        public Thumbnail standard;
        public Thumbnail maxres;
    }

    public class VideoSnippet
    {
        public DateTime publishedAt;
        public string channelId;
        public string title;
        public string description;
        public Thumbnails thumbnails;
        public string channelTitle;
        public List<string> tags;
    }

    public class ContentDetails
    {
        public string duration;
        public string dimension;
        public string definition;
        public string caption;
    }

    public class Statistics
    {
        public string viewCount;
        public string likeCount;
        public string dislikeCount;
        public string favoriteCount;
        public string commentCount;
    }

    public class Video
    {
        public string id;
        public VideoSnippet snippet;
        public ContentDetails contentDetails;
        public Statistics statistics;
    }

    public class RootObject
    {
        public List<Video> items;
    }
}
