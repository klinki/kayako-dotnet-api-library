using System.Xml.Serialization;

namespace KayakoRestApi.Core.Constants
{
    public enum UserRole
    {
        [XmlEnum(Name = "user")]
        User,

        [XmlEnum(Name = "manager")]
        Manager
    }
}