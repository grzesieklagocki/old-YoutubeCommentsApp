using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YouTubeComments.Model.Api
{
    public class CommentListParameters
    {
        public enum ModerationStatus
        {
            /// <summary>
            /// Retrieve threads of published comments.
            /// This is the default value.
            /// A comment thread can be included in the response if its top-level comment has been published.
            /// </summary>
            published,
            /// <summary>
            /// Retrieve comment threads that are awaiting review by a moderator.
            /// A comment thread can be included in the response if the top-level comment or at least one of the replies to that comment are awaiting review.
            /// </summary>
            heldForReview,
            /// <summary>
            /// Retrieve comment threads classified as likely to be spam.
            /// A comment thread can be included in the response if the top-level comment or at least one of the replies to that comment is considered likely to be spam.
            /// </summary>
            likelySpam
        }

        public enum Order
        {
            /// <summary>
            ///  Comment threads are ordered by time.
            ///  This is the default behavior.
            /// </summary>
            time,
            /// <summary>
            /// Comment threads are ordered by relevance.
            /// </summary>
            relevance
        }

        public enum TextFormat
        {
            /// <summary>
            /// Returns the comments in HTML format. This is the default value.
            /// </summary>
            html,
            /// <summary>
            /// Returns the comments in plain text format.
            /// </summary>
            plainText
        }

        [Flags]
        public enum CommentPartsFlags
        {
            /// <summary>
            /// Quota cost: 2
            /// </summary>
            snippet = 0b00000001,
            /// <summary>
            /// Quota cost: 2
            /// </summary>
            replies = 0b00000010
        }


        /// <summary>
        /// The maxResults parameter specifies the maximum number of items that should be returned in the result set.
        /// Acceptable values are 1 to 100, inclusive.
        /// The default value is 20.
        /// </summary>
        public int MaxResults { get => parameters.maxResults; set => parameters.maxResults = value; }

        /// <summary>
        /// This parameter can only be used in a properly authorized request.
        /// Set this parameter to limit the returned comment threads to a particular moderation state.
        /// The default value is published.
        /// </summary>
        public ModerationStatus Status { get => parameters.moderationStatus; set => parameters.moderationStatus = value; }

        /// <summary>
        /// The order parameter specifies the order in which the API response should list comment threads.
        /// </summary>
        public Order OrderBy { get => parameters.order; set => parameters.order = value; }

        /// <summary>
        /// The pageToken parameter identifies a specific page in the result set that should be returned. In an API response, the nextPageToken property identifies the next page of the result that can be retrieved.
        /// </summary>
        public string PageToken { get => parameters.pageToken; set => parameters.pageToken = value; }

        /// <summary>
        /// The searchTerms parameter instructs the API to limit the API response to only contain comments that contain the specified search terms.
        /// </summary>
        public string SearchTerms { get => parameters.searchTerms; set => parameters.searchTerms = value; }

        /// <summary>
        /// Set this parameter's value to html or plainText to instruct the API to return the comments left by users in html formatted or in plain text.
        /// The default value is html.
        /// </summary>
        public TextFormat Format { get => parameters.textFormat; set => parameters.textFormat = value; }

        public CommentPartsFlags Parts { get; set; }

        public int QuotaCost { get => CalculateQuotaCost(); }


        private Parameters parameters;

        /// <summary>
        /// Constructor
        /// </summary>
        public CommentListParameters()
        {
            parameters = new Parameters() { maxResults = 20 };
        }


        public Dictionary<string, string> ToDictionary()
        {
            var dictionary = new Dictionary<string, string>();
            FieldInfo[] fields = parameters.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(parameters);

                if (value == null || (field.FieldType.IsEnum && (int)value == 0))
                    continue;

                dictionary.Add(field.Name, value.ToString());
            }

            dictionary.Add("part", "id"
            + (IsFlagSet(CommentPartsFlags.snippet) ? ",snippet" : "")
            + (IsFlagSet(CommentPartsFlags.replies) ? ",replies" : ""));

            return dictionary;
        }


        private bool IsFlagSet(CommentPartsFlags flag)
        {
            return (Parts & flag) != 0;
        }

        private int CalculateQuotaCost()
        {
            int cost = 1;

            cost += IsFlagSet(CommentPartsFlags.snippet) ? 2 : 0;
            cost += IsFlagSet(CommentPartsFlags.replies) ? 2 : 0;

            return cost;
        }

        private struct Parameters
        {
            public int maxResults;
            public ModerationStatus moderationStatus;
            public Order order;
            public string pageToken;
            public string searchTerms;
            public TextFormat textFormat;
        }
    }
}
