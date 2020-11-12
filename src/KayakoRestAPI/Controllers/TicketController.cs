using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;
using KayakoRestApi.Core.Tickets.Ticket;
using KayakoRestApi.Core.Tickets.TicketAttachment;
using KayakoRestApi.Core.Tickets.TicketCount;
using KayakoRestApi.Core.Tickets.TicketCustomField;
using KayakoRestApi.Core.Tickets.TicketNote;
using KayakoRestApi.Core.Tickets.TicketPost;
using KayakoRestApi.Core.Tickets.TicketPriority;
using KayakoRestApi.Core.Tickets.TicketSearch;
using KayakoRestApi.Core.Tickets.TicketStatus;
using KayakoRestApi.Core.Tickets.TicketTimeTrack;
using KayakoRestApi.Core.Tickets.TicketType;
using KayakoRestApi.Net;
using KayakoRestApi.RequestBase;
using KayakoRestApi.Text;
using KayakoRestApi.Utilities;

namespace KayakoRestApi.Controllers
{
    public interface ITicketController
    {
        #region Ticket Count Api Methods

        TicketCount GetTicketCounts();

        #endregion

        #region Ticket Search Methods

        TicketCollection SearchTickets(TicketSearchQuery query);

        #endregion

        #region Ticket Api Methods

        TicketCollection GetTickets(int[] departmentIds);

        TicketCollection GetTickets(int[] departmentIds, int count, int start);

        TicketCollection GetTickets(int[] departmentIds, int[] ticketStatusIds, int[] ownerStaffIds, int[] userIds);

        TicketCollection GetTickets(int[] departmentIds, int[] ticketStatusIds, int[] ownerStaffIds, int[] userIds, int count, int start);

        Ticket GetTicket(int ticketId);

        Ticket GetTicket(string displayId);

        Ticket CreateTicket(TicketRequest ticketRequest);

        Ticket UpdateTicket(TicketRequest request);

        bool DeleteTicket(int ticketId);

        #endregion

        #region Ticket Priority Methods

        TicketPriorityCollection GetTicketPriorities();

        TicketPriority GetTicketPriority(int priorityId);

        #endregion

        #region Ticket Status Methods

        TicketStatusCollection GetTicketStatuses();

        TicketStatus GetTicketStatus(int statusId);

        #endregion

        #region Ticket Time Track Methods

        TicketTimeTrackCollection GetTicketTimeTracks(int ticketId);

        TicketTimeTrack GetTicketTimeTrack(int ticketId, int timeTrackNoteId);

        TicketTimeTrack AddTicketTimeTrackingNote(TicketTimeTrackRequest request);

        bool DeleteTicketTimeTrackingNote(int ticketId, int timeTrackNoteId);

        #endregion

        #region Ticket Type Methods

        TicketTypeCollection GetTicketTypes();

        TicketType GetTicketType(int ticketTypeId);

        #endregion

        #region Ticket Posts Methods

        TicketPostCollection GetTicketPosts(int ticketId);

        TicketPost GetTicketPost(int ticketId, int postId);

        TicketPost AddTicketPost(TicketPostRequest request);

        bool DeleteTicketPost(int ticketId, int ticketPostId);

        #endregion

        #region Ticket Attachments Methods

        TicketAttachmentCollection GetTicketAttachments(int ticketId);

        TicketAttachment GetTicketAttachment(int ticketId, int attachmentId);

        TicketAttachment AddTicketAttachment(TicketAttachmentRequest request);

        bool DeleteTicketAttachment(int ticketId, int attachmentId);

        #endregion

        #region Ticket Note Methods

        TicketNoteCollection GetTicketNotes(int ticketId);

        TicketNote GetTicketNote(int ticketId, int noteId);

        TicketNote AddTicketNote(TicketNoteRequest request);

        bool DeleteTicketNote(int ticketId, int noteId);

        #endregion

        #region Ticket Custom Fields Methods

        TicketCustomFields GetTicketCustomFields(int ticketId);

        TicketCustomFields UpdateTicketCustomFields(int ticketId, TicketCustomFields customFields);

        #endregion
    }

    /// <summary>
    ///     Provides access to Ticket API Methods
    /// </summary>
    public sealed class TicketController : BaseController, ITicketController
    {
        public TicketController(string apiKey, string secretKey, string apiUrl, IWebProxy proxy)
            : base(apiKey, secretKey, apiUrl, proxy) { }

        public TicketController(string apiKey, string secretKey, string apiUrl, IWebProxy proxy, ApiRequestType requestType)
            : base(apiKey, secretKey, apiUrl, proxy, requestType) { }

        public TicketController(IKayakoApiRequest kayakoApiRequest)
            : base(kayakoApiRequest) { }

        #region Ticket Count Api Methods

        /// <summary>
        ///     Retrieve a list of counts for different departments, ticket status'es, owners etc.
        /// </summary>
        public TicketCount GetTicketCounts()
        {
            const string apiMethod = "/Tickets/TicketCount/";

            var ticketCount = this.Connector.ExecuteGet<TicketCount>(apiMethod);

            return ticketCount;
        }

        #endregion

        #region Ticket Search Methods

        /// <summary>
        ///     Run a search on tickets. You can combine the search factors to make the span multiple areas.
        ///     For example, to search the full name, contents and email you can send the arguments as:
        ///     query=John&amp;fullname=1&amp;email=1&amp;contents=1
        /// </summary>
        public TicketCollection SearchTickets(TicketSearchQuery query)
        {
            if (string.IsNullOrEmpty(query.Query))
            {
                throw new ArgumentException("A search query must be provided");
            }

            const string apiMethod = "/Tickets/TicketSearch";

            var parameters = query.GetRequestBodyParameters();

            var tickets = this.Connector.ExecutePost<TicketCollection>(apiMethod, parameters.ToString());

            return tickets;
        }

        #endregion

        private static string JoinIntParameters(IReadOnlyCollection<int> values)
        {
            if (values != null && values.Count > 0)
            {
                var sb = new StringBuilder();

                foreach (var val in values)
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                    {
                        sb.Append(",");
                    }

                    sb.Append(val);
                }

                return sb.ToString();
            }

            return "-1";
        }

        #region Ticket Api Methods

        /// <summary>
        ///     Retrieve a filtered list of tickets from the help desk.
        /// </summary>
        /// <param name="departmentIds">Filter the tickets by the specified department id(s)</param>
        public TicketCollection GetTickets(int[] departmentIds) => this.GetTickets(departmentIds, null, null, null, -1, -1);

        /// <summary>
        ///     Retrieve a filtered list of tickets from the help desk.
        /// </summary>
        /// <param name="departmentIds">Filter the tickets by the specified department id(s)</param>
        /// <param name="count">Total ticket count for retrieval</param>
        /// <param name="start">Start ticket for retrieval</param>
        /// <returns></returns>
        public TicketCollection GetTickets(int[] departmentIds, int count, int start) => this.GetTickets(departmentIds, null, null, null, count, start);

        /// <summary>
        ///     Retrieve a filtered list of tickets from the help desk.
        /// </summary>
        /// <param name="departmentIds">Filter the tickets by the specified department id(s)</param>
        /// <param name="ticketStatusIds">
        ///     Filter the tickets by the specified ticket status id(s). Pass null or empty array for no
        ///     filter
        /// </param>
        /// <param name="ownerStaffIds">
        ///     Filter the tickets by the specified owner staff id(s). Pass null or empty array for no
        ///     filter
        /// </param>
        /// <param name="userIds">Filter the tickets by the specified user id(s). Pass null or empty array for no filter</param>
        /// <returns></returns>
        public TicketCollection GetTickets(int[] departmentIds, int[] ticketStatusIds, int[] ownerStaffIds, int[] userIds) => this.GetTickets(departmentIds, ticketStatusIds, ownerStaffIds, userIds, -1, -1);

        /// <summary>
        ///     Retrieve a filtered list of tickets from the help desk.
        /// </summary>
        /// <param name="departmentIds">Filter the tickets by the specified department id(s)</param>
        /// <param name="ticketStatusIds">
        ///     Filter the tickets by the specified ticket status id(s). Pass null or empty array for no
        ///     filter
        /// </param>
        /// <param name="ownerStaffIds">
        ///     Filter the tickets by the specified owner staff id(s). Pass null or empty array for no
        ///     filter
        /// </param>
        /// <param name="userIds">Filter the tickets by the specified user id(s). Pass null or empty array for no filter</param>
        /// <param name="count">Total ticket count for retrieval</param>
        /// <param name="start">Start ticket for retrieval</param>
        public TicketCollection GetTickets(int[] departmentIds, int[] ticketStatusIds, int[] ownerStaffIds, int[] userIds, int count, int start)
        {
            var deptIdParameter = JoinIntParameters(departmentIds);
            var ticketStatusIdParameter = JoinIntParameters(ticketStatusIds);
            var ownerStaffIdParameter = JoinIntParameters(ownerStaffIds);
            var userIdParameter = JoinIntParameters(userIds);

            var apiMethod = string.Format("/Tickets/Ticket/ListAll/{0}/{1}/{2}/{3}/{4}/{5}",
                deptIdParameter,
                ticketStatusIdParameter,
                ownerStaffIdParameter,
                userIdParameter,
                count,
                start);

            var tickets = this.Connector.ExecuteGet<TicketCollection>(apiMethod);

            return tickets;
        }

        /// <summary>
        ///     Retrieve the ticket identified by <paramref name="ticketId" />.
        /// </summary>
        /// <param name="ticketId">The unique numeric identifier of the ticket</param>
        /// <returns>The ticket!</returns>
        public Ticket GetTicket(int ticketId) => this.GetTicketRequest(ticketId.ToString());

        /// <summary>
        ///     Retrieve the ticket identified by <paramref name="ticketId" />.
        /// </summary>
        /// <param name="ticketId">The ticket mask Id (e.g. ABC-123-4567).</param>
        /// <returns>The ticket!</returns>
        public Ticket GetTicket(string displayId) => this.GetTicketRequest(displayId);

        private Ticket GetTicketRequest(string id)
        {
            var apiMethod = string.Format("/Tickets/Ticket/{0}/", id);

            var tickets = this.Connector.ExecuteGet<TicketCollection>(apiMethod);

            return tickets.Count > 0 ? tickets[0] : null;
        }

        public Ticket CreateTicket(TicketRequest ticketRequest)
        {
            const string apiMethod = "/Tickets/Ticket";

            ticketRequest.EnsureValidData(RequestTypes.Create);

            var parameters = new RequestBodyBuilder();

            parameters.AppendRequestData("subject", ticketRequest.Subject);
            parameters.AppendRequestData("fullname", ticketRequest.FullName);
            parameters.AppendRequestData("email", ticketRequest.Email);
            parameters.AppendRequestData("contents", ticketRequest.Contents);
            parameters.AppendRequestData("departmentid", ticketRequest.DepartmentId);
            parameters.AppendRequestData("ticketstatusid", ticketRequest.TicketStatusId);
            parameters.AppendRequestData("ticketpriorityid", ticketRequest.TicketPriorityId);
            parameters.AppendRequestData("tickettypeid", ticketRequest.TicketTypeId);

            if (ticketRequest.AutoUserId != null)
            {
                parameters.AppendRequestData("autouserid", Convert.ToInt32(ticketRequest.AutoUserId));
            }
            else if (ticketRequest.UserId != null)
            {
                parameters.AppendRequestData("userid", ticketRequest.UserId);
            }
            else if (ticketRequest.StaffId != null)
            {
                parameters.AppendRequestData("staffid", ticketRequest.StaffId);
            }

            if (ticketRequest.OwnerStaffId != null)
            {
                parameters.AppendRequestData("ownerstaffid", ticketRequest.OwnerStaffId);
            }

            if (ticketRequest.TemplateGroupId != null)
            {
                parameters.AppendRequestData("templategroup", ticketRequest.TemplateGroupId);
            }
            else if (!string.IsNullOrEmpty(ticketRequest.TemplateGroupName))
            {
                parameters.AppendRequestData("templategroup", ticketRequest.TemplateGroupName);
            }

            if (ticketRequest.IgnoreAutoResponder != null)
            {
                parameters.AppendRequestData("ignoreautoresponder", Convert.ToInt32(ticketRequest.IgnoreAutoResponder));
            }

            if (ticketRequest.CreationType != null)
            {
                parameters.AppendRequestData("type", EnumUtility.ToApiString(ticketRequest.CreationType));
            }

            var tickets = this.Connector.ExecutePost<TicketCollection>(apiMethod, parameters.ToString());

            return tickets.Count > 0 ? tickets[0] : null;
        }

        public Ticket UpdateTicket(TicketRequest request)
        {
            request.EnsureValidData(RequestTypes.Update);

            var parameters = new RequestBodyBuilder();

            if (!string.IsNullOrEmpty(request.Subject))
            {
                parameters.AppendRequestData("subject", request.Subject);
            }

            if (!string.IsNullOrEmpty(request.FullName))
            {
                parameters.AppendRequestData("fullname", request.FullName);
            }

            if (!string.IsNullOrEmpty(request.Email))
            {
                parameters.AppendRequestData("email", request.Email);
            }

            if (request.DepartmentId != null)
            {
                parameters.AppendRequestData("departmentid", request.DepartmentId);
            }

            if (request.TicketStatusId != null)
            {
                parameters.AppendRequestData("ticketstatusid", request.TicketStatusId);
            }

            if (request.TicketPriorityId != null)
            {
                parameters.AppendRequestData("ticketpriorityid", request.TicketPriorityId);
            }

            if (request.TicketTypeId != null)
            {
                parameters.AppendRequestData("tickettypeid", request.TicketTypeId);
            }

            if (request.OwnerStaffId != null)
            {
                parameters.AppendRequestData("ownerstaffid", request.OwnerStaffId);
            }

            if (request.UserId != null)
            {
                parameters.AppendRequestData("userid", request.UserId);
            }

            if (request.TemplateGroupId != null)
            {
                parameters.AppendRequestData("templategroup", request.TemplateGroupId);
            }
            else if (!string.IsNullOrEmpty(request.TemplateGroupName))
            {
                parameters.AppendRequestData("templategroup", request.TemplateGroupName);
            }

            var apiMethod = string.Format("/Tickets/Ticket/{0}", request.Id);

            var tickets = this.Connector.ExecutePut<TicketCollection>(apiMethod, parameters.ToString());

            if (tickets != null && tickets.Count > 0)
            {
                return tickets[0];
            }

            return null;
        }

        public bool DeleteTicket(int ticketId)
        {
            var apiMethod = string.Format("/Tickets/Ticket/{0}", ticketId);

            return this.Connector.ExecuteDelete(apiMethod);
        }

        #endregion

        #region Ticket Priority Methods

        /// <summary>
        ///     Retrieve a list of all ticket statues in the help desk.
        /// </summary>
        /// <remarks>
        ///     See - http://wiki.kayako.com/display/DEV/REST+-+TicketStatus
        /// </remarks>
        /// <returns>The ticket statuses</returns>
        public TicketPriorityCollection GetTicketPriorities()
        {
            const string apiMethod = "/Tickets/TicketPriority/";

            return this.Connector.ExecuteGet<TicketPriorityCollection>(apiMethod);
        }

        /// <summary>
        ///     Retrieve the ticket status identified by <paramref name="priorityId" />.
        /// </summary>
        /// <remarks>
        ///     See -
        ///     http://wiki.kayako.com/display/DEV/REST+-+TicketStatus#REST-TicketStatus-GET%2FTickets%2FTicketStatus%2F%24id%24
        /// </remarks>
        /// <param name="priorityId">The unique numeric identifier of the ticket status.</param>
        /// <returns>The ticket status</returns>
        public TicketPriority GetTicketPriority(int priorityId)
        {
            var apiMethod = string.Format("/Tickets/TicketPriority/{0}", priorityId);

            var priorities = this.Connector.ExecuteGet<TicketPriorityCollection>(apiMethod);

            if (priorities != null && priorities.Count > 0)
            {
                return priorities[0];
            }

            return null;
        }

        #endregion

        #region Ticket Status Methods

        /// <summary>
        ///     Retrieve a list of all ticket statues in the help desk.
        /// </summary>
        /// <remarks>
        ///     See - http://wiki.kayako.com/display/DEV/REST+-+TicketStatus
        /// </remarks>
        /// <returns>The ticket statuses</returns>
        public TicketStatusCollection GetTicketStatuses()
        {
            const string apiMethod = "/Tickets/TicketStatus/";

            return this.Connector.ExecuteGet<TicketStatusCollection>(apiMethod);
        }

        /// <summary>
        ///     Retrieve the ticket status identified by <paramref name="statusId" />.
        /// </summary>
        /// <remarks>
        ///     See -
        ///     http://wiki.kayako.com/display/DEV/REST+-+TicketStatus#REST-TicketStatus-GET%2FTickets%2FTicketStatus%2F%24id%24
        /// </remarks>
        /// <param name="statusId">The unique numeric identifier of the ticket status.</param>
        /// <returns>The ticket status</returns>
        public TicketStatus GetTicketStatus(int statusId)
        {
            var apiMethod = string.Format("/Tickets/TicketStatus/{0}", statusId);

            var statuses = this.Connector.ExecuteGet<TicketStatusCollection>(apiMethod);

            if (statuses != null && statuses.Count > 0)
            {
                return statuses[0];
            }

            return null;
        }

        #endregion

        #region Ticket Time Track Methods

        /// <summary>
        ///     Retrieve a list of a ticket's time track notes.
        /// </summary>
        public TicketTimeTrackCollection GetTicketTimeTracks(int ticketId)
        {
            var apiMethod = string.Format("/Tickets/TicketTimeTrack/ListAll/{0}", ticketId);

            var ticketTimeTracks = this.Connector.ExecuteGet<TicketTimeTrackCollection>(apiMethod);

            return ticketTimeTracks;
        }

        /// <summary>
        ///     Retrieve a ticket's time track note
        /// </summary>
        /// <param name="ticketId">The unique numeric identifier of the ticket</param>
        /// <param name="timeTrackNoteId">The unique numeric identifier of the ticket time tracking note</param>
        public TicketTimeTrack GetTicketTimeTrack(int ticketId, int timeTrackNoteId)
        {
            var apiMethod = string.Format("/Tickets/TicketTimeTrack/{0}/{1}", ticketId, timeTrackNoteId);

            var ticketTimeTracks = this.Connector.ExecuteGet<TicketTimeTrackCollection>(apiMethod);

            if (ticketTimeTracks != null && ticketTimeTracks.Count > 0)
            {
                return ticketTimeTracks[0];
            }

            return null;
        }

        /// <summary>
        ///     Add a new time tracking note to a ticket
        /// </summary>
        public TicketTimeTrack AddTicketTimeTrackingNote(TicketTimeTrackRequest request)
        {
            request.EnsureValidData(RequestTypes.Create);

            const string apiMethod = "/Tickets/TicketTimeTrack";

            var parameters = new RequestBodyBuilder();
            parameters.AppendRequestData("ticketid", request.TicketId);
            parameters.AppendRequestData("contents", request.Contents);
            parameters.AppendRequestData("staffid", request.StaffId);
            parameters.AppendRequestData("worktimeline", request.WorkTimeline);
            parameters.AppendRequestData("billtimeline", request.BillTimeline);
            parameters.AppendRequestData("timespent", request.TimeSpent);
            parameters.AppendRequestData("timebillable", request.TimeBillable);

            if (request.WorkerStaffId != null)
            {
                parameters.AppendRequestData("workerstaffid", request.WorkerStaffId);
            }

            if (request.NoteColor != null)
            {
                parameters.AppendRequestData("notecolor", EnumUtility.ToApiString(request.NoteColor));
            }

            var ticketTimeTracks = this.Connector.ExecutePost<TicketTimeTrackCollection>(apiMethod, parameters.ToString());

            if (ticketTimeTracks != null && ticketTimeTracks.Count > 0)
            {
                return ticketTimeTracks[0];
            }

            return null;
        }

        /// <summary>
        ///     Delete the ticket time tracking note identified by identifier linked to the ticket identifier
        /// </summary>
        /// <param name="ticketId">The unique numeric identifier of the ticket</param>
        /// <param name="timeTrackNoteId">The unique numeric identifier of the ticket time tracking note</param>
        public bool DeleteTicketTimeTrackingNote(int ticketId, int timeTrackNoteId)
        {
            var apiMethod = string.Format("/Tickets/TicketTimeTrack/{0}/{1}", ticketId, timeTrackNoteId);

            return this.Connector.ExecuteDelete(apiMethod);
        }

        #endregion

        #region Ticket Type Methods

        public TicketTypeCollection GetTicketTypes()
        {
            const string apiMethod = "/Tickets/TicketType/";

            return this.Connector.ExecuteGet<TicketTypeCollection>(apiMethod);
        }

        public TicketType GetTicketType(int ticketTypeId)
        {
            var apiMethod = string.Format("/Tickets/TicketType/{0}", ticketTypeId);

            var ticketTypes = this.Connector.ExecuteGet<TicketTypeCollection>(apiMethod);

            if (ticketTypes != null && ticketTypes.Count > 0)
            {
                return ticketTypes[0];
            }

            return null;
        }

        #endregion

        #region Ticket Posts Methods

        /// <summary>
        ///     Retrieve a list of all the ticket posts that belong to a ticket given ticket's id.
        /// </summary>
        /// <param name="ticketId">The unique numeric identifier of the ticket.</param>
        /// <returns>TicketPosts</returns>
        public TicketPostCollection GetTicketPosts(int ticketId)
        {
            var apiMethod = string.Format("/Tickets/TicketPost/ListAll/{0}", ticketId);

            var posts = this.Connector.ExecuteGet<TicketPostCollection>(apiMethod);

            return posts;
        }

        /// <summary>
        ///     Retrieve the post identified by <paramref name="postId" /> that belong to the ticket identified by
        ///     <paramref name="ticketId" />.
        /// </summary>
        /// <param name="ticketId">The unique numeric identifier of the ticket.</param>
        /// <param name="postId">The unique numeric identifier of the ticket post.</param>
        /// <returns>The ticket post</returns>
        public TicketPost GetTicketPost(int ticketId, int postId)
        {
            var apiMethod = string.Format("/Tickets/TicketPost/{0}/{1}", ticketId, postId);

            var posts = this.Connector.ExecuteGet<TicketPostCollection>(apiMethod);

            return posts.Count > 0 ? posts[0] : null;
        }

        /// <summary>
        ///     Add a new post to an existing ticket.. Only <paramref name="userid" /> or <paramref name="staffid" /> should be
        ///     set.
        ///     <remarks>
        ///         See - http://wiki.kayako.com/display/DEV/REST+-+TicketPost#REST-TicketPost-POST%2FTickets%2FTicketPost
        ///     </remarks>
        /// </summary>
        /// <param name="request">Ticket request</param>
        /// <returns></returns>
        public TicketPost AddTicketPost(TicketPostRequest request)
        {
            const string apiMethod = "/Tickets/TicketPost";

            request.EnsureValidData(RequestTypes.Create);

            var parameters = new RequestBodyBuilder();

            parameters.AppendRequestData("ticketid", request.TicketId);
            parameters.AppendRequestData("subject", request.Subject);
            parameters.AppendRequestData("contents", request.Contents);

            if (request.UserId == null && request.StaffId == null)
            {
                throw new ArgumentException("Neither UserId nor StaffId are set, one of these fields are required field and cannot be null.");
            }

            if (request.UserId != null && request.StaffId != null)
            {
                throw new ArgumentException("UserId are StaffId are both set, only one of these fields must be set.");
            }

            if (request.UserId != null)
            {
                parameters.AppendRequestData("userid", request.UserId.Value);
            }

            if (request.StaffId != null)
            {
                parameters.AppendRequestData("staffid", request.StaffId.Value);

                if (request.IsPrivate != null)
                {
                    parameters.AppendRequestDataBool("isprivate", request.IsPrivate.Value);
                }
            }

            var posts = this.Connector.ExecutePost<TicketPostCollection>(apiMethod, parameters.ToString());

            return posts.Count > 0 ? posts[0] : null;
        }

        public bool DeleteTicketPost(int ticketId, int ticketPostId)
        {
            var apiMethod = string.Format("/Tickets/TicketPost/{0}/{1}", ticketId, ticketPostId);

            return this.Connector.ExecuteDelete(apiMethod);
        }

        #endregion

        #region Ticket Attachments Methods

        public TicketAttachmentCollection GetTicketAttachments(int ticketId)
        {
            var apiMethod = string.Format("/Tickets/TicketAttachment/ListAll/{0}", ticketId);

            return this.Connector.ExecuteGet<TicketAttachmentCollection>(apiMethod);
        }

        public TicketAttachment GetTicketAttachment(int ticketId, int attachmentId)
        {
            var apiMethod = string.Format("/Tickets/TicketAttachment/{0}/{1}", ticketId, attachmentId);

            var attachments = this.Connector.ExecuteGet<TicketAttachmentCollection>(apiMethod);

            if (attachments != null && attachments.Count > 0)
            {
                return attachments[0];
            }

            return null;
        }

        /// <summary>
        ///     Add an attachment to a ticket post.
        /// </summary>
        /// <param name="request">Ticket attachment request.</param>
        public TicketAttachment AddTicketAttachment(TicketAttachmentRequest request)
        {
            const string apiMethod = "/Tickets/TicketAttachment";

            request.EnsureValidData(RequestTypes.Create);

            var parameters = new RequestBodyBuilder();
            parameters.AppendRequestData("ticketid", request.TicketId);
            parameters.AppendRequestData("ticketpostid", request.TicketPostId);
            parameters.AppendRequestData("filename", request.FileName);
            parameters.AppendRequestData("contents", request.Contents);

            var attachments = this.Connector.ExecutePost<TicketAttachmentCollection>(apiMethod, parameters.ToString());

            if (attachments != null && attachments.Count > 0)
            {
                return attachments[0];
            }

            return null;
        }

        public bool DeleteTicketAttachment(int ticketId, int attachmentId)
        {
            var apiMethod = string.Format("/Tickets/TicketAttachment/{0}/{1}", ticketId, attachmentId);

            return this.Connector.ExecuteDelete(apiMethod);
        }

        #endregion

        #region Ticket Note Methods

        /// <summary>
        ///     Retrieve a list of a ticket's notes.
        /// </summary>
        /// <remarks>
        ///     http://wiki.kayako.com/display/DEV/REST+-+TicketNote#REST-TicketNote-GET%2FTickets%2FTicketNote%2FListAll%2F%24ticketid%24
        /// </remarks>
        /// <param name="ticketId">The unique numeric identifier of the ticket.</param>
        /// <returns>Ticket notes.</returns>
        public TicketNoteCollection GetTicketNotes(int ticketId)
        {
            var apiMethod = string.Format("/Tickets/TicketNote/ListAll/{0}", ticketId);

            return this.Connector.ExecuteGet<TicketNoteCollection>(apiMethod);
        }

        /// <summary>
        ///     Retrieve the note identified by <paramref name="noteId" /> that belongs to a ticket identified by
        ///     <paramref name="ticketId" />.
        /// </summary>
        /// <param name="ticketId">The unique numeric identifier of the ticket.</param>
        /// <param name="noteId">The unique numeric identifier of the ticket note.</param>
        /// <returns>List of ticket notes</returns>
        public TicketNote GetTicketNote(int ticketId, int noteId)
        {
            var apiMethod = string.Format("/Tickets/TicketNote/{0}/{1}/", ticketId, noteId);

            var notes = this.Connector.ExecuteGet<TicketNoteCollection>(apiMethod);

            if (notes != null && notes.Count > 0)
            {
                return notes[0];
            }

            return null;
        }

        /// <summary>
        ///     Add a new note to a ticket.
        /// </summary>
        /// <remarks>
        ///     http://wiki.kayako.com/display/DEV/REST+-+TicketNote#REST-TicketNote-POST%2FTickets%2FTicketNote
        /// </remarks>
        /// <returns>The added ticket note.</returns>
        public TicketNote AddTicketNote(TicketNoteRequest request)
        {
            const string apiMethod = "/Tickets/TicketNote";

            request.EnsureValidData(RequestTypes.Create);

            var parameters = new RequestBodyBuilder();

            parameters.AppendRequestData("ticketid", request.TicketId);
            parameters.AppendRequestData("contents", request.Content);
            parameters.AppendRequestData("notecolor", (int) request.NoteColor);

            if (request.FullName == null && request.StaffId == null)
            {
                throw new ArgumentException("Neither FullName nor StaffId are set, one of these fields are required field and cannot be null.");
            }

            if (request.FullName != null && request.StaffId != null)
            {
                throw new ArgumentException("FullName are StaffId are both set, only one of these fields must be set.");
            }

            if (request.FullName != null)
            {
                parameters.AppendRequestData("fullname", request.FullName);
            }

            if (request.StaffId != null)
            {
                parameters.AppendRequestData("staffid", request.StaffId.Value);
            }

            if (request.ForStaffId != null)
            {
                parameters.AppendRequestData("forstaffid", request.ForStaffId.Value);
            }

            var notes = this.Connector.ExecutePost<TicketNoteCollection>(apiMethod, parameters.ToString());

            return notes.Count > 0 ? notes[0] : null;
        }

        public bool DeleteTicketNote(int ticketId, int noteId)
        {
            var apiMethod = string.Format("/Tickets/TicketNote/{0}/{1}", ticketId, noteId);

            return this.Connector.ExecuteDelete(apiMethod);
        }

        #endregion

        #region Ticket Custom Fields Methods

        /// <summary>
        ///     Retrieve a list of a ticket's custom fields.
        /// </summary>
        public TicketCustomFields GetTicketCustomFields(int ticketId)
        {
            var apiMethod = string.Format("/Tickets/TicketCustomField/{0}", ticketId);

            return this.Connector.ExecuteGet<TicketCustomFields>(apiMethod);
        }

        /// <summary>
        ///     Update the custom field values for a ticket. Please note all custom fields for the ticket must be sent through with
        ///     their values.
        /// </summary>
        public TicketCustomFields UpdateTicketCustomFields(int ticketId, TicketCustomFields customFields)
        {
            var apiMethod = string.Format("/Tickets/TicketCustomField/{0}", ticketId);

            var sb = new StringBuilder();
            foreach (var group in customFields.FieldGroups)
            {
                foreach (var field in group.Fields)
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                    {
                        sb.Append("&");
                    }

                    sb.AppendFormat("{0}={1}", field.Name, HttpUtility.UrlEncode(field.FieldContent ?? string.Empty));
                }
            }

            return this.Connector.ExecutePost<TicketCustomFields>(apiMethod, sb.ToString());
        }

        #endregion
    }
}