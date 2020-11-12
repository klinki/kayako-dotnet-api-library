using System.Net;
using KayakoRestApi.Net;

namespace KayakoRestApi.Controllers
{
    public class BaseController
    {
        internal BaseController(string apiKey, string secretKey, string apiUrl, IWebProxy proxy) => this.Connector = new KayakoApiRequest(apiKey, secretKey, apiUrl, proxy, ApiRequestType.QueryString);

        internal BaseController(string apiKey, string secretKey, string apiUrl, IWebProxy proxy, ApiRequestType requestType) => this.Connector = new KayakoApiRequest(apiKey, secretKey, apiUrl, proxy, requestType);

        internal BaseController(IKayakoApiRequest kayakoApiRequest) => this.Connector = kayakoApiRequest;

        internal IKayakoApiRequest Connector { get; }
    }
}