using System.Xml.Serialization;

namespace KayakoRestApi.Core.Constants
{
    /// <summary>
    ///     Represents the flag types
    /// </summary>
    /// <remarks>
    ///     http://wiki.kayako.com/display/DEV/Mobile+-+Constants
    /// </remarks>
    public enum FlagType
    {
        [XmlEnum(Name = "0")]
        NoFlag = 0,

        [XmlEnum(Name = "1")]
        Purple = 1,

        [XmlEnum(Name = "2")]
        Orange = 2,

        [XmlEnum(Name = "3")]
        Green = 3,

        [XmlEnum(Name = "4")]
        Yellow = 4,

        [XmlEnum(Name = "5")]
        Red = 5,

        [XmlEnum(Name = "6")]
        Blue = 6
    }
}