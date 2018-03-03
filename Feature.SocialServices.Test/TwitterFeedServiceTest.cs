using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Foundation.SocialServices.Services;

namespace Feature.SocialServices.Test
{
    [TestFixture]
    public class TwitterFeedServiceTest
    {
        [Test]
        public void Should_Return_Tweets()
        {
            var test = new TwitterFeedService("9AJLndlNaqSjHGHckVsmYHa1X", "NwH5IMAXNkFZfH5G8IgmfeWjk7LBodFtTJxwGXyFqNKfTPUfOI", "480297435-45jq7xC9BajE6q3OZBCmkPweIiVR8IKdlqLEvadl", "XgxtWW5C6ICXJjxPR8wtfK8GlBHDkARSMR9V0VjfMfGgC");

            var results = test.GetTwitterStatuses("Edgesmash", 100);

            Assert.IsNotEmpty(results);
            //Assert.IsNotEmpty(results.FirstOrDefault().Text);
            //Assert.AreEqual(results.FirstOrDefault().Author.ScreenName, "Edgesmash");

            //Console.WriteLine(results.FirstOrDefault().Author.ScreenName);
        }
    }
}
