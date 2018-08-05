using System;
using CoffeeShop.Domain.DTO;
using CoffeeShop.Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ingredient = CoffeeShop.Domain.DTO.Ingredient;
using Position = CoffeeShop.Domain.DTO.Position;
using Size = CoffeeShop.Domain.DTO.Size;

namespace CoffeeShop.Domain.Tests.UnitTests
{
	[TestClass]
	public class DTOIngredientTests
	{
		[TestMethod]
		public void OverrideEuals()
		{
			var ingredient1 = new Ingredient(1, 1, new Size(1, "1", new Money(1), new Position(1, "1", "1") ),new DTO.Consumable(1, "1", 0));
			var ingredient2 = new Ingredient(1, 1, new Size(1, "1", new Money(1), new Position(1, "1", "1")), new DTO.Consumable(1, "1", 0));

			Assert.AreEqual(ingredient1, ingredient2);
		}

		[TestMethod]
		public void OverrideEualsOperator()
		{
			var ingredient1 = new Ingredient(1, 1, new Size(1, "1", new Money(1), new Position(1, "1", "1")), new DTO.Consumable(1, "1", 0));
			var ingredient2 = new Ingredient(1, 1, new Size(1, "1", new Money(1), new Position(1, "1", "1")), new DTO.Consumable(1, "1", 0));
			var ingredient3 = new Ingredient(2, 2, new Size(2, "2", new Money(1), new Position(2, "2", "2")), new DTO.Consumable(2, "2", 0));
			Ingredient ingredient4 = null;

			Assert.IsTrue(ingredient1 == ingredient2);
			Assert.IsFalse(ingredient1 == ingredient3);
			Assert.IsTrue(ingredient1 != ingredient3);
			Assert.IsFalse(ingredient1 == ingredient3);
			Assert.IsTrue(ingredient1 != null);
			Assert.IsFalse(ingredient1 == null);
			Assert.IsTrue(ingredient4 == null);
			Assert.IsFalse(ingredient4 != null);
		}

	}
}
