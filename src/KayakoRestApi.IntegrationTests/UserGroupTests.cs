using System;
using System.Diagnostics;
using KayakoRestApi.Core.Constants;
using KayakoRestApi.Core.Users;
using KayakoRestApi.IntegrationTests.TestBase;
using NUnit.Framework;

namespace KayakoRestApi.IntegrationTests
{
    [TestFixture(Description = "A set of tests testing Api methods around User Groups")]
    public class UserGroupTests : UnitTestBase
    {
        private static UserGroup TestData => new UserGroup { GroupType = UserGroupType.Guest, IsMaster = false, Title = "Title User Group" };

        [Test]
        public void GetAllGetUserGroups()
        {
            var userOrganizations = TestSetup.KayakoApiService.Users.GetUserGroups();

            Assert.IsNotNull(userOrganizations, "No user groups were returned");
            Assert.IsNotEmpty(userOrganizations, "No user groups were returned");
        }

        [Test]
        public void GetUserGroup()
        {
            var userGroups = TestSetup.KayakoApiService.Users.GetUserGroups();

            Assert.IsNotNull(userGroups, "No user groups were returned");
            Assert.IsNotEmpty(userGroups, "No user groups were returned");

            var userGroupToGet = userGroups[new Random().Next(userGroups.Count)];

            Trace.WriteLine("GetUserGroup using user group id: " + userGroupToGet.Id);

            var userGroup = TestSetup.KayakoApiService.Users.GetUserGroup(userGroupToGet.Id);

            this.CompareUserGroup(userGroup, userGroupToGet);
        }

        [Test(Description = "Tests creating, updating and deleting user groups")]
        public void CreateUpdateDeleteUserGroup()
        {
            var dummyData = TestData;

            var createdUserGroup = TestSetup.KayakoApiService.Users.CreateUserGroup(UserGroupRequest.FromResponseData(dummyData));

            Assert.IsNotNull(createdUserGroup);
            dummyData.Id = createdUserGroup.Id;
            this.CompareUserGroup(dummyData, createdUserGroup);

            dummyData.Title = "UPDATED: User Group Title";

            var updatedUserGroup = TestSetup.KayakoApiService.Users.UpdateUserGroup(UserGroupRequest.FromResponseData(dummyData));

            Assert.IsNotNull(updatedUserGroup);
            this.CompareUserGroup(dummyData, updatedUserGroup);

            var success = TestSetup.KayakoApiService.Users.DeleteUserGroup(updatedUserGroup.Id);

            Assert.IsTrue(success);
        }

        private void CompareUserGroup(UserGroup one, UserGroup two)
        {
            Assert.AreEqual(one.GroupType, two.GroupType);
            Assert.AreEqual(one.Id, two.Id);
            Assert.AreEqual(one.IsMaster, two.IsMaster);
            Assert.AreEqual(one.Title, two.Title);

            AssertObjectXmlEqual(one, two);
        }
    }
}