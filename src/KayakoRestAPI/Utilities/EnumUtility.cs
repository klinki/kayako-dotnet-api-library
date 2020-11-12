using System.Xml.Serialization;

namespace KayakoRestApi.Utilities
{
    public static class EnumUtility
    {
        public static string ToApiString(object enumValue)
        {
            var fi = enumValue.GetType().GetField(enumValue.ToString());

            var attributes = (XmlEnumAttribute[]) fi.GetCustomAttributes(typeof(XmlEnumAttribute), false);

            return attributes.Length > 0 ? attributes[0].Name : enumValue.ToString();
        }
    }
}