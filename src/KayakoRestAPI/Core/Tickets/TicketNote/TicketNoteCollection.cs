using System.Collections.Generic;
using System.Xml.Serialization;

namespace KayakoRestApi.Core.Tickets.TicketNote
{
    /// <summary>
    ///     Definition of a list of ticket notes
    ///     <remarks>
    ///         See : http://wiki.kayako.com/display/DEV/REST+-+TicketNote
    ///     </remarks>
    /// </summary>
    [XmlRoot("notes")]
    public class TicketNoteCollection : List<TicketNote> { }
}