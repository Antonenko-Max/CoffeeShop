using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeShop.Domain.Tests.UnitTests
{
	[TestClass]
	public class ConsumableTests
	{
		[TestMethod]
		public void TryUpdateComsumableWorks()
		{
			var dtoConsumable = new Domain.DTO.Consumable(1, "water", 0);
			var consumable = new Domain.Model.Consumable(1);
			var result = consumable.TryUpdateConsumable(dtoConsumable, out var error);

			Assert.IsTrue(result);
			Assert.IsTrue(string.IsNullOrWhiteSpace(error));
			Assert.AreEqual(0, consumable.Amount);
			Assert.AreEqual("water", consumable.Name);
		}

		[TestMethod]
		public void DTOworks()
		{
			var dtoConsumable = new Domain.DTO.Consumable(1, "water", 0);
			var consumable = new Domain.Model.Consumable(1);
			consumable.TryUpdateConsumable(dtoConsumable, out _);
			var testConsumable = consumable.DTO;

			Assert.AreEqual(1, testConsumable.Id);
			Assert.AreEqual(0, testConsumable.Amount);
			Assert.AreEqual("water", testConsumable.Name);
		}
	}
}
