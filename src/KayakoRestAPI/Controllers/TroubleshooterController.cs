using System.Net;
using KayakoRestApi.Core.Troubleshooter;
using KayakoRestApi.Net;
using KayakoRestApi.RequestBase;
using KayakoRestApi.Text;
using KayakoRestApi.Utilities;

namespace KayakoRestApi.Controllers
{
    public interface ITroubleshooterController
    {
        TroubleshooterCategoryCollection GetTroubleshooterCategories();

        TroubleshooterCategory GetTroubleshooterCategory(int troubleshooterCategoryId);

        TroubleshooterCategory CreateTroubleshooterCategory(TroubleshooterCategoryRequest troubleshooterCategoryRequest);

        TroubleshooterCategory UpdateTroubleshooterCategory(TroubleshooterCategoryRequest troubleshooterCategoryRequest);

        bool DeleteTroubleshooterCategory(int troubleshooterCategoryId);

        TroubleshooterStepCollection GetTroubleshooterSteps();

        TroubleshooterStep GetTroubleshooterStep(int troubleshooterStepId);

        TroubleshooterStep CreateTroubleshooterStep(TroubleshooterStepRequest troubleshooterStepRequest);

        TroubleshooterStep UpdateTroubleshooterStep(TroubleshooterStepRequest troubleshooterStepRequest);

        bool DeleteTroubleshooterStep(int troubleshooterStepId);

        TroubleshooterCommentCollection GetTroubleshooterComments(int troubleshooterStepId);

        TroubleshooterComment GetTroubleshooterComment(int troubleshooterCommentId);

        TroubleshooterComment CreateTroubleshooterComment(TroubleshooterCommentRequest troubleshooterCommentRequest);

        bool DeleteTroubleshooterComment(int troubleshooterCommentId);

        TroubleshooterAttachmentCollection GetTroubleshooterAttachments(int troubleshooterStepId);

        TroubleshooterAttachment GetTroubleshooterAttachment(int troubleshooterStepId, int troubleshooterAttachmentId);

        TroubleshooterAttachment CreateTroubleshooterAttachment(TroubleshooterAttachmentRequest troubleshooterAttachmentRequest);

        bool DeleteTroubleshooterAttachment(int troubleshooterStepId, int troubleshooterAttachmentId);
    }

    public sealed class TroubleshooterController : BaseController, ITroubleshooterController
    {
        private const string TroubleshooterCategoryBaseUrl = "/Troubleshooter/Category";
        private const string TroubleshooterStepBaseUrl = "/Troubleshooter/Step";
        private const string TroubleshooterCommentBaseUrl = "/Troubleshooter/Comment";
        private const string TroubleshooterAttachmentBaseUrl = "/Troubleshooter/Attachment";

        public TroubleshooterController(string apiKey, string secretKey, string apiUrl, IWebProxy proxy)
            : base(apiKey, secretKey, apiUrl, proxy) { }

        public TroubleshooterController(string apiKey, string secretKey, string apiUrl, IWebProxy proxy, ApiRequestType requestType)
            : base(apiKey, secretKey, apiUrl, proxy, requestType) { }

        public TroubleshooterController(IKayakoApiRequest kayakoApiRequest)
            : base(kayakoApiRequest) { }

        #region Troubleshooter Category Methods

        public TroubleshooterCategoryCollection GetTroubleshooterCategories() => this.Connector.ExecuteGet<TroubleshooterCategoryCollection>(TroubleshooterCategoryBaseUrl);

        public TroubleshooterCategory GetTroubleshooterCategory(int troubleshooterCategoryId)
        {
            var apiMethod = string.Format("{0}/{1}", TroubleshooterCategoryBaseUrl, troubleshooterCategoryId);

            var troubleshooterCategories = this.Connector.ExecuteGet<TroubleshooterCategoryCollection>(apiMethod);

            if (troubleshooterCategories != null && troubleshooterCategories.Count > 0)
            {
                return troubleshooterCategories[0];
            }

            return null;
        }

        public TroubleshooterCategory CreateTroubleshooterCategory(TroubleshooterCategoryRequest troubleshooterCategoryRequest)
        {
            var parameters = this.PopulateRequestParameters(troubleshooterCategoryRequest, RequestTypes.Create);

            var troubleshooterCategories = this.Connector.ExecutePost<TroubleshooterCategoryCollection>(TroubleshooterCategoryBaseUrl, parameters.ToString());

            if (troubleshooterCategories != null && troubleshooterCategories.Count > 0)
            {
                return troubleshooterCategories[0];
            }

            return null;
        }

        public TroubleshooterCategory UpdateTroubleshooterCategory(TroubleshooterCategoryRequest troubleshooterCategoryRequest)
        {
            var apiMethod = string.Format("{0}/{1}", TroubleshooterCategoryBaseUrl, troubleshooterCategoryRequest.Id);
            var parameters = this.PopulateRequestParameters(troubleshooterCategoryRequest, RequestTypes.Update);

            var troubleshooterCategories = this.Connector.ExecutePut<TroubleshooterCategoryCollection>(apiMethod, parameters.ToString());

            if (troubleshooterCategories != null && troubleshooterCategories.Count > 0)
            {
                return troubleshooterCategories[0];
            }

            return null;
        }

        public bool DeleteTroubleshooterCategory(int troubleshooterCategoryId)
        {
            var apiMethod = string.Format("{0}/{1}", TroubleshooterCategoryBaseUrl, troubleshooterCategoryId);

            return this.Connector.ExecuteDelete(apiMethod);
        }

        private RequestBodyBuilder PopulateRequestParameters(TroubleshooterCategoryRequest troubleshooterCategoryRequest, RequestTypes requestType)
        {
            troubleshooterCategoryRequest.EnsureValidData(requestType);

            var parameters = new RequestBodyBuilder();
            parameters.AppendRequestDataNonEmptyString("title", troubleshooterCategoryRequest.Title);
            parameters.AppendRequestData("categorytype", EnumUtility.ToApiString(troubleshooterCategoryRequest.CategoryType));

            if (requestType == RequestTypes.Create)
            {
                parameters.AppendRequestDataNonNegativeInt("staffid", troubleshooterCategoryRequest.StaffId);
            }

            if (troubleshooterCategoryRequest.DisplayOrder.HasValue)
            {
                parameters.AppendRequestDataNonNegativeInt("displayorder", troubleshooterCategoryRequest.DisplayOrder.Value);
            }

            parameters.AppendRequestDataNonEmptyString("description", troubleshooterCategoryRequest.Description);
            parameters.AppendRequestDataBool("uservisibilitycustom", troubleshooterCategoryRequest.UserVisibilityCustom);
            parameters.AppendRequestDataArrayCommaSeparated("usergroupidlist", troubleshooterCategoryRequest.UserGroupIdList);
            parameters.AppendRequestDataBool("staffvisibilitycustom", troubleshooterCategoryRequest.StaffVisibilityCustom);
            parameters.AppendRequestDataArrayCommaSeparated("staffgroupidlist", troubleshooterCategoryRequest.StaffGroupIdList);

            return parameters;
        }

        #endregion

        #region Troubleshooter Step Methods

        public TroubleshooterStepCollection GetTroubleshooterSteps() => this.Connector.ExecuteGet<TroubleshooterStepCollection>(TroubleshooterStepBaseUrl);

        public TroubleshooterStep GetTroubleshooterStep(int troubleshooterStepId)
        {
            var apiMethod = string.Format("{0}/{1}", TroubleshooterStepBaseUrl, troubleshooterStepId);

            var troubleshooterSteps = this.Connector.ExecuteGet<TroubleshooterStepCollection>(apiMethod);

            if (troubleshooterSteps != null && troubleshooterSteps.Count > 0)
            {
                return troubleshooterSteps[0];
            }

            return null;
        }

        public TroubleshooterStep CreateTroubleshooterStep(TroubleshooterStepRequest troubleshooterStepRequest)
        {
            var parameters = this.PopulateRequestParameters(troubleshooterStepRequest, RequestTypes.Create);

            var troubleshooterSteps = this.Connector.ExecutePost<TroubleshooterStepCollection>(TroubleshooterStepBaseUrl, parameters.ToString());

            if (troubleshooterSteps != null && troubleshooterSteps.Count > 0)
            {
                return troubleshooterSteps[0];
            }

            return null;
        }

        public TroubleshooterStep UpdateTroubleshooterStep(TroubleshooterStepRequest troubleshooterStepRequest)
        {
            var apiMethod = string.Format("{0}/{1}", TroubleshooterStepBaseUrl, troubleshooterStepRequest.Id);
            var parameters = this.PopulateRequestParameters(troubleshooterStepRequest, RequestTypes.Update);

            var troubleshooterStep = this.Connector.ExecutePut<TroubleshooterStepCollection>(apiMethod, parameters.ToString());

            if (troubleshooterStep != null && troubleshooterStep.Count > 0)
            {
                return troubleshooterStep[0];
            }

            return null;
        }

        public bool DeleteTroubleshooterStep(int troubleshooterStepId)
        {
            var apiMethod = string.Format("{0}/{1}", TroubleshooterStepBaseUrl, troubleshooterStepId);

            return this.Connector.ExecuteDelete(apiMethod);
        }

        private RequestBodyBuilder PopulateRequestParameters(TroubleshooterStepRequest troubleshooterStepRequest, RequestTypes requestType)
        {
            troubleshooterStepRequest.EnsureValidData(requestType);

            var parameters = new RequestBodyBuilder();

            if (requestType == RequestTypes.Create)
            {
                parameters.AppendRequestData("categoryid", troubleshooterStepRequest.CategoryId);
            }

            parameters.AppendRequestDataNonEmptyString("subject", troubleshooterStepRequest.Subject);
            parameters.AppendRequestDataNonEmptyString("contents", troubleshooterStepRequest.Contents);

            parameters.AppendRequestDataNonNegativeInt(requestType == RequestTypes.Create ? "staffid" : "editedstaffid",
                troubleshooterStepRequest.StaffId);

            if (troubleshooterStepRequest.DisplayOrder.HasValue)
            {
                parameters.AppendRequestDataNonNegativeInt("displayorder", troubleshooterStepRequest.DisplayOrder.Value);
            }

            parameters.AppendRequestDataBool("allowcomments", troubleshooterStepRequest.AllowComments);
            parameters.AppendRequestDataBool("enableticketredirection", troubleshooterStepRequest.EnableTicketRedirection);

            if (troubleshooterStepRequest.RedirectDepartmentId.HasValue)
            {
                parameters.AppendRequestDataNonNegativeInt("redirectdepartmentid", troubleshooterStepRequest.RedirectDepartmentId.Value);
            }

            if (troubleshooterStepRequest.TicketTypeId.HasValue)
            {
                parameters.AppendRequestDataNonNegativeInt("tickettypeid", troubleshooterStepRequest.TicketTypeId.Value);
            }

            if (troubleshooterStepRequest.TicketPriorityId.HasValue)
            {
                parameters.AppendRequestDataNonNegativeInt("ticketpriorityid", troubleshooterStepRequest.TicketPriorityId.Value);
            }

            parameters.AppendRequestDataNonEmptyString("ticketsubject", troubleshooterStepRequest.TicketSubject);

            if (troubleshooterStepRequest.StepStatus.HasValue)
            {
                parameters.AppendRequestData("stepstatus", EnumUtility.ToApiString(troubleshooterStepRequest.StepStatus.Value));
            }

            parameters.AppendRequestDataArrayCommaSeparated("parentstepidlist", troubleshooterStepRequest.ParentStepIdList);

            return parameters;
        }

        #endregion

        #region Troubleshooter Comment Methods

        public TroubleshooterCommentCollection GetTroubleshooterComments(int troubleshooterStepId)
        {
            var apiMethod = string.Format("{0}/ListAll/{1}", TroubleshooterCommentBaseUrl, troubleshooterStepId);

            return this.Connector.ExecuteGet<TroubleshooterCommentCollection>(apiMethod);
        }

        public TroubleshooterComment GetTroubleshooterComment(int troubleshooterCommentId)
        {
            var apiMethod = string.Format("{0}/{1}", TroubleshooterCommentBaseUrl, troubleshooterCommentId);

            var troubleshooterComments = this.Connector.ExecuteGet<TroubleshooterCommentCollection>(apiMethod);

            if (troubleshooterComments != null && troubleshooterComments.Count > 0)
            {
                return troubleshooterComments[0];
            }

            return null;
        }

        public TroubleshooterComment CreateTroubleshooterComment(TroubleshooterCommentRequest troubleshooterCommentRequest)
        {
            var parameters = this.PopulateRequestParameters(troubleshooterCommentRequest, RequestTypes.Create);

            var troubleshooterComments = this.Connector.ExecutePost<TroubleshooterCommentCollection>(TroubleshooterCommentBaseUrl, parameters.ToString());

            if (troubleshooterComments != null && troubleshooterComments.Count > 0)
            {
                return troubleshooterComments[0];
            }

            return null;
        }

        public bool DeleteTroubleshooterComment(int troubleshooterCommentId)
        {
            var apiMethod = string.Format("{0}/{1}", TroubleshooterCommentBaseUrl, troubleshooterCommentId);

            return this.Connector.ExecuteDelete(apiMethod);
        }

        private RequestBodyBuilder PopulateRequestParameters(TroubleshooterCommentRequest troubleshooterCommentRequest, RequestTypes requestType)
        {
            troubleshooterCommentRequest.EnsureValidData(requestType);

            var parameters = new RequestBodyBuilder();
            parameters.AppendRequestDataNonNegativeInt("troubleshooterstepid", troubleshooterCommentRequest.TroubleshooterStepId);
            parameters.AppendRequestDataNonEmptyString("contents", troubleshooterCommentRequest.Contents);
            parameters.AppendRequestData("creatortype", EnumUtility.ToApiString(troubleshooterCommentRequest.CreatorType));
            parameters.AppendRequestDataNonNegativeInt("creatorid", troubleshooterCommentRequest.CreatorId);
            parameters.AppendRequestDataNonEmptyString("fullname", troubleshooterCommentRequest.FullName);
            parameters.AppendRequestDataNonEmptyString("email", troubleshooterCommentRequest.Email);
            parameters.AppendRequestDataNonNegativeInt("parentcommentid", troubleshooterCommentRequest.ParentCommentId);

            return parameters;
        }

        #endregion

        #region Troubleshooter Attachment Methods

        public TroubleshooterAttachmentCollection GetTroubleshooterAttachments(int troubleshooterStepId)
        {
            var apiMethod = string.Format("{0}/ListAll/{1}", TroubleshooterAttachmentBaseUrl, troubleshooterStepId);

            return this.Connector.ExecuteGet<TroubleshooterAttachmentCollection>(apiMethod);
        }

        public TroubleshooterAttachment GetTroubleshooterAttachment(int troubleshooterStepId, int troubleshooterAttachmentId)
        {
            var apiMethod = string.Format("{0}/{1}/{2}", TroubleshooterAttachmentBaseUrl, troubleshooterStepId, troubleshooterAttachmentId);

            var troubleshooterAttachments = this.Connector.ExecuteGet<TroubleshooterAttachmentCollection>(apiMethod);

            if (troubleshooterAttachments != null && troubleshooterAttachments.Count > 0)
            {
                return troubleshooterAttachments[0];
            }

            return null;
        }

        public TroubleshooterAttachment CreateTroubleshooterAttachment(TroubleshooterAttachmentRequest troubleshooterAttachmentRequest)
        {
            var parameters = PopulateRequestParameters(troubleshooterAttachmentRequest, RequestTypes.Create);

            var troubleshooterAttachments = this.Connector.ExecutePost<TroubleshooterAttachmentCollection>(TroubleshooterAttachmentBaseUrl, parameters.ToString());

            if (troubleshooterAttachments != null && troubleshooterAttachments.Count > 0)
            {
                return troubleshooterAttachments[0];
            }

            return null;
        }

        public bool DeleteTroubleshooterAttachment(int troubleshooterStepId, int troubleshooterAttachmentId)
        {
            var apiMethod = string.Format("{0}/{1}/{2}", TroubleshooterAttachmentBaseUrl, troubleshooterStepId, troubleshooterAttachmentId);

            return this.Connector.ExecuteDelete(apiMethod);
        }

        private static RequestBodyBuilder PopulateRequestParameters(TroubleshooterAttachmentRequest troubleshooterAttachmentRequest, RequestTypes requestType)
        {
            troubleshooterAttachmentRequest.EnsureValidData(requestType);

            var parameters = new RequestBodyBuilder();
            parameters.AppendRequestDataNonNegativeInt("troubleshooterstepid", troubleshooterAttachmentRequest.TroubleshooterStepId);
            parameters.AppendRequestDataNonEmptyString("filename", troubleshooterAttachmentRequest.FileName);
            parameters.AppendRequestDataNonEmptyString("contents", troubleshooterAttachmentRequest.Contents);

            return parameters;
        }

        #endregion
    }
}