using System.Xml.Serialization;

namespace KayakoRestApi.Core.Tickets.TicketCount
{
    /// <summary>
    ///     Represents a Ticket Count grouped by owner staff
    /// </summary>
    /// <remarks>
    ///     http://wiki.kayako.com/display/DEV/REST+-+TicketCount
    /// </remarks>
    public class OwnerStaff
    {
        /// <summary>
        ///     The unique numeric identifier of the owner staff
        /// </summary>
        [XmlAttribute("id")]
        public int Id { get; set; }

        /// <summary>
        ///     The time representing the last activity
        /// </summary>
        [XmlAttribute("lastactivity")]
        public long LastActivity { get; set; }

        /// <summary>
        ///     The total number of items
        /// </summary>
        [XmlAttribute("totalitems")]
        public int TotalItems { get; set; }

        /// <summary>
        ///     The total unresolved items
        /// </summary>
        [XmlAttribute("totalunresolveditems")]
        public int TotalUnresolvedItems { get; set; }
    }
}