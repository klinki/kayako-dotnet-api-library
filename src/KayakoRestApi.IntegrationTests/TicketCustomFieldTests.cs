using System;
using System.Linq;
using KayakoRestApi.Core.Constants;
using KayakoRestApi.IntegrationTests.TestBase;
using NUnit.Framework;

namespace KayakoRestApi.IntegrationTests
{
    [TestFixture(Description = "A set of tests testing Api methods around Ticket Custom Fields")]
    public class TicketCustomFieldTests : UnitTestBase
    {
        [Test]
        public void GetTicketsCustomFields()
        {
            var ticketCustomFields = TestSetup.KayakoApiService.Tickets.GetTicketCustomFields(2);

            Assert.IsNotNull(ticketCustomFields.FieldGroups, "No ticket custom fields were returned");
            Assert.IsNotEmpty(ticketCustomFields.FieldGroups, "No ticket custom fields were returned");

            this.OutputMessage("Result: ", ticketCustomFields);
        }

        [Test]
        public void UpdateTicketCustomFields()
        {
            var depts = TestSetup.KayakoApiService.Departments.GetDepartments();
            var tickets = TestSetup.KayakoApiService.Tickets.GetTickets(depts.Select(d => d.Id).ToArray());

            var idToUse = -1;

            foreach (var ticket in tickets)
            {
                var ticketCustomFields = TestSetup.KayakoApiService.Tickets.GetTicketCustomFields(ticket.Id);

                if (ticketCustomFields.FieldGroups.Count > 0)
                {
                    if (ticketCustomFields.FieldGroups.Any(tcf => tcf.Fields.Length > 0 && tcf.Fields.Any(a => a.Type == TicketCustomFieldType.Text || a.Type == TicketCustomFieldType.TextArea)))
                    {
                        idToUse = ticket.Id;
                        break;
                    }
                }
            }

            if (idToUse != -1)
            {
                var ticketCustomFields = TestSetup.KayakoApiService.Tickets.GetTicketCustomFields(idToUse);

                var group = ticketCustomFields.FieldGroups.FirstOrDefault(tcf => tcf.Fields.Length > 0 && tcf.Fields.Any(a => a.Type == TicketCustomFieldType.Text || a.Type == TicketCustomFieldType.TextArea));
                var field = group.Fields.FirstOrDefault(type => type.Type == TicketCustomFieldType.Text || type.Type == TicketCustomFieldType.TextArea);
                field.FieldContent = string.Format("This was updated at : {0:dd/MM/yyyy HH:mm:ss}", DateTime.Now);

                var updatedTicketCustomFields = TestSetup.KayakoApiService.Tickets.UpdateTicketCustomFields(idToUse, ticketCustomFields);

                var updatedGroup = updatedTicketCustomFields.FieldGroups.FirstOrDefault(customField => customField.Fields.Length > 0 && customField.Fields.Any(a => a.Type == TicketCustomFieldType.Text || a.Type == TicketCustomFieldType.TextArea));
                var updatedField = updatedGroup.Fields.FirstOrDefault(type => type.Type == TicketCustomFieldType.Text || type.Type == TicketCustomFieldType.TextArea);

                Assert.AreEqual(field.FieldContent, updatedField.FieldContent);
            }
            else
            {
                throw new Exception("Could not find any tickets with any text/text area custom fields.");
            }
        }
    }
}