using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using KayakoRestApi.Utilities;

namespace KayakoRestApi.Data
{
    public class UnixDateTime : IXmlSerializable
    {
        private long unixDateTime;

        public UnixDateTime() { }

        public UnixDateTime(DateTime dateTime) => this.unixDateTime = UnixTimeUtility.ToUnixTime(dateTime);

        public UnixDateTime(long epochDateTime) => this.unixDateTime = epochDateTime;

        [XmlIgnore]
        public long UnixTimeStamp
        {
            get => this.unixDateTime;
            set => this.unixDateTime = value;
        }

        [XmlIgnore]
        public DateTime DateTime => this.unixDateTime != 0 ? UnixTimeUtility.FromUnixTime(this.unixDateTime) : DateTime.MinValue;

        public XmlSchema GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();

            if (!reader.IsEmptyElement)
            {
                var value = reader.ReadElementContentAsString();

                if (long.TryParse(value, out this.unixDateTime))
                {
                    return;
                }
            }

            this.unixDateTime = 0;
        }

        public void WriteXml(XmlWriter writer) => writer.WriteString(this.unixDateTime.ToString());
    }
}