using KayakoRestApi.Core.Constants;
using KayakoRestApi.RequestBase;
using KayakoRestApi.RequestBase.Attributes;

namespace KayakoRestApi.Core.News
{
    public class NewsCategoryRequest : RequestBaseObject
    {
        [RequiredField(RequestTypes.Update)]
        [ResponseProperty("Id")]
        public int Id { get; set; }

        [RequiredField]
        [ResponseProperty("Title")]
        public string Title { get; set; }

        [RequiredField]
        [ResponseProperty("VisibilityType")]
        public NewsCategoryVisibilityType VisibilityType { get; set; }

        public static NewsCategoryRequest FromResponseData(NewsCategory responseData) => FromResponseType<NewsCategory, NewsCategoryRequest>(responseData);

        public static NewsCategory ToResponseData(NewsCategoryRequest requestData) => ToResponseType<NewsCategoryRequest, NewsCategory>(requestData);
    }
}