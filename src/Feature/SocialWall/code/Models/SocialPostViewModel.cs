using System.Collections.Generic;
using Foundation.SocialServices.Models;

namespace Feature.SocialWall.Models
{
    public class SocialPostViewModel
    {
        public SocialPostViewModel()
        {
            Posts = new List<SocialPost>();
        }

        public string Title { get; set; }

        public List<SocialPost> Posts { get; set; }
    }
}