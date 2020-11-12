using KayakoRestApi.RequestBase;
using KayakoRestApi.RequestBase.Attributes;

namespace KayakoRestApi.Core.Staff
{
    public class StaffGroupRequest : RequestBaseObject
    {
        [RequiredField(RequestTypes.Update)]
        [ResponseProperty("Id")]
        public int Id { get; set; }

        [RequiredField]
        [ResponseProperty("Title")]
        public string Title { get; set; }

        [RequiredField(RequestTypes.Create)]
        [ResponseProperty("IsAdmin")]
        public bool IsAdmin { get; set; }

        public static StaffGroupRequest FromResponseData(StaffGroup responseData) => FromResponseType<StaffGroup, StaffGroupRequest>(responseData);

        public static StaffGroup ToResponseData(StaffGroupRequest requestData) => ToResponseType<StaffGroupRequest, StaffGroup>(requestData);
    }
}