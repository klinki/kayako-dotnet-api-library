using KayakoRestApi.RequestBase;
using KayakoRestApi.RequestBase.Attributes;

namespace KayakoRestApi.Core.News
{
    public class NewsSubscriberRequest : RequestBaseObject
    {
        [RequiredField(RequestTypes.Update)]
        [ResponseProperty("Id")]
        public int Id { get; set; }

        [RequiredField]
        [ResponseProperty("Email")]
        public string Email { get; set; }

        [OptionalField]
        [ResponseProperty("IsValidated")]
        public bool? IsValidated { get; set; }

        public static NewsSubscriberRequest FromResponseData(NewsSubscriber responseData) => FromResponseType<NewsSubscriber, NewsSubscriberRequest>(responseData);

        public static NewsSubscriber ToResponseData(NewsSubscriberRequest requestData) => ToResponseType<NewsSubscriberRequest, NewsSubscriber>(requestData);
    }
}