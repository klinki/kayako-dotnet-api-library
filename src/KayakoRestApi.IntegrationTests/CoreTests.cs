﻿using NUnit.Framework;

namespace KayakoRestApi.IntegrationTests
{
	[TestFixture(Description = "A set of tests testing Api methods around Cusom Fields")]
	public class CoreTests : UnitTestBase
	{
		[Test]
		public void GetList()
		{
			string getList = TestSetup.KayakoApiService.Core.GetListTest();

			OutputMessage(getList);
            Assert.That(getList, Is.Not.Null.Or.Empty);
		}

		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		public void Get(int id)
		{
			string get = TestSetup.KayakoApiService.Core.GetTest(id);

			OutputMessage(get);
            Assert.That(get, Is.Not.Null.Or.Empty);
		}

		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		public void Put(int id)
		{
			string put = TestSetup.KayakoApiService.Core.PutTest(id);

			OutputMessage(put);
            Assert.That(put, Is.Not.Null.Or.Empty);
		}

		public void Post()
		{
			string post = TestSetup.KayakoApiService.Core.PostTest();

			OutputMessage(post);
            Assert.That(post, Is.Not.Null.Or.Empty);
		}

		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		public void Delete(int id)
		{
			bool res = TestSetup.KayakoApiService.Core.DeleteTest(id);

			Assert.IsTrue(res);
		}
	}
}
