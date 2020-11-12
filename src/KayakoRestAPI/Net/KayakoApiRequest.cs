using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using KayakoRestApi.Core.Test;

namespace KayakoRestApi.Net
{
    public interface IKayakoApiRequest
    {
        TTarget ExecutePut<TTarget>(string apiMethod, string parameters)
            where TTarget : class, new();

        TTarget ExecutePost<TTarget>(string apiMethod, string parameters)
            where TTarget : class, new();

        TTarget ExecuteGet<TTarget>(string apiMethod)
            where TTarget : class, new();

        bool ExecuteDelete(string apiMethod);
    }

    [Serializable]
    internal class KayakoApiRequest : IKayakoApiRequest
    {
        private readonly string apiKey;
        private readonly string apiUrl;
        private readonly IWebProxy proxy;
        private readonly ApiRequestType requestType;
        private readonly string secretKey;
        private string encodedSignature;
        private string salt;
        private string signature;

        internal KayakoApiRequest(string apiKey, string secretKey, string apiUrl, IWebProxy proxy, ApiRequestType requestType)
        {
            this.apiKey = apiKey;
            this.secretKey = secretKey;
            this.apiUrl = apiUrl;
            this.proxy = proxy;
            this.requestType = requestType;

            this.ComputeSaltAndSignature();
        }

        private void ComputeSaltAndSignature()
        {
            // Generate a new globally unique identifier for the salt
            var salt = Guid.NewGuid().ToString();
            this.salt = salt;

            // Initialize the keyed hash object using the secret key as the key
            var hashObject = new HMACSHA256(Encoding.UTF8.GetBytes(this.secretKey));

            // Computes the signature by hashing the salt with the secret key as the key
            var signature = hashObject.ComputeHash(Encoding.UTF8.GetBytes(salt));

            // Base 64 Encode
            this.signature = Convert.ToBase64String(signature);

            // URLEncode
            this.encodedSignature = HttpUtility.UrlEncode(this.signature);
        }

        #region Api Connection Methods

        #region Execute Put/Post/Delete/Get

        /// <summary>
        ///     Generic method for extracting data via PUSH.
        /// </summary>
        /// <typeparam name="TTarget">Target type to extract</typeparam>
        /// <param name="apiMethod">URL to request data.</param>
        /// <param name="parameters">Parameters to post.</param>
        /// <returns>TTarget result of the extraction</returns>
        public TTarget ExecutePut<TTarget>(string apiMethod, string parameters)
            where TTarget : class, new() => this.ExecuteCall<TTarget>(apiMethod, parameters, HttpMethod.Put);

        /// <summary>
        ///     Generic method for extracting data via POST.
        /// </summary>
        /// <typeparam name="TTarget">Target type to extract</typeparam>
        /// <param name="apiMethod">URL to request data.</param>
        /// <param name="parameters">Parameters to post.</param>
        /// <returns>TTarget result of the extraction</returns>
        public TTarget ExecutePost<TTarget>(string apiMethod, string parameters)
            where TTarget : class, new() => this.ExecuteCall<TTarget>(apiMethod, parameters, HttpMethod.Post);

        /// <summary>
        ///     Generic method for extracting data.
        /// </summary>
        /// <typeparam name="TTarget">Target type to extract</typeparam>
        /// <param name="apiMethod">URL to request data.</param>
        /// <returns>TTarget result of the extraction</returns>
        public TTarget ExecuteGet<TTarget>(string apiMethod)
            where TTarget : class, new() => this.ExecuteCall<TTarget>(apiMethod, string.Empty, HttpMethod.Get);

        /// <summary>
        ///     Generic method for extracting data via DELETE.
        /// </summary>
        /// <param name="apiMethod">URL to request data</param>
        /// <returns>The success of the delete</returns>
        public bool ExecuteDelete(string apiMethod)
        {
            var requestUrl = this.GetRequestUrl(apiMethod);
            requestUrl = this.AppendSecurityCredentials(requestUrl, HttpMethod.Delete);

            var request = (HttpWebRequest) WebRequest.Create(requestUrl);
            request.Method = "DELETE";

            if (this.proxy != null)
            {
                request.Proxy = this.proxy;
            }

            try
            {
                using var response = request.GetResponse() as HttpWebResponse;
                if (response?.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
            }
            catch (WebException ex)
            {
                var sr = new StreamReader(ex.Response.GetResponseStream() ?? Stream.Null);
                var s = sr.ReadToEnd();

                throw new InvalidOperationException(s, ex);
            }

            return false;
        }

        #endregion

        private string GetRequestUrl(string apiMethod) => string.Format(this.requestType == ApiRequestType.QueryString ? "{0}?e={1}" : "{0}{1}", this.apiUrl, apiMethod);

        private string AppendSecurityCredentials(string inputString, HttpMethod httpMethod)
        {
            var signature = this.signature;

            if (httpMethod == HttpMethod.Get)
            {
                signature = this.encodedSignature;
            }

            return string.Format("{0}&apikey={1}&salt={2}&signature={3}", inputString, this.apiKey, this.salt, signature);
        }

        #region Execute Requests to Api

        private TTarget ExecuteCall<TTarget>(string apiMethod, string parameters, HttpMethod httpMethod)
            where TTarget : class, new()
        {
            var requestUrl = this.GetRequestUrl(apiMethod);

            if (httpMethod == HttpMethod.Get)
            {
                requestUrl = this.AppendSecurityCredentials(requestUrl, httpMethod);
            }

            var request = WebRequest.Create(requestUrl);
            request.Method = httpMethod.ToString();

            if (this.proxy != null)
            {
                request.Proxy = this.proxy;
            }

            if (httpMethod != HttpMethod.Get)
            {
                request.ContentType = "application/x-www-form-urlencoded";

                parameters = this.AppendSecurityCredentials(parameters, httpMethod);

                var bytes = Encoding.UTF8.GetBytes(parameters);

                request.ContentLength = bytes.Length;

                using var os = request.GetRequestStream();
                os.Write(bytes, 0, bytes.Length);
            }

            return this.ProcessWebRequest<TTarget>(request);
        }

        private TTarget ProcessWebRequest<TTarget>(WebRequest request)
            where TTarget : class, new()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(TTarget));
                using var webResponse = request.GetResponse() as HttpWebResponse;
                using var sr = new StreamReader(webResponse?.GetResponseStream() ?? Stream.Null);
                var streamContents = sr.ReadToEnd();

                if (typeof(TTarget) == typeof(TestData))
                {
                    return (TTarget) (object) new TestData(streamContents);
                }

                using var serializerStream = new StringReader(streamContents);
                var responseData = (TTarget) serializer.Deserialize(serializerStream);
                return responseData;
            }
            catch (WebException ex)
            {
                var response = ex.Response as HttpWebResponse;

                using var reader = new StreamReader(response?.GetResponseStream() ?? Stream.Null);
                var streamContents = reader.ReadToEnd();

                throw new InvalidOperationException(streamContents, ex.InnerException);
            }
        }

        #endregion

        #endregion
    }
}