using System.Collections.Generic;
using System.Xml.Serialization;

namespace KayakoRestApi.Core.Tickets.TicketPost
{
    /// <summary>
    ///     Definition of a list of ticket posts
    ///     <remarks>
    ///         See : http://wiki.kayako.com/display/DEV/REST+-+TicketPost
    ///     </remarks>
    /// </summary>
    [XmlRoot("posts")]
    public class TicketPostCollection : List<TicketPost> { }
}