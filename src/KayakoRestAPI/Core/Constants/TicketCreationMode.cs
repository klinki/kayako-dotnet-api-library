using System.Xml.Serialization;

namespace KayakoRestApi.Core.Constants
{
    public enum TicketCreationMode
    {
        [XmlEnum("1")]
        Supportcenter = 1,
        
        [XmlEnum("2")]
        Staffcp = 2,
        
        [XmlEnum("3")]
        Email = 3,
        
        [XmlEnum("4")]
        API = 4,
        
        [XmlEnum("5")]
        Sitebadge = 5,
        
        [XmlEnum("6")]
        Mobile = 6,
        
        [XmlEnum("7")]
        Staffapi = 7,
    }
}