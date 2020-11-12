using System.Xml.Serialization;

namespace KayakoRestApi.Core.Constants
{
    public enum TicketRequestCreationType
    {
        [XmlEnum("default")]
        Default,

        [XmlEnum("phone")]
        Phone
    }
}