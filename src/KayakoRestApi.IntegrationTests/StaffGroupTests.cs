using System;
using System.Diagnostics;
using KayakoRestApi.Core.Staff;
using KayakoRestApi.IntegrationTests.TestBase;
using NUnit.Framework;

namespace KayakoRestApi.IntegrationTests
{
    [TestFixture(Description = "A set of tests testing Api methods around Staff Groups")]
    public class StaffGroupTests : UnitTestBase
    {
        private static StaffGroup TestData => new StaffGroup { IsAdmin = false, Title = "Staff Group Test" };

        [Test]
        public void GetAllStaffGroups()
        {
            var staffGroups = TestSetup.KayakoApiService.Staff.GetStaffGroups();

            Assert.IsNotNull(staffGroups, "No staff groups were returned");
            Assert.IsNotEmpty(staffGroups, "No staff groups were returned");
        }

        [Test]
        public void GetStaffGroup()
        {
            var staffGroups = TestSetup.KayakoApiService.Staff.GetStaffGroups();

            Assert.IsNotNull(staffGroups, "No staff groups were returned");
            Assert.IsNotEmpty(staffGroups, "No staff groups were returned");

            var staffGroupToGet = staffGroups[new Random().Next(staffGroups.Count)];

            Trace.WriteLine("GetStaffGroup using staff group id: " + staffGroupToGet.Id);

            var staffGroup = TestSetup.KayakoApiService.Staff.GetStaffGroup(staffGroupToGet.Id);

            this.CompareStaffGroups(staffGroup, staffGroupToGet);
        }

        [Test(Description = "Tests creating, updating and deleting staff groups")]
        public void CreateUpdateDeleteStaffGroup()
        {
            var dummyStaffGroup = TestData;

            var createdStaffGroup = TestSetup.KayakoApiService.Staff.CreateStaffGroup(StaffGroupRequest.FromResponseData(dummyStaffGroup));

            Assert.IsNotNull(createdStaffGroup);
            dummyStaffGroup.Id = createdStaffGroup.Id;
            this.CompareStaffGroups(dummyStaffGroup, createdStaffGroup);

            dummyStaffGroup.IsAdmin = true;
            dummyStaffGroup.Title = "UPDATED: Staff Group Test";

            var updatedStaffGroup = TestSetup.KayakoApiService.Staff.UpdateStaffGroup(StaffGroupRequest.FromResponseData(dummyStaffGroup));

            Assert.IsNotNull(updatedStaffGroup);
            this.CompareStaffGroups(dummyStaffGroup, updatedStaffGroup);

            var success = TestSetup.KayakoApiService.Staff.DeleteStaffGroup(updatedStaffGroup.Id);

            Assert.IsTrue(success);
        }

        private void CompareStaffGroups(StaffGroup one, StaffGroup two)
        {
            Assert.AreEqual(one.Id, two.Id);
            Assert.AreEqual(one.IsAdmin, two.IsAdmin);
            Assert.AreEqual(one.Title, two.Title);

            AssertObjectXmlEqual(one, two);
        }
    }
}