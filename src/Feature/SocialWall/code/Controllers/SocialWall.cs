using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Feature.SocialWall.Controllers
{
    public class SocialWallController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/SocialWall/Index.cshtml");
        }
    }
}