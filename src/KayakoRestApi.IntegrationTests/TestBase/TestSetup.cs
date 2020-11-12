using System.IO;
using KayakoRestApi.Net;

namespace KayakoRestApi.IntegrationTests.TestBase
{
    public static class TestSetup
    {
        public static string ApiKey => ReadTextFile("TestBase/ApiKey.txt");

        public static string SecretKey => ReadTextFile("TestBase/SecretKey.txt");

        public static string ApiUrl => ReadTextFile("TestBase/ApiUrl.txt");

        public static KayakoClient KayakoApiService => new KayakoClient(ApiKey, SecretKey, ApiUrl, ApiRequestType.Url);

        private static string ReadTextFile(string textFilePath)
        {
            using var fileStream = File.OpenRead(textFilePath);
            using var streamReader = new StreamReader(fileStream);
            return streamReader.ReadToEnd();
        }
    }
}