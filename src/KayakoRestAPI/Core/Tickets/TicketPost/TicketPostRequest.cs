using KayakoRestApi.RequestBase;
using KayakoRestApi.RequestBase.Attributes;

namespace KayakoRestApi.Core.Tickets.TicketPost
{
    public class TicketPostRequest : RequestBaseObject
    {
        /// <summary>
        ///     The unique numeric identifier of the ticket.
        /// </summary>
        [RequiredField]
        public int TicketId { get; set; }

        /// <summary>
        ///     The ticket post subject
        /// </summary>
        [RequiredField]
        public string Subject { get; set; }

        /// <summary>
        ///     The ticket post contents
        /// </summary>
        [RequiredField]
        public string Contents { get; set; }

        /// <summary>
        ///     The User Id, if the ticket post is to be created as a user
        /// </summary>
        [EitherField("StaffId")]
        public int? UserId { get; set; }

        /// <summary>
        ///     The Staff Id, if the ticket post is to be created as a staff
        /// </summary>
        [EitherField("UserId")]
        public int? StaffId { get; set; }

        /// <summary>
        ///     Indicates whether the ticket post is private (hidden from the customer) or not. Applies only to post created by
        ///     staff user. This parameter needs to be declared either 0 or 1 in case you are creating ticket using staffid
        /// </summary>
        public bool? IsPrivate { get; set; }

        public static TicketPostRequest FromResponseData(TicketPost responseData) => FromResponseType<TicketPost, TicketPostRequest>(responseData);

        public static TicketPost ToResponseData(TicketPostRequest requestData) => ToResponseType<TicketPostRequest, TicketPost>(requestData);
    }
}