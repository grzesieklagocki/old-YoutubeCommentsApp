using System;
using System.Collections.Generic;

namespace YouTubeComments.Model.Api
{
    public class Comment
    {
        public string AuthorDisplayName { get; set; }
        public string AuthorProfileImageUrl { get; set; }
        public string AuthorChannelUrl { get; set; }
        public string TextDisplay { get; set; }
        public int LikeCount { get; set; }
        public DateTime PublishedAt { get; set; }
    }

    public class TopLevelComment
    {
        public string ID { get; set; }
        public Comment Snippet { get; set; }
    }

    public class Snippet
    {
        public TopLevelComment TopLevelComment { get; set; }
        public int TotalReplyCount { get; set; }
    }

    public class Item
    {
        public Snippet Snippet { get; set; }
    }

    public class CommentThreadListResponse
    {
        public string NextPageToken { get; set; }
        public List<Item> Items { get; set; }
    }
}
