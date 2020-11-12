using System.Net;
using KayakoRestApi.Core.Constants;
using KayakoRestApi.Core.CustomFields;
using KayakoRestApi.Net;

namespace KayakoRestApi.Controllers
{
    public interface ICustomFieldController
    {
        CustomFieldCollection GetCustomFields();

        CustomFieldOptionCollection GetCustomFieldOptions(int customFieldId);
    }

    public sealed class CustomFieldController : BaseController, ICustomFieldController
    {
        public CustomFieldController(string apiKey, string secretKey, string apiUrl, IWebProxy proxy)
            : base(apiKey, secretKey, apiUrl, proxy) { }

        public CustomFieldController(string apiKey, string secretKey, string apiUrl, IWebProxy proxy, ApiRequestType requestType)
            : base(apiKey, secretKey, apiUrl, proxy, requestType) { }

        public CustomFieldController(IKayakoApiRequest kayakoRestApi)
            : base(kayakoRestApi) { }

        #region Api Methods

        /// <summary>
        ///     Retrieve a list of a custom fields.
        /// </summary>
        public CustomFieldCollection GetCustomFields() => this.Connector.ExecuteGet<CustomFieldCollection>(ApiBaseMethods.CustomFields);

        /// <summary>
        ///     Retrieve the list of custom field options
        /// </summary>
        public CustomFieldOptionCollection GetCustomFieldOptions(int customFieldId)
        {
            var apiMethod = string.Format("{0}/ListOptions/{1}", ApiBaseMethods.CustomFields, customFieldId);

            return this.Connector.ExecuteGet<CustomFieldOptionCollection>(apiMethod);
        }

        #endregion
    }
}