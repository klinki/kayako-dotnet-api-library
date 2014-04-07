using System.Collections.Generic;
using KayakoRestApi.Core.Constants;
using KayakoRestApi.Core.Tickets;
using KayakoRestApi.UnitTests.Utilities;
using NUnit.Framework;

namespace KayakoRestApi.UnitTests.Tickets
{
	[TestFixture]
	public class TicketCollectionSerializationTests
	{
		[Ignore]
		public void TicketCollectionDeserialization()
		{
			var tickets =  new TicketCollection
				{
				};

			var expectedTickets = XmlDataUtility.ReadFromFile<TicketCollection>("TestData/TicketCollection.xml");

			AssertUtility.ObjectsEqual(expectedTickets, tickets);
		}
	}
}
