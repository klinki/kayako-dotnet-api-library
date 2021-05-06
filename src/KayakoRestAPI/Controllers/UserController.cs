using System;
using System.Net;
using KayakoRestApi.Core.Users;
using KayakoRestApi.Net;
using KayakoRestApi.RequestBase;
using KayakoRestApi.Text;
using KayakoRestApi.Utilities;

namespace KayakoRestApi.Controllers
{
    public interface IUserController
    {
        #region User Search Methods

        UserCollection UserSearch(string query);

        #endregion

        #region User Methods

        UserCollection GetUsers();

        UserCollection GetUsers(int filter);

        UserCollection GetUsers(int filter, int max);

        User GetUser(int userId);

        User CreateUser(UserRequest user, string password, bool? sendWelcomeEmail = null);

        User UpdateUser(UserRequest user);

        bool DeleteUser(int userId);

        #endregion

        #region User Group Methods

        UserGroupCollection GetUserGroups();

        UserGroup GetUserGroup(int userGroupId);

        UserGroup CreateUserGroup(UserGroupRequest userGroup);

        UserGroup UpdateUserGroup(UserGroupRequest userGroup);

        bool DeleteUserGroup(int userGroupId);

        #endregion

        #region User Organization Methods

        UserOrganizationCollection GetUserOrganizations();

        UserOrganization GetUserOrganization(int id);

        UserOrganization CreateUserOrganization(UserOrganizationRequest org);

        UserOrganization UpdateUserOrganization(UserOrganizationRequest org);

        bool DeleteUserOrganization(int id);

        #endregion
    }

    /// <summary>
    ///     Provides access to User API Methods
    /// </summary>
    public sealed class UserController : BaseController, IUserController
    {
        internal UserController(string apiKey, string secretKey, string apiUrl, IWebProxy proxy)
            : base(apiKey, secretKey, apiUrl, proxy) { }

        internal UserController(string apiKey, string secretKey, string apiUrl, IWebProxy proxy, ApiRequestType requestType)
            : base(apiKey, secretKey, apiUrl, proxy, requestType) { }

        #region Api Methods

        #region User Methods

        /// <summary>
        ///     Retrieve a list of all users in the help desk starting from a marker (user id) till the item
        ///     fetch limit is reached (by default this is 1000).
        /// </summary>
        public UserCollection GetUsers() => this.GetUsers(0, 1000);

        /// <summary>
        ///     Retrieve a list of all users in the help desk starting from a marker (user id) till the item
        ///     fetch limit is reached (by default this is 1000).
        /// </summary>
        public UserCollection GetUsers(int filter) => this.GetUsers(filter, 1000);

        /// <summary>
        ///     Retrieve a list of all users in the help desk starting from a marker (user id) till the item
        ///     fetch limit is reached (by default this is 1000).
        /// </summary>
        public UserCollection GetUsers(int filter, int max)
        {
            var apiMethod = string.Format("/Base/User/Filter/{0}/{1}", filter, max);

            return this.Connector.ExecuteGet<UserCollection>(apiMethod);
        }

        /// <summary>
        ///     Retrieve the user identified by their unique indentifier
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User GetUser(int userId)
        {
            var apiMethod = string.Format("/Base/User/{0}", userId);

            var users = this.Connector.ExecuteGet<UserCollection>(apiMethod);

            if (users != null && users.Count > 0)
            {
                return users[0];
            }

            return null;
        }

        /// <summary>
        ///     Create a new User record
        /// </summary>
        public User CreateUser(UserRequest user, string password, bool? sendWelcomeEmail = null)
        {
            const string apiMethod = "/Base/User/";

            var parameters = PopulateRequestParameters(user, RequestTypes.Create);

            if (!string.IsNullOrEmpty(password))
            {
                parameters.AppendRequestData("password", password);
            }

            if (sendWelcomeEmail.HasValue)
            {
                parameters.AppendRequestData("sendwelcomeemail", Convert.ToInt32(sendWelcomeEmail.Value));
            }

            var users = this.Connector.ExecutePost<UserCollection>(apiMethod, parameters.ToString());

            if (users != null && users.Count > 0)
            {
                return users[0];
            }

            return null;
        }

        /// <summary>
        ///     Update the user identified by their unique identifier.
        /// </summary>
        public User UpdateUser(UserRequest user)
        {
            var apiMethod = string.Format("/Base/User/{0}", user.Id);

            var parameters = PopulateRequestParameters(user, RequestTypes.Update);

            var users = this.Connector.ExecutePut<UserCollection>(apiMethod, parameters.ToString());

            return users.Count > 0 ? users[0] : null;
        }

        /// <summary>
        ///     Delete the user identified by its unique identifier
        /// </summary>
        public bool DeleteUser(int userId)
        {
            var url = string.Format("/Base/User/{0}", userId);

            return this.Connector.ExecuteDelete(url);
        }

        #endregion

        #region User Group Methods

        /// <summary>
        ///     Retrieve a list of all user groups in the help desk.
        /// </summary>
        public UserGroupCollection GetUserGroups()
        {
            const string apiMethod = "/Base/UserGroup/";

            return this.Connector.ExecuteGet<UserGroupCollection>(apiMethod);
        }

        /// <summary>
        ///     Retrieve the user group identified by user group identifier.
        /// </summary>
        public UserGroup GetUserGroup(int userGroupId)
        {
            var apiMethod = string.Format("/Base/UserGroup/{0}", userGroupId);

            var userGroups = this.Connector.ExecuteGet<UserGroupCollection>(apiMethod);

            if (userGroups != null && userGroups.Count > 0)
            {
                return userGroups[0];
            }

            return null;
        }

        /// <summary>
        ///     Retrieve the user group identified by its unique identifier
        /// </summary>
        public UserGroup CreateUserGroup(UserGroupRequest userGroup)
        {
            const string apiMethod = "/Base/UserGroup/";

            var parameters = PopulateRequestParameters(userGroup, RequestTypes.Create);

            var userGroups = this.Connector.ExecutePost<UserGroupCollection>(apiMethod, parameters.ToString());

            if (userGroups != null && userGroups.Count > 0)
            {
                return userGroups[0];
            }

            return null;
        }

        /// <summary>
        ///     Update the user group identified by its unique identifier
        /// </summary>
        public UserGroup UpdateUserGroup(UserGroupRequest userGroup)
        {
            var apiMethod = string.Format("/Base/UserGroup/{0}", userGroup.Id);

            var parameters = PopulateRequestParameters(userGroup, RequestTypes.Create);

            var userGroups = this.Connector.ExecutePut<UserGroupCollection>(apiMethod, parameters.ToString());

            return userGroups.Count > 0 ? userGroups[0] : null;
        }

        /// <summary>
        ///     Delete the user group identified by its unique identifier
        /// </summary>
        public bool DeleteUserGroup(int userGroupId)
        {
            var apiMethod = string.Format("/Base/UserGroup/{0}", userGroupId);

            return this.Connector.ExecuteDelete(apiMethod);
        }

        #endregion

        #region User Organization Methods

        /// <summary>
        ///     Retrieve a list of all organizations in the help desk
        /// </summary>
        /// <returns>TicketPosts</returns>
        public UserOrganizationCollection GetUserOrganizations()
        {
            const string apiMethod = "/Base/UserOrganization/";

            var orgs = this.Connector.ExecuteGet<UserOrganizationCollection>(apiMethod);

            return orgs;
        }

        /// <summary>
        ///     Retrieve a list of all organizations in the help desk
        /// </summary>
        /// <returns>TicketPosts</returns>
        public UserOrganization GetUserOrganization(int id)
        {
            var apiMethod = string.Format("/Base/UserOrganization/{0}", id);

            var orgs = this.Connector.ExecuteGet<UserOrganizationCollection>(apiMethod);

            if (orgs != null && orgs.Count > 0)
            {
                return orgs[0];
            }

            return null;
        }

        /// <summary>
        ///     Create a new user organization record
        /// </summary>
        /// <remarks>
        ///     See -
        ///     http://wiki.kayako.com/display/DEV/REST+-+UserOrganization#REST-UserOrganization-POST%2FBase%2FUserOrganization
        /// </remarks>
        /// <param name="org">Organisation to create</param>
        /// <returns>Added organisation.</returns>
        public UserOrganization CreateUserOrganization(UserOrganizationRequest org)
        {
            const string apiMethod = "/Base/UserOrganization";

            var parameters = PopulateRequestParameters(org, RequestTypes.Create);

            var orgs = this.Connector.ExecutePost<UserOrganizationCollection>(apiMethod, parameters.ToString());

            return orgs.Count > 0 ? orgs[0] : null;
        }

        /// <summary>
        ///     Update the user organization identified by its unique identifier
        /// </summary>
        /// <param name="org"></param>
        /// <returns></returns>
        public UserOrganization UpdateUserOrganization(UserOrganizationRequest org)
        {
            var apiMethod = string.Format("/Base/UserOrganization/{0}", org.Id);

            var parameters = PopulateRequestParameters(org, RequestTypes.Update);

            var orgs = this.Connector.ExecutePut<UserOrganizationCollection>(apiMethod, parameters.ToString());

            if (orgs != null && orgs.Count > 0)
            {
                return orgs[0];
            }

            return null;
        }

        public bool DeleteUserOrganization(int id)
        {
            var apiMethod = string.Format("/Base/UserOrganization/{0}", id);

            return this.Connector.ExecuteDelete(apiMethod);
        }

        #endregion

        #region User Search Methods

        /// <summary>
        ///     Run a search on Users. You can search users from email, full name, phone, organization name and user group.
        /// </summary>
        public UserCollection UserSearch(string query)
        {
            const string apiMethod = "/Base/UserSearch";

            var parameters = new RequestBodyBuilder();
            parameters.AppendRequestData("query", query);

            var users = this.Connector.ExecutePost<UserCollection>(apiMethod, parameters.ToString());

            return users;
        }

        #endregion

        #endregion

        #region Request Parameter Builders

        private static RequestBodyBuilder PopulateRequestParameters(UserRequest user, RequestTypes requestType)
        {
            user.EnsureValidData(requestType);

            var parameters = new RequestBodyBuilder();

            if (!string.IsNullOrEmpty(user.FullName))
            {
                parameters.AppendRequestData("fullname", user.FullName);
            }

            if (user.GroupId > 0)
            {
                parameters.AppendRequestData("usergroupid", user.GroupId);
            }

            if (user.EmailAddresses != null && user.EmailAddresses.Length > 0)
            {
                parameters.AppendRequestDataArray("email[]", user.EmailAddresses);
            }

            if (user.OrganizationId != null && user.OrganizationId.HasValue && user.OrganizationId.Value > 0)
            {
                parameters.AppendRequestData("userorganizationid", user.OrganizationId.Value);
            }

            parameters.AppendRequestData("salutation", EnumUtility.ToApiString(user.Salutation));

            if (!string.IsNullOrEmpty(user.Designation))
            {
                parameters.AppendRequestData("designation", user.Designation);
            }

            if (!string.IsNullOrEmpty(user.Phone))
            {
                parameters.AppendRequestData("phone", user.Phone);
            }

            parameters.AppendRequestData("isenabled", Convert.ToInt32(user.IsEnabled));

            parameters.AppendRequestData("userrole", EnumUtility.ToApiString(user.Role));

            if (!string.IsNullOrEmpty(user.TimeZone))
            {
                parameters.AppendRequestData("timezone", user.TimeZone);
            }

            parameters.AppendRequestData("enabledst", Convert.ToInt32(user.EnableDst));

            if (user.SlaPlanId != null)
            {
                parameters.AppendRequestData("slaplanid", user.SlaPlanId);
            }

            if (user.SlaPlanExpiry != null)
            {
                parameters.AppendRequestData("slaplanexpiry", user.SlaPlanExpiry);
            }

            if (user.Expiry != null)
            {
                parameters.AppendRequestData("userexpiry", user.Expiry);
            }

            return parameters;
        }

        private static RequestBodyBuilder PopulateRequestParameters(UserGroupRequest userGroup, RequestTypes requestType)
        {
            userGroup.EnsureValidData(requestType);

            var parameters = new RequestBodyBuilder();

            if (!string.IsNullOrEmpty(userGroup.Title))
            {
                parameters.AppendRequestData("title", userGroup.Title);
            }

            parameters.AppendRequestData("grouptype", EnumUtility.ToApiString(userGroup.GroupType));

            //parameters.AppendRequestData("ismaster", Convert.ToInt32(userGroup.Ismaster));

            return parameters;
        }

        private static RequestBodyBuilder PopulateRequestParameters(UserOrganizationRequest org, RequestTypes requestType)
        {
            org.EnsureValidData(requestType);

            var parameters = new RequestBodyBuilder();

            parameters.AppendRequestData("name", org.Name);
            parameters.AppendRequestData("organizationtype", EnumUtility.ToApiString(org.OrganizationType));

            if (!string.IsNullOrEmpty(org.Address))
            {
                parameters.AppendRequestData("address", org.Address);
            }

            if (!string.IsNullOrEmpty(org.City))
            {
                parameters.AppendRequestData("city", org.City);
            }

            if (!string.IsNullOrEmpty(org.State))
            {
                parameters.AppendRequestData("state", org.State);
            }

            if (!string.IsNullOrEmpty(org.PostalCode))
            {
                parameters.AppendRequestData("postalcode", org.PostalCode);
            }

            if (!string.IsNullOrEmpty(org.Country))
            {
                parameters.AppendRequestData("country", org.Country);
            }

            if (!string.IsNullOrEmpty(org.Phone))
            {
                parameters.AppendRequestData("phone", org.Phone);
            }

            if (!string.IsNullOrEmpty(org.Fax))
            {
                parameters.AppendRequestData("fax", org.Fax);
            }

            if (!string.IsNullOrEmpty(org.Website))
            {
                parameters.AppendRequestData("website", org.Website);
            }

            if (org.SlaPlanId != null)
            {
                parameters.AppendRequestData("slaplanid", org.SlaPlanId);
            }

            if (org.SlaPlanExpiry != null)
            {
                parameters.AppendRequestData("slaplanexpiry", org.SlaPlanExpiry);
            }

            return parameters;
        }

        #endregion
    }
}