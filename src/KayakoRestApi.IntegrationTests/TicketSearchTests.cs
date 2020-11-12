using System;
using System.Linq;
using KayakoRestApi.Core.Departments;
using KayakoRestApi.Core.Tickets.TicketSearch;
using KayakoRestApi.IntegrationTests.TestBase;
using NUnit.Framework;

namespace KayakoRestApi.IntegrationTests
{
    [TestFixture(Description = "A set of tests testing Api methods around Ticket Statuses")]
    public class TicketSearchTests : UnitTestBase
    {
        [Test]
        public void DoTicketSearch()
        {
            var depts = TestSetup.KayakoApiService.Departments.GetDepartments();
            depts.Add(new Department { Id = 0 });

            var tickets = TestSetup.KayakoApiService.Tickets.GetTickets(depts.Select(d => d.Id).ToArray());

            var randomTicket = tickets[new Random().Next(tickets.Count)];

            var expectedSearchAmount = tickets.Count(t => t.Email.Equals(randomTicket.Email, StringComparison.InvariantCultureIgnoreCase));

            var query = new TicketSearchQuery(randomTicket.Email);
            query.AddSearchField(TicketSearchField.EmailAddress);

            var queriedTickets = TestSetup.KayakoApiService.Tickets.SearchTickets(query);

            Assert.AreEqual(expectedSearchAmount, queriedTickets.Count);
        }
    }
}