using System.Net;
using KayakoRestApi.Controllers;
using KayakoRestApi.Net;

namespace KayakoRestApi
{
    public interface IKayakoClient
    {
        ICoreController Core { get; }

        ICustomFieldController CustomFields { get; }

        IDepartmentController Departments { get; }

        IKnowledgebaseController Knowledgebase { get; }

        INewsController News { get; }

        IStaffController Staff { get; }

        ITicketController Tickets { get; }

        ITroubleshooterController Troubleshooter { get; }

        IUserController Users { get; }
    }

    /// <summary>
    ///     This service allows the interaction with the Kayako REST Api.
    /// </summary>
    /// <remarks>
    ///     See - http://wiki.kayako.com/display/DEV/REST+Api
    /// </remarks>
    public class KayakoClient : IKayakoClient
    {
        /// <summary>
        ///     Initializes a new instance of the KayakoRestApi.KayakoService class.
        /// </summary>
        /// <param name="apiKey">Api Key.</param>
        /// <param name="secretKey">Secret Api Key.</param>
        /// <param name="apiUrl">URL of Kayako REST Api</param>
        public KayakoClient(string apiKey, string secretKey, string apiUrl)
        {
            this.coreController = new CoreController(apiKey, secretKey, apiUrl, null);
            this.customFields = new CustomFieldController(apiKey, secretKey, apiUrl, null);
            this.departments = new DepartmentController(apiKey, secretKey, apiUrl, null);
            this.knowledgebase = new KnowledgebaseController(apiKey, secretKey, apiUrl, null);
            this.news = new NewsController(apiKey, secretKey, apiUrl, null);
            this.staff = new StaffController(apiKey, secretKey, apiUrl, null);
            this.tickets = new TicketController(apiKey, secretKey, apiUrl, null);
            this.troubleshooter = new TroubleshooterController(apiKey, secretKey, apiUrl, null);
            this.users = new UserController(apiKey, secretKey, apiUrl, null);
        }

        /// <summary>
        ///     Initializes a new instance of the KayakoRestApi.KayakoService class.
        /// </summary>
        /// <param name="apiKey">Api Key.</param>
        /// <param name="secretKey">Secret Api Key.</param>
        /// <param name="apiUrl">URL of Kayako REST Api</param>
        /// <param name="requestType">Determines how the request URL is formed</param>
        public KayakoClient(string apiKey, string secretKey, string apiUrl, ApiRequestType requestType)
        {
            this.coreController = new CoreController(apiKey, secretKey, apiUrl, null, requestType);
            this.customFields = new CustomFieldController(apiKey, secretKey, apiUrl, null, requestType);
            this.departments = new DepartmentController(apiKey, secretKey, apiUrl, null, requestType);
            this.knowledgebase = new KnowledgebaseController(apiKey, secretKey, apiUrl, null, requestType);
            this.news = new NewsController(apiKey, secretKey, apiUrl, null, requestType);
            this.staff = new StaffController(apiKey, secretKey, apiUrl, null, requestType);
            this.tickets = new TicketController(apiKey, secretKey, apiUrl, null, requestType);
            this.troubleshooter = new TroubleshooterController(apiKey, secretKey, apiUrl, null, requestType);
            this.users = new UserController(apiKey, secretKey, apiUrl, null, requestType);
        }

        /// <summary>
        ///     Initializes a new instance of the KayakoRestApi.KayakoService class.
        /// </summary>
        /// <param name="apiKey">Api Key.</param>
        /// <param name="secretKey">Secret Api Key.</param>
        /// <param name="apiUrl">URL of Kayako REST Api</param>
        /// <param name="proxy">An IWebProxy object representing any proxy details required for internet connection</param>
        public KayakoClient(string apiKey, string secretKey, string apiUrl, IWebProxy proxy)
        {
            this.coreController = new CoreController(apiKey, secretKey, apiUrl, proxy);
            this.customFields = new CustomFieldController(apiKey, secretKey, apiUrl, proxy);
            this.departments = new DepartmentController(apiKey, secretKey, apiUrl, proxy);
            this.knowledgebase = new KnowledgebaseController(apiKey, secretKey, apiUrl, proxy);
            this.news = new NewsController(apiKey, secretKey, apiUrl, proxy);
            this.staff = new StaffController(apiKey, secretKey, apiUrl, proxy);
            this.tickets = new TicketController(apiKey, secretKey, apiUrl, proxy);
            this.troubleshooter = new TroubleshooterController(apiKey, secretKey, apiUrl, proxy);
            this.users = new UserController(apiKey, secretKey, apiUrl, proxy);
        }

        /// <summary>
        ///     Initializes a new instance of the KayakoRestApi.KayakoService class.
        /// </summary>
        /// <param name="apiKey">Api Key.</param>
        /// <param name="secretKey">Secret Api Key.</param>
        /// <param name="apiUrl">URL of Kayako REST Api</param>
        /// <param name="requestType">Determines how the request URL is formed</param>
        public KayakoClient(string apiKey, string secretKey, string apiUrl, IWebProxy proxy, ApiRequestType requestType)
        {
            this.coreController = new CoreController(apiKey, secretKey, apiUrl, proxy, requestType);
            this.customFields = new CustomFieldController(apiKey, secretKey, apiUrl, proxy, requestType);
            this.departments = new DepartmentController(apiKey, secretKey, apiUrl, proxy, requestType);
            this.knowledgebase = new KnowledgebaseController(apiKey, secretKey, apiUrl, proxy, requestType);
            this.news = new NewsController(apiKey, secretKey, apiUrl, proxy, requestType);
            this.staff = new StaffController(apiKey, secretKey, apiUrl, proxy, requestType);
            this.tickets = new TicketController(apiKey, secretKey, apiUrl, proxy, requestType);
            this.troubleshooter = new TroubleshooterController(apiKey, secretKey, apiUrl, proxy, requestType);
            this.users = new UserController(apiKey, secretKey, apiUrl, proxy, requestType);
        }

        #region Private Properties

        private readonly ICoreController coreController;
        private readonly ICustomFieldController customFields;
        private readonly IDepartmentController departments;
        private readonly IKnowledgebaseController knowledgebase;
        private readonly INewsController news;
        private readonly IStaffController staff;
        private readonly ITicketController tickets;
        private readonly ITroubleshooterController troubleshooter;
        private readonly IUserController users;

        #endregion

        #region Public Properies

        /// <summary>
        ///     Provides access to Core API Methods
        /// </summary>
        public ICoreController Core => this.coreController;

        /// <summary>
        ///     Provides access to Custom Field API Methods
        /// </summary>
        public ICustomFieldController CustomFields => this.customFields;

        /// <summary>
        ///     Provides access to Deparment API Methods
        /// </summary>
        public IDepartmentController Departments => this.departments;

        /// <summary>
        ///     Provides access to Knowledgebase API Methods
        /// </summary>
        public IKnowledgebaseController Knowledgebase => this.knowledgebase;

        /// <summary>
        ///     Provides access to News API Methods
        /// </summary>
        public INewsController News => this.news;

        /// <summary>
        ///     Provides access to Staff API Methods
        /// </summary>
        public IStaffController Staff => this.staff;

        /// <summary>
        ///     Provides access to Ticket API Methods
        /// </summary>
        public ITicketController Tickets => this.tickets;

        /// <summary>
        ///     Provides access to Troubleshooter API Methods
        /// </summary>
        public ITroubleshooterController Troubleshooter => this.troubleshooter;

        /// <summary>
        ///     Provides access to User API Methods
        /// </summary>
        public IUserController Users => this.users;

        #endregion
    }
}