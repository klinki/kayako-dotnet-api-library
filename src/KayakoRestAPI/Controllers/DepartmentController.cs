using System.Net;
using KayakoRestApi.Core.Constants;
using KayakoRestApi.Core.Departments;
using KayakoRestApi.Net;
using KayakoRestApi.RequestBase;
using KayakoRestApi.Text;
using KayakoRestApi.Utilities;

namespace KayakoRestApi.Controllers
{
    public interface IDepartmentController
    {
        DepartmentCollection GetDepartments();

        Department GetDepartment(int id);

        Department UpdateDepartment(DepartmentRequest dept);

        Department CreateDepartment(DepartmentRequest dept);

        bool DeleteDepartment(int id);
    }

    /// <summary>
    ///     Provides access to Deparment API Methods
    /// </summary>
    public sealed class DepartmentController : BaseController, IDepartmentController
    {
        public DepartmentController(string apiKey, string secretKey, string apiUrl, IWebProxy proxy)
            : base(apiKey, secretKey, apiUrl, proxy) { }

        public DepartmentController(string apiKey, string secretKey, string apiUrl, IWebProxy proxy, ApiRequestType requestType)
            : base(apiKey, secretKey, apiUrl, proxy, requestType) { }

        public DepartmentController(IKayakoApiRequest kayakoApiRequest)
            : base(kayakoApiRequest) { }

        private static RequestBodyBuilder PopulateRequestParameters(DepartmentRequest dept, RequestTypes requestType)
        {
            dept.EnsureValidData(requestType);

            var parameters = new RequestBodyBuilder();

            if (!string.IsNullOrEmpty(dept.Title))
            {
                parameters.AppendRequestData("title", dept.Title);
            }

            parameters.AppendRequestData("type", EnumUtility.ToApiString(dept.Type));

            if (requestType == RequestTypes.Create)
            {
                parameters.AppendRequestData("module", EnumUtility.ToApiString(dept.Module));
            }

            if (dept.DisplayOrder > 0)
            {
                parameters.AppendRequestData("displayorder", dept.DisplayOrder);
            }

            if (dept.ParentDepartmentId > 0)
            {
                parameters.AppendRequestData("parentdepartmentid", dept.ParentDepartmentId);
            }

            parameters.AppendRequestData("uservisibilitycustom", dept.UserVisibilityCustom ? 1 : 0);

            if (dept.UserGroups != null && dept.UserGroups.Count > 0)
            {
                parameters.AppendRequestDataArray("usergroupid[]", dept.UserGroups);
            }

            return parameters;
        }

        #region Api Methods

        /// <summary>
        ///     Retrieve a list of all departments and sub-departments in the help desk.
        /// </summary>
        public DepartmentCollection GetDepartments() => this.Connector.ExecuteGet<DepartmentCollection>(ApiBaseMethods.Departments);

        /// <summary>
        ///     Retrieve the department identified by its internal identifier
        /// </summary>
        /// <param name="id">The unique numeric identifer of the department to retrieve</param>
        /// <returns></returns>
        public Department GetDepartment(int id)
        {
            var apiMethod = string.Format("{0}/{1}", ApiBaseMethods.Departments, id);

            var depts = this.Connector.ExecuteGet<DepartmentCollection>(apiMethod);

            if (depts != null && depts.Count > 0)
            {
                return depts[0];
            }

            return null;
        }

        /// <summary>
        ///     Update the department identified by its internal identifier
        /// </summary>
        /// <param name="dept">Data to update the department. Department Id and Title must be supplied</param>
        /// <returns>Department data representing the updated department</returns>
        public Department UpdateDepartment(DepartmentRequest dept)
        {
            var apiMethod = string.Format("{0}/{1}", ApiBaseMethods.Departments, dept.Id);

            var parameters = PopulateRequestParameters(dept, RequestTypes.Update);

            var depts = this.Connector.ExecutePut<DepartmentCollection>(apiMethod, parameters.ToString());

            if (depts != null && depts.Count > 0)
            {
                return depts[0];
            }

            return null;
        }

        /// <summary>
        ///     Creates a new department
        /// </summary>
        /// <param name="dept">Department data to create a new department with. Department Title, Module and Type must be supplied</param>
        /// <returns>Department data representing the department created</returns>
        public Department CreateDepartment(DepartmentRequest dept)
        {
            var parameters = PopulateRequestParameters(dept, RequestTypes.Create);

            var depts = this.Connector.ExecutePost<DepartmentCollection>(ApiBaseMethods.Departments, parameters.ToString());

            if (depts != null && depts.Count > 0)
            {
                return depts[0];
            }

            return null;
        }

        /// <summary>
        ///     Delete the Department identified by its internal identifier
        /// </summary>
        /// <param name="id">The unique numeric identifer of the department to delete</param>
        /// <returns>The success of the deletion</returns>
        public bool DeleteDepartment(int id)
        {
            var apiMethod = string.Format("{0}/{1}", ApiBaseMethods.Departments, id);

            return this.Connector.ExecuteDelete(apiMethod);
        }

        #endregion
    }
}