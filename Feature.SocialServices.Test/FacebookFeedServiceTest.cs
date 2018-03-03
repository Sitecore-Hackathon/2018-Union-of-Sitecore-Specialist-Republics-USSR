using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.SocialServices.Services;
using NUnit.Framework;

namespace Feature.SocialServices.Test
{
    [TestFixture]
    class FacebookFeedServiceTest
    {
        [Test]
        public void Should_Return_Facebook()
        {
            var test = new FacebookFeedService("685686671555799", "5c40a26b1efeec3a90c7b94640dc6a74");

            var results = test.GetPosts("safestore", 100);

            Assert.IsNotEmpty(results);
            //Assert.IsNotEmpty(results.FirstOrDefault().Text);
            //Assert.AreEqual(results.FirstOrDefault().Author.ScreenName, "safestore");

            //Console.WriteLine(results.FirstOrDefault().Author.ScreenName);
        }
    }
}
