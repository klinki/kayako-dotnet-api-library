using System.Net;
using KayakoRestApi.Core.Constants;
using KayakoRestApi.Core.Test;
using KayakoRestApi.Net;

namespace KayakoRestApi.Controllers
{
    public interface ICoreController
    {
        string GetListTest();

        string GetTest(int id);

        string PostTest();

        string PutTest(int id);

        bool DeleteTest(int id);
    }

    public sealed class CoreController : BaseController, ICoreController
    {
        public CoreController(string apiKey, string secretKey, string apiUrl, IWebProxy proxy)
            : base(apiKey, secretKey, apiUrl, proxy) { }

        public CoreController(string apiKey, string secretKey, string apiUrl, IWebProxy proxy, ApiRequestType requestType)
            : base(apiKey, secretKey, apiUrl, proxy, requestType) { }

        public CoreController(IKayakoApiRequest kayakoApiRequest)
            : base(kayakoApiRequest) { }

        #region Api Methods

        /// <summary>
        ///     Test a GET list request
        /// </summary>
        public string GetListTest() => this.Connector.ExecuteGet<TestData>(ApiBaseMethods.CoreTest);

        /// <summary>
        ///     Test a GET request
        /// </summary>
        public string GetTest(int id)
        {
            var apiMethod = string.Format("{0}/{1}", ApiBaseMethods.CoreTest, id);

            return this.Connector.ExecuteGet<TestData>(apiMethod);
        }

        /// <summary>
        ///     Test a POST request
        /// </summary>
        public string PostTest() => this.Connector.ExecutePost<TestData>(ApiBaseMethods.CoreTest, string.Empty);

        /// <summary>
        ///     Test a PUT request
        /// </summary>
        public string PutTest(int id)
        {
            var apiMethod = string.Format("{0}/{1}", ApiBaseMethods.CoreTest, id);

            return this.Connector.ExecutePut<TestData>(apiMethod, string.Empty);
        }

        /// <summary>
        ///     Test a DELETE request
        /// </summary>
        public bool DeleteTest(int id)
        {
            var apiMethod = string.Format("{0}/{1}", ApiBaseMethods.CoreTest, id);

            return this.Connector.ExecuteDelete(apiMethod);
        }

        #endregion
    }
}