using System.Collections.Generic;
using System.Xml.Serialization;

namespace KayakoRestApi.Core.Tickets.TicketPriority
{
    /// <summary>
    ///     Definition of a list of Ticket Priorities.
    ///     <remarks>
    ///         See : http://wiki.kayako.com/display/DEV/REST+-+TicketPriority
    ///     </remarks>
    /// </summary>
    [XmlRoot("ticketpriorities")]
    public class TicketPriorityCollection : List<TicketPriority> { }
}