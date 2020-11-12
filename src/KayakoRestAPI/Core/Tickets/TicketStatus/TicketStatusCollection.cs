using System.Collections.Generic;
using System.Xml.Serialization;

namespace KayakoRestApi.Core.Tickets.TicketStatus
{
    /// <summary>
    ///     Definition of a list Ticket Statuses.
    ///     <remarks>
    ///         See : http://wiki.kayako.com/display/DEV/REST+-+TicketStatus
    ///     </remarks>
    /// </summary>
    [XmlRoot("ticketstatuses")]
    public class TicketStatusCollection : List<TicketStatus> { }
}