using System.Collections.Generic;
using KayakoRestApi.Core.Constants;
using KayakoRestApi.RequestBase;
using KayakoRestApi.RequestBase.Attributes;

namespace KayakoRestApi.Core.Departments
{
    public class DepartmentRequest : RequestBaseObject
    {
        [RequiredField(RequestTypes.Update)]
        [ResponseProperty("Id")]
        public int Id { get; set; }

        [RequiredField]
        [ResponseProperty("Title")]
        public string Title { get; set; }

        [RequiredField(RequestTypes.Update)]
        [ResponseProperty("Module")]
        public DepartmentModule Module { get; set; }

        [RequiredField(RequestTypes.Update)]
        [ResponseProperty("Type")]
        public DepartmentType Type { get; set; }

        [OptionalField]
        [ResponseProperty("DisplayOrder")]
        public int DisplayOrder { get; set; }

        [OptionalField]
        [ResponseProperty("ParentDepartmentId")]
        public int ParentDepartmentId { get; set; }

        [OptionalField]
        [ResponseProperty("UserVisibilityCustom")]
        public bool UserVisibilityCustom { get; set; }

        [OptionalField]
        [ResponseProperty("UserGroups")]
        public List<int> UserGroups { get; set; }

        public static DepartmentRequest FromResponseData(Department responseData) => FromResponseType<Department, DepartmentRequest>(responseData);

        public static Department ToResponseData(DepartmentRequest requestData) => ToResponseType<DepartmentRequest, Department>(requestData);
    }
}