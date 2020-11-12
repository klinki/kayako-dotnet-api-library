using System;
using System.Diagnostics;
using System.Globalization;
using KayakoRestApi.Core.Constants;
using KayakoRestApi.Core.Tickets.TicketTimeTrack;
using KayakoRestApi.IntegrationTests.TestBase;
using NUnit.Framework;

namespace KayakoRestApi.IntegrationTests
{
    [TestFixture(Description = "A set of tests testing Api methods around Ticket Time Tracks")]
    public class TicketTimeTrackTests : UnitTestBase
    {
        private TicketTimeTrack TestData
        {
            get
            {
                var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                var diff = DateTime.Now - origin;

                var ticket = TestSetup.KayakoApiService.Tickets.GetTickets(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 })[0];

                Assert.IsNotNull(ticket);

                var ticketTimeTrack = new TicketTimeTrack
                {
                    TicketId = ticket.Id,
                    BillDate = Math.Floor(diff.TotalSeconds).ToString(CultureInfo.InvariantCulture),
                    Contents = "Test Contents",
                    CreatorStaffId = 1,
                    TimeBillable = 5000,
                    TimeWorked = 6000,
                    WorkerStaffId = 1,
                    WorkDate = Math.Floor(diff.TotalSeconds).ToString(CultureInfo.InvariantCulture),
                    NoteColor = NoteColor.Green
                };
                
                return ticketTimeTrack;
            }
        }

        [Test]
        public void GetTicketTimeTracks()
        {
            var tickets = TestSetup.KayakoApiService.Tickets.GetTickets(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });

            foreach (var t in tickets)
            {
                var ticketTimeTracks = TestSetup.KayakoApiService.Tickets.GetTicketTimeTracks(t.Id);

                if (ticketTimeTracks.Count > 0)
                {
                    break;
                }
            }

            Assert.IsNotNull(tickets, "No ticket time tracks were returned");
            Assert.IsNotEmpty(tickets, "No ticket time tracks returned");
        }

        [Test]
        public void GetTicketTimeTrack()
        {
            var tickets = TestSetup.KayakoApiService.Tickets.GetTickets(new[] { 1, 2 });

            Assert.IsNotNull(tickets, "No tickets were returned");
            Assert.IsNotEmpty(tickets, "No tickets were returned");

            TicketTimeTrackCollection ticketTimeTracks = null;
            foreach (var t in tickets)
            {
                ticketTimeTracks = TestSetup.KayakoApiService.Tickets.GetTicketTimeTracks(t.Id);

                if (ticketTimeTracks.Count > 0)
                {
                    break;
                }
            }

            Assert.IsNotNull(ticketTimeTracks, "No ticket time tracks were returned");
            Assert.IsNotEmpty(ticketTimeTracks, "No ticket time tracks were returned");

            var randomTicketTimeTrackToGet = ticketTimeTracks[new Random().Next(ticketTimeTracks.Count)];

            Trace.WriteLine("GetTicketType using ticket time tracks id: " + randomTicketTimeTrackToGet.Id);

            var ticketTimeTrack = TestSetup.KayakoApiService.Tickets.GetTicketTimeTrack(randomTicketTimeTrackToGet.TicketId, randomTicketTimeTrackToGet.Id);

            this.CompareTicketTimeTracks(ticketTimeTrack, randomTicketTimeTrackToGet);
        }

        [Test(Description = "Tests creating, updating and deleting Ticket Time Tracks")]
        public void CreateUpdateDeleteTimeTracks()
        {
            var dummyData = this.TestData;

            var request = TicketTimeTrackRequest.FromResponseData(dummyData);

            var createdTicketTimeTrack = TestSetup.KayakoApiService.Tickets.AddTicketTimeTrackingNote(request);

            Assert.IsNotNull(createdTicketTimeTrack);
            dummyData.Id = createdTicketTimeTrack.Id;
            dummyData.CreatorStaffName = createdTicketTimeTrack.CreatorStaffName;
            dummyData.WorkerStaffName = createdTicketTimeTrack.WorkerStaffName;

            this.CompareTicketTimeTracks(dummyData, createdTicketTimeTrack);

            var success = TestSetup.KayakoApiService.Tickets.DeleteTicketTimeTrackingNote(createdTicketTimeTrack.TicketId, createdTicketTimeTrack.Id);

            Assert.IsTrue(success);
        }

        private void CompareTicketTimeTracks(TicketTimeTrack one, TicketTimeTrack two)
        {
            Assert.AreEqual(one.BillDate, two.BillDate);
            Assert.AreEqual(one.Contents, two.Contents);
            Assert.AreEqual(one.CreatorStaffId, two.CreatorStaffId);
            Assert.AreEqual(one.CreatorStaffName, two.CreatorStaffName);
            Assert.AreEqual(one.Id, two.Id);
            Assert.AreEqual(one.NoteColor, two.NoteColor);
            Assert.AreEqual(one.TicketId, two.TicketId);
            Assert.AreEqual(one.TimeBillable, two.TimeBillable);
            Assert.AreEqual(one.TimeWorked, two.TimeWorked);
            Assert.AreEqual(one.WorkDate, two.WorkDate);
            Assert.AreEqual(one.WorkerStaffId, two.WorkerStaffId);
            Assert.AreEqual(one.WorkerStaffName, two.WorkerStaffName);

            AssertObjectXmlEqual(one, two);
        }
    }
}