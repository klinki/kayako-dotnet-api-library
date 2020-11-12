using System.Collections.Generic;
using System.Xml.Serialization;

namespace KayakoRestApi.Core.Departments
{
    /// <summary>
    ///     Definition of a list of departments
    ///     <remarks>
    ///         See : http://wiki.kayako.com/display/DEV/REST+-+Department
    ///     </remarks>
    /// </summary>
    [XmlRoot("departments")]
    public class DepartmentCollection : List<Department> { }
}