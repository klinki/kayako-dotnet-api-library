using System.Xml.Serialization;

namespace KayakoRestApi.Core.Constants
{
    public enum UserGroupType
    {
        [XmlEnum(Name = "guest")]
        Guest,

        [XmlEnum(Name = "registered")]
        Registered
    }
}