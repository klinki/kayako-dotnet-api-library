using KayakoRestApi.RequestBase;
using KayakoRestApi.RequestBase.Attributes;

namespace KayakoRestApi.Core.Knowledgebase
{
    public class KnowledgebaseAttachmentRequest : RequestBaseObject
    {
        [RequiredField]
        [ResponseProperty("KnowledgebaseArticleId")]
        public int KnowledgebaseArticleId { get; set; }

        [RequiredField]
        [ResponseProperty("FileName")]
        public string FileName { get; set; }

        [RequiredField]
        [ResponseProperty("Contents")]
        public string Contents { get; set; }

        public static KnowledgebaseAttachmentRequest FromResponseData(KnowledgebaseAttachment responseData) => FromResponseType<KnowledgebaseAttachment, KnowledgebaseAttachmentRequest>(responseData);

        public static KnowledgebaseAttachment ToResponseData(KnowledgebaseAttachmentRequest requestData) => ToResponseType<KnowledgebaseAttachmentRequest, KnowledgebaseAttachment>(requestData);
    }
}