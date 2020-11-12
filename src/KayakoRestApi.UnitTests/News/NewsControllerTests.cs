using System;
using System.Globalization;
using System.Linq;
using KayakoRestApi.Controllers;
using KayakoRestApi.Core.Constants;
using KayakoRestApi.Core.News;
using KayakoRestApi.Data;
using KayakoRestApi.Net;
using Moq;
using NUnit.Framework;

namespace KayakoRestApi.UnitTests.News
{
    [TestFixture]
    public class NewsControllerTests
    {
        [SetUp]
        public void Setup()
        {
            this.kayakoApiRequest = new Mock<IKayakoApiRequest>();
            this.newsController = new NewsController(this.kayakoApiRequest.Object);

            this.responseNewsCategoryCollection = new NewsCategoryCollection
            {
                new NewsCategory(),
                new NewsCategory()
            };

            this.responseNewsItemCollection = new NewsItemCollection
            {
                new NewsItem(),
                new NewsItem()
            };

            this.responseNewsSubscriberCollection = new NewsSubscriberCollection
            {
                new NewsSubscriber(),
                new NewsSubscriber(),
                new NewsSubscriber()
            };

            this.responseNewsItemCommentCollection = new NewsItemCommentCollection
            {
                new NewsItemComment(),
                new NewsItemComment()
            };
        }

        private INewsController newsController;
        private Mock<IKayakoApiRequest> kayakoApiRequest;
        private NewsCategoryCollection responseNewsCategoryCollection;
        private NewsItemCollection responseNewsItemCollection;
        private NewsSubscriberCollection responseNewsSubscriberCollection;
        private NewsItemCommentCollection responseNewsItemCommentCollection;

        [Test]
        public void GetNewsCategories()
        {
            const string apiMethod = "/News/Category";
            this.kayakoApiRequest.Setup(x => x.ExecuteGet<NewsCategoryCollection>(apiMethod)).Returns(this.responseNewsCategoryCollection);

            var newsCategories = this.newsController.GetNewsCategories();

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<NewsCategoryCollection>(apiMethod), Times.Once());

            Assert.That(newsCategories, Is.EqualTo(this.responseNewsCategoryCollection));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetNewsCategory(int newsCategoryId)
        {
            var apiMethod = string.Format("/News/Category/{0}", newsCategoryId);
            this.kayakoApiRequest.Setup(x => x.ExecuteGet<NewsCategoryCollection>(apiMethod)).Returns(this.responseNewsCategoryCollection);

            var newsCategory = this.newsController.GetNewsCategory(newsCategoryId);

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<NewsCategoryCollection>(apiMethod), Times.Once());

            Assert.That(newsCategory, Is.EqualTo(this.responseNewsCategoryCollection.First()));
        }

        [Test]
        public void CreateNewsCategory()
        {
            var newsCategoryRequest = new NewsCategoryRequest
            {
                Title = "TitleCategory",
                VisibilityType = NewsCategoryVisibilityType.Private
            };

            const string apiMethod = "/News/Category";
            const string parameters = "title=TitleCategory&visibilitytype=private";

            this.kayakoApiRequest.Setup(x => x.ExecutePost<NewsCategoryCollection>(apiMethod, parameters)).Returns(this.responseNewsCategoryCollection);

            var newsCategory = this.newsController.CreateNewsCategory(newsCategoryRequest);

            this.kayakoApiRequest.Verify(x => x.ExecutePost<NewsCategoryCollection>(apiMethod, parameters), Times.Once());
            Assert.That(newsCategory, Is.EqualTo(this.responseNewsCategoryCollection.FirstOrDefault()));
        }

        [Test]
        public void UpdateNewsCategory()
        {
            var newsCategoryRequest = new NewsCategoryRequest
            {
                Id = 1,
                Title = "TitleCategory",
                VisibilityType = NewsCategoryVisibilityType.Private
            };

            var apiMethod = string.Format("/News/Category/{0}", newsCategoryRequest.Id);
            const string parameters = "title=TitleCategory&visibilitytype=private";

            this.kayakoApiRequest.Setup(x => x.ExecutePut<NewsCategoryCollection>(apiMethod, parameters)).Returns(this.responseNewsCategoryCollection);

            var newsCategory = this.newsController.UpdateNewsCategory(newsCategoryRequest);

            this.kayakoApiRequest.Verify(x => x.ExecutePut<NewsCategoryCollection>(apiMethod, parameters), Times.Once());
            Assert.That(newsCategory, Is.EqualTo(this.responseNewsCategoryCollection.FirstOrDefault()));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void DeleteNewsCategory(int newsCategoryId)
        {
            var apiMethod = string.Format("/News/Category/{0}", newsCategoryId);

            this.kayakoApiRequest.Setup(x => x.ExecuteDelete(apiMethod)).Returns(true);

            var deleteSuccess = this.newsController.DeleteNewsCategory(newsCategoryId);

            this.kayakoApiRequest.Verify(x => x.ExecuteDelete(apiMethod), Times.Once());
            Assert.IsTrue(deleteSuccess);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetNewsItemsByCategoryId(int newsCategoryId)
        {
            var apiMethod = string.Format("/News/NewsItem/ListAll/{0}", newsCategoryId);
            this.kayakoApiRequest.Setup(x => x.ExecuteGet<NewsItemCollection>(apiMethod)).Returns(this.responseNewsItemCollection);

            var newsItems = this.newsController.GetNewsItems(newsCategoryId);

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<NewsItemCollection>(apiMethod), Times.Once());

            Assert.That(newsItems, Is.EqualTo(this.responseNewsItemCollection));
        }

        [Test]
        public void GetNewsItems()
        {
            const string apiMethod = "/News/NewsItem";
            this.kayakoApiRequest.Setup(x => x.ExecuteGet<NewsItemCollection>(apiMethod)).Returns(this.responseNewsItemCollection);

            var newsItems = this.newsController.GetNewsItems();

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<NewsItemCollection>(apiMethod), Times.Once());

            Assert.That(newsItems, Is.EqualTo(this.responseNewsItemCollection));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetNewsItem(int newsItemId)
        {
            var apiMethod = string.Format("/News/NewsItem/{0}", newsItemId);
            this.kayakoApiRequest.Setup(x => x.ExecuteGet<NewsItemCollection>(apiMethod)).Returns(this.responseNewsItemCollection);

            var newsItem = this.newsController.GetNewsItem(newsItemId);

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<NewsItemCollection>(apiMethod), Times.Once());

            Assert.That(newsItem, Is.EqualTo(this.responseNewsItemCollection.FirstOrDefault()));
        }

        [Test]
        public void CreateNewsItem()
        {
            const string apiMethod = "/News/NewsItem";
            const string parameters = @"subject=Subject&contents=Contents&staffid=1&newstype=3&newsstatus=1&fromname=FromName&email=email@domain.com&customemailsubject=CustomEmailSubject&sendemail=0&allowcomments=1&uservisibilitycustom=1&usergroupidlist=1,2&staffvisibilitycustom=1&staffgroupidlist=1,2&expiry=12/31/2015&newscategoryidlist=1";

            var newsItemRequest = new NewsItemRequest
            {
                Subject = "Subject",
                Contents = "Contents",
                StaffId = 1,
                NewsItemType = NewsItemType.Private,
                NewsItemStatus = NewsItemStatus.Draft,
                FromName = "FromName",
                Email = "email@domain.com",
                CustomEmailSubject = "CustomEmailSubject",
                SendEmail = false,
                AllowComments = true,
                UserVisibilityCustom = true,
                UserGroupIdList = new[] { 1, 2 },
                StaffVisibilityCustom = true,
                StaffGroupIdList = new[] { 1, 2 },
                Expiry = new UnixDateTime(DateTime.Parse("31/12/2015 15:30:00", CultureInfo.CreateSpecificCulture("en-GB"))),
                Categories = new[] { 1 }
            };

            this.kayakoApiRequest.Setup(x => x.ExecutePost<NewsItemCollection>(apiMethod, parameters)).Returns(this.responseNewsItemCollection);

            var newsItem = this.newsController.CreateNewsItem(newsItemRequest);

            this.kayakoApiRequest.Verify(x => x.ExecutePost<NewsItemCollection>(apiMethod, parameters), Times.Once());
            Assert.That(newsItem, Is.EqualTo(this.responseNewsItemCollection.FirstOrDefault()));
        }

        [Test]
        public void UpdateNewsItem()
        {
            var newsItemRequest = new NewsItemRequest
            {
                Id = 1,
                Subject = "Subject",
                Contents = "Contents",
                StaffId = 1,
                NewsItemType = NewsItemType.Private,
                NewsItemStatus = NewsItemStatus.Draft,
                FromName = "FromName",
                Email = "email@domain.com",
                CustomEmailSubject = "CustomEmailSubject",
                SendEmail = false,
                AllowComments = true,
                UserVisibilityCustom = true,
                UserGroupIdList = new[] { 1, 2 },
                StaffVisibilityCustom = true,
                StaffGroupIdList = new[] { 1, 2 },
                Expiry = new UnixDateTime(DateTime.Parse("31/12/2015 15:30:00", CultureInfo.CreateSpecificCulture("en-GB"))),
                Categories = new[] { 1 }
            };

            var apiMethod = string.Format("/News/NewsItem/{0}", newsItemRequest.Id);
            const string parameters = @"subject=Subject&contents=Contents&editedstaffid=1&newsstatus=1&fromname=FromName&email=email@domain.com&customemailsubject=CustomEmailSubject&sendemail=0&allowcomments=1&uservisibilitycustom=1&usergroupidlist=1,2&staffvisibilitycustom=1&staffgroupidlist=1,2&expiry=12/31/2015&newscategoryidlist=1";

            this.kayakoApiRequest.Setup(x => x.ExecutePut<NewsItemCollection>(apiMethod, parameters)).Returns(this.responseNewsItemCollection);

            var newsItem = this.newsController.UpdateNewsItem(newsItemRequest);

            this.kayakoApiRequest.Verify(x => x.ExecutePut<NewsItemCollection>(apiMethod, parameters), Times.Once());
            Assert.That(newsItem, Is.EqualTo(this.responseNewsItemCollection.FirstOrDefault()));
        }

        [TestCase(1, true)]
        [TestCase(2, false)]
        [TestCase(3, true)]
        public void DeleteNewsItem(int newsItemId, bool success)
        {
            var apiMethod = string.Format("/News/NewsItem/{0}", newsItemId);

            this.kayakoApiRequest.Setup(x => x.ExecuteDelete(apiMethod)).Returns(success);

            var deleteSuccess = this.newsController.DeleteNewsItem(newsItemId);

            this.kayakoApiRequest.Verify(x => x.ExecuteDelete(apiMethod), Times.Once());
            Assert.That(deleteSuccess, Is.EqualTo(success));
        }

        [Test]
        public void GetNewsSubscribers()
        {
            const string apiMethod = "/News/Subscriber";

            this.kayakoApiRequest.Setup(x => x.ExecuteGet<NewsSubscriberCollection>(apiMethod)).Returns(this.responseNewsSubscriberCollection);

            var newsSubscribers = this.newsController.GetNewsSubscribers();

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<NewsSubscriberCollection>(apiMethod), Times.Once());
            Assert.That(newsSubscribers, Is.EqualTo(this.responseNewsSubscriberCollection));
        }

        [TestCase(1)]
        public void GetNewsSubscriber(int newsSubscriberId)
        {
            var apiMethod = string.Format("/News/Subscriber/{0}", newsSubscriberId);

            this.kayakoApiRequest.Setup(x => x.ExecuteGet<NewsSubscriberCollection>(apiMethod)).Returns(this.responseNewsSubscriberCollection);

            var newsSubscriber = this.newsController.GetNewsSubscriber(newsSubscriberId);

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<NewsSubscriberCollection>(apiMethod), Times.Once());
            Assert.That(newsSubscriber, Is.EqualTo(this.responseNewsSubscriberCollection.FirstOrDefault()));
        }

        [Test]
        public void CreateNewsSubscriber()
        {
            const string apiMethod = "/News/Subscriber";
            const string parameters = "email=email@domain.com&isvalidated=1";

            var newsSubscriberRequest = new NewsSubscriberRequest
            {
                Email = "email@domain.com",
                IsValidated = true
            };

            this.kayakoApiRequest.Setup(x => x.ExecutePost<NewsSubscriberCollection>(apiMethod, parameters)).Returns(this.responseNewsSubscriberCollection);

            var newsSubscriber = this.newsController.CreateNewsSubscriber(newsSubscriberRequest);

            this.kayakoApiRequest.Verify(x => x.ExecutePost<NewsSubscriberCollection>(apiMethod, parameters), Times.Once());
            Assert.That(newsSubscriber, Is.EqualTo(this.responseNewsSubscriberCollection.FirstOrDefault()));
        }

        [Test]
        public void UpdateNewsSubscriber()
        {
            var newsSubscriberRequest = new NewsSubscriberRequest
            {
                Id = 1,
                Email = "email@domain.com"
            };

            const string apiMethod = "/News/Subscriber/1";
            const string parameters = "email=email@domain.com";

            this.kayakoApiRequest.Setup(x => x.ExecutePut<NewsSubscriberCollection>(apiMethod, parameters)).Returns(this.responseNewsSubscriberCollection);

            var newsSubscriber = this.newsController.UpdateNewsSubscriber(newsSubscriberRequest);

            this.kayakoApiRequest.Verify(x => x.ExecutePut<NewsSubscriberCollection>(apiMethod, parameters), Times.Once());
            Assert.That(newsSubscriber, Is.EqualTo(this.responseNewsSubscriberCollection.FirstOrDefault()));
        }

        [TestCase(1, true)]
        [TestCase(2, false)]
        [TestCase(3, true)]
        public void DeleteNewsSubscriber(int newsSubscriberId, bool success)
        {
            var apiMethod = string.Format("/News/Subscriber/{0}", newsSubscriberId);

            this.kayakoApiRequest.Setup(x => x.ExecuteDelete(apiMethod)).Returns(success);

            var deleteResult = this.newsController.DeleteNewsSubscriber(newsSubscriberId);

            Assert.That(deleteResult, Is.EqualTo(success));
        }

        [TestCase(1)]
        [TestCase(2)]
        public void GetNewsItemComments(int newsItemId)
        {
            var apiMethod = string.Format("/News/Comment/ListAll/{0}", newsItemId);
            this.kayakoApiRequest.Setup(x => x.ExecuteGet<NewsItemCommentCollection>(apiMethod)).Returns(this.responseNewsItemCommentCollection);

            var newsItemComments = this.newsController.GetNewsItemComments(newsItemId);

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<NewsItemCommentCollection>(apiMethod), Times.Once());
            Assert.That(newsItemComments, Is.EqualTo(this.responseNewsItemCommentCollection));
        }

        [TestCase(1)]
        [TestCase(2)]
        public void GetNewsItemComment(int newsItemCommentId)
        {
            var apiMethod = string.Format("/News/Comment/{0}", newsItemCommentId);
            this.kayakoApiRequest.Setup(x => x.ExecuteGet<NewsItemCommentCollection>(apiMethod)).Returns(this.responseNewsItemCommentCollection);

            var newsItemComment = this.newsController.GetNewsItemComment(newsItemCommentId);

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<NewsItemCommentCollection>(apiMethod), Times.Once());
            Assert.That(newsItemComment, Is.EqualTo(this.responseNewsItemCommentCollection.FirstOrDefault()));
        }

        [Test]
        public void CreateNewsItemComment()
        {
            var newsItemCommentRequest = new NewsItemCommentRequest
            {
                NewsItemId = 1,
                Contents = "Contents",
                CreatorType = NewsItemCommentCreatorType.Staff,
                CreatorId = 1,
                FullName = "FullName",
                Email = "email@domain.com",
                ParentCommentId = 3
            };

            const string apiMethod = "/News/Comment";
            const string parameters = "newsitemid=1&contents=Contents&creatortype=1&creatorid=1&email=email@domain.com&parentcommentid=3";

            this.kayakoApiRequest.Setup(x => x.ExecutePost<NewsItemCommentCollection>(apiMethod, parameters)).Returns(this.responseNewsItemCommentCollection);

            var newsItemComment = this.newsController.CreateNewsItemComment(newsItemCommentRequest);

            this.kayakoApiRequest.Verify(x => x.ExecutePost<NewsItemCommentCollection>(apiMethod, parameters), Times.Once());
            Assert.That(newsItemComment, Is.EqualTo(this.responseNewsItemCommentCollection.FirstOrDefault()));
        }

        [TestCase(1, true)]
        [TestCase(2, false)]
        [TestCase(3, true)]
        public void DeleteNewsItemComment(int newsItemCommentId, bool success)
        {
            var apiMethod = string.Format("/News/Comment/{0}", newsItemCommentId);
            this.kayakoApiRequest.Setup(x => x.ExecuteDelete(apiMethod)).Returns(success);

            var deleteSuccess = this.newsController.DeleteNewsItemComment(newsItemCommentId);

            this.kayakoApiRequest.Verify(x => x.ExecuteDelete(apiMethod), Times.Once());
            Assert.That(deleteSuccess, Is.EqualTo(success));
        }
    }
}