using KayakoRestApi.Core.Constants;
using KayakoRestApi.RequestBase;
using KayakoRestApi.RequestBase.Attributes;

namespace KayakoRestApi.Core.Troubleshooter
{
    public class TroubleshooterCommentRequest : RequestBaseObject
    {
        [RequiredField(RequestTypes.Create)]
        [ResponseProperty("TroubleshooterStepId")]
        public int TroubleshooterStepId { get; set; }

        [RequiredField(RequestTypes.Create)]
        [ResponseProperty("Contents")]
        public string Contents { get; set; }

        [RequiredField(RequestTypes.Create)]
        [ResponseProperty("CreatorType")]
        public TroubleshooterCommentCreatorType CreatorType { get; set; }

        [ResponseProperty("CreatorId")]
        public int CreatorId { get; set; }

        [ResponseProperty("FullName")]
        public string FullName { get; set; }

        [ResponseProperty("Email")]
        public string Email { get; set; }

        [ResponseProperty("ParentCommentId")]
        public int ParentCommentId { get; set; }

        public static TroubleshooterCommentRequest FromResponseData(TroubleshooterComment responseData) => FromResponseType<TroubleshooterComment, TroubleshooterCommentRequest>(responseData);

        public static TroubleshooterComment ToResponseData(TroubleshooterCommentRequest requestData) => ToResponseType<TroubleshooterCommentRequest, TroubleshooterComment>(requestData);
    }
}