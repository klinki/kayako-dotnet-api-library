using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using NUnit.Framework;

namespace KayakoRestApi.IntegrationTests.TestBase
{
    public class UnitTestBase
    {
        public void OutputMessage<T>(string preMessage, T dataToOutput)
        {
            var serializedObject = SerializeObject(dataToOutput);

            OutputMessage(string.Format("{0}{1}{2}", preMessage, Environment.NewLine, serializedObject));
        }

        public static void OutputMessage(string message) => Console.WriteLine(message);

        public static void AssertObjectXmlEqual<T>(T expected, T actual)
        {
            var expectedXml = SerializeObject(expected);
            var actualXml = SerializeObject(actual);

            Assert.AreEqual(expectedXml, actualXml);
        }

        private static string SerializeObject<T>(T serializeObject)
        {
            var serializer = new XmlSerializer(typeof(T));

            var sb = new StringBuilder();

            using var sw = new StringWriter(sb);
            serializer.Serialize(sw, serializeObject);

            return sb.ToString();
        }
    }
}