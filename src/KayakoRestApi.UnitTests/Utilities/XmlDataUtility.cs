using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace KayakoRestApi.UnitTests.Utilities
{
    public class XmlDataUtility
    {
        public static T ReadFromFile<T>(string filePath) => DeserializeObject<T>(filePath);

        private static T DeserializeObject<T>(string filePah)
        {
            var xmlFile = Path.Combine(Directory.GetCurrentDirectory(), filePah);

            var serializer = new XmlSerializer(typeof(T));

            using var fs = new FileStream(xmlFile, FileMode.Open);
            using var xtr = new XmlTextReader(fs);
            return (T) serializer.Deserialize(xtr);
        }
    }
}