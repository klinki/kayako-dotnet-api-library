using System.Collections.Generic;
using System.Xml.Serialization;

namespace KayakoRestApi.Core.Users
{
    /// <summary>
    ///     Definition of a list of end users.
    ///     <remarks>
    ///         See : http://wiki.kayako.com/display/DEV/REST+-+User
    ///     </remarks>
    /// </summary>
    [XmlRoot("users")]
    public class UserCollection : List<User> { }
}