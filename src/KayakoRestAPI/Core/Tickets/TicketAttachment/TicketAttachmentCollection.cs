using System.Collections.Generic;
using System.Xml.Serialization;

namespace KayakoRestApi.Core.Tickets.TicketAttachment
{
    [XmlRoot("attachments")]
    public class TicketAttachmentCollection : List<TicketAttachment> { }
}