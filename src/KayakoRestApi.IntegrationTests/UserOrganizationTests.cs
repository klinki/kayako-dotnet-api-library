using System;
using System.Diagnostics;
using KayakoRestApi.Core.Constants;
using KayakoRestApi.Core.Users;
using KayakoRestApi.IntegrationTests.TestBase;
using KayakoRestApi.Utilities;
using NUnit.Framework;

namespace KayakoRestApi.IntegrationTests
{
    [TestFixture(Description = "A set of tests testing Api methods around User Organizations")]
    public class UserOrganizationTests : UnitTestBase
    {
        private UserOrganization TestData
        {
            get
            {
                var testUserOrg = new UserOrganization
                {
                    Address = "Test Address String",
                    City = "City String",
                    Country = "Country String",
                    Fax = "01234567890",
                    Name = "Name of the User Org",
                    OrganizationType = UserOrganizationType.Restricted,
                    Phone = "01987654321",
                    PostalCode = "BS30 6EL",
                    SlaPlanExpiry = 0,
                    SlaPlanId = 1,
                    State = "State String",
                    Website = "http://wwww.google.co.uk"
                };

                return testUserOrg;
            }
        }

        [Test]
        public void GetAllGetUserOrganizations()
        {
            var userOrganizations = TestSetup.KayakoApiService.Users.GetUserOrganizations();

            Assert.IsNotNull(userOrganizations, "No user organizations were returned");
            Assert.IsNotEmpty(userOrganizations, "No user organizations were returned");
        }

        [Test]
        public void GetUserOrganization()
        {
            var userOrganizations = TestSetup.KayakoApiService.Users.GetUserOrganizations();

            Assert.IsNotNull(userOrganizations, "No user organizations were returned");
            Assert.IsNotEmpty(userOrganizations, "No user organizations were returned");

            var userOrgToGet = userOrganizations[new Random().Next(userOrganizations.Count)];

            Trace.WriteLine("GetUserOrganization using user organization id: " + userOrgToGet.Id);

            var userOrganization = TestSetup.KayakoApiService.Users.GetUserOrganization(userOrgToGet.Id);

            this.CompareUserOrganizations(userOrganization, userOrgToGet);
        }

        [Test(Description = "Tests creating, updating and deleting user organizations")]
        public void CreateUpdateDeleteUserOrganization()
        {
            var dummyData = this.TestData;

            var createdUserOrg = TestSetup.KayakoApiService.Users.CreateUserOrganization(UserOrganizationRequest.FromResponseData(dummyData));

            Assert.IsNotNull(createdUserOrg);
            dummyData.Id = createdUserOrg.Id;

            dummyData.Address = "UPDATED: Test Address String";
            dummyData.City = "UPDATED: City String";
            dummyData.Country = "UPDATED: Country String";
            dummyData.Fax = "05555666444";
            dummyData.Name = "UPDATED: Name";
            dummyData.OrganizationType = UserOrganizationType.Shared;
            dummyData.Phone = "02223334455";
            dummyData.PostalCode = "BS8 1UB";

            dummyData.SlaPlanExpiry = UnixTimeUtility.ToUnixTime(DateTime.Now);
            dummyData.SlaPlanId = 1;
            dummyData.State = "UPDATED: State String";
            dummyData.Website = "http://wwww.test.com";

            var updatedUserOrg = TestSetup.KayakoApiService.Users.UpdateUserOrganization(UserOrganizationRequest.FromResponseData(dummyData));
            dummyData.Dateline = updatedUserOrg.Dateline;
            dummyData.LastUpdated = updatedUserOrg.LastUpdated;

            Assert.IsNotNull(updatedUserOrg);
            this.CompareUserOrganizations(dummyData, updatedUserOrg);

            var success = TestSetup.KayakoApiService.Users.DeleteUserOrganization(updatedUserOrg.Id);

            Assert.IsTrue(success);
        }

        private void CompareUserOrganizations(UserOrganization one, UserOrganization two)
        {
            Assert.AreEqual(one.Address, two.Address);
            Assert.AreEqual(one.City, two.City);
            Assert.AreEqual(one.Country, two.Country);
            Assert.IsTrue(one.Dateline.Equals(two.Dateline));
            Assert.AreEqual(one.Fax, two.Fax);
            Assert.AreEqual(one.Id, two.Id);

            Assert.IsTrue(one.LastUpdated.Equals(two.LastUpdated));
            Assert.AreEqual(one.Name, two.Name);
            Assert.AreEqual(one.OrganizationType, two.OrganizationType);
            Assert.AreEqual(one.Phone, two.Phone);
            Assert.AreEqual(one.PostalCode, two.PostalCode);
            Assert.IsTrue(one.SlaPlanExpiry.Equals(two.SlaPlanExpiry));
            Assert.AreEqual(one.SlaPlanId, two.SlaPlanId);
            Assert.AreEqual(one.State, two.State);
            Assert.AreEqual(one.Website, two.Website);

            AssertObjectXmlEqual(one, two);
        }
    }
}