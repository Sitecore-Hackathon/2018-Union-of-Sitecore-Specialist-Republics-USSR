using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook;
using Foundation.SocialServices.Models;

namespace Foundation.SocialServices.Services
{
    public class FacebookFeedService
    {
        private readonly string _clientId;
        private readonly string _clientSecret;

        public FacebookFeedService(string ClientId, string clientSecret)
        {
            _clientId = ClientId;
            _clientSecret = clientSecret;
            this.ThirdPartyServicesAPITimeout = TimeSpan.FromSeconds(60);
        }

        public TimeSpan ThirdPartyServicesAPITimeout { get; private set; }


        public IEnumerable<SocialPost> GetPosts(string pageName, int count)
        {
            var items = new List<dynamic>();

            try
            {
                var task = new Task<object>(() =>
                {
                    var client = new FacebookClient();

                    dynamic accessToken = client.Get("oauth/access_token", new
                    {
                        client_id = _clientId,
                        client_secret = _clientSecret,
                        grant_type = "client_credentials"
                    });

                    client.AppId = _clientId;
                    client.AppSecret = _clientSecret;
                    client.AccessToken = accessToken.access_token;

                    return client.Get(pageName + "/posts", new {limit = count, fields = "id,created_time,from,message,permalink_url,picture" }); // TODO Move facebook page name to config
                });

                dynamic result = null;

                task.Start();
                var isFinished = task.Wait(ThirdPartyServicesAPITimeout);

                if (isFinished && task.IsCompleted)
                {
                    result = task.Result;
                }

                items = result != null && result.data != null ? result.data : new List<dynamic>();
            }
            catch (Exception ex)
            {
                //EventLogProvider.LogEvent(EventType.ERROR, "SocialService", "GetFacebookPosts",
                //    EventLogProvider.GetExceptionLogMessage(ex));
            }


            var posts = new List<SocialPost>();
            foreach (var item in items)
            {
                var post = new SocialPost
                {
                    Author = item.from.name,
                    Message = item.message,
                    Url = item.permalink_url,
                    ImageUrl =item.picture,
                    Date = DateTime.Parse((string)item.created_time),
                    Type = SocialTypes.Facebook,
                };
                posts.Add(post);
            }

            return posts;
        }
    }
}