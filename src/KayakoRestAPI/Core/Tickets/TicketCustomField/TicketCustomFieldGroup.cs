using System.Xml.Serialization;

namespace KayakoRestApi.Core.Tickets.TicketCustomField
{
    [XmlRoot("group")]
    public class TicketCustomFieldGroup
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("title")]
        public string Title { get; set; }

        [XmlElement("field")]
        public TicketCustomField[] Fields { get; set; }
    }
}