using System.Net;
using KayakoRestApi.Core.Knowledgebase;
using KayakoRestApi.Net;
using KayakoRestApi.RequestBase;
using KayakoRestApi.Text;
using KayakoRestApi.Utilities;

namespace KayakoRestApi.Controllers
{
    public interface IKnowledgebaseController
    {
        KnowledgebaseCategoryCollection GetKnowledgebaseCategories();

        KnowledgebaseCategoryCollection GetKnowledgebaseCategories(int count, int start);

        KnowledgebaseCategory GetKnowledgebaseCategory(int knowledgebaseCategoryId);

        KnowledgebaseCategory CreateKnowledgebaseCategory(KnowledgebaseCategoryRequest knowledgebaseCategoryRequest);

        KnowledgebaseCategory UpdateKnowledgebaseCategory(KnowledgebaseCategoryRequest knowledgebaseCategoryRequest);

        bool DeleteKnowledgebaseCategory(int knowledgebaseCategoryId);

        KnowledgebaseArticleCollection GetKnowledgebaseArticles();

        KnowledgebaseArticleCollection GetKnowledgebaseArticles(int count, int start);

        KnowledgebaseArticle GetKnowledgebaseArticle(int knowledgebaseArticleId);

        KnowledgebaseArticle CreateKnowledgebaseArticle(KnowledgebaseArticleRequest knowledgebaseArticleRequest);

        KnowledgebaseArticle UpdateKnowledgebaseArticle(KnowledgebaseArticleRequest knowledgebaseArticleRequest);

        bool DeleteKnowledgebaseArticle(int knowledgebaseArticleId);

        KnowledgebaseCommentCollection GetKnowledgebaseComments(int knowledgebaseArticleId);

        KnowledgebaseComment GetKnowledgebaseComment(int knowledgebaseCommentId);

        KnowledgebaseComment CreateKnowledgebaseComment(KnowledgebaseCommentRequest knowledgebaseCommentRequest);

        bool DeleteKnowledgebaseComment(int knowledgebaseCommentId);

        KnowledgebaseAttachmentCollection GetKnowledgebaseAttachments(int knowledgebaseArticleId);

        KnowledgebaseAttachment GetKnowledgebaseAttachment(int knowledgebaseArticleId, int attachmentId);

        KnowledgebaseAttachment CreateKnowledgebaseAttachment(KnowledgebaseAttachmentRequest knowledgebaseAttachmentRequest);

        bool DeleteKnowledgebaseAttachment(int knowledgebaseArticleId, int knowledgebaseAttachmentId);
    }

    public sealed class KnowledgebaseController : BaseController, IKnowledgebaseController
    {
        private const string KnowledgebaseCategoryBaseUrl = "/Knowledgebase/Category";
        private const string KnowledgebaseArticleBaseUrl = "/Knowledgebase/Article";
        private const string KnowledgebaseCommentBaseUrl = "/Knowledgebase/Comment";
        private const string KnowledgebaseAttachmentBaseUrl = "/Knowledgebase/Attachment";

        public KnowledgebaseController(string apiKey, string secretKey, string apiUrl, IWebProxy proxy)
            : base(apiKey, secretKey, apiUrl, proxy) { }

        public KnowledgebaseController(string apiKey, string secretKey, string apiUrl, IWebProxy proxy, ApiRequestType requestType)
            : base(apiKey, secretKey, apiUrl, proxy, requestType) { }

        public KnowledgebaseController(IKayakoApiRequest kayakoApiRequest)
            : base(kayakoApiRequest) { }

        #region Knowledgebase Category Methods

        public KnowledgebaseCategoryCollection GetKnowledgebaseCategories() => this.GetKnowledgebaseCategories(-1, -1);

        public KnowledgebaseCategoryCollection GetKnowledgebaseCategories(int count, int start)
        {
            var apiMethod = string.Format("{0}/ListAll/{1}/{2}", KnowledgebaseCategoryBaseUrl, count, start);

            return this.Connector.ExecuteGet<KnowledgebaseCategoryCollection>(apiMethod);
        }

        public KnowledgebaseCategory GetKnowledgebaseCategory(int knowledgebaseCategoryId)
        {
            var apiMethod = string.Format("{0}/{1}", KnowledgebaseCategoryBaseUrl, knowledgebaseCategoryId);

            var knowledgebaseCategories = this.Connector.ExecuteGet<KnowledgebaseCategoryCollection>(apiMethod);

            if (knowledgebaseCategories != null && knowledgebaseCategories.Count > 0)
            {
                return knowledgebaseCategories[0];
            }

            return null;
        }

        public KnowledgebaseCategory CreateKnowledgebaseCategory(KnowledgebaseCategoryRequest knowledgebaseCategoryRequest)
        {
            var parameters = this.PopulateRequestParameters(knowledgebaseCategoryRequest, RequestTypes.Create);

            var knowledgebaseCategories = this.Connector.ExecutePost<KnowledgebaseCategoryCollection>(KnowledgebaseCategoryBaseUrl, parameters.ToString());

            if (knowledgebaseCategories != null && knowledgebaseCategories.Count > 0)
            {
                return knowledgebaseCategories[0];
            }

            return null;
        }

        public KnowledgebaseCategory UpdateKnowledgebaseCategory(KnowledgebaseCategoryRequest knowledgebaseCategoryRequest)
        {
            var apiMethod = string.Format("{0}/{1}", KnowledgebaseCategoryBaseUrl, knowledgebaseCategoryRequest.Id);
            var parameters = this.PopulateRequestParameters(knowledgebaseCategoryRequest, RequestTypes.Update);

            var knowledgebaseCategories = this.Connector.ExecutePut<KnowledgebaseCategoryCollection>(apiMethod, parameters.ToString());

            if (knowledgebaseCategories != null && knowledgebaseCategories.Count > 0)
            {
                return knowledgebaseCategories[0];
            }

            return null;
        }

        public bool DeleteKnowledgebaseCategory(int knowledgebaseCategoryId)
        {
            var apiMethod = string.Format("{0}/{1}", KnowledgebaseCategoryBaseUrl, knowledgebaseCategoryId);

            return this.Connector.ExecuteDelete(apiMethod);
        }

        private RequestBodyBuilder PopulateRequestParameters(KnowledgebaseCategoryRequest knowledgebaseCategoryRequest, RequestTypes requestType)
        {
            knowledgebaseCategoryRequest.EnsureValidData(requestType);

            var parameters = new RequestBodyBuilder();
            parameters.AppendRequestDataNonEmptyString("title", knowledgebaseCategoryRequest.Title);

            if (knowledgebaseCategoryRequest.CategoryType.HasValue)
            {
                parameters.AppendRequestData("categorytype",
                    EnumUtility.ToApiString(knowledgebaseCategoryRequest.CategoryType.Value));
            }

            if (knowledgebaseCategoryRequest.ParentCategoryId.HasValue)
            {
                parameters.AppendRequestData("parentcategoryid", knowledgebaseCategoryRequest.ParentCategoryId.Value);
            }

            if (knowledgebaseCategoryRequest.DisplayOrder.HasValue)
            {
                parameters.AppendRequestDataNonNegativeInt("displayorder", knowledgebaseCategoryRequest.DisplayOrder.Value);
            }

            if (knowledgebaseCategoryRequest.ArticleSortOrder.HasValue)
            {
                parameters.AppendRequestData("articlesortorder",
                    EnumUtility.ToApiString(knowledgebaseCategoryRequest.ArticleSortOrder.Value));
            }

            parameters.AppendRequestDataBool("allowcomments", knowledgebaseCategoryRequest.AllowComments);
            parameters.AppendRequestDataBool("allowrating", knowledgebaseCategoryRequest.AllowRating);
            parameters.AppendRequestDataBool("ispublished", knowledgebaseCategoryRequest.IsPublished);
            parameters.AppendRequestDataBool("uservisibilitycustom", knowledgebaseCategoryRequest.UserVisibilityCustom);
            parameters.AppendRequestDataArrayCommaSeparated("usergroupidlist", knowledgebaseCategoryRequest.UserGroupIdList);
            parameters.AppendRequestDataBool("staffvisibilitycustom", knowledgebaseCategoryRequest.StaffVisibilityCustom);
            parameters.AppendRequestDataArrayCommaSeparated("staffgroupidlist", knowledgebaseCategoryRequest.StaffGroupIdList);

            if (requestType == RequestTypes.Create && knowledgebaseCategoryRequest.StaffId.HasValue)
            {
                parameters.AppendRequestDataNonNegativeInt("staffid", knowledgebaseCategoryRequest.StaffId.Value);
            }

            return parameters;
        }

        #endregion

        #region Knowledgebase Article Methods

        public KnowledgebaseArticleCollection GetKnowledgebaseArticles() => this.Connector.ExecuteGet<KnowledgebaseArticleCollection>(KnowledgebaseArticleBaseUrl);

        public KnowledgebaseArticleCollection GetKnowledgebaseArticles(int count, int start)
        {
            var apiMethod = string.Format("{0}/ListAll/{1}/{2}", KnowledgebaseArticleBaseUrl, count, start);

            return this.Connector.ExecuteGet<KnowledgebaseArticleCollection>(apiMethod);
        }

        public KnowledgebaseArticle GetKnowledgebaseArticle(int knowledgebaseArticleId)
        {
            var apiMethod = string.Format("{0}/{1}", KnowledgebaseArticleBaseUrl, knowledgebaseArticleId);

            var knowledgebaseArticles = this.Connector.ExecuteGet<KnowledgebaseArticleCollection>(apiMethod);

            if (knowledgebaseArticles != null && knowledgebaseArticles.Count > 0)
            {
                return knowledgebaseArticles[0];
            }

            return null;
        }

        public KnowledgebaseArticle CreateKnowledgebaseArticle(KnowledgebaseArticleRequest knowledgebaseArticleRequest)
        {
            var parameters = this.PopulateRequestParameters(knowledgebaseArticleRequest, RequestTypes.Create);

            var knowledgebaseArticles = this.Connector.ExecutePost<KnowledgebaseArticleCollection>(KnowledgebaseArticleBaseUrl, parameters.ToString());

            if (knowledgebaseArticles != null && knowledgebaseArticles.Count > 0)
            {
                return knowledgebaseArticles[0];
            }

            return null;
        }

        public KnowledgebaseArticle UpdateKnowledgebaseArticle(KnowledgebaseArticleRequest knowledgebaseArticleRequest)
        {
            var apiMethod = string.Format("{0}/{1}", KnowledgebaseArticleBaseUrl, knowledgebaseArticleRequest.Id);
            var parameters = this.PopulateRequestParameters(knowledgebaseArticleRequest, RequestTypes.Update);

            var knowledgebaseArticles = this.Connector.ExecutePut<KnowledgebaseArticleCollection>(apiMethod, parameters.ToString());

            if (knowledgebaseArticles != null && knowledgebaseArticles.Count > 0)
            {
                return knowledgebaseArticles[0];
            }

            return null;
        }

        public bool DeleteKnowledgebaseArticle(int knowledgebaseArticleId)
        {
            var apiMethod = string.Format("{0}/{1}", KnowledgebaseArticleBaseUrl, knowledgebaseArticleId);

            return this.Connector.ExecuteDelete(apiMethod);
        }

        private RequestBodyBuilder PopulateRequestParameters(KnowledgebaseArticleRequest knowledgebaseArticleRequest, RequestTypes requestType)
        {
            knowledgebaseArticleRequest.EnsureValidData(requestType);

            var parameters = new RequestBodyBuilder();
            parameters.AppendRequestDataNonEmptyString("subject", knowledgebaseArticleRequest.Subject);
            parameters.AppendRequestDataNonEmptyString("contents", knowledgebaseArticleRequest.Contents);

            if (requestType == RequestTypes.Create && knowledgebaseArticleRequest.CreatorId.HasValue)
            {
                parameters.AppendRequestDataNonNegativeInt("creatorid", knowledgebaseArticleRequest.CreatorId.Value);
            }

            if (knowledgebaseArticleRequest.ArticleStatus.HasValue)
            {
                parameters.AppendRequestData("articlestatus", EnumUtility.ToApiString(knowledgebaseArticleRequest.ArticleStatus.Value));
            }

            parameters.AppendRequestDataBool("isfeatured", knowledgebaseArticleRequest.IsFeatured);
            parameters.AppendRequestDataBool("allowcomments", knowledgebaseArticleRequest.AllowComments);
            parameters.AppendRequestDataArrayCommaSeparated("categoryid", knowledgebaseArticleRequest.CategoryIds);

            if (requestType == RequestTypes.Update && knowledgebaseArticleRequest.EditedStaffId.HasValue)
            {
                parameters.AppendRequestDataNonNegativeInt("editedstaffid", knowledgebaseArticleRequest.EditedStaffId.Value);
            }

            return parameters;
        }

        #endregion

        #region Knowledgebase Comments Methods

        public KnowledgebaseCommentCollection GetKnowledgebaseComments(int knowledgebaseArticleId)
        {
            var apiMethod = string.Format("{0}/ListAll/{1}", KnowledgebaseCommentBaseUrl, knowledgebaseArticleId);

            return this.Connector.ExecuteGet<KnowledgebaseCommentCollection>(apiMethod);
        }

        public KnowledgebaseComment GetKnowledgebaseComment(int knowledgebaseCommentId)
        {
            var apiMethod = string.Format("{0}/{1}", KnowledgebaseCommentBaseUrl, knowledgebaseCommentId);

            var knowledgebaseComments = this.Connector.ExecuteGet<KnowledgebaseCommentCollection>(apiMethod);

            if (knowledgebaseComments != null && knowledgebaseComments.Count > 0)
            {
                return knowledgebaseComments[0];
            }

            return null;
        }

        public KnowledgebaseComment CreateKnowledgebaseComment(KnowledgebaseCommentRequest knowledgebaseCommentRequest)
        {
            var parameters = this.PopulateRequestParameters(knowledgebaseCommentRequest, RequestTypes.Create);

            var knowledgebaseComments = this.Connector.ExecutePost<KnowledgebaseCommentCollection>(KnowledgebaseCommentBaseUrl, parameters.ToString());

            if (knowledgebaseComments != null && knowledgebaseComments.Count > 0)
            {
                return knowledgebaseComments[0];
            }

            return null;
        }

        public bool DeleteKnowledgebaseComment(int knowledgebaseCommentId)
        {
            var apiMethod = string.Format("{0}/{1}", KnowledgebaseCommentBaseUrl, knowledgebaseCommentId);

            return this.Connector.ExecuteDelete(apiMethod);
        }

        private RequestBodyBuilder PopulateRequestParameters(KnowledgebaseCommentRequest knowledgebaseCommentRequest, RequestTypes requestType)
        {
            knowledgebaseCommentRequest.EnsureValidData(requestType);

            var parameters = new RequestBodyBuilder();
            parameters.AppendRequestDataNonNegativeInt("knowledgebasearticleid", knowledgebaseCommentRequest.KnowledgebaseArticleId);
            parameters.AppendRequestDataNonEmptyString("contents", knowledgebaseCommentRequest.Contents);
            parameters.AppendRequestData("creatortype", EnumUtility.ToApiString(knowledgebaseCommentRequest.CreatorType));

            if (knowledgebaseCommentRequest.CreatorId.HasValue)
            {
                parameters.AppendRequestDataNonNegativeInt("creatorid", knowledgebaseCommentRequest.CreatorId.Value);
            }
            else
            {
                parameters.AppendRequestDataNonEmptyString("fullname", knowledgebaseCommentRequest.FullName);
            }

            parameters.AppendRequestDataNonEmptyString("email", knowledgebaseCommentRequest.Email);

            if (knowledgebaseCommentRequest.ParentCommentId.HasValue)
            {
                parameters.AppendRequestDataNonNegativeInt("parentcommentid", knowledgebaseCommentRequest.ParentCommentId.Value);
            }

            return parameters;
        }

        #endregion

        #region Knowledgebase Attachment Methods

        public KnowledgebaseAttachmentCollection GetKnowledgebaseAttachments(int knowledgebaseArticleId)
        {
            var apiMethod = string.Format("{0}/ListAll/{1}", KnowledgebaseAttachmentBaseUrl, knowledgebaseArticleId);

            return this.Connector.ExecuteGet<KnowledgebaseAttachmentCollection>(apiMethod);
        }

        public KnowledgebaseAttachment GetKnowledgebaseAttachment(int knowledgebaseArticleId, int attachmentId)
        {
            var apiMethod = string.Format("{0}/{1}/{2}", KnowledgebaseAttachmentBaseUrl, knowledgebaseArticleId, attachmentId);

            var knowledgebaseAttachments = this.Connector.ExecuteGet<KnowledgebaseAttachmentCollection>(apiMethod);

            if (knowledgebaseAttachments != null && knowledgebaseAttachments.Count > 0)
            {
                return knowledgebaseAttachments[0];
            }

            return null;
        }

        public KnowledgebaseAttachment CreateKnowledgebaseAttachment(KnowledgebaseAttachmentRequest knowledgebaseAttachmentRequest)
        {
            const string apiMethod = KnowledgebaseAttachmentBaseUrl;
            var parameters = this.PopulateRequestParameters(knowledgebaseAttachmentRequest, RequestTypes.Create);

            var knowledgebaseAttachments = this.Connector.ExecutePost<KnowledgebaseAttachmentCollection>(apiMethod, parameters.ToString());

            if (knowledgebaseAttachments != null && knowledgebaseAttachments.Count > 0)
            {
                return knowledgebaseAttachments[0];
            }

            return null;
        }

        public bool DeleteKnowledgebaseAttachment(int knowledgebaseArticleId, int knowledgebaseAttachmentId)
        {
            var apiMethod = string.Format("{0}/{1}/{2}", KnowledgebaseAttachmentBaseUrl, knowledgebaseArticleId, knowledgebaseAttachmentId);

            return this.Connector.ExecuteDelete(apiMethod);
        }

        private RequestBodyBuilder PopulateRequestParameters(KnowledgebaseAttachmentRequest knowledgebaseAttachmentRequest, RequestTypes requestType)
        {
            knowledgebaseAttachmentRequest.EnsureValidData(requestType);

            var parameters = new RequestBodyBuilder();
            parameters.AppendRequestDataNonNegativeInt("kbarticleid", knowledgebaseAttachmentRequest.KnowledgebaseArticleId);
            parameters.AppendRequestDataNonEmptyString("filename", knowledgebaseAttachmentRequest.FileName);
            parameters.AppendRequestDataNonEmptyString("contents", knowledgebaseAttachmentRequest.Contents);

            return parameters;
        }

        #endregion
    }
}