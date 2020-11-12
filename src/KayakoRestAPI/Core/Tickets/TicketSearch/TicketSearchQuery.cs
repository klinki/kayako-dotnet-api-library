using System;
using System.Collections.Generic;
using System.Reflection;
using KayakoRestApi.Text;

namespace KayakoRestApi.Core.Tickets.TicketSearch
{
    /// <summary>
    ///     Object to build up a search query which will search tickets
    ///     <remarks>http://wiki.kayako.com/display/DEV/REST+-+TicketSearch#REST-TicketSearch-PostVariables</remarks>
    /// </summary>
    public class TicketSearchQuery
    {
        /// <summary>
        ///     Initialises the ticket search query with the query data
        /// </summary>
        public TicketSearchQuery(string query)
        {
            this.Query = query;
            this.SearchFieldsValue = new List<TicketSearchField>();
        }

        /// <summary>
        ///     Initialises the ticket search query with the query data
        /// </summary>
        public TicketSearchQuery(string query, TicketSearchField[] searchFields)
        {
            this.Query = query;
            this.SearchFieldsValue = new List<TicketSearchField>(searchFields);
        }

        public string Query { get; }

        private List<TicketSearchField> SearchFieldsValue { get; set; }

        public List<TicketSearchField> SearchFields
        {
            get => this.SearchFieldsValue;
            set => this.SearchFieldsValue = value;
        }

        /// <summary>
        ///     Populates the post parameters to send to Kayako Api service
        /// </summary>
        /// <returns></returns>
        internal RequestBodyBuilder GetRequestBodyParameters()
        {
            var parameters = new RequestBodyBuilder();
            parameters.AppendRequestData("query", this.Query);

            var props = typeof(TicketSearchField).GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var p in props)
            {
                if (this.SearchFieldsValue.Contains((TicketSearchField) p.GetValue(typeof(TicketSearchField))))
                {
                    if (p.GetCustomAttributes(typeof(RequestParameterNameAttribute), false) is RequestParameterNameAttribute[] att)
                    {
                        parameters.AppendRequestData(att[0].RequestName, 1);
                    }
                }
            }

            return parameters;
        }

        /// <summary>
        ///     Add a search field to be included in the search
        /// </summary>
        /// <param name="searchField"></param>
        public void AddSearchField(TicketSearchField searchField) => this.SearchFieldsValue.Add(searchField);
    }

    /// <summary>
    ///     Enum representing the various search fields available
    /// </summary>
    public enum TicketSearchField
    {
        [RequestParameterName("ticketid")]
        TicketId,

        [RequestParameterName("contents")]
        Contents,

        [RequestParameterName("author")]
        Author,

        [RequestParameterName("email")]
        EmailAddress,

        [RequestParameterName("creatoremail")]
        CreatorEmailAddress,

        [RequestParameterName("fullname")]
        FullName,

        [RequestParameterName("notes")]
        Notes,

        [RequestParameterName("usergroup")]
        UserGroup,

        [RequestParameterName("userorganization")]
        UserOrganization,

        [RequestParameterName("user")]
        User,

        [RequestParameterName("tags")]
        Tags
    }

    [AttributeUsage(AttributeTargets.Field)]
    internal sealed class RequestParameterNameAttribute : Attribute
    {
        public RequestParameterNameAttribute(string requestname) => this.RequestName = requestname;

        public string RequestName { get; }
    }
}