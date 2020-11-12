using KayakoRestApi.RequestBase;
using KayakoRestApi.RequestBase.Attributes;

namespace KayakoRestApi.Core.Troubleshooter
{
    public class TroubleshooterAttachmentRequest : RequestBaseObject
    {
        [RequiredField]
        [ResponseProperty("TroubleshooterStepId")]
        public int TroubleshooterStepId { get; set; }

        [RequiredField]
        [ResponseProperty("FileName")]
        public string FileName { get; set; }

        [RequiredField]
        [ResponseProperty("Contents")]
        public string Contents { get; set; }

        public static TroubleshooterAttachmentRequest FromResponseData(TroubleshooterAttachment responseData) => FromResponseType<TroubleshooterAttachment, TroubleshooterAttachmentRequest>(responseData);

        public static TroubleshooterAttachment ToResponseData(TroubleshooterAttachmentRequest requestData) => ToResponseType<TroubleshooterAttachmentRequest, TroubleshooterAttachment>(requestData);
    }
}