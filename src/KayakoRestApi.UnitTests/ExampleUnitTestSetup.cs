using System.Diagnostics;
using KayakoRestApi.Controllers;
using KayakoRestApi.Core.Departments;
using Moq;
using NUnit.Framework;

namespace KayakoRestApi.UnitTests
{
    [TestFixture]
    public class ExampleUnitTestSetup
    {
        [SetUp]
        public void Setup()
        {
            this.coreController = new Mock<ICoreController>();
            this.customFieldController = new Mock<ICustomFieldController>();
            this.departmentController = new Mock<IDepartmentController>();
            this.knowledgebaseController = new Mock<IKnowledgebaseController>();
            this.newsController = new Mock<INewsController>();
            this.staffController = new Mock<IStaffController>();
            this.ticketController = new Mock<ITicketController>();
            this.troubleshooterController = new Mock<ITroubleshooterController>();
            this.userController = new Mock<IUserController>();

            this.kayakoClient = new Mock<IKayakoClient>();
            this.kayakoClient.Setup(x => x.Core).Returns(this.coreController.Object);
            this.kayakoClient.Setup(x => x.CustomFields).Returns(this.customFieldController.Object);
            this.kayakoClient.Setup(x => x.Departments).Returns(this.departmentController.Object);
            this.kayakoClient.Setup(x => x.Knowledgebase).Returns(this.knowledgebaseController.Object);
            this.kayakoClient.Setup(x => x.News).Returns(this.newsController.Object);
            this.kayakoClient.Setup(x => x.Staff).Returns(this.staffController.Object);
            this.kayakoClient.Setup(x => x.Tickets).Returns(this.ticketController.Object);
            this.kayakoClient.Setup(x => x.Troubleshooter).Returns(this.troubleshooterController.Object);
            this.kayakoClient.Setup(x => x.Users).Returns(this.userController.Object);
        }

        private Mock<IKayakoClient> kayakoClient;
        private Mock<ICoreController> coreController;
        private Mock<ICustomFieldController> customFieldController;
        private Mock<IDepartmentController> departmentController;
        private Mock<IKnowledgebaseController> knowledgebaseController;
        private Mock<INewsController> newsController;
        private Mock<IStaffController> staffController;
        private Mock<ITicketController> ticketController;
        private Mock<ITroubleshooterController> troubleshooterController;
        private Mock<IUserController> userController;

        [Test]
        public void ListDepartments()
        {
            var departmentCollection = new DepartmentCollection
            {
                new Department { Title = "Department 1" },
                new Department { Title = "Department 2" },
                new Department { Title = "Department 3" }
            };

            this.departmentController.Setup(x => x.GetDepartments()).Returns(departmentCollection);

            var departments = this.kayakoClient.Object.Departments.GetDepartments();

            Assert.That(departments, Is.EqualTo(departmentCollection));

            foreach (var department in departments)
            {
                Trace.WriteLine(department.Title);
            }
        }
    }
}