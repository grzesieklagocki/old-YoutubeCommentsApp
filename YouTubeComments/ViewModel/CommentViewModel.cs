using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using YouTube;
using System.Collections.ObjectModel;

namespace YouTubeComments.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class CommentViewModel : ViewModelBase
    {
        public string ID { get; private set; }
        public string Text { get; private set; }
        public string AuthorChannelUrl { get; private set; }
        public string AuthorDisplayName { get; private set; }
        public string AuthorProfileImageUrl { get; private set; }
        public int LikeCount { get; private set; }
        public int ReplyCount { get; private set; }
        public DateTime PublishedAt { get; private set; }

        public bool CanLoadReplies { get => ReplyCount > 0; }
        public bool IsReply { get => ID == null; }

        public ObservableCollection<Comment> Replies { get; private set; }



        public CommentViewModel(Comment comment, string id = null, int replyCount = 0)
        {
            ID = id;
            Text = comment.TextDisplay;
            AuthorChannelUrl = comment.AuthorChannelUrl;
            AuthorDisplayName = comment.AuthorDisplayName;
            AuthorProfileImageUrl = comment.AuthorProfileImageUrl;
            LikeCount = comment.LikeCount;
            ReplyCount = replyCount;
            PublishedAt = comment.PublishedAt;
        }
    }
}
