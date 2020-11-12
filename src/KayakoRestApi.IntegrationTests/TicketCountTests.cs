using KayakoRestApi.IntegrationTests.TestBase;
using NUnit.Framework;

namespace KayakoRestApi.IntegrationTests
{
    [TestFixture(Description = "A set of tests testing Api methods around Ticket Counts")]
    public class TicketCountTests : UnitTestBase
    {
        [Test]
        public void GetAllTicketCount()
        {
            var ticketCount = TestSetup.KayakoApiService.Tickets.GetTicketCounts();

            Assert.IsNotNull(ticketCount, "No ticket counts were returned");

            this.OutputMessage("Ticket Count Result: ", ticketCount);
        }
    }
}