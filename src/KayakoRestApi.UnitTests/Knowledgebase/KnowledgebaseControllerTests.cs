using System;
using System.Linq;
using System.Text;
using KayakoRestApi.Controllers;
using KayakoRestApi.Core.Constants;
using KayakoRestApi.Core.Knowledgebase;
using KayakoRestApi.Net;
using Moq;
using NUnit.Framework;

namespace KayakoRestApi.UnitTests.Knowledgebase
{
    [TestFixture]
    public class KnowledgebaseControllerTests
    {
        [SetUp]
        public void Setup()
        {
            this.kayakoApiRequest = new Mock<IKayakoApiRequest>();
            this.knowledgebaseController = new KnowledgebaseController(this.kayakoApiRequest.Object);

            this.responseKnowledgebaseCategoryCollection = new KnowledgebaseCategoryCollection
            {
                new KnowledgebaseCategory(),
                new KnowledgebaseCategory()
            };

            this.responseKnowledgebaseArticleCollection = new KnowledgebaseArticleCollection
            {
                new KnowledgebaseArticle(),
                new KnowledgebaseArticle()
            };

            this.responseKnowledgebaseCommentCollection = new KnowledgebaseCommentCollection
            {
                new KnowledgebaseComment(),
                new KnowledgebaseComment()
            };

            this.responseKnowledgebaseAttachmentCollection = new KnowledgebaseAttachmentCollection
            {
                new KnowledgebaseAttachment(),
                new KnowledgebaseAttachment()
            };
        }

        private IKnowledgebaseController knowledgebaseController;
        private Mock<IKayakoApiRequest> kayakoApiRequest;
        private KnowledgebaseCategoryCollection responseKnowledgebaseCategoryCollection;
        private KnowledgebaseArticleCollection responseKnowledgebaseArticleCollection;
        private KnowledgebaseCommentCollection responseKnowledgebaseCommentCollection;
        private KnowledgebaseAttachmentCollection responseKnowledgebaseAttachmentCollection;

        [Test]
        public void GetKnowledgebaseCategories()
        {
            const string apiMethod = "/Knowledgebase/Category/ListAll/-1/-1";
            this.kayakoApiRequest.Setup(x => x.ExecuteGet<KnowledgebaseCategoryCollection>(apiMethod)).Returns(this.responseKnowledgebaseCategoryCollection);

            var knowledgebaseCategories = this.knowledgebaseController.GetKnowledgebaseCategories();

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<KnowledgebaseCategoryCollection>(apiMethod), Times.Once());

            Assert.That(knowledgebaseCategories, Is.EqualTo(this.responseKnowledgebaseCategoryCollection));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetKnowledgebaseCategory(int knowledgebaseCategoryId)
        {
            var apiMethod = string.Format("/Knowledgebase/Category/{0}", knowledgebaseCategoryId);

            this.kayakoApiRequest.Setup(x => x.ExecuteGet<KnowledgebaseCategoryCollection>(apiMethod)).Returns(this.responseKnowledgebaseCategoryCollection);

            var knowledgebaseCategory = this.knowledgebaseController.GetKnowledgebaseCategory(knowledgebaseCategoryId);

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<KnowledgebaseCategoryCollection>(apiMethod), Times.Once());

            Assert.That(knowledgebaseCategory, Is.EqualTo(this.responseKnowledgebaseCategoryCollection.FirstOrDefault()));
        }

        [Test]
        public void CreateKnowledgebaseCategory()
        {
            var knowledgebaseCategoryRequest = new KnowledgebaseCategoryRequest
            {
                Title = "Title",
                CategoryType = KnowledgebaseCategoryType.Inherit,
                ParentCategoryId = 3,
                DisplayOrder = 3,
                ArticleSortOrder = KnowledgebaseCategoryArticleSortOrder.SortCreationDate,
                AllowComments = true,
                AllowRating = false,
                IsPublished = true,
                UserVisibilityCustom = true,
                UserGroupIdList = new[] { 1, 2, 3 },
                StaffVisibilityCustom = false,
                StaffGroupIdList = new[] { 1, 2, 3 },
                StaffId = 3
            };

            const string apiMethod = "/Knowledgebase/Category";
            const string parameters = "title=Title&categorytype=4&parentcategoryid=3&displayorder=3&articlesortorder=4&allowcomments=1&allowrating=0&ispublished=1&uservisibilitycustom=1&usergroupidlist=1,2,3&staffvisibilitycustom=0&staffgroupidlist=1,2,3&staffid=3";

            this.kayakoApiRequest.Setup(x => x.ExecutePost<KnowledgebaseCategoryCollection>(apiMethod, parameters)).Returns(this.responseKnowledgebaseCategoryCollection);

            var knowledgebaseCategory = this.knowledgebaseController.CreateKnowledgebaseCategory(knowledgebaseCategoryRequest);

            this.kayakoApiRequest.Verify(x => x.ExecutePost<KnowledgebaseCategoryCollection>(apiMethod, parameters), Times.Once());
            Assert.That(knowledgebaseCategory, Is.EqualTo(this.responseKnowledgebaseCategoryCollection.FirstOrDefault()));
        }

        [Test]
        public void UpdateKnowledgebaseCategory()
        {
            var knowledgebaseCategoryRequest = new KnowledgebaseCategoryRequest
            {
                Id = 3,
                Title = "Title",
                CategoryType = KnowledgebaseCategoryType.Inherit,
                ParentCategoryId = 3,
                DisplayOrder = 3,
                ArticleSortOrder = KnowledgebaseCategoryArticleSortOrder.SortCreationDate,
                AllowComments = true,
                AllowRating = false,
                IsPublished = true,
                UserVisibilityCustom = true,
                UserGroupIdList = new[] { 1, 2, 3 },
                StaffVisibilityCustom = false,
                StaffGroupIdList = new[] { 1, 2, 3 }
            };

            var apiMethod = string.Format("/Knowledgebase/Category/{0}", knowledgebaseCategoryRequest.Id);
            const string parameters = "title=Title&categorytype=4&parentcategoryid=3&displayorder=3&articlesortorder=4&allowcomments=1&allowrating=0&ispublished=1&uservisibilitycustom=1&usergroupidlist=1,2,3&staffvisibilitycustom=0&staffgroupidlist=1,2,3";

            this.kayakoApiRequest.Setup(x => x.ExecutePut<KnowledgebaseCategoryCollection>(apiMethod, parameters)).Returns(this.responseKnowledgebaseCategoryCollection);

            var knowledgebaseCategory = this.knowledgebaseController.UpdateKnowledgebaseCategory(knowledgebaseCategoryRequest);

            this.kayakoApiRequest.Verify(x => x.ExecutePut<KnowledgebaseCategoryCollection>(apiMethod, parameters), Times.Once());
            Assert.That(knowledgebaseCategory, Is.EqualTo(this.responseKnowledgebaseCategoryCollection.FirstOrDefault()));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void DeleteKnowledgebaseCategory(int knowledgebaseCategoryId)
        {
            var apiMethod = string.Format("/Knowledgebase/Category/{0}", knowledgebaseCategoryId);

            this.kayakoApiRequest.Setup(x => x.ExecuteDelete(apiMethod)).Returns(true);

            var deleteSuccess = this.knowledgebaseController.DeleteKnowledgebaseCategory(knowledgebaseCategoryId);

            this.kayakoApiRequest.Verify(x => x.ExecuteDelete(apiMethod), Times.Once());

            Assert.IsTrue(deleteSuccess);
        }

        [TestCase(1, 1)]
        [TestCase(1, 3)]
        public void GetKnowledgebaseArticlesPaging(int count, int start)
        {
            var apiMethod = string.Format("/Knowledgebase/Article/ListAll/{0}/{1}", count, start);
            this.kayakoApiRequest.Setup(x => x.ExecuteGet<KnowledgebaseArticleCollection>(apiMethod)).Returns(this.responseKnowledgebaseArticleCollection);

            var knowledgebaseArticles = this.knowledgebaseController.GetKnowledgebaseArticles(count, start);

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<KnowledgebaseArticleCollection>(apiMethod), Times.Once());

            Assert.That(knowledgebaseArticles, Is.EqualTo(this.responseKnowledgebaseArticleCollection));
        }

        [Test]
        public void GetKnowledgebaseArticles()
        {
            const string apiMethod = "/Knowledgebase/Article";
            this.kayakoApiRequest.Setup(x => x.ExecuteGet<KnowledgebaseArticleCollection>(apiMethod)).Returns(this.responseKnowledgebaseArticleCollection);

            var knowledgebaseArticles = this.knowledgebaseController.GetKnowledgebaseArticles();

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<KnowledgebaseArticleCollection>(apiMethod), Times.Once());

            Assert.That(knowledgebaseArticles, Is.EqualTo(this.responseKnowledgebaseArticleCollection));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetKnowledgebaseArticle(int knowledgebaseArticleId)
        {
            var apiMethod = string.Format("/Knowledgebase/Article/{0}", knowledgebaseArticleId);

            this.kayakoApiRequest.Setup(x => x.ExecuteGet<KnowledgebaseArticleCollection>(apiMethod)).Returns(this.responseKnowledgebaseArticleCollection);

            var knowledgebaseArticle = this.knowledgebaseController.GetKnowledgebaseArticle(knowledgebaseArticleId);

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<KnowledgebaseArticleCollection>(apiMethod), Times.Once());

            Assert.That(knowledgebaseArticle, Is.EqualTo(this.responseKnowledgebaseArticleCollection.FirstOrDefault()));
        }

        [Test]
        public void CreateKnowledgebaseArticle()
        {
            var knowledgebaseArticleRequest = new KnowledgebaseArticleRequest
            {
                CreatorId = 3,
                Subject = "Subject",
                Contents = "Contents",
                ArticleStatus = KnowledgebaseArticleStatus.Published,
                IsFeatured = false,
                AllowComments = true,
                CategoryIds = new[] { 1 }
            };

            const string apiMethod = "/Knowledgebase/Article";
            const string parameters = "subject=Subject&contents=Contents&creatorid=3&articlestatus=1&isfeatured=0&allowcomments=1&categoryid=1";

            this.kayakoApiRequest.Setup(x => x.ExecutePost<KnowledgebaseArticleCollection>(apiMethod, parameters)).Returns(this.responseKnowledgebaseArticleCollection);

            var knowledgebaseArticle = this.knowledgebaseController.CreateKnowledgebaseArticle(knowledgebaseArticleRequest);

            this.kayakoApiRequest.Verify(x => x.ExecutePost<KnowledgebaseArticleCollection>(apiMethod, parameters), Times.Once());
            Assert.That(knowledgebaseArticle, Is.EqualTo(this.responseKnowledgebaseArticleCollection.FirstOrDefault()));
        }

        [Test]
        public void UpdateKnowledgebaseArticle()
        {
            var knowledgebaseArticleRequest = new KnowledgebaseArticleRequest
            {
                Id = 1,
                EditedStaffId = 3,
                Subject = "Subject",
                Contents = "Contents",
                ArticleStatus = KnowledgebaseArticleStatus.Published,
                IsFeatured = false,
                AllowComments = true,
                CategoryIds = new[] { 1, 2, 3 }
            };

            var apiMethod = string.Format("/Knowledgebase/Article/{0}", knowledgebaseArticleRequest.Id);
            const string parameters = "subject=Subject&contents=Contents&articlestatus=1&isfeatured=0&allowcomments=1&categoryid=1,2,3&editedstaffid=3";

            this.kayakoApiRequest.Setup(x => x.ExecutePut<KnowledgebaseArticleCollection>(apiMethod, parameters)).Returns(this.responseKnowledgebaseArticleCollection);

            var knowledgebaseArticle = this.knowledgebaseController.UpdateKnowledgebaseArticle(knowledgebaseArticleRequest);

            this.kayakoApiRequest.Verify(x => x.ExecutePut<KnowledgebaseArticleCollection>(apiMethod, parameters), Times.Once());
            Assert.That(knowledgebaseArticle, Is.EqualTo(this.responseKnowledgebaseArticleCollection.FirstOrDefault()));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void DeleteKnowledgebaseArticle(int knowledgebaseArticleId)
        {
            var apiMethod = string.Format("/Knowledgebase/Article/{0}", knowledgebaseArticleId);

            this.kayakoApiRequest.Setup(x => x.ExecuteDelete(apiMethod)).Returns(true);

            var deleteSuccess = this.knowledgebaseController.DeleteKnowledgebaseArticle(knowledgebaseArticleId);

            this.kayakoApiRequest.Verify(x => x.ExecuteDelete(apiMethod), Times.Once());

            Assert.IsTrue(deleteSuccess);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetKnowledgebaseCommentsForArticle(int articleId)
        {
            var apiMethod = string.Format("/Knowledgebase/Comment/ListAll/{0}", articleId);

            this.kayakoApiRequest.Setup(x => x.ExecuteGet<KnowledgebaseCommentCollection>(apiMethod)).Returns(this.responseKnowledgebaseCommentCollection);

            var knowledgebaseComments = this.knowledgebaseController.GetKnowledgebaseComments(articleId);

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<KnowledgebaseCommentCollection>(apiMethod), Times.Once());

            Assert.That(knowledgebaseComments, Is.EqualTo(this.responseKnowledgebaseCommentCollection));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetKnowledgebaseCommentById(int commentId)
        {
            var apiMethod = string.Format("/Knowledgebase/Comment/{0}", commentId);

            this.kayakoApiRequest.Setup(x => x.ExecuteGet<KnowledgebaseCommentCollection>(apiMethod)).Returns(this.responseKnowledgebaseCommentCollection);

            var knowledgebaseComment = this.knowledgebaseController.GetKnowledgebaseComment(commentId);

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<KnowledgebaseCommentCollection>(apiMethod), Times.Once());

            Assert.That(knowledgebaseComment, Is.EqualTo(this.responseKnowledgebaseCommentCollection.First()));
        }

        [Test]
        public void CreateKnowledgebaseComment()
        {
            var knowledgebaseCommentRequest = new KnowledgebaseCommentRequest
            {
                KnowledgebaseArticleId = 1,
                Contents = "Contents",
                CreatorType = KnowledgebaseCommentCreatorType.User,
                CreatorId = 3,
                Email = "email@domain.com",
                ParentCommentId = 1
            };

            const string apiMethod = "/Knowledgebase/Comment";
            const string parameters = "knowledgebasearticleid=1&contents=Contents&creatortype=2&creatorid=3&email=email@domain.com&parentcommentid=1";

            this.kayakoApiRequest.Setup(x => x.ExecutePost<KnowledgebaseCommentCollection>(apiMethod, parameters)).Returns(this.responseKnowledgebaseCommentCollection);

            var knowledgebaseComment = this.knowledgebaseController.CreateKnowledgebaseComment(knowledgebaseCommentRequest);

            this.kayakoApiRequest.Verify(x => x.ExecutePost<KnowledgebaseCommentCollection>(apiMethod, parameters), Times.Once());
            Assert.That(knowledgebaseComment, Is.EqualTo(this.responseKnowledgebaseCommentCollection.First()));
        }

        [TestCase(1, true)]
        [TestCase(2, false)]
        [TestCase(3, true)]
        public void DeleteKnowledgebaseComment(int commentId, bool success)
        {
            var apiMethod = string.Format("/Knowledgebase/Comment/{0}", commentId);

            this.kayakoApiRequest.Setup(x => x.ExecuteDelete(apiMethod)).Returns(success);

            var deleteSuccess = this.knowledgebaseController.DeleteKnowledgebaseComment(commentId);

            this.kayakoApiRequest.Verify(x => x.ExecuteDelete(apiMethod));

            Assert.That(deleteSuccess, Is.EqualTo(success));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetKnowledgebaseAttachments(int knowledgebaseArticleId)
        {
            var apiMethod = string.Format("/Knowledgebase/Attachment/ListAll/{0}", knowledgebaseArticleId);

            this.kayakoApiRequest.Setup(x => x.ExecuteGet<KnowledgebaseAttachmentCollection>(apiMethod)).Returns(this.responseKnowledgebaseAttachmentCollection);

            var knowledgebaseAttachments = this.knowledgebaseController.GetKnowledgebaseAttachments(knowledgebaseArticleId);

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<KnowledgebaseAttachmentCollection>(apiMethod));

            Assert.That(knowledgebaseAttachments, Is.EqualTo(this.responseKnowledgebaseAttachmentCollection));
        }

        [TestCase(1, 2)]
        [TestCase(3, 4)]
        [TestCase(5, 6)]
        public void GetKnowledgebaseAttachment(int knowledgebaseArticleId, int knowledgebaseAttachmentId)
        {
            var apiMethod = string.Format("/Knowledgebase/Attachment/{0}/{1}", knowledgebaseArticleId, knowledgebaseAttachmentId);

            this.kayakoApiRequest.Setup(x => x.ExecuteGet<KnowledgebaseAttachmentCollection>(apiMethod)).Returns(this.responseKnowledgebaseAttachmentCollection);

            var knowledgebaseAttachment = this.knowledgebaseController.GetKnowledgebaseAttachment(knowledgebaseArticleId, knowledgebaseAttachmentId);

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<KnowledgebaseAttachmentCollection>(apiMethod));

            Assert.That(knowledgebaseAttachment, Is.EqualTo(this.responseKnowledgebaseAttachmentCollection.First()));
        }

        [Test]
        public void CreateKnowledgebaseAttachement()
        {
            var contents = Convert.ToBase64String(Encoding.UTF8.GetBytes("This is the contents"));

            var knowledgebaseAttachmentRequest = new KnowledgebaseAttachmentRequest
            {
                KnowledgebaseArticleId = 1,
                FileName = "fileName",
                Contents = contents
            };

            const string apiMethod = "/Knowledgebase/Attachment";
            var parameters = string.Format("kbarticleid=1&filename=fileName&contents={0}", contents);

            this.kayakoApiRequest.Setup(x => x.ExecutePost<KnowledgebaseAttachmentCollection>(apiMethod, parameters)).Returns(this.responseKnowledgebaseAttachmentCollection);

            var knowledgebaseAttachment = this.knowledgebaseController.CreateKnowledgebaseAttachment(knowledgebaseAttachmentRequest);

            this.kayakoApiRequest.Verify(x => x.ExecutePost<KnowledgebaseAttachmentCollection>(apiMethod, parameters));

            Assert.That(knowledgebaseAttachment, Is.EqualTo(this.responseKnowledgebaseAttachmentCollection.First()));
        }

        [TestCase(1, 2, true)]
        [TestCase(3, 4, false)]
        [TestCase(5, 6, true)]
        public void DeleteKnowledgebaseAttachment(int knowledgebaseArticleId, int knowledgebaseAttachmentId, bool success)
        {
            var apiMethod = string.Format("/Knowledgebase/Attachment/{0}/{1}", knowledgebaseArticleId, knowledgebaseAttachmentId);

            this.kayakoApiRequest.Setup(x => x.ExecuteDelete(apiMethod)).Returns(success);

            var deleteSuccess = this.knowledgebaseController.DeleteKnowledgebaseAttachment(knowledgebaseArticleId, knowledgebaseAttachmentId);

            this.kayakoApiRequest.Verify(x => x.ExecuteDelete(apiMethod));

            Assert.That(deleteSuccess, Is.EqualTo(success));
        }
    }
}