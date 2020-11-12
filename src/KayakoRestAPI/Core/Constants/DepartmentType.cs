using System.Xml.Serialization;

namespace KayakoRestApi.Core.Constants
{
    public enum DepartmentType
    {
        [XmlEnum(Name = "public")]
        Public,

        [XmlEnum(Name = "private")]
        Private
    }
}