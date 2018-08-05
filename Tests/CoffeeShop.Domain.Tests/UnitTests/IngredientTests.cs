using System;
using System.Collections.Generic;
using CoffeeShop.Domain.DTO;
using CoffeeShop.Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ingredient = CoffeeShop.Domain.Model.Ingredient;
using DTOIngredient = CoffeeShop.Domain.DTO.Ingredient;
using Position = CoffeeShop.Domain.DTO.Position;
using Size = CoffeeShop.Domain.DTO.Size;

namespace CoffeeShop.Domain.Tests.UnitTests
{
	[TestClass]
	public class IngredientTests
	{
		[TestMethod]
		public void TryUpdateIngredientWorks()
		{
			DTOIngredient dtoIngredient = new DTOIngredient(1, 0, new Size(1, "size1", new Money(1), new Position(1, "1", "1")),new DTO.Consumable(1, "water", 0));
			Ingredient ingredient = new Ingredient(1, new Model.Size(1, new PositionStub(1, "1", "1")), new Model.Consumable(1));
			var result = ingredient.TryUpdateIngredient(dtoIngredient, out var error);
			Assert.IsTrue(result);
			Assert.IsTrue(string.IsNullOrWhiteSpace(error));
			Assert.AreEqual(0, ingredient.Amount);
		}

		[TestMethod]
		public void DTOworks()
		{
			DTOIngredient dtoIngredient = new DTOIngredient(1, 0, new Size(1, "size1", new Money(1), new Position(1, "1", "1")), new DTO.Consumable(1, "water", 0));
			Ingredient ingredient = new Ingredient(1, new Model.Size(1, new PositionStub(1, "1", "1")), new Model.Consumable(1));
			ingredient.TryUpdateIngredient(dtoIngredient, out _);
			DTOIngredient testIngredient = ingredient.DTO;

			Assert.AreEqual(1, testIngredient.Id);
			Assert.AreEqual(0, testIngredient.Amount);
			Assert.AreEqual(1, testIngredient.Position.Id);
			Assert.AreEqual(1, testIngredient.Size.Id);

		}

		[TestMethod]
		public void DoesIngredientExistSuccess_DTOList()
		{
			var ingredient1 = new DTOIngredient(1, 1, new Size(1, "1", new Money(1), new Position(1, "1", "1") ), new DTO.Consumable(1, "1", 1));
			var ingredient2 = new DTOIngredient(2, 1, new Size(1, "1", new Money(1), new Position(1, "1", "1")), new DTO.Consumable(2, "2", 1));
			var ingredient3 = new DTOIngredient(1, 1, new Size(1, "1", new Money(1), new Position(1, "1", "1")), new DTO.Consumable(3, "3", 1));
			var ingredients = new List<DTOIngredient>() { ingredient1, ingredient2 };
			var result = Ingredient.DoesIngredientExist(ingredient3, ingredients, out var error);
			Assert.IsTrue(result);
			Assert.AreEqual($"Ingredient {ingredient3.Consumable.Name} already exists", error);
		}

		[TestMethod]
		public void DoesIngredientExistFail_DTOList()
		{
			var ingredient1 = new DTOIngredient(1, 1, new Size(1, "1", new Money(1), new Position(1, "1", "1")), new DTO.Consumable(1, "1", 1));
			var ingredient2 = new DTOIngredient(2, 1, new Size(1, "1", new Money(1), new Position(1, "1", "1")), new DTO.Consumable(2, "2", 1));
			var ingredient3 = new DTOIngredient(3, 1, new Size(1, "1", new Money(1), new Position(1, "1", "1")), new DTO.Consumable(3, "3", 1));
			var ingredients = new List<DTOIngredient>() { ingredient1, ingredient2 };
			var result = Ingredient.DoesIngredientExist(ingredient3, ingredients, out var error);
			Assert.IsFalse(result);
			Assert.AreEqual($"Ingredient {ingredient3.Consumable.Name} cannot be found", error);
		}

		[TestMethod]
		public void DoesConsumableExist_DTOList()
		{
			var ingredient1 = new DTOIngredient(1, 1, new Size(1, "1", new Money(1), new Position(1, "1", "1")), new DTO.Consumable(1, "1", 1));
			var ingredient2 = new DTOIngredient(2, 1, new Size(1, "1", new Money(1), new Position(1, "1", "1")), new DTO.Consumable(2, "2", 1));
			var ingredient3 = new DTOIngredient(3, 1, new Size(1, "1", new Money(1), new Position(1, "1", "1")), new DTO.Consumable(1, "1", 1));
			var ingredients = new List<DTOIngredient>() { ingredient1, ingredient2 };
			var result = Ingredient.DoesConsumableExist(ingredient3, ingredients, out var error);
			Assert.IsTrue(result);
			Assert.AreEqual($"Ingredient {ingredient3.Consumable.Name} is already present", error);
		}


		private class PositionStub : Domain.Model.Position
		{
			public PositionStub(int id, string name, string category) : base(id)
			{
				this.name = name;
				this.category = category;
			}

			public new List<Domain.Model.Size> Sizes { get => sizes; set => sizes = value; }
		}

	}

}
