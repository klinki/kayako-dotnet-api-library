using System.Net;
using KayakoRestApi.Core.News;
using KayakoRestApi.Net;
using KayakoRestApi.RequestBase;
using KayakoRestApi.Text;
using KayakoRestApi.Utilities;

namespace KayakoRestApi.Controllers
{
    public interface INewsController
    {
        NewsCategoryCollection GetNewsCategories();

        NewsCategory GetNewsCategory(int newsCategoryId);

        NewsCategory CreateNewsCategory(NewsCategoryRequest newsCategoryRequest);

        NewsCategory UpdateNewsCategory(NewsCategoryRequest newsCategoryRequest);

        bool DeleteNewsCategory(int newsCategoryId);

        NewsItemCollection GetNewsItems(int newsCategoryId);

        NewsItemCollection GetNewsItems();

        NewsItem GetNewsItem(int newsItemId);

        NewsItem CreateNewsItem(NewsItemRequest newsItemRequest);

        NewsItem UpdateNewsItem(NewsItemRequest newsItemRequest);

        bool DeleteNewsItem(int newsItemId);

        NewsSubscriberCollection GetNewsSubscribers();

        NewsSubscriber GetNewsSubscriber(int newsSubscriberId);

        NewsSubscriber CreateNewsSubscriber(NewsSubscriberRequest newsSubscriberRequest);

        NewsSubscriber UpdateNewsSubscriber(NewsSubscriberRequest newsSubscriberRequest);

        bool DeleteNewsSubscriber(int newsSubscriberId);

        NewsItemCommentCollection GetNewsItemComments(int newsItemId);

        NewsItemComment GetNewsItemComment(int newsItemCommentId);

        NewsItemComment CreateNewsItemComment(NewsItemCommentRequest newsItemCommentRequest);

        bool DeleteNewsItemComment(int newsItemCommentId);
    }

    public sealed class NewsController : BaseController, INewsController
    {
        private const string NewsCategoryBaseUrl = "/News/Category";
        private const string NewsItemBaseUrl = "/News/NewsItem";
        private const string NewsSubscriberBaseUrl = "/News/Subscriber";
        private const string NewsItemCommentBaseUrl = "/News/Comment";

        public NewsController(string apiKey, string secretKey, string apiUrl, IWebProxy proxy)
            : base(apiKey, secretKey, apiUrl, proxy) { }

        public NewsController(string apiKey, string secretKey, string apiUrl, IWebProxy proxy, ApiRequestType requestType)
            : base(apiKey, secretKey, apiUrl, proxy, requestType) { }

        public NewsController(IKayakoApiRequest kayakoApiRequest)
            : base(kayakoApiRequest) { }

        #region News Category Methods

        public NewsCategoryCollection GetNewsCategories() => this.Connector.ExecuteGet<NewsCategoryCollection>(NewsCategoryBaseUrl);

        public NewsCategory GetNewsCategory(int newsCategoryId)
        {
            var apiMethod = string.Format("{0}/{1}", NewsCategoryBaseUrl, newsCategoryId);

            var newsCategories = this.Connector.ExecuteGet<NewsCategoryCollection>(apiMethod);

            if (newsCategories != null && newsCategories.Count > 0)
            {
                return newsCategories[0];
            }

            return null;
        }

        public NewsCategory CreateNewsCategory(NewsCategoryRequest newsCategoryRequest)
        {
            var parameters = PopulateRequestParameters(newsCategoryRequest, RequestTypes.Create);

            var newsCategories = this.Connector.ExecutePost<NewsCategoryCollection>(NewsCategoryBaseUrl, parameters.ToString());

            if (newsCategories != null && newsCategories.Count > 0)
            {
                return newsCategories[0];
            }

            return null;
        }

        public NewsCategory UpdateNewsCategory(NewsCategoryRequest newsCategoryRequest)
        {
            var apiMethod = string.Format("{0}/{1}", NewsCategoryBaseUrl, newsCategoryRequest.Id);
            var parameters = PopulateRequestParameters(newsCategoryRequest, RequestTypes.Update);

            var newsCategories = this.Connector.ExecutePut<NewsCategoryCollection>(apiMethod, parameters.ToString());

            if (newsCategories != null && newsCategories.Count > 0)
            {
                return newsCategories[0];
            }

            return null;
        }

        public bool DeleteNewsCategory(int newsCategoryId)
        {
            var apiMethod = string.Format("{0}/{1}", NewsCategoryBaseUrl, newsCategoryId);

            return this.Connector.ExecuteDelete(apiMethod);
        }

        private static RequestBodyBuilder PopulateRequestParameters(NewsCategoryRequest newsCategory, RequestTypes requestType)
        {
            newsCategory.EnsureValidData(requestType);

            var parameters = new RequestBodyBuilder();

            if (!string.IsNullOrEmpty(newsCategory.Title))
            {
                parameters.AppendRequestData("title", newsCategory.Title);
            }

            parameters.AppendRequestData("visibilitytype", EnumUtility.ToApiString(newsCategory.VisibilityType));

            return parameters;
        }

        #endregion

        #region News Item Methods

        public NewsItemCollection GetNewsItems(int newsCategoryId)
        {
            var apiMethod = string.Format("{0}/ListAll/{1}", NewsItemBaseUrl, newsCategoryId);

            return this.Connector.ExecuteGet<NewsItemCollection>(apiMethod);
        }

        public NewsItemCollection GetNewsItems() => this.Connector.ExecuteGet<NewsItemCollection>(NewsItemBaseUrl);

        public NewsItem GetNewsItem(int newsItemId)
        {
            var apiMethod = string.Format("{0}/{1}", NewsItemBaseUrl, newsItemId);

            var newsItems = this.Connector.ExecuteGet<NewsItemCollection>(apiMethod);

            if (newsItems != null && newsItems.Count > 0)
            {
                return newsItems[0];
            }

            return null;
        }

        public NewsItem CreateNewsItem(NewsItemRequest newsItemRequest)
        {
            var parameters = PopulateRequestParameters(newsItemRequest, RequestTypes.Create);

            var newsItems = this.Connector.ExecutePost<NewsItemCollection>(NewsItemBaseUrl, parameters.ToString());

            if (newsItems != null && newsItems.Count > 0)
            {
                return newsItems[0];
            }

            return null;
        }

        public NewsItem UpdateNewsItem(NewsItemRequest newsItemRequest)
        {
            var apiMethod = string.Format("{0}/{1}", NewsItemBaseUrl, newsItemRequest.Id);

            var parameters = PopulateRequestParameters(newsItemRequest, RequestTypes.Update);

            var newsItems = this.Connector.ExecutePut<NewsItemCollection>(apiMethod, parameters.ToString());

            if (newsItems != null && newsItems.Count > 0)
            {
                return newsItems[0];
            }

            return null;
        }

        public bool DeleteNewsItem(int newsItemId)
        {
            var apiMethod = string.Format("{0}/{1}", NewsItemBaseUrl, newsItemId);

            return this.Connector.ExecuteDelete(apiMethod);
        }

        private static RequestBodyBuilder PopulateRequestParameters(NewsItemRequest newsItem, RequestTypes requestType)
        {
            newsItem.EnsureValidData(requestType);

            var parameters = new RequestBodyBuilder();
            parameters.AppendRequestDataNonEmptyString("subject", newsItem.Subject);
            parameters.AppendRequestDataNonEmptyString("contents", newsItem.Contents);

            if (requestType == RequestTypes.Create)
            {
                parameters.AppendRequestDataNonNegativeInt("staffid", newsItem.StaffId);
            }
            else
            {
                parameters.AppendRequestDataNonNegativeInt("editedstaffid", newsItem.StaffId);
            }

            if (requestType == RequestTypes.Create && newsItem.NewsItemType.HasValue)
            {
                parameters.AppendRequestData("newstype", EnumUtility.ToApiString(newsItem.NewsItemType));
            }

            if (newsItem.NewsItemStatus.HasValue)
            {
                parameters.AppendRequestData("newsstatus", EnumUtility.ToApiString(newsItem.NewsItemStatus));
            }

            parameters.AppendRequestDataNonEmptyString("fromname", newsItem.FromName);
            parameters.AppendRequestDataNonEmptyString("email", newsItem.Email);
            parameters.AppendRequestDataNonEmptyString("customemailsubject", newsItem.CustomEmailSubject);
            parameters.AppendRequestDataBool("sendemail", newsItem.SendEmail);
            parameters.AppendRequestDataBool("allowcomments", newsItem.AllowComments);
            parameters.AppendRequestDataBool("uservisibilitycustom", newsItem.UserVisibilityCustom);
            parameters.AppendRequestDataArrayCommaSeparated("usergroupidlist", newsItem.UserGroupIdList);
            parameters.AppendRequestDataBool("staffvisibilitycustom", newsItem.StaffVisibilityCustom);
            parameters.AppendRequestDataArrayCommaSeparated("staffgroupidlist", newsItem.StaffGroupIdList);
            parameters.AppendRequestData("expiry", newsItem.Expiry.DateTime.ToString("M/d/yyyy"));
            parameters.AppendRequestDataArrayCommaSeparated("newscategoryidlist", newsItem.Categories);

            return parameters;
        }

        #endregion

        #region News Subscriber Methods

        public NewsSubscriberCollection GetNewsSubscribers() => this.Connector.ExecuteGet<NewsSubscriberCollection>(NewsSubscriberBaseUrl);

        public NewsSubscriber GetNewsSubscriber(int newsSubscriberId)
        {
            var apiMethod = string.Format("{0}/{1}", NewsSubscriberBaseUrl, newsSubscriberId);

            var newsSubscribers = this.Connector.ExecuteGet<NewsSubscriberCollection>(apiMethod);

            if (newsSubscribers != null && newsSubscribers.Count > 0)
            {
                return newsSubscribers[0];
            }

            return null;
        }

        public NewsSubscriber CreateNewsSubscriber(NewsSubscriberRequest newsSubscriberRequest)
        {
            var parameters = this.PopulateRequestParameters(newsSubscriberRequest, RequestTypes.Create);

            var newsSubscriber = this.Connector.ExecutePost<NewsSubscriberCollection>(NewsSubscriberBaseUrl, parameters.ToString());

            if (newsSubscriber != null && newsSubscriber.Count > 0)
            {
                return newsSubscriber[0];
            }

            return null;
        }

        public NewsSubscriber UpdateNewsSubscriber(NewsSubscriberRequest newsSubscriberRequest)
        {
            var apiMethod = string.Format("{0}/{1}", NewsSubscriberBaseUrl, newsSubscriberRequest.Id);

            var parameters = this.PopulateRequestParameters(newsSubscriberRequest, RequestTypes.Update);

            var newsSubscriber = this.Connector.ExecutePut<NewsSubscriberCollection>(apiMethod, parameters.ToString());

            if (newsSubscriber != null && newsSubscriber.Count > 0)
            {
                return newsSubscriber[0];
            }

            return null;
        }

        public bool DeleteNewsSubscriber(int newsSubscriberId)
        {
            var apiMethod = string.Format("{0}/{1}", NewsSubscriberBaseUrl, newsSubscriberId);

            return this.Connector.ExecuteDelete(apiMethod);
        }

        private RequestBodyBuilder PopulateRequestParameters(NewsSubscriberRequest newsSubscriberRequest, RequestTypes requestTypes)
        {
            newsSubscriberRequest.EnsureValidData(requestTypes);

            var requestBodyBuilder = new RequestBodyBuilder();
            requestBodyBuilder.AppendRequestDataNonEmptyString("email", newsSubscriberRequest.Email);

            if (requestTypes == RequestTypes.Create)
            {
                requestBodyBuilder.AppendRequestDataBool("isvalidated", newsSubscriberRequest.IsValidated);
            }

            return requestBodyBuilder;
        }

        #endregion

        #region News Item Comment Methods

        public NewsItemCommentCollection GetNewsItemComments(int newsItemId)
        {
            var apiMethod = string.Format("{0}/ListAll/{1}", NewsItemCommentBaseUrl, newsItemId);

            return this.Connector.ExecuteGet<NewsItemCommentCollection>(apiMethod);
        }

        public NewsItemComment GetNewsItemComment(int newsItemCommentId)
        {
            var apiMethod = string.Format("{0}/{1}", NewsItemCommentBaseUrl, newsItemCommentId);

            var newsItemComments = this.Connector.ExecuteGet<NewsItemCommentCollection>(apiMethod);

            if (newsItemComments != null && newsItemComments.Count > 0)
            {
                return newsItemComments[0];
            }

            return null;
        }

        public NewsItemComment CreateNewsItemComment(NewsItemCommentRequest newsItemCommentRequest)
        {
            newsItemCommentRequest.EnsureValidData(RequestTypes.Create);

            var parameters = new RequestBodyBuilder();
            parameters.AppendRequestData("newsitemid", newsItemCommentRequest.NewsItemId);
            parameters.AppendRequestDataNonEmptyString("contents", newsItemCommentRequest.Contents);
            parameters.AppendRequestData("creatortype", EnumUtility.ToApiString(newsItemCommentRequest.CreatorType));

            if (newsItemCommentRequest.CreatorId != null)
            {
                parameters.AppendRequestData("creatorid", newsItemCommentRequest.CreatorId);
            }
            else
            {
                parameters.AppendRequestDataNonEmptyString("fullname", newsItemCommentRequest.FullName);
            }

            parameters.AppendRequestDataNonEmptyString("email", newsItemCommentRequest.Email);
            parameters.AppendRequestData("parentcommentid", newsItemCommentRequest.ParentCommentId);

            var newsItemComments = this.Connector.ExecutePost<NewsItemCommentCollection>(NewsItemCommentBaseUrl, parameters.ToString());

            if (newsItemComments != null && newsItemComments.Count > 0)
            {
                return newsItemComments[0];
            }

            return null;
        }

        public bool DeleteNewsItemComment(int newsItemCommentId)
        {
            var apiMethod = string.Format("{0}/{1}", NewsItemCommentBaseUrl, newsItemCommentId);

            return this.Connector.ExecuteDelete(apiMethod);
        }

        #endregion
    }
}