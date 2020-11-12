using System;
using System.Linq;
using System.Text;
using KayakoRestApi.Controllers;
using KayakoRestApi.Core.Constants;
using KayakoRestApi.Core.Troubleshooter;
using KayakoRestApi.Net;
using Moq;
using NUnit.Framework;

namespace KayakoRestApi.UnitTests.Troubleshooter
{
    [TestFixture]
    public class TroubleshooterControllerTests
    {
        [SetUp]
        public void Setup()
        {
            this.kayakoApiRequest = new Mock<IKayakoApiRequest>();
            this.troubleshooterController = new TroubleshooterController(this.kayakoApiRequest.Object);

            this.responseTroubleshooterCategoryCollection = new TroubleshooterCategoryCollection
            {
                new TroubleshooterCategory(),
                new TroubleshooterCategory()
            };

            this.responseTroubleshooterStepCollection = new TroubleshooterStepCollection
            {
                new TroubleshooterStep(),
                new TroubleshooterStep()
            };

            this.responseTroubleshooterCommentCollection = new TroubleshooterCommentCollection
            {
                new TroubleshooterComment(),
                new TroubleshooterComment()
            };

            this.responseTroubleshooterAttachmentCollection = new TroubleshooterAttachmentCollection
            {
                new TroubleshooterAttachment(),
                new TroubleshooterAttachment()
            };
        }

        private ITroubleshooterController troubleshooterController;
        private Mock<IKayakoApiRequest> kayakoApiRequest;
        private TroubleshooterCategoryCollection responseTroubleshooterCategoryCollection;
        private TroubleshooterStepCollection responseTroubleshooterStepCollection;
        private TroubleshooterCommentCollection responseTroubleshooterCommentCollection;
        private TroubleshooterAttachmentCollection responseTroubleshooterAttachmentCollection;

        [Test]
        public void GetTroubleshooterCategories()
        {
            const string apiMethod = "/Troubleshooter/Category";
            this.kayakoApiRequest.Setup(x => x.ExecuteGet<TroubleshooterCategoryCollection>(apiMethod)).Returns(this.responseTroubleshooterCategoryCollection);

            var troubleshooterCategories = this.troubleshooterController.GetTroubleshooterCategories();

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<TroubleshooterCategoryCollection>(apiMethod), Times.Once());

            Assert.That(troubleshooterCategories, Is.EqualTo(this.responseTroubleshooterCategoryCollection));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetTroubleshooterCategory(int troubleshooterCategoryId)
        {
            var apiMethod = string.Format("/Troubleshooter/Category/{0}", troubleshooterCategoryId);
            this.kayakoApiRequest.Setup(x => x.ExecuteGet<TroubleshooterCategoryCollection>(apiMethod)).Returns(this.responseTroubleshooterCategoryCollection);

            var troubleshooterCategory = this.troubleshooterController.GetTroubleshooterCategory(troubleshooterCategoryId);

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<TroubleshooterCategoryCollection>(apiMethod), Times.Once());

            Assert.That(troubleshooterCategory, Is.EqualTo(this.responseTroubleshooterCategoryCollection.First()));
        }

        [Test]
        public void CreateTroubleshooterCategory()
        {
            var troubleshooterCategoryRequest = new TroubleshooterCategoryRequest
            {
                Title = "TitleCategory",
                CategoryType = TroubleshooterCategoryType.Private,
                StaffId = 2,
                DisplayOrder = 1,
                Description = "Description",
                UserVisibilityCustom = true,
                UserGroupIdList = new[] { 1, 2, 3 },
                StaffVisibilityCustom = true,
                StaffGroupIdList = new[] { 3, 4, 5 }
            };

            const string apiMethod = "/Troubleshooter/Category";
            const string parameters = "title=TitleCategory&categorytype=3&staffid=2&displayorder=1&description=Description&uservisibilitycustom=1&usergroupidlist=1,2,3&staffvisibilitycustom=1&staffgroupidlist=3,4,5";

            this.kayakoApiRequest.Setup(x => x.ExecutePost<TroubleshooterCategoryCollection>(apiMethod, parameters)).Returns(this.responseTroubleshooterCategoryCollection);

            var troubleshooterCategory = this.troubleshooterController.CreateTroubleshooterCategory(troubleshooterCategoryRequest);

            this.kayakoApiRequest.Verify(x => x.ExecutePost<TroubleshooterCategoryCollection>(apiMethod, parameters), Times.Once());
            Assert.That(troubleshooterCategory, Is.EqualTo(this.responseTroubleshooterCategoryCollection.FirstOrDefault()));
        }

        [Test]
        public void UpdateTroubleshooterCategory()
        {
            var troubleshooterCategoryRequest = new TroubleshooterCategoryRequest
            {
                Title = "TitleCategory",
                CategoryType = TroubleshooterCategoryType.Public,
                DisplayOrder = 1,
                Description = "Description",
                UserVisibilityCustom = true,
                UserGroupIdList = new[] { 1, 2, 3 },
                StaffVisibilityCustom = true,
                StaffGroupIdList = new[] { 3, 4, 5 }
            };

            var apiMethod = string.Format("/Troubleshooter/Category/{0}", troubleshooterCategoryRequest.Id);
            const string parameters = "title=TitleCategory&categorytype=2&displayorder=1&description=Description&uservisibilitycustom=1&usergroupidlist=1,2,3&staffvisibilitycustom=1&staffgroupidlist=3,4,5";

            this.kayakoApiRequest.Setup(x => x.ExecutePut<TroubleshooterCategoryCollection>(apiMethod, parameters)).Returns(this.responseTroubleshooterCategoryCollection);

            var troubleshooterCategory = this.troubleshooterController.UpdateTroubleshooterCategory(troubleshooterCategoryRequest);

            this.kayakoApiRequest.Verify(x => x.ExecutePut<TroubleshooterCategoryCollection>(apiMethod, parameters), Times.Once());
            Assert.That(troubleshooterCategory, Is.EqualTo(this.responseTroubleshooterCategoryCollection.FirstOrDefault()));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void DeleteTroubleshooterCategory(int troubleshooterCategoryId)
        {
            var apiMethod = string.Format("/Troubleshooter/Category/{0}", troubleshooterCategoryId);

            this.kayakoApiRequest.Setup(x => x.ExecuteDelete(apiMethod)).Returns(true);

            var deleteSuccess = this.troubleshooterController.DeleteTroubleshooterCategory(troubleshooterCategoryId);

            this.kayakoApiRequest.Verify(x => x.ExecuteDelete(apiMethod), Times.Once());
            Assert.IsTrue(deleteSuccess);
        }

        [Test]
        public void GetTroubleshooterSteps()
        {
            const string apiMethod = "/Troubleshooter/Step";
            this.kayakoApiRequest.Setup(x => x.ExecuteGet<TroubleshooterStepCollection>(apiMethod)).Returns(this.responseTroubleshooterStepCollection);

            var troubleshooterSteps = this.troubleshooterController.GetTroubleshooterSteps();

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<TroubleshooterStepCollection>(apiMethod), Times.Once());

            Assert.That(troubleshooterSteps, Is.EqualTo(this.responseTroubleshooterStepCollection));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetTroubleshooterStep(int troubleshooterStepId)
        {
            var apiMethod = string.Format("/Troubleshooter/Step/{0}", troubleshooterStepId);
            this.kayakoApiRequest.Setup(x => x.ExecuteGet<TroubleshooterStepCollection>(apiMethod)).Returns(this.responseTroubleshooterStepCollection);

            var troubleshooterStep = this.troubleshooterController.GetTroubleshooterStep(troubleshooterStepId);

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<TroubleshooterStepCollection>(apiMethod), Times.Once());

            Assert.That(troubleshooterStep, Is.EqualTo(this.responseTroubleshooterStepCollection.First()));
        }

        [Test]
        public void CreateTroubleshooterStep()
        {
            var troubleshooterStepRequest = new TroubleshooterStepRequest
            {
                CategoryId = 1,
                Subject = "Subject",
                Contents = "Contents",
                StaffId = 3,
                DisplayOrder = 15,
                AllowComments = true,
                EnableTicketRedirection = false,
                RedirectDepartmentId = 4,
                TicketTypeId = 4,
                TicketPriorityId = 2,
                TicketSubject = "Ticket Subject",
                StepStatus = TroubleshooterStepStatus.Published,
                ParentStepIdList = new[] { 1, 2, 3 }
            };

            const string apiMethod = "/Troubleshooter/Step";
            const string parameters = "categoryid=1&subject=Subject&contents=Contents&staffid=3&displayorder=15&allowcomments=1&enableticketredirection=0&redirectdepartmentid=4&tickettypeid=4&ticketpriorityid=2&ticketsubject=Ticket Subject&stepstatus=1&parentstepidlist=1,2,3";

            this.kayakoApiRequest.Setup(x => x.ExecutePost<TroubleshooterStepCollection>(apiMethod, parameters)).Returns(this.responseTroubleshooterStepCollection);

            var troubleshooterStep = this.troubleshooterController.CreateTroubleshooterStep(troubleshooterStepRequest);

            this.kayakoApiRequest.Verify(x => x.ExecutePost<TroubleshooterStepCollection>(apiMethod, parameters), Times.Once());
            Assert.That(troubleshooterStep, Is.EqualTo(this.responseTroubleshooterStepCollection.FirstOrDefault()));
        }

        [Test]
        public void UpdateTroubleshooterStep()
        {
            var troubleshooterStepRequest = new TroubleshooterStepRequest
            {
                StaffId = 3,
                Subject = "Subject",
                Contents = "Contents",
                DisplayOrder = 4,
                AllowComments = true,
                EnableTicketRedirection = false,
                RedirectDepartmentId = 3,
                TicketTypeId = 1,
                TicketPriorityId = 3,
                TicketSubject = "Ticket Subject",
                StepStatus = TroubleshooterStepStatus.Published,
                ParentStepIdList = new[] { 1, 3, 5 }
            };

            var apiMethod = string.Format("/Troubleshooter/Step/{0}", troubleshooterStepRequest.Id);
            const string parameters = "subject=Subject&contents=Contents&editedstaffid=3&displayorder=4&allowcomments=1&enableticketredirection=0&redirectdepartmentid=3&tickettypeid=1&ticketpriorityid=3&ticketsubject=Ticket Subject&stepstatus=1&parentstepidlist=1,3,5";

            this.kayakoApiRequest.Setup(x => x.ExecutePut<TroubleshooterStepCollection>(apiMethod, parameters)).Returns(this.responseTroubleshooterStepCollection);

            var troubleshooterStep = this.troubleshooterController.UpdateTroubleshooterStep(troubleshooterStepRequest);

            this.kayakoApiRequest.Verify(x => x.ExecutePut<TroubleshooterStepCollection>(apiMethod, parameters), Times.Once());
            Assert.That(troubleshooterStep, Is.EqualTo(this.responseTroubleshooterStepCollection.FirstOrDefault()));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void DeleteTroubleshooterStep(int troubleshooterStepId)
        {
            var apiMethod = string.Format("/Troubleshooter/Step/{0}", troubleshooterStepId);

            this.kayakoApiRequest.Setup(x => x.ExecuteDelete(apiMethod)).Returns(true);

            var deleteSuccess = this.troubleshooterController.DeleteTroubleshooterStep(troubleshooterStepId);

            this.kayakoApiRequest.Verify(x => x.ExecuteDelete(apiMethod), Times.Once());
            Assert.IsTrue(deleteSuccess);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetTroubleshooterComments(int troubleshooterStepId)
        {
            var apiMethod = string.Format("/Troubleshooter/Comment/ListAll/{0}", troubleshooterStepId);
            this.kayakoApiRequest.Setup(x => x.ExecuteGet<TroubleshooterCommentCollection>(apiMethod)).Returns(this.responseTroubleshooterCommentCollection);

            var troubleshooterComments = this.troubleshooterController.GetTroubleshooterComments(troubleshooterStepId);

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<TroubleshooterCommentCollection>(apiMethod), Times.Once());

            Assert.That(troubleshooterComments, Is.EqualTo(this.responseTroubleshooterCommentCollection));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetTroubleshooterComment(int troubleshooterCommentId)
        {
            var apiMethod = string.Format("/Troubleshooter/Comment/{0}", troubleshooterCommentId);
            this.kayakoApiRequest.Setup(x => x.ExecuteGet<TroubleshooterCommentCollection>(apiMethod)).Returns(this.responseTroubleshooterCommentCollection);

            var troubleshooterComment = this.troubleshooterController.GetTroubleshooterComment(troubleshooterCommentId);

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<TroubleshooterCommentCollection>(apiMethod), Times.Once());

            Assert.That(troubleshooterComment, Is.EqualTo(this.responseTroubleshooterCommentCollection.First()));
        }

        [Test]
        public void CreateTroubleshooterComment()
        {
            var troubleshooterCommentRequest = new TroubleshooterCommentRequest
            {
                TroubleshooterStepId = 1,
                Contents = "Contents",
                CreatorType = TroubleshooterCommentCreatorType.User,
                CreatorId = 1,
                FullName = "FullName",
                Email = "email@domain.com",
                ParentCommentId = 3
            };

            const string apiMethod = "/Troubleshooter/Comment";
            const string parameters = "troubleshooterstepid=1&contents=Contents&creatortype=2&creatorid=1&fullname=FullName&email=email@domain.com&parentcommentid=3";

            this.kayakoApiRequest.Setup(x => x.ExecutePost<TroubleshooterCommentCollection>(apiMethod, parameters)).Returns(this.responseTroubleshooterCommentCollection);

            var troubleshooterComment = this.troubleshooterController.CreateTroubleshooterComment(troubleshooterCommentRequest);

            this.kayakoApiRequest.Verify(x => x.ExecutePost<TroubleshooterCommentCollection>(apiMethod, parameters), Times.Once());
            Assert.That(troubleshooterComment, Is.EqualTo(this.responseTroubleshooterCommentCollection.FirstOrDefault()));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void DeleteTroubleshooterComment(int troubleshooterCommentId)
        {
            var apiMethod = string.Format("/Troubleshooter/Comment/{0}", troubleshooterCommentId);

            this.kayakoApiRequest.Setup(x => x.ExecuteDelete(apiMethod)).Returns(true);

            var deleteSuccess = this.troubleshooterController.DeleteTroubleshooterComment(troubleshooterCommentId);

            this.kayakoApiRequest.Verify(x => x.ExecuteDelete(apiMethod), Times.Once());
            Assert.IsTrue(deleteSuccess);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetTroubleshooterAttachments(int troubleshooterStepId)
        {
            var apiMethod = string.Format("/Troubleshooter/Attachment/ListAll/{0}", troubleshooterStepId);
            this.kayakoApiRequest.Setup(x => x.ExecuteGet<TroubleshooterAttachmentCollection>(apiMethod)).Returns(this.responseTroubleshooterAttachmentCollection);

            var troubleshooterAttachments = this.troubleshooterController.GetTroubleshooterAttachments(troubleshooterStepId);

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<TroubleshooterAttachmentCollection>(apiMethod), Times.Once());

            Assert.That(troubleshooterAttachments, Is.EqualTo(this.responseTroubleshooterAttachmentCollection));
        }

        [TestCase(1, 1)]
        [TestCase(2, 3)]
        [TestCase(4, 6)]
        public void GetTroubleshooterAttachment(int troubleshooterStepId, int troubleshooterAttachmentId)
        {
            var apiMethod = string.Format("/Troubleshooter/Attachment/{0}/{1}", troubleshooterStepId, troubleshooterAttachmentId);
            this.kayakoApiRequest.Setup(x => x.ExecuteGet<TroubleshooterAttachmentCollection>(apiMethod)).Returns(this.responseTroubleshooterAttachmentCollection);

            var troubleshooterAttachment = this.troubleshooterController.GetTroubleshooterAttachment(troubleshooterStepId, troubleshooterAttachmentId);

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<TroubleshooterAttachmentCollection>(apiMethod), Times.Once());

            Assert.That(troubleshooterAttachment, Is.EqualTo(this.responseTroubleshooterAttachmentCollection.FirstOrDefault()));
        }

        [Test]
        public void CreateTroubleshooterAttachment()
        {
            var contents = Convert.ToBase64String(Encoding.UTF8.GetBytes("This is the file contents"));

            var troubleshooterAttachmentRequest = new TroubleshooterAttachmentRequest
            {
                TroubleshooterStepId = 1,
                FileName = "test.txt",
                Contents = contents
            };

            const string apiMethod = "/Troubleshooter/Attachment";
            var parameters = string.Format("troubleshooterstepid=1&filename=test.txt&contents={0}", contents);

            this.kayakoApiRequest.Setup(x => x.ExecutePost<TroubleshooterAttachmentCollection>(apiMethod, parameters)).Returns(this.responseTroubleshooterAttachmentCollection);

            var troubleshooterAttachment = this.troubleshooterController.CreateTroubleshooterAttachment(troubleshooterAttachmentRequest);

            this.kayakoApiRequest.Verify(x => x.ExecutePost<TroubleshooterAttachmentCollection>(apiMethod, parameters), Times.Once());
            Assert.That(troubleshooterAttachment, Is.EqualTo(this.responseTroubleshooterAttachmentCollection.FirstOrDefault()));
        }

        [TestCase(1, 1)]
        [TestCase(2, 3)]
        [TestCase(4, 5)]
        public void DeleteTroubleshooterAttachment(int troubleshooterStepId, int troubleshooterAttachmentId)
        {
            var apiMethod = string.Format("/Troubleshooter/Attachment/{0}/{1}", troubleshooterStepId, troubleshooterAttachmentId);

            this.kayakoApiRequest.Setup(x => x.ExecuteDelete(apiMethod)).Returns(true);

            var deleteSuccess = this.troubleshooterController.DeleteTroubleshooterAttachment(troubleshooterStepId, troubleshooterAttachmentId);

            this.kayakoApiRequest.Verify(x => x.ExecuteDelete(apiMethod), Times.Once());
            Assert.IsTrue(deleteSuccess);
        }
    }
}