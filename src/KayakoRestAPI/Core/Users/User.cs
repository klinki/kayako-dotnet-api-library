using System.Xml.Serialization;
using KayakoRestApi.Core.Constants;
using KayakoRestApi.Data;

namespace KayakoRestApi.Core.Users
{
    /// <summary>
    ///     Represents an end user.
    ///     <remarks>
    ///         See : http://wiki.kayako.com/display/DEV/REST+-+User
    ///     </remarks>
    /// </summary>
    [XmlType("user")]
    public class User
    {
        /// <summary>
        ///     Gets a value indicating the Id of the user
        /// </summary>
        [XmlElement("id")]
        public int Id { get; set; }

        /// <summary>
        ///     The User Group Id to assign to this User
        /// </summary>
        [XmlElement("usergroupid")]
        public int GroupId { get; set; }

        /// <summary>
        ///     The User Role, available options are: user, manager. Default: user
        /// </summary>
        [XmlElement("userrole")]
        public UserRole Role { get; set; }

        /// <summary>
        ///     The User Organization Id
        /// </summary>
        [XmlElement("userorganizationid")]
        public KNullable<int> OrganizationId { get; set; }

        /// <summary>
        ///     The User Salutation, available options are: Mr., Ms., Mrs., Dr.
        /// </summary>
        [XmlElement("salutation")]
        public UserSalutation Salutation { get; set; }

        /// <summary>
        ///     The User Expiry, 0 = never expires
        /// </summary>
        [XmlElement("userexpiry")]
        public long Expiry { get; set; }

        /// <summary>
        ///     The full name of User
        /// </summary>
        [XmlElement("fullname")]
        public string FullName { get; set; }

        /// <summary>
        ///     The email addresses for the User
        /// </summary>
        [XmlElement("email")]
        public string[] EmailAddresses { get; set; }

        /// <summary>
        ///     The User Designation/Title
        /// </summary>
        [XmlElement("designation")]
        public string Designation { get; set; }

        /// <summary>
        ///     The phone number for the user
        /// </summary>
        [XmlElement("phone")]
        public string Phone { get; set; }

        /// <summary>
        ///     Gets a value indicating the date
        /// </summary>
        [XmlElement("dateline")]
        public KNullable<long> Dateline { get; set; }

        /// <summary>
        ///     Gets a value indicating the last visit of the user
        /// </summary>
        [XmlElement("lastvisit")]
        public long LastVisit { get; set; }

        /// <summary>
        ///     Indicates whether the user is enabled/disabled
        /// </summary>
        [XmlElement("isenabled")]
        public bool IsEnabled { get; set; }

        /// <summary>
        ///     The Time Zone the user resides in
        /// </summary>
        [XmlElement("timezone")]
        public string TimeZone { get; set; }

        /// <summary>
        ///     Indciates whether daylight savings is enabled/disabled
        /// </summary>
        [XmlElement("enabledst")]
        public bool EnableDst { get; set; }

        /// <summary>
        ///     The SLA Plan Id to assign to the user
        /// </summary>
        [XmlElement("slaplanid")]
        public KNullable<int> SlaPlanId { get; set; }

        /// <summary>
        ///     The SLA Plan Expiry, 0 = never expires
        /// </summary>
        [XmlElement("slaplanexpiry")]
        public KNullable<long> SlaPlanExpiry { get; set; }
    }
}