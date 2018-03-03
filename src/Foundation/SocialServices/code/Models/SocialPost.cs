

using System;

namespace Foundation.SocialServices.Models
{
    public class SocialPost
    {
        public string Author { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public DateTime Date { get; set; }
        public SocialTypes Type { get; set; }
    }

    public enum SocialTypes
    {
        Twitter,
        Facebook
    }
}