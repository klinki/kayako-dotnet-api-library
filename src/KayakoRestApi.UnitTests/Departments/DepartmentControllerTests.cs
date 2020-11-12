using System.Linq;
using KayakoRestApi.Controllers;
using KayakoRestApi.Core.Constants;
using KayakoRestApi.Core.Departments;
using KayakoRestApi.Net;
using KayakoRestApi.Text;
using KayakoRestApi.Utilities;
using Moq;
using NUnit.Framework;

namespace KayakoRestApi.UnitTests.Departments
{
    [TestFixture]
    public class DepartmentControllerTests
    {
        [SetUp]
        public void SetUp()
        {
            this.kayakoApiRequest = new Mock<IKayakoApiRequest>();

            this.departmentController = new DepartmentController(this.kayakoApiRequest.Object);
        }

        private IDepartmentController departmentController;
        private Mock<IKayakoApiRequest> kayakoApiRequest;

        [Test]
        public void GetAllDepartments()
        {
            var departments = new DepartmentCollection { new Department { Title = "Title", DisplayOrder = 2, Type = DepartmentType.Public } };
            this.kayakoApiRequest.Setup(x => x.ExecuteGet<DepartmentCollection>(ApiBaseMethods.Departments)).Returns(departments);

            var departmentsResult = this.departmentController.GetDepartments();

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<DepartmentCollection>(ApiBaseMethods.Departments));
            Assert.That(departmentsResult, Is.EqualTo(departments));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetDepartment(int departmentId)
        {
            var departments = new DepartmentCollection { new Department { Title = "Title", DisplayOrder = 2, Type = DepartmentType.Public } };

            var apiMethod = string.Format("{0}/{1}", ApiBaseMethods.Departments, departmentId);
            this.kayakoApiRequest.Setup(x => x.ExecuteGet<DepartmentCollection>(apiMethod)).Returns(departments);

            var departmentsResult = this.departmentController.GetDepartment(departmentId);

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<DepartmentCollection>(apiMethod), Times.Once());
            Assert.That(departmentsResult, Is.EqualTo(departments.First()));
        }

        [Test]
        public void CreateDepartment()
        {
            var departmentRequest = new DepartmentRequest { Title = "Title", DisplayOrder = 2, Type = DepartmentType.Public };
            var departments = new DepartmentCollection { new Department { Title = "Title", DisplayOrder = 2, Type = DepartmentType.Public } };

            var parameters = new RequestBodyBuilder();
            parameters.AppendRequestData("title", departmentRequest.Title);
            parameters.AppendRequestData("type", EnumUtility.ToApiString(departmentRequest.Type));
            parameters.AppendRequestData("module", EnumUtility.ToApiString(departmentRequest.Module));
            parameters.AppendRequestData("displayorder", departmentRequest.DisplayOrder);
            parameters.AppendRequestData("uservisibilitycustom", 0);

            this.kayakoApiRequest.Setup(x => x.ExecutePost<DepartmentCollection>(ApiBaseMethods.Departments, parameters.ToString())).Returns(departments);

            var departmentCreated = this.departmentController.CreateDepartment(departmentRequest);

            this.kayakoApiRequest.Verify(x => x.ExecutePost<DepartmentCollection>(ApiBaseMethods.Departments, parameters.ToString()), Times.Once());
            Assert.That(departmentCreated, Is.EqualTo(departments.First()));
        }

        [Test]
        public void UpdateDepartment()
        {
            var departmentRequest = new DepartmentRequest { Title = "Title", DisplayOrder = 2, Type = DepartmentType.Public, Id = 12 };
            var departments = new DepartmentCollection { new Department { Title = "Title", DisplayOrder = 2, Type = DepartmentType.Public } };

            var apiMethod = string.Format("{0}/{1}", ApiBaseMethods.Departments, departmentRequest.Id);

            var parameters = new RequestBodyBuilder();
            parameters.AppendRequestData("title", departmentRequest.Title);
            parameters.AppendRequestData("type", EnumUtility.ToApiString(departmentRequest.Type));
            parameters.AppendRequestData("displayorder", departmentRequest.DisplayOrder);
            parameters.AppendRequestData("uservisibilitycustom", 0);

            this.kayakoApiRequest.Setup(x => x.ExecutePut<DepartmentCollection>(apiMethod, parameters.ToString())).Returns(departments);

            var departmentUpdated = this.departmentController.UpdateDepartment(departmentRequest);

            this.kayakoApiRequest.Verify(x => x.ExecutePut<DepartmentCollection>(apiMethod, parameters.ToString()), Times.Once());
            Assert.That(departmentUpdated, Is.EqualTo(departments.First()));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void DeleteDepartment(int departmentId)
        {
            var apiMethod = string.Format("{0}/{1}", ApiBaseMethods.Departments, departmentId);
            this.kayakoApiRequest.Setup(x => x.ExecuteDelete(apiMethod)).Returns(true);

            var deleteResult = this.departmentController.DeleteDepartment(departmentId);

            this.kayakoApiRequest.Verify(x => x.ExecuteDelete(apiMethod), Times.Once());
            Assert.That(deleteResult, Is.EqualTo(true));
        }
    }
}