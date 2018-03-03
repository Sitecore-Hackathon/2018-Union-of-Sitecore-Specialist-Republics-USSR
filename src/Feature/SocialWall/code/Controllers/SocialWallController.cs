using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Feature.SocialWall.Models;
using Foundation.SocialServices.Models;
using Foundation.SocialServices.Services;
using Sitecore.Data;
using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Presentation;
using Sitecore.Sites;

namespace Feature.SocialWall.Controllers
{
    public class SocialWallController : SitecoreController
    {
        public override ActionResult Index()
        {
            var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            var dataSource = Sitecore.Context.Database.GetItem(dataSourceId);

            var title = dataSource.Fields["Title"].Value ?? "Social wall";
            var countValue = dataSource.Fields["Count"].Value ?? "100";
            var socialServicesValue = string.IsNullOrEmpty(dataSource.Fields["Social Services"].Value) ? "" : dataSource.Fields["Social Services"].Value;
            var socialServices = socialServicesValue.Split(new [] {'|'}, StringSplitOptions.RemoveEmptyEntries);

            int count = 0;
            int.TryParse(countValue, out count);

            var posts = new List<SocialPost>();
            foreach (var socialService in socialServices)
            {
                var socialServiceItem = Sitecore.Context.Database.GetItem(socialService);
                if (socialServiceItem.TemplateID == new ID("{60660225-7FD1-4303-BBCA-DF2FDE1EC113}")) //facebook
                {
                    var clientId = socialServiceItem.Fields["Client Id"].Value;
                    var developerKey = socialServiceItem.Fields["Developer Key"].Value;
                    var pageName = socialServiceItem.Fields["Page name"].Value;

                    var facebookFeedService = new FacebookFeedService(clientId, developerKey);

                    posts.AddRange(facebookFeedService.GetPosts(pageName, count).ToList());
                }
                else if (socialServiceItem.TemplateID == new ID("{91A6C2AA-F068-4E3C-923D-C8378FDB9B38}")) //Twitter
                {
                    var consumerKey = socialServiceItem.Fields["Consumer Key"].Value;
                    var consumerSecret = socialServiceItem.Fields["Consumer Secret"].Value;
                    var accessToken = socialServiceItem.Fields["Access Token"].Value;
                    var accessTokenSecret = socialServiceItem.Fields["Access Token Secret"].Value;
                    var twitterName = socialServiceItem.Fields["Twitter Name"].Value;


                    var twitterFeedService = new TwitterFeedService(consumerKey, consumerSecret, accessToken, accessTokenSecret);

                    posts.AddRange(twitterFeedService.GetTwitterStatuses(twitterName, count).ToList());
                }
            }

            return View("~/Views/SocialWall/Index.cshtml", new SocialPostViewModel
            {
                Title = title,
                Posts = posts.OrderByDescending(x=>x.Date).Take(count).ToList()
            });
        }
    }
}