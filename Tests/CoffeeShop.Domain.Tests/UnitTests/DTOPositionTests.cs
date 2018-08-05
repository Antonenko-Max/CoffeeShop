using System;
using CoffeeShop.Domain.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeShop.Domain.Tests.UnitTests
{
	[TestClass]
	public class DTOPositionTests
	{
		[TestMethod]
		public void EqualTests()
		{
			Position position1 = new Position(1, "1", "1");
			Position position2 = new Position(1, "1", "1");
			Position position3 = new Position(1, "2", "1");

			Assert.AreEqual(position1, position2);
			Assert.AreNotEqual(position1, position3);
		}

		[TestMethod]
		public void EqualOperatorTest()
		{
			Position position1 = new Position(1, "1", "1");
			Position position2 = new Position(1, "1", "1");
			Position position3 = new Position(2, "2", "2");
			Position position4 = null;

			Assert.IsTrue(position1 == position2);
			Assert.IsFalse(position1 != position2);
			Assert.IsTrue(position1 != position3);
			Assert.IsFalse(position1 == position3);
			Assert.IsTrue(position1 != null);
			Assert.IsTrue(position4 == null);
		}
	}
}
