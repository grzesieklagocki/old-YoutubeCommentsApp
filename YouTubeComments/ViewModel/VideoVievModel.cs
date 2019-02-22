using GalaSoft.MvvmLight;
using PropertyChanged;
using YouTubeComments.Model.Api;

namespace YouTubeComments.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class VideoVievModel : ViewModelBase
    {
        public string Title { get; private set; }
        public string ChannelTitle { get; private set; }
        public string ChannelID { get; private set; }
        public long ViewCount { get; private set; }
        public long CommentCount { get; private set; }
        public long LikeCount { get; private set; }
        public long DislikeCount { get; private set; }
        public string ThumbnailUrl { get; private set; }

        public double LikePercent { get => LikeCount / (LikeCount + DislikeCount) * 100; }

        public VideoVievModel(Video video)
        {
            Title = video.snippet.title;
            ChannelTitle = video.snippet.channelTitle;
            ChannelID = video.snippet.channelId;
            ViewCount = video.statistics.viewCount;
            LikeCount = video.statistics.likeCount;
            DislikeCount = video.statistics.dislikeCount;
            ThumbnailUrl = video.snippet.thumbnails.medium.url;
            CommentCount = video.statistics.commentCount;
        }
    }
}
