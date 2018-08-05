using System;
using System.Collections.Generic;
using CoffeeShop.Domain.DTO;
using CoffeeShop.Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DTOIngredient = CoffeeShop.Domain.DTO.Ingredient;
using Ingredient = CoffeeShop.Domain.Model.Ingredient;
using Position = CoffeeShop.Domain.Model.Position;
using Size = CoffeeShop.Domain.Model.Size;
using DTOSize = CoffeeShop.Domain.DTO.Size;

namespace CoffeeShop.Domain.Tests.UnitTests
{
	[TestClass]
	public class SizeTests
	{
		[TestMethod]
		public void TryUpdateSizeTest()
		{
			var size = new Size(1, new PositionStub(1, "1", "1"));
			var dtoSize = new DTO.Size(1, "size1", new Money(1), new DTO.Position(1, "1", "1"));
			size.UpdateSize(dtoSize);

			Assert.AreEqual(1, size.Id);
			Assert.AreEqual("size1", size.Name);
			Assert.AreEqual(1, size.Price.Amount);
		}

		[TestMethod]
		public void UpdateIngredientsTest()
		{
			var consumables = new List<Model.Consumable>() { new Model.Consumable(1)};
			var size = new Size(1, new PositionStub(1, "1", "1"));
			var dtoIngredient = new DTOIngredient(1, 0, new DTO.Size(1, "size1", new Money(1), new DTO.Position(1, "1", "1") ), new DTO.Consumable(1, "water", 0));
			var ingredients = new List<DTOIngredient> { dtoIngredient };
			var domainIngredient = new Ingredient(1, size, consumables[0]);

			size.UpdateIngredients(ingredients, consumables);
			Assert.AreEqual(domainIngredient, size.Ingredients[0]);
			Assert.AreEqual(ingredients.Count, size.Ingredients.Count);
		}

		[TestMethod]
		public void ValidateIngredientsReturnsError_IfSizeId_DoesntMach()
		{
			var size = new Size(1, Position.Create(new DTO.Position(1, "1", "1")));
			var ingredient = new DTOIngredient(1, 1, new DTOSize(2, "1", new Money(), new DTO.Position(1, "1", "1")), new DTO.Consumable(1, "1", 0));
			var ingredients = new List<DTOIngredient>() { ingredient };

			var list = size.ValidateIngredients(ingredients, out var error);
			Assert.AreEqual("Ingredient 1 corresponds to another size /n", error);
			Assert.AreEqual(0, list.Count);
		}

		[TestMethod]
		public void ValidateIngredientsReturnErrorIfIngredientIdRepeat()
		{
			var size = new Size(1, Position.Create(new DTO.Position(1, "1", "1")));
			var ingredient1 = new DTOIngredient(1, 1, new DTOSize(1, "1", new Money(), new DTO.Position(1, "1", "1")), new DTO.Consumable(1, "1", 0));
			var ingredient2 = new DTOIngredient(1, 1, new DTOSize(1, "1", new Money(), new DTO.Position(1, "1", "1")), new DTO.Consumable(1, "1", 0));
			var ingredients = new List<DTOIngredient>() { ingredient1, ingredient2 };

			var list = size.ValidateIngredients(ingredients, out string error);
			Assert.AreEqual("Ingredient 1 already exists /n", error);
			Assert.AreEqual(1, list.Count);
		}

		[TestMethod]
		public void ValidateIngredientsReturnsError_IfConsumableRepeat()
		{
			var size = new Size(1, Position.Create(new DTO.Position(1, "1", "1")));
			var ingredient1 = new DTOIngredient(1, 1, new DTOSize(1, "1", new Money(), new DTO.Position(1, "1", "1")), new DTO.Consumable(1, "1", 0));
			var ingredient2 = new DTOIngredient(2, 1, new DTOSize(1, "1", new Money(), new DTO.Position(1, "1", "1")), new DTO.Consumable(1, "1", 0));
			var ingredients = new List<DTOIngredient>() { ingredient1, ingredient2 };

			var list = size.ValidateIngredients(ingredients, out string error);
			Assert.AreEqual("Ingredient 1 is already present /n", error);
			Assert.AreEqual(1, list.Count);
		}

		[TestMethod]
		public void DTOworks()
		{
			var size = new Size(1, new PositionStub(1, "1", "1"));
			var dtoSize = new DTO.Size(1, "size1", new Money(1), new DTO.Position(1, "1", "1"));
			size.UpdateSize(dtoSize);
			var testSize = size.DTO;

			Assert.AreEqual(1, testSize.Id);
			Assert.AreEqual("size1", testSize.Name);
			Assert.AreEqual(1, testSize.Price.Amount);
			Assert.AreEqual(1, testSize.Position.Id);
		}

		[TestMethod]
		public void DoesSizeExistSuccess_DTOList()
		{
			var size1 = new DTOSize(1, "1", new Money(1), new DTO.Position(1, "1", "1") );
			var size2 = new DTOSize(2, "2", new Money(1), new DTO.Position(1, "1", "1"));
			var size3 = new DTOSize(1, "3", new Money(1), new DTO.Position(1, "1", "1"));
			var sizes = new List<DTOSize>() { size1, size2 };
			var result = Size.DoesSizeExist(size3, sizes, out var error);
			Assert.IsTrue(result);
			Assert.AreEqual($"Size {size3.Name} already exists", error);
		}

		[TestMethod]
		public void DoesSizeExistFail_DTOList()
		{
			var size1 = new DTOSize(1, "1", new Money(1), new DTO.Position(1, "1", "1"));
			var size2 = new DTOSize(2, "2", new Money(1), new DTO.Position(1, "1", "1"));
			var size3 = new DTOSize(3, "3", new Money(1), new DTO.Position(1, "1", "1"));
			var sizes = new List<DTOSize>() { size1, size2 };
			var result = Size.DoesSizeExist(size3, sizes, out var error);
			Assert.IsFalse(result);
			Assert.AreEqual($"Size {size3.Name} cannot be found", error);
		}

		[TestMethod]
		public void DoesSizeNameExist_DTOList()
		{
			var size1 = new DTOSize(1, "1", new Money(1), new DTO.Position(1, "1", "1"));
			var size2 = new DTOSize(2, "2", new Money(1), new DTO.Position(1, "1", "1"));
			var size3 = new DTOSize(1, "2", new Money(1), new DTO.Position(1, "1", "1"));
			var sizes = new List<DTOSize>() { size1, size2 };
			var result = Size.DoesSizeNameExist(size3, sizes, out var error);
			Assert.IsTrue(result);
			Assert.AreEqual($"Name {size3.Name} is already taken", error);
		}

		[TestMethod]
		public void AddIngredientWorks()
		{
			var size = new Size(1, Position.Create(new DTO.Position(1, "1", "1")));
			var consumables = new List<Model.Consumable>() {new Model.Consumable(1)};
			var dtoIngredient = new DTOIngredient(1, 1, new DTOSize(1, "1", new Money(1), new DTO.Position(1, "1", "1")),
				new DTO.Consumable(1, "1", 1));
			var ingredient = new Ingredient(1, size, new Model.Consumable(1));
			size.AddIngredient(dtoIngredient, consumables);
			Assert.AreEqual(ingredient, size.Ingredients[0]);
		}

		[TestMethod]
		public void DeleteIngredientWorks()
		{
			var size = new Size(1, Position.Create(new DTO.Position(1, "1", "1")));
			var consumables = new List<Model.Consumable>() { new Model.Consumable(1) };
			var dtoIngredient = new DTOIngredient(1, 1, new DTOSize(1, "1", new Money(1), new DTO.Position(1, "1", "1")),
				new DTO.Consumable(1, "1", 1));
			var ingredient = new Ingredient(1, size, new Model.Consumable(1));
			size.AddIngredient(dtoIngredient, consumables);
			size.DeleteIngredient(dtoIngredient);
			Assert.AreEqual(0, size.Ingredients.Count);
		}

		[TestMethod]
		public void UpdateIngredientWorks()
		{
			var size = new Size(1, Position.Create(new DTO.Position(1, "1", "1")));
			var consumables = new List<Model.Consumable>() { new Model.Consumable(1) };
			var dtoIngredient1 = new DTOIngredient(1, 1, new DTOSize(1, "1", new Money(1), new DTO.Position(1, "1", "1")),
				new DTO.Consumable(1, "1", 1));
			var ingredient = new Ingredient(1, size, new Model.Consumable(1));
			size.AddIngredient(dtoIngredient1, consumables);
			var dtoIngredient2 = new DTOIngredient(1, 2, new DTOSize(1, "1", new Money(1), new DTO.Position(1, "1", "1")),
				new DTO.Consumable(1, "1", 1));
			size.UpdateIngredient(dtoIngredient2);
			Assert.AreEqual(2, size.Ingredients[0].Amount);
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
