using System;
using System.Net;
using KayakoRestApi.Core.Staff;
using KayakoRestApi.Net;
using KayakoRestApi.RequestBase;
using KayakoRestApi.Text;

namespace KayakoRestApi.Controllers
{
    public interface IStaffController
    {
        StaffUserCollection GetStaffUsers();

        StaffUser GetStaffUser(int staffId);

        StaffUser UpdateStaffUser(StaffUserRequest staffUser);

        StaffUser CreateStaffUser(StaffUserRequest staffUser);

        bool DeleteStaffUser(int staffId);

        StaffGroupCollection GetStaffGroups();

        StaffGroup GetStaffGroup(int groupId);

        StaffGroup CreateStaffGroup(StaffGroupRequest staffGroup);

        StaffGroup UpdateStaffGroup(StaffGroupRequest staffGroup);

        bool DeleteStaffGroup(int staffGroupId);
    }

    /// <summary>
    ///     Provides access to Staff API Methods
    /// </summary>
    public sealed class StaffController : BaseController, IStaffController
    {
        internal StaffController(string apiKey, string secretKey, string apiUrl, IWebProxy proxy)
            : base(apiKey, secretKey, apiUrl, proxy) { }

        internal StaffController(string apiKey, string secretKey, string apiUrl, IWebProxy proxy, ApiRequestType requestType)
            : base(apiKey, secretKey, apiUrl, proxy, requestType) { }

        #region Api Methods

        #region Staff Methos

        /// <summary>
        ///     Retrieve a list of all staff users in the help desk.
        /// </summary>
        /// <returns>The staff</returns>
        public StaffUserCollection GetStaffUsers()
        {
            const string apiMethod = "/Base/Staff/";

            return this.Connector.ExecuteGet<StaffUserCollection>(apiMethod);
        }

        /// <summary>
        ///     Retrieve a staff identified by <paramref name="staffId" />
        /// </summary>
        /// <param name="staffId">The numeric identifier of the staff.</param>
        /// <returns>The staff user.</returns>
        public StaffUser GetStaffUser(int staffId)
        {
            var apiMethod = string.Format("/Base/Staff/{0}", staffId);

            var users = this.Connector.ExecuteGet<StaffUserCollection>(apiMethod);

            if (users != null && users.Count > 0)
            {
                return users[0];
            }

            return null;
        }

        /// <summary>
        ///     Update the staff identified by <paramref name="staffUser" />
        /// </summary>
        /// <remarks>
        ///     http://wiki.kayako.com/display/DEV/REST+-+Staff#REST-Staff-PUT%2FBase%2FStaff%2F%24id%24
        /// </remarks>
        /// <param name="staffUser">User to updated</param>
        /// <returns>Updated user.</returns>
        public StaffUser UpdateStaffUser(StaffUserRequest staffUser)
        {
            var apiMethod = string.Format("/Base/Staff/{0}", staffUser.Id);

            var parameters = PopulateRequestParameters(staffUser, RequestTypes.Update);

            var users = this.Connector.ExecutePut<StaffUserCollection>(apiMethod, parameters.ToString());

            return users.Count > 0 ? users[0] : null;
        }

        /// <summary>
        ///     Create a new Staff record
        /// </summary>
        /// <param name="staffUser">Data representing the new staff</param>
        /// <returns></returns>
        public StaffUser CreateStaffUser(StaffUserRequest staffUser)
        {
            const string apiMethod = "/Base/Staff/";

            var parameters = PopulateRequestParameters(staffUser, RequestTypes.Create);

            var staff = this.Connector.ExecutePost<StaffUserCollection>(apiMethod, parameters.ToString());

            if (staff != null && staff.Count > 0)
            {
                return staff[0];
            }

            return null;
        }

        /// <summary>
        ///     Delete the staff identified by <paramref name="staffId" />.
        /// </summary>
        /// <param name="staffId">The numeric identifier of the staff.</param>
        /// <returns>True if staff removed, false otherwise.</returns>
        public bool DeleteStaffUser(int staffId)
        {
            var apiMethod = string.Format("/Base/Staff/{0}", staffId);

            return this.Connector.ExecuteDelete(apiMethod);
        }

        #endregion

        #region Staff Group Methods

        /// <summary>
        ///     Retrieve a list of all staff user groups in the help desk.
        /// </summary>
        /// <returns>List of staff groups</returns>
        public StaffGroupCollection GetStaffGroups()
        {
            const string apiMethod = "/Base/StaffGroup/";

            return this.Connector.ExecuteGet<StaffGroupCollection>(apiMethod);
        }

        /// <summary>
        ///     Retrieve a staff group identified by <paramref name="groupId" />.
        /// </summary>
        /// <param name="groupId">The numeric identifier of the staff group.</param>
        /// <returns>The staff group</returns>
        public StaffGroup GetStaffGroup(int groupId)
        {
            var apiMethod = string.Format("/Base/StaffGroup/{0}/", groupId);

            var grps = this.Connector.ExecuteGet<StaffGroupCollection>(apiMethod);

            if (grps != null && grps.Count > 0)
            {
                return grps[0];
            }

            return null;
        }

        /// <summary>
        ///     Create a staff group
        /// </summary>
        /// <param name="staffGroup">Data representing the staff group to create</param>
        /// <returns>Data representing the staff group created</returns>
        public StaffGroup CreateStaffGroup(StaffGroupRequest staffGroup)
        {
            const string apiMethod = "/Base/StaffGroup";

            var parameters = PopulateRequestParameters(staffGroup, RequestTypes.Create);

            var staffGroups = this.Connector.ExecutePost<StaffGroupCollection>(apiMethod, parameters.ToString());

            if (staffGroups != null && staffGroups.Count > 0)
            {
                return staffGroups[0];
            }

            return null;
        }

        /// <summary>
        ///     Update the staff group identified by its internal identifer
        /// </summary>
        /// <param name="staffGroup">Data representing the staff group to update. Staff Group Id and Title must be supplied</param>
        /// <returns></returns>
        public StaffGroup UpdateStaffGroup(StaffGroupRequest staffGroup)
        {
            var apiMethod = string.Format("/Base/StaffGroup/{0}", staffGroup.Id);

            var parameters = PopulateRequestParameters(staffGroup, RequestTypes.Update);

            var staffGroups = this.Connector.ExecutePut<StaffGroupCollection>(apiMethod, parameters.ToString());

            if (staffGroups != null && staffGroups.Count > 0)
            {
                return staffGroups[0];
            }

            return null;
        }

        /// <summary>
        ///     Delete the staff group identified by its internal identifier
        /// </summary>
        /// <param name="staffGroupId">The Id of the Staff Group to delete</param>
        /// <returns>The success of the deletion</returns>
        public bool DeleteStaffGroup(int staffGroupId)
        {
            var apiMethod = string.Format("/Base/StaffGroup/{0}", staffGroupId);

            return this.Connector.ExecuteDelete(apiMethod);
        }

        #endregion

        #endregion

        #region Request Parameter Builders

        private static RequestBodyBuilder PopulateRequestParameters(StaffUserRequest staffUser, RequestTypes requestType)
        {
            staffUser.EnsureValidData(requestType);

            var parameters = new RequestBodyBuilder();

            if (!string.IsNullOrEmpty(staffUser.FirstName))
            {
                parameters.AppendRequestData("firstname", staffUser.FirstName);
            }

            if (!string.IsNullOrEmpty(staffUser.LastName))
            {
                parameters.AppendRequestData("lastname", staffUser.LastName);
            }

            if (!string.IsNullOrEmpty(staffUser.UserName))
            {
                parameters.AppendRequestData("username", staffUser.UserName);
            }

            if (!string.IsNullOrEmpty(staffUser.Password))
            {
                parameters.AppendRequestData("password", staffUser.Password);
            }

            if (staffUser.GroupId > 0)
            {
                parameters.AppendRequestData("staffgroupid", staffUser.GroupId);
            }

            if (!string.IsNullOrEmpty(staffUser.Email))
            {
                parameters.AppendRequestData("email", staffUser.Email);
            }

            if (!string.IsNullOrEmpty(staffUser.Designation))
            {
                parameters.AppendRequestData("designation", staffUser.Designation);
            }

            if (!string.IsNullOrEmpty(staffUser.MobileNumber))
            {
                parameters.AppendRequestData("mobilenumber", staffUser.MobileNumber);
            }

            parameters.AppendRequestData("isenabled", Convert.ToInt32(staffUser.IsEnabled));

            if (!string.IsNullOrEmpty(staffUser.Greeting))
            {
                parameters.AppendRequestData("greeting", staffUser.Greeting);
            }

            if (!string.IsNullOrEmpty(staffUser.Signature))
            {
                parameters.AppendRequestData("staffsignature", staffUser.Signature);
            }

            if (!string.IsNullOrEmpty(staffUser.TimeZone))
            {
                parameters.AppendRequestData("timezone", staffUser.TimeZone);
            }

            parameters.AppendRequestData("enabledst", Convert.ToInt32(staffUser.EnableDst));

            return parameters;
        }

        private static RequestBodyBuilder PopulateRequestParameters(StaffGroupRequest staffGroup, RequestTypes requestType)
        {
            staffGroup.EnsureValidData(requestType);

            var parameters = new RequestBodyBuilder();

            if (!string.IsNullOrEmpty(staffGroup.Title))
            {
                parameters.AppendRequestData("title", staffGroup.Title);
            }

            parameters.AppendRequestData("isadmin", Convert.ToInt32(staffGroup.IsAdmin));

            return parameters;
        }

        #endregion
    }
}