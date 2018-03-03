using System;
using System.Collections.Generic;
using System.Linq;
using TweetSharp;
using System.Web;
using System.Threading.Tasks;
using Foundation.SocialServices.Models;

namespace Foundation.SocialServices.Services
{
    public class TwitterFeedService
    {
        private readonly string consumerKey;
        private readonly string consumerSecret;
        private readonly string accessToken;
        private readonly string accessTokenSecret;

        public TwitterFeedService(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
        {
            this.consumerKey = consumerKey;
            this.consumerSecret = consumerSecret;
            this.accessToken = accessToken;
            this.accessTokenSecret = accessTokenSecret;
            this.ThirdPartyServicesAPITimeout = TimeSpan.FromSeconds(60);
        }

        public TimeSpan ThirdPartyServicesAPITimeout { get; private set; }

        public IEnumerable<SocialPost> GetTwitterStatuses(string screenName, int count)
        {
            var items = Enumerable.Empty<TwitterStatus>();

            try
            {
                var service = new TwitterService(consumerKey,consumerSecret);

                service.AuthenticateWith(accessToken,accessTokenSecret);

                //do not use ListTweetsOnUserTimelineAsync
                var task = new Task<IEnumerable<TwitterStatus>>(() =>
                {
                    return service.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions
                    {
                        ScreenName = screenName,
                        Count = count
                    });
                });

                task.Start(); //execute task in current task scheduler
                var isFinished = task.Wait(ThirdPartyServicesAPITimeout);

                if (isFinished && task.IsCompleted)
                {
                    items = task.Result;
                }
            }
            catch (Exception ex)
            {
                //EventLogProvider.LogEvent(EventType.ERROR, "SocialService", "GetTwitterStatuses",
                //    EventLogProvider.GetExceptionLogMessage(ex));
            }

            var posts = new List<SocialPost>();
            foreach (var item in items)
            {
                var post = new SocialPost
                {
                    Author = item.User.ScreenName,
                    Message = item.Text,
                    Url = item.ToTwitterUrl().ToString(),
                    ImageUrl = GetTweetImage(item.Entities.Media),
                    Date = item.CreatedDate ,
                    Type = SocialTypes.Twitter,
                };
                posts.Add(post);
            }

            return posts;
        }

        private string GetTweetImage(IList<TwitterMedia> media)
        {
            var image = media.FirstOrDefault(c => c.MediaType == TwitterMediaType.Photo);

            return image != null ? image.MediaUrlHttps : "";
        }
    }
}