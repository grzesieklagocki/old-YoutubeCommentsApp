using GalaSoft.MvvmLight;
using PropertyChanged;
using YouTube;

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
            Title = video.Snippet.Title;
            ChannelTitle = video.Snippet.ChannelTitle;
            ChannelID = video.Snippet.ChannelId;
            ViewCount = video.Statistics.ViewCount;
            LikeCount = video.Statistics.LikeCount;
            DislikeCount = video.Statistics.DislikeCount;
            ThumbnailUrl = video.Snippet.Thumbnails.Medium.Url;
            CommentCount = video.Statistics.CommentCount;
        }
    }
}
