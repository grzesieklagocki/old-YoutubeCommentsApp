using System;
using System.Collections.Generic;

namespace YouTubeComments
{
    public class PageInfo
    {
        public int totalResults;
        public int resultsPerPage;
    }

    public class Comment
    {
        public string id;
        public string authorDisplayName;
        public string authorProfileImageUrl;
        public string authorChannelUrl;
        public string textDisplay;
        public int likeCount;
        public DateTime publishedAt;
    }

    public class TopLevelComment
    {
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
        public PageInfo pageInfo;
        public List<Item> items;
    }
}
