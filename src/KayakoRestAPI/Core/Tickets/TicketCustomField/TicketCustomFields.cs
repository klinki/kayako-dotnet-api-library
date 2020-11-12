using System.Collections.Generic;
using System.Xml.Serialization;

namespace KayakoRestApi.Core.Tickets.TicketCustomField
{
    [XmlRoot("customfields")]
    public class TicketCustomFields
    {
        public TicketCustomFields() => this.FieldGroups = new List<TicketCustomFieldGroup>();

        [XmlElement("group")]
        public List<TicketCustomFieldGroup> FieldGroups { get; set; }
    }
}