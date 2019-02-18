using System;
using System.Collections.Generic;

namespace YouTubeComments.Model.Api
{
    public class Comment
    {
        public string authorDisplayName;
        public string authorProfileImageUrl;
        public string authorChannelUrl;
        public string textDisplay;
        public int likeCount;
        public DateTime publishedAt;
    }

    public class TopLevelComment
    {
        public string id;
        public Comment snippet;
    }

    public class Snippet
    {
        public TopLevelComment topLevelComment;
        public int totalReplyCount;
    }

    public class Item
    {
        public Snippet snippet;
    }

    public class CommentThreadListResponse
    {
        public string nextPageToken;
        public List<Item> items;
    }
}
