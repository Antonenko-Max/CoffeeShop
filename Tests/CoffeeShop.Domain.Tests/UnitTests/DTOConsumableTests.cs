using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeShop.Domain.Tests.UnitTests
{
	[TestClass]
	public class DTOConsumableTests
	{
		[TestMethod]
		public void OverrideEquals()
		{
			var consumable1 = new DTO.Consumable(1, "1", 0);
			var consumable2 = new DTO.Consumable(1, "1", 0);
			var consumable3 = new DTO.Consumable(2, "2", 0);

			Assert.AreEqual(consumable1, consumable2);
			Assert.AreNotEqual(consumable1, consumable3);
		}

		[TestMethod]
		public void OverrideEqualsOperator()
		{
			var consumable1 = new DTO.Consumable(1, "1", 0);
			var consumable2 = new DTO.Consumable(1, "1", 0);
			var consumable3 = new DTO.Consumable(2, "2", 0);
			DTO.Consumable consumable4 = null;

			Assert.IsTrue(consumable1 == consumable2);
			Assert.IsFalse(consumable1 != consumable2);
			Assert.IsTrue(consumable1 != consumable3);
			Assert.IsFalse(consumable1 == consumable3);
			Assert.IsFalse(consumable1 == null);
			Assert.IsTrue(consumable1 != null);
			Assert.IsTrue(consumable4 == null);
			Assert.IsFalse(consumable4 != null);
		}
	}
}
