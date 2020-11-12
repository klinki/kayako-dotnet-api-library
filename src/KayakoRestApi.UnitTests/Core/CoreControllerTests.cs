using KayakoRestApi.Controllers;
using KayakoRestApi.Core.Constants;
using KayakoRestApi.Core.Test;
using KayakoRestApi.Net;
using Moq;
using NUnit.Framework;

namespace KayakoRestApi.UnitTests.Core
{
    [TestFixture(Description = "A set of tests testing Api methods around Cusom Fields")]
    public class CoreControllerTests
    {
        [SetUp]
        public void SetUp()
        {
            this.kayakoApiRequest = new Mock<IKayakoApiRequest>();
            this.coreController = new CoreController(this.kayakoApiRequest.Object);

            this.getTestData = new TestData("Test");
        }

        private ICoreController coreController;
        private Mock<IKayakoApiRequest> kayakoApiRequest;

        private TestData getTestData;

        [Test]
        public void GetList()
        {
            this.kayakoApiRequest.Setup(x => x.ExecuteGet<TestData>(ApiBaseMethods.CoreTest)).Returns(this.getTestData);
            var getListResult = this.coreController.GetListTest();

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<TestData>(ApiBaseMethods.CoreTest), Times.Once());
            Assert.That(getListResult, Is.EqualTo(this.getTestData.Data));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Get(int id)
        {
            var apiMethod = string.Format("{0}/{1}", ApiBaseMethods.CoreTest, id);
            this.kayakoApiRequest.Setup(x => x.ExecuteGet<TestData>(apiMethod)).Returns(this.getTestData);

            var getResult = this.coreController.GetTest(id);

            this.kayakoApiRequest.Verify(x => x.ExecuteGet<TestData>(apiMethod), Times.Once());
            Assert.That(getResult, Is.EqualTo(this.getTestData.Data));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Put(int id)
        {
            var apiMethod = string.Format("{0}/{1}", ApiBaseMethods.CoreTest, id);
            this.kayakoApiRequest.Setup(x => x.ExecutePut<TestData>(apiMethod, string.Empty)).Returns(this.getTestData);

            var putResult = this.coreController.PutTest(id);

            this.kayakoApiRequest.Verify(x => x.ExecutePut<TestData>(apiMethod, string.Empty), Times.Once());
            Assert.That(putResult, Is.EqualTo(this.getTestData.Data));
        }

        [Test]
        public void Post()
        {
            this.kayakoApiRequest.Setup(x => x.ExecutePost<TestData>(ApiBaseMethods.CoreTest, string.Empty)).Returns(this.getTestData);

            var postResult = this.coreController.PostTest();

            this.kayakoApiRequest.Verify(x => x.ExecutePost<TestData>(ApiBaseMethods.CoreTest, string.Empty), Times.Once());
            Assert.That(postResult, Is.EqualTo(this.getTestData.Data));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Delete(int id)
        {
            var apiMethod = string.Format("{0}/{1}", ApiBaseMethods.CoreTest, id);
            this.kayakoApiRequest.Setup(x => x.ExecuteDelete(apiMethod)).Returns(true);

            var deleteResult = this.coreController.DeleteTest(id);

            this.kayakoApiRequest.Verify(x => x.ExecuteDelete(apiMethod), Times.Once());
            Assert.That(deleteResult, Is.EqualTo(true));
        }
    }
}