using System;
using System.Collections.Generic;

namespace YouTubeComments
{
    public class CommentReply
    {
        public Author snippet;
        public string textDisplay;
        public int likeCount;
        public DateTime publishedAt;
    }

    public class Author
    {
        public string authorDisplayName;
        public string authorProfileImageUrl;
        public string authorChannelUrl;
    }

    public class CommentListResponse
    {
        public string nextPageToken;
        public PageInfo pageInfo;
        public List<CommentReply> items;
    }
}
