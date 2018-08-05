using System;
using CoffeeShop.Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeShop.Domain.Tests.UnitTests
{
	[TestClass]
	public class DTOSizeTests
	{
		[TestMethod]
		public void OverrideEquals()
		{
			var size1 = new DTO.Size(1, "size1", new Money(1), new DTO.Position(1, "1", "1"));
			var size2 = new DTO.Size(1, "size1", new Money(1), new DTO.Position(1, "1", "1"));
			Assert.AreEqual(size1, size2);
		}

		[TestMethod]
		public void OverrideEqualsOperator()
		{
			var size1 = new DTO.Size(1, "size1", new Money(1), new DTO.Position(1, "1", "1"));
			var size2 = new DTO.Size(1, "size1", new Money(1), new DTO.Position(1, "1", "1"));
			var size3 = new DTO.Size(2, "size2", new Money(1), new DTO.Position(2, "2", "2"));
			Size size4 = null;

			Assert.IsTrue(size1 == size2);
			Assert.IsFalse(size1 != size2);
			Assert.IsTrue(size1 != size3);
			Assert.IsFalse(size1 == size3);
			Assert.IsFalse(size1 == null);
			Assert.IsTrue(size1 != null);
			Assert.IsTrue(size4 == null);
			Assert.IsFalse(size4 != null);
		}
	}
}
