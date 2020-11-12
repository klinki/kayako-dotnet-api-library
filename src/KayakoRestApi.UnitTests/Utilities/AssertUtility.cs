using System.IO;
using System.Text;
using System.Xml.Serialization;
using NUnit.Framework;

namespace KayakoRestApi.UnitTests.Utilities
{
    public static class AssertUtility
    {
        public static void ObjectsEqual<T>(T expected, T actual) => Assert.That(SerializeObject(expected), Is.EqualTo(SerializeObject(actual)));

        private static string SerializeObject<T>(T objectToSerialize)
        {
            var serializer = new XmlSerializer(typeof(T));
            var sb = new StringBuilder();

            using var sw = new StringWriter(sb);
            serializer.Serialize(sw, objectToSerialize);

            return sb.ToString();
        }
    }
}