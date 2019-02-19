using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public string ViewCount { get; private set; }
        public string LikeCount { get; private set; }
        public string DislikeCount { get; private set; }
        public string ThumbnailUrl { get; private set; }

        public float RatingCount
        {
            get
            {
                if (int.TryParse(LikeCount, out int likes) && int.TryParse(DislikeCount, out int dislikes))
                    return likes + dislikes;
                else return 0;
            }
        }

        public VideoVievModel(Video video)
        {
            Title = video.snippet.title;
            ChannelTitle = video.snippet.channelTitle;
            ViewCount = video.statistics.viewCount;
            LikeCount = video.statistics.likeCount;
            DislikeCount = video.statistics.dislikeCount;
            ThumbnailUrl = video.snippet.thumbnails.medium.url;
        }

    }
}
