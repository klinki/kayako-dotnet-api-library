using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using KayakoRestApi;
using KayakoRestApi.Net;

namespace KayakoRestAPI.Testing
{
    /// <summary>
    ///     A number of example uses of the .Net Api for Kayako
    /// </summary>
    public static class Program
    {
        private const string ApiKey = "8c75489a-45c6-b114-e597-88c5f462ff53";
        private const string SecretKey = "NWQ3N2YyMzEtNTYwMi1lMzE0LWQ1OTAtMGM1ZGQyZDdjYmVkZTIyZGVlMTMtZDJiOS01OTk0LTg5ZmMtMjE4MmNjMjZkMmIx";
        private const string ApiURL = @"http://jamietestingagain.kayako.com/api/"; //Note: No trailing ?

        private static void Main(string[] args)
        {
            var client = new KayakoClient(ApiKey, SecretKey, ApiURL, ApiRequestType.Url);

            var tickets = client.Tickets.GetTickets(new[] { 1, 2, 3, 4, 5, 6 }, -1, -1);

            Console.WriteLine(string.Join(",", tickets.Select(t => t.Id.ToString()).ToArray()));

            tickets = client.Tickets.GetTickets(new[] { 1, 2, 3, 4, 5, 6 }, 4, -1);

            Console.WriteLine(string.Join(",", tickets.Select(t => t.Id.ToString()).ToArray()));

            tickets = client.Tickets.GetTickets(new[] { 1, 2, 3, 4, 5, 6 }, 4, 0);

            Console.WriteLine(string.Join(",", tickets.Select(t => t.Id.ToString()).ToArray()));

            tickets = client.Tickets.GetTickets(new[] { 1, 2, 3, 4, 5, 6 }, -1, 4);

            Console.WriteLine(string.Join(",", tickets.Select(t => t.Id.ToString()).ToArray()));

            tickets = client.Tickets.GetTickets(new[] { 1, 2, 3, 4, 5, 6 }, 0, 4);

            Console.WriteLine(string.Join(",", tickets.Select(t => t.Id.ToString()).ToArray()));

            Console.ReadLine();
        }

        private static T DeserializeObject<T>(string xmlFile)
        {
            var serializer = new XmlSerializer(typeof(T));

            using var fs = new FileStream(xmlFile, FileMode.Open);
            using var xtr = new XmlTextReader(fs);
            return (T) serializer.Deserialize(xtr);
        }

        private static void OutputData<T>(string prefix, object o)
        {
            var serializer = new XmlSerializer(typeof(T));
            var sb = new StringBuilder();

            using (var sw = new StringWriter(sb))
            {
                serializer.Serialize(sw, o);
            }

            Console.WriteLine(prefix + Environment.NewLine);
            Console.WriteLine(sb.ToString());
        }
    }
}