using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Feature.SocialWall.Models;
using Foundation.SocialServices.Services;

namespace Feature.SocialWall.Controllers
{
    public class SocialWallController : Controller
    {
        public ActionResult Index()
        {
            var twitterFeedService = new TwitterFeedService("9AJLndlNaqSjHGHckVsmYHa1X", "NwH5IMAXNkFZfH5G8IgmfeWjk7LBodFtTJxwGXyFqNKfTPUfOI", "480297435-45jq7xC9BajE6q3OZBCmkPweIiVR8IKdlqLEvadl", "XgxtWW5C6ICXJjxPR8wtfK8GlBHDkARSMR9V0VjfMfGgC");
            var facebookFeedService = new FacebookFeedService("685686671555799", "5c40a26b1efeec3a90c7b94640dc6a74");

            var posts = twitterFeedService.GetTwitterStatuses("Edgesmash", 100).ToList();
            posts.AddRange(facebookFeedService.GetPosts("safestore", 100).ToList());

            return View("~/Views/SocialWall/Index.cshtml", new SocialPostViewModel {Posts = posts});
        }
    }
}