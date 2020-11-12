using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using KayakoRestApi.Core.Tickets.TicketAttachment;
using KayakoRestApi.IntegrationTests.TestBase;
using NUnit.Framework;

namespace KayakoRestApi.IntegrationTests
{
    [TestFixture(Description = "A set of tests testing Api methods around Ticket Attachments")]
    public class TicketAttachmentTests : UnitTestBase
    {
        [Test]
        public void GetAllTicketAttachments()
        {
            var depts = TestSetup.KayakoApiService.Departments.GetDepartments();
            var ticket = TestSetup.KayakoApiService.Tickets.GetTicket(1);

            var attachments = TestSetup.KayakoApiService.Tickets.GetTicketAttachments(ticket.Id);

            Assert.IsNotNull(attachments, "No ticket attachments were returned for ticket id " + ticket.Id);
            Assert.IsNotEmpty(attachments, "No ticket attachments were returned for ticket id " + ticket.Id);
        }

        [Test]
        public void GetTicketAttachment()
        {
            var depts = TestSetup.KayakoApiService.Departments.GetDepartments();
            var ticket = TestSetup.KayakoApiService.Tickets.GetTicket(1);

            var attachments = TestSetup.KayakoApiService.Tickets.GetTicketAttachments(ticket.Id);

            Assert.IsNotNull(attachments, "No ticket attachments were returned for ticket id " + ticket.Id);
            Assert.IsNotEmpty(attachments, "No ticket attachments were returned for ticket id " + ticket.Id);

            var randomTicketAttachmentToGet = attachments[new Random().Next(attachments.Count)];

            Trace.WriteLine("GetTicketAttachment using ticket attachment id: " + randomTicketAttachmentToGet.Id);

            var ticketNote = TestSetup.KayakoApiService.Tickets.GetTicketAttachment(ticket.Id, randomTicketAttachmentToGet.Id);

            this.CompareTicketAttachment(ticketNote, randomTicketAttachmentToGet);
        }

        [Test(Description = "Tests creating and deleting ticket attachment")]
        public void CreateDeleteTicketAttachment()
        {
            var depts = TestSetup.KayakoApiService.Departments.GetDepartments();
            var staff = TestSetup.KayakoApiService.Staff.GetStaffUsers();
            var randomStaffUser = staff[new Random().Next(staff.Count)];
            var tickets = TestSetup.KayakoApiService.Tickets.GetTickets(depts.Select(d => d.Id).ToArray());
            var randomTicket = tickets[new Random().Next(tickets.Count)];
            var ticketPosts = TestSetup.KayakoApiService.Tickets.GetTicketPosts(randomTicket.Id);
            var randomPost = ticketPosts[new Random().Next(ticketPosts.Count)];

            var contents = Convert.ToBase64String(Encoding.UTF8.GetBytes("This is the file contents"));

            var request = new TicketAttachmentRequest
            {
                TicketId = randomTicket.Id,
                TicketPostId = randomPost.Id,
                FileName = "TheFilename.txt",
                Contents = contents
            };

            var createdAttachment = TestSetup.KayakoApiService.Tickets.AddTicketAttachment(request);

            Assert.AreEqual(createdAttachment.TicketId, randomTicket.Id);
            Assert.AreEqual(createdAttachment.TicketPostId, randomPost.Id);
            Assert.AreEqual(createdAttachment.FileName, "TheFilename.txt");

            //Assert.AreEqual(createdAttachment.Contents, contents);

            var success = TestSetup.KayakoApiService.Tickets.DeleteTicketAttachment(randomTicket.Id, createdAttachment.Id);

            Assert.IsTrue(success);
        }

        private void CompareTicketAttachment(TicketAttachment one, TicketAttachment two)
        {
            Assert.AreEqual(one.Dateline, two.Dateline);
            Assert.AreEqual(one.FileName, two.FileName);
            Assert.AreEqual(one.FileSize, two.FileSize);
            Assert.AreEqual(one.FileType, two.FileType);
            Assert.AreEqual(one.Id, two.Id);
            Assert.AreEqual(one.TicketId, two.TicketId);
            Assert.AreEqual(one.TicketPostId, two.TicketPostId);

            //AssertObjectXmlEqual<TicketAttachment>(one, two);
        }
    }
}