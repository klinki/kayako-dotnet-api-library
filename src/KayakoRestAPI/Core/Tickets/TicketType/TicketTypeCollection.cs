using System.Collections.Generic;
using System.Xml.Serialization;

namespace KayakoRestApi.Core.Tickets.TicketType
{
    /// <summary>
    ///     Definition of a list of ticket types
    ///     <remarks>
    ///         See : http://wiki.kayako.com/display/DEV/REST+-+TicketType
    ///     </remarks>
    /// </summary>
    [XmlRoot("tickettypes")]
    public class TicketTypeCollection : List<TicketType> { }
}