using System.Collections.Generic;
using System.Xml.Serialization;

namespace KayakoRestApi.Core.Tickets.Ticket
{
    /// <summary>
    ///     Definition of a list end tickets.
    ///     <remarks>
    ///         See : http://wiki.kayako.com/display/DEV/REST+-+Ticket
    ///     </remarks>
    /// </summary>
    [XmlRoot("tickets")]
    public class TicketCollection : List<Ticket> { }
}