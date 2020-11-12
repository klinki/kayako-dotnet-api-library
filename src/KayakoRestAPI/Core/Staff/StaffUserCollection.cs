using System.Collections.Generic;
using System.Xml.Serialization;

namespace KayakoRestApi.Core.Staff
{
    /// <summary>
    ///     Definition of a list of staff.
    ///     <remarks>
    ///         See : http://wiki.kayako.com/display/DEV/REST+-+Staff
    ///     </remarks>
    /// </summary>
    [XmlRoot("staffusers")]
    public class StaffUserCollection : List<StaffUser> { }
}