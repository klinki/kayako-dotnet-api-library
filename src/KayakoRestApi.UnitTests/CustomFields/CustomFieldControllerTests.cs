using KayakoRestApi.Controllers;
using KayakoRestApi.Core.CustomFields;
using KayakoRestApi.Net;
using KayakoRestApi.UnitTests.Utilities;
using Moq;
using NUnit.Framework;

namespace KayakoRestApi.UnitTests.CustomFields
{
    [TestFixture]
    public class CustomFieldControllerTests
    {
        [SetUp]
        public void Setup()
        {
            this.kayakoApiRequest = new Mock<IKayakoApiRequest>();
            this.customFieldController = new CustomFieldController(this.kayakoApiRequest.Object);

            this.responseCustomFieldCollection = new CustomFieldCollection
            {
                new CustomField(),
                new CustomField()
            };

            this.responseCustomFieldOptionsCollection = new CustomFieldOptionCollection
            {
                new CustomFieldOption(),
                new CustomFieldOption()
            };
        }

        private ICustomFieldController customFieldController;
        private Mock<IKayakoApiRequest> kayakoApiRequest;
        private CustomFieldCollection responseCustomFieldCollection;
        private CustomFieldOptionCollection responseCustomFieldOptionsCollection;

        [Test]
        public void GetCustomFields()
        {
            const string apiMethod = "/Base/CustomField";

            this.kayakoApiRequest.Setup(x => x.ExecuteGet<CustomFieldCollection>(apiMethod)).Returns(this.responseCustomFieldCollection);

            var customFields = this.customFieldController.GetCustomFields();

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<CustomFieldCollection>(apiMethod), Times.Once());
            AssertUtility.ObjectsEqual(customFields, this.responseCustomFieldCollection);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetCustomFieldListOptions(int customFieldId)
        {
            var apiMethod = string.Format("/Base/CustomField/ListOptions/{0}", customFieldId);

            this.kayakoApiRequest.Setup(x => x.ExecuteGet<CustomFieldOptionCollection>(apiMethod)).Returns(this.responseCustomFieldOptionsCollection);

            var customFieldOptions = this.customFieldController.GetCustomFieldOptions(customFieldId);

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<CustomFieldOptionCollection>(apiMethod), Times.Once());
            AssertUtility.ObjectsEqual(customFieldOptions, this.responseCustomFieldOptionsCollection);
        }
    }
}