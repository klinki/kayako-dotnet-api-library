using System;
using System.Diagnostics;
using KayakoRestApi.Core.Constants;
using KayakoRestApi.Core.News;
using KayakoRestApi.IntegrationTests.TestBase;
using NUnit.Framework;

namespace KayakoRestApi.IntegrationTests.News
{
    [TestFixture(Description = "A set of tests testing Api methods around News Categories")]
    public class NewsCategoryTests : UnitTestBase
    {
        [Test]
        public void GetAllNewsCategories()
        {
            var newsCategories = TestSetup.KayakoApiService.News.GetNewsCategories();

            Assert.IsNotNull(newsCategories, "No news categories were returned");
            Assert.IsNotEmpty(newsCategories, "No news categories were returned");
        }

        [Test]
        public void GetNewsCategory()
        {
            var newsCategories = TestSetup.KayakoApiService.News.GetNewsCategories();

            Assert.IsNotNull(newsCategories, "No news categories were returned");
            Assert.IsNotEmpty(newsCategories, "No news categories were returned");

            var newsCategoryToGet = newsCategories[new Random().Next(newsCategories.Count)];

            Trace.WriteLine("GetNewsCategory using news category id: " + newsCategoryToGet.Id);

            var newsCategory = TestSetup.KayakoApiService.News.GetNewsCategory(newsCategoryToGet.Id);

            AssertObjectXmlEqual(newsCategory, newsCategoryToGet);
        }

        [Test(Description = "Tests creating, updating and deleting news categories")]
        public void CreateUpdateDeleteNewsCategory()
        {
            var newsCategoryRequest = new NewsCategoryRequest
            {
                Title = "TitleFromIntegrationTest",
                VisibilityType = NewsCategoryVisibilityType.Public
            };

            var newsCategory = TestSetup.KayakoApiService.News.CreateNewsCategory(newsCategoryRequest);

            Assert.IsNotNull(newsCategory);
            Assert.That(newsCategory.Title, Is.EqualTo(newsCategoryRequest.Title));
            Assert.That(newsCategory.VisibilityType, Is.EqualTo(newsCategoryRequest.VisibilityType));

            newsCategoryRequest.Id = newsCategory.Id;
            newsCategoryRequest.Title += "_Updated";

            newsCategory = TestSetup.KayakoApiService.News.UpdateNewsCategory(newsCategoryRequest);

            Assert.IsNotNull(newsCategory);
            Assert.That(newsCategory.Title, Is.EqualTo(newsCategoryRequest.Title));
            Assert.That(newsCategory.VisibilityType, Is.EqualTo(newsCategoryRequest.VisibilityType));

            var deleteResult = TestSetup.KayakoApiService.News.DeleteNewsCategory(newsCategory.Id);
            Assert.IsTrue(deleteResult);
        }
    }
}