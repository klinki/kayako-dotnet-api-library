using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace KayakoRestApi.Data
{
    [Serializable]
    public class KNullable<T> : IXmlSerializable
        where T : struct
    {
        public KNullable() => this.ValueData = new T?();

        public KNullable(T value) => this.ValueData = value;

        private T? ValueData { get; set; }

        public T Value => this.ValueData.Value;

        public bool HasValue => this.ValueData.HasValue;

        public static KNullable<T> ToKNullable(T value) => new KNullable<T>(value);

        public static implicit operator KNullable<T>(T value) => new KNullable<T>(value);

        public static T FromKNullable(KNullable<T> value) => value.Value;

        public static explicit operator T(KNullable<T> value) => value.Value;

        public override int GetHashCode() => this.ValueData.GetHashCode();

        public override bool Equals(object obj) => this.ValueData.Equals(obj);

        public override string ToString() => this.ValueData?.ToString();

        #region IXmlSerializable Methods

        public XmlSchema GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();

            if (!reader.IsEmptyElement)
            {
                var value = reader.ReadElementContentAsString();

                if (!string.IsNullOrEmpty(value))
                {
                    this.ValueData = (T) Convert.ChangeType(value, typeof(T));
                }
            }
            else
            {
                this.ValueData = new T?();
            }
        }

        public void WriteXml(XmlWriter writer) => writer.WriteString(this.ValueData.HasValue ? this.ValueData.Value.ToString() : string.Empty);

        #endregion
    }
}