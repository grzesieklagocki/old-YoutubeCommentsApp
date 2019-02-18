using System;
using System.Collections.Generic;

namespace YouTubeComments.Model.Api
{
    public class CommentReply
    {
        public Comment snippet;
    }

    public class CommentListResponse
    {
        public string nextPageToken;
        public List<CommentReply> items;
    }
}
