using System.Collections.Generic;
using System.Xml.Serialization;

namespace KayakoRestApi.Core.Tickets.TicketTimeTrack
{
    /// <summary>
    ///     Definition of a list of ticket time tracks
    ///     <remarks>
    ///         See : http://wiki.kayako.com/display/DEV/REST+-+TicketTimeTrack
    ///     </remarks>
    /// </summary>
    [XmlRoot("timetracks")]
    public class TicketTimeTrackCollection : List<TicketTimeTrack> { }
}