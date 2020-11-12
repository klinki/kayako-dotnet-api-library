using KayakoRestApi.Core.Constants;
using KayakoRestApi.Core.CustomFields;
using KayakoRestApi.UnitTests.Utilities;
using NUnit.Framework;

namespace KayakoRestApi.UnitTests.CustomFields
{
    [TestFixture]
    public class CustomFieldCollectionSerializationTests
    {
        [Test]
        public void CustomFieldCollectionDeserialization()
        {
            var customFieldCollection = new CustomFieldCollection
            {
                new CustomField
                {
                    CustomFieldId = 1,
                    CustomFieldGroupId = 1,
                    Title = "Customer ID",
                    FieldType = CustomFieldFieldType.Text,
                    FieldName = "cn4ezfbjzyoj",
                    DefaultValue = string.Empty,
                    IsRequired = false,
                    UserEditable = false,
                    StaffEditable = true,
                    RegexValidate = string.Empty,
                    DisplayOrder = 1,
                    EncryptInDatabase = false,
                    Description = string.Empty
                },
                new CustomField
                {
                    CustomFieldId = 2,
                    CustomFieldGroupId = 1,
                    Title = "Product",
                    FieldType = CustomFieldFieldType.Select,
                    FieldName = "llcut44uri9d",
                    DefaultValue = string.Empty,
                    IsRequired = false,
                    UserEditable = false,
                    StaffEditable = true,
                    RegexValidate = string.Empty,
                    DisplayOrder = 2,
                    EncryptInDatabase = false,
                    Description = string.Empty
                }
            };

            var expectedCustomFieldCollection = XmlDataUtility.ReadFromFile<CustomFieldCollection>("TestData/CustomFieldCollection.xml");

            AssertUtility.ObjectsEqual(expectedCustomFieldCollection, customFieldCollection);
        }
    }
}