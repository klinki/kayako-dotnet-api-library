using System;
using System.Collections.Generic;
using System.Diagnostics;
using KayakoRestApi.Core.Constants;
using KayakoRestApi.Core.Departments;
using KayakoRestApi.IntegrationTests.TestBase;
using NUnit.Framework;

namespace KayakoRestApi.IntegrationTests
{
    [TestFixture(Description = "A set of tests testing Api methods around Departments")]
    public class DepartmentTests : UnitTestBase
    {
        private static Department TestData
            => new Department
            {
                Title = "Test Department",
                Type = DepartmentType.Public,
                Module = DepartmentModule.Tickets,
                DisplayOrder = 16,
                ParentDepartmentId = 0,
                UserVisibilityCustom = true,
                UserGroups = new List<int> { 1, 2, 3 }
            };

        [Test]
        public void GetAllDepartments()
        {
            var departments = TestSetup.KayakoApiService.Departments.GetDepartments();

            Assert.IsNotNull(departments, "No departments were returned");
            Assert.IsNotEmpty(departments, "No departments were returned");
        }

        [Test]
        public void GetDepartment()
        {
            var departments = TestSetup.KayakoApiService.Departments.GetDepartments();

            Assert.IsNotNull(departments, "No departments were returned");
            Assert.IsNotEmpty(departments, "No departments were returned");

            var deptToGet = departments[new Random().Next(departments.Count)];

            Trace.WriteLine("GetDepartment using department id: " + deptToGet.Id);

            var dept = TestSetup.KayakoApiService.Departments.GetDepartment(deptToGet.Id);

            this.CompareDepartments(dept, deptToGet);
        }

        [Test(Description = "Tests creating, updating and deleting departments")]
        public void CreateUpdateDeleteDepartment()
        {
            var dummyData = TestData;

            var createdDept = TestSetup.KayakoApiService.Departments.CreateDepartment(DepartmentRequest.FromResponseData(dummyData));

            Assert.IsNotNull(createdDept);
            dummyData.Id = createdDept.Id;
            this.CompareDepartments(dummyData, createdDept);

            dummyData.Title = "Updated Title";
            dummyData.Type = DepartmentType.Private;
            dummyData.DisplayOrder = 34;
            dummyData.UserVisibilityCustom = false;
            dummyData.UserGroups = new List<int>();

            var updatedDept = TestSetup.KayakoApiService.Departments.UpdateDepartment(DepartmentRequest.FromResponseData(dummyData));

            Assert.IsNotNull(updatedDept);
            this.CompareDepartments(dummyData, updatedDept);

            var success = TestSetup.KayakoApiService.Departments.DeleteDepartment(updatedDept.Id);

            Assert.IsTrue(success);
        }

        private void CompareDepartments(Department one, Department two)
        {
            Assert.AreEqual(one.Id, two.Id);
            Assert.AreEqual(one.Title, two.Title);
            Assert.AreEqual(one.Type, two.Type);
            Assert.AreEqual(one.Module, two.Module);
            Assert.AreEqual(one.ParentDepartmentId, two.ParentDepartmentId);
            Assert.AreEqual(one.DisplayOrder, two.DisplayOrder);
            Assert.AreEqual(one.UserVisibilityCustom, two.UserVisibilityCustom);
            Assert.AreEqual(one.UserGroups, two.UserGroups);

            AssertObjectXmlEqual(one, two);
        }
    }
}