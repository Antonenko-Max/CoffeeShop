using System;
using System.Collections.Generic;
using CoffeeShop.Data;
using CoffeeShop.Domain.Model;
using CoffeeShop.Windows;
using Domain.UseCases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Position = CoffeeShop.Domain.DTO.Position;
using Size = CoffeeShop.Domain.DTO.Size;
using Ingredient = CoffeeShop.Domain.DTO.Ingredient;
using Consumable = CoffeeShop.Domain.DTO.Consumable;


namespace CoffeeShop.DesktopClient.Tests.FunctionalTests
{
	[TestClass]
	public class PositionListsFacadeTests
	{
		[TestMethod]
		public void AddPositionWorks()
		{
			var facade = CreateFacade();
			var position1 = new Position(0, "1", "1");
			var position2 = new Position(1, "2", "2");
			facade.AddPosition(position1);
			var positions = facade.AddPosition(position2);
			Assert.AreEqual(position1, positions[0]);
			Assert.AreEqual(position2, positions[1]);
			Assert.AreEqual(2, positions.Count);
		}

		[TestMethod]
		public void CantAddPositionsWithIdenticalName()
		{
			var facade = CreateFacade();
			var position1 = new Position(0, "1", "1");
			var position2 = new Position(1, "1", "1");
			facade.AddPosition(position1);
			var positions = facade.AddPosition(position2);
			Assert.AreEqual(position1, positions[0]);
			Assert.AreEqual(1, positions.Count);
		}

		[TestMethod]
		public void AutoEnumerationWorks()
		{
			var facade = CreateFacade();
			var position1 = new Position(0, "1", "1");
			var position2 = new Position(0, "2", "2");
			facade.AddPosition(position1);
			var positions = facade.AddPosition(position2);
			Assert.AreEqual(position1, positions[0]);
			Assert.AreEqual(1, positions[1].Id);
			Assert.AreEqual(2, positions.Count);
		}

		[TestMethod]
		public void UpdatePositionWorks()
		{
			var facade = CreateFacade();
			var position1 = new Position(0, "1", "1");
			var position2 = new Position(0, "2", "2");
			facade.AddPosition(position1);
			var positions = facade.UpdatePosition(position2);
			Assert.AreEqual(position2, positions[0]);
			Assert.AreEqual(1, positions.Count);
		}

		[TestMethod]
		public void CantUpdatePositionToExistingName()
		{
			var facade = CreateFacade();
			var position1 = new Position(0, "1", "1");
			var position2 = new Position(1, "2", "2");
			var position3 = new Position(0, "2", "2");
			facade.AddPosition(position1);
			facade.AddPosition(position2);
			var positions = facade.UpdatePosition(position3);
			Assert.AreEqual(position1.Name, positions[0].Name);
			Assert.AreEqual(position2.Name, positions[1].Name);
			Assert.AreEqual(2, positions.Count);
		}

		[TestMethod]
		public void DeletePositionWorks()
		{
			var facade = CreateFacade();
			var position1 = new Position(0, "1", "1");
			var position2 = new Position(0, "2", "2");
			var positions = facade.AddPosition(position1);
			positions = facade.DeletePosition(position2);
			Assert.AreEqual(0, positions.Count);
		}

		[TestMethod]
		public void UpdatePositionMethodDoesntBreakOrder()
		{
			var facade = CreateFacade();
			var position1 = new Position(1, "1", "1");
			var position2 = new Position(2, "2", "2");
			var position3 = new Position(2, "3", "3");
			facade.AddPosition(position1);
			facade.AddPosition(position2);
			var positions = facade.UpdatePosition(position3);
			Assert.AreEqual(position1, positions[0]);
			Assert.AreEqual(position3, positions[1]);
			Assert.AreEqual(2, positions.Count);
		}

		[TestMethod]
		public void AddSizeWorks()
		{
			var facade = CreateFacade();
			var position = new Position(1, "1", "1");
			var positions = facade.AddPosition(position);
			position = positions[0];
			var size1 = new Size(0, "1", new Money(1), position);
			var size2 = new Size(1, "2", new Money(1), position);
			facade.AddSize(size1);
			var sizes = facade.AddSize(size2);
			Assert.AreEqual(size1, sizes[0]);
			Assert.AreEqual(size2, sizes[1]);
			Assert.AreEqual(2, sizes.Count);
		}

		[TestMethod]
		public void CantAddSizesWithIdenticalName()
		{
			var facade = CreateFacade();
			var position = new Position(1, "1", "1");
			var positions = facade.AddPosition(position);
			position = positions[0];
			var size1 = new Size(0, "1", new Money(1), position);
			var size2 = new Size(1, "1", new Money(1), position);
			facade.AddSize(size1);
			var sizes = facade.AddSize(size2);
			Assert.AreEqual(size1, sizes[0]);
			Assert.AreEqual(1, sizes.Count);
		}

		[TestMethod]
		public void AutoEnumerationSizesWorks()
		{
			var facade = CreateFacade();
			var position = new Position(1, "1", "1");
			var positions = facade.AddPosition(position);
			position = positions[0];
			var size1 = new Size(0, "1", new Money(1), position);
			var size2 = new Size(0, "2", new Money(1), position);
			facade.AddSize(size1);
			var sizes = facade.AddSize(size2);
			Assert.AreEqual(size1, sizes[0]);
			Assert.AreEqual(1, sizes[1].Id);
			Assert.AreEqual(2, sizes.Count);
		}

		[TestMethod]
		public void UpdateSizeWorks()
		{
			var facade = CreateFacade();
			var position = new Position(1, "1", "1");
			var positions = facade.AddPosition(position);
			position = positions[0];
			var size1 = new Size(0, "1", new Money(1), position);
			var size2 = new Size(0, "2", new Money(1), position);
			facade.AddSize(size1);
			var sizes = facade.UpdateSize(size2);
			Assert.AreEqual(size2, sizes[0]);
			Assert.AreEqual(1, sizes.Count);
		}

		[TestMethod]
		public void CantUpdateSizeToExistingName()
		{
			var facade = CreateFacade();
			var position = new Position(1, "1", "1");
			var positions = facade.AddPosition(position);
			position = positions[0];
			var size1 = new Size(0, "1", new Money(1), position);
			var size2 = new Size(1, "2", new Money(1), position);
			var size3 = new Size(0, "2", new Money(1), position);
			facade.AddSize(size1);
			facade.AddSize(size2);
			var sizes = facade.UpdateSize(size3);
			Assert.AreEqual(size1, sizes[0]);
			Assert.AreEqual(size2, sizes[1]);
			Assert.AreEqual(2, sizes.Count);
		}

		[TestMethod]
		public void DeleteSizeWorks()
		{
			var facade = CreateFacade();
			var position = new Position(1, "1", "1");
			var positions = facade.AddPosition(position);
			position = positions[0];
			var size1 = new Size(0, "1", new Money(1), position);
			var size2 = new Size(0, "2", new Money(1), position);
			facade.AddSize(size1);
			var sizes = facade.DeleteSize(size2);
			Assert.AreEqual(0, sizes.Count);
		}

		[TestMethod]
		public void AddIngredientWorks()
		{
			var facade = CreateFacade();
			facade.LoadPositionList();
			var position = new Position(1, "1", "1");
			facade.AddPosition(position);
			var size = new Size(0, "1", new Money(1), position);
			facade.AddSize(size);
			var ingredient1 = new Ingredient(1, 1, size, new Consumable(1, "1", 1));
			var ingredient2 = new Ingredient(2, 1, size, new Consumable(2, "2", 1));
			facade.AddIngredient(ingredient1);
			var ingredients = facade.AddIngredient(ingredient2);
			
			Assert.AreEqual(ingredient1, ingredients[0]);
			Assert.AreEqual(ingredient2, ingredients[1]);
			Assert.AreEqual(2, ingredients.Count);
		}

		[TestMethod]
		public void CantAddIngredientWithIdenticalConsumable()
		{
			var facade = CreateFacade();
			facade.LoadPositionList();
			var position = new Position(1, "1", "1");
			facade.AddPosition(position);
			var size = new Size(0, "1", new Money(1), position);
			facade.AddSize(size);
			var ingredient1 = new Ingredient(1, 1, size, new Consumable(1, "1", 1));
			var ingredient2 = new Ingredient(2, 1, size, new Consumable(1, "1", 1));
			facade.AddIngredient(ingredient1);
			var ingredients = facade.AddIngredient(ingredient2);

			Assert.AreEqual(ingredient1, ingredients[0]);
			Assert.AreEqual(1, ingredients.Count);
		}

		[TestMethod]
		public void AutoEnumerationIngredientWorks()
		{
			var facade = CreateFacade();
			facade.LoadPositionList();
			var position = new Position(1, "1", "1");
			facade.AddPosition(position);
			var size = new Size(0, "1", new Money(1), position);
			facade.AddSize(size);
			var ingredient1 = new Ingredient(0, 1, size, new Consumable(1, "1", 1));
			var ingredient2 = new Ingredient(0, 1, size, new Consumable(2, "2", 1));
			facade.AddIngredient(ingredient1);
			var ingredients = facade.AddIngredient(ingredient2);

			Assert.AreEqual(ingredient1, ingredients[0]);
			Assert.AreEqual(1, ingredients[1].Id);
			Assert.AreEqual(2, ingredients.Count);
		}

		[TestMethod]
		public void UpdateIngredientWorks()
		{
			var facade = CreateFacade();
			facade.LoadPositionList();
			var position = new Position(1, "1", "1");
			facade.AddPosition(position);
			var size = new Size(0, "1", new Money(1), position);
			facade.AddSize(size);
			var ingredient1 = new Ingredient(1, 1, size, new Consumable(1, "1", 1));
			var ingredient2 = new Ingredient(1, 1, size, new Consumable(1, "1", 2));
			facade.AddIngredient(ingredient1);
			var ingredients = facade.UpdateIngredient(ingredient2);

			Assert.AreEqual(ingredient2, ingredients[0]);
			Assert.AreEqual(ingredient2.Amount, ingredients[0].Amount);
			Assert.AreEqual(1, ingredients.Count);
		}

		[TestMethod]
		public void CantUpdateIngredientToExistingConsumable()
		{
			var facade = CreateFacade();
			facade.LoadPositionList();
			var position = new Position(1, "1", "1");
			facade.AddPosition(position);
			var size = new Size(0, "1", new Money(1), position);
			facade.AddSize(size);
			var ingredient1 = new Ingredient(1, 1, size, new Consumable(1, "1", 1));
			var ingredient2 = new Ingredient(2, 1, size, new Consumable(2, "2", 1));
			var ingredient3 = new Ingredient(1, 1, size, new Consumable(2, "2", 1));
			facade.AddIngredient(ingredient1);
			facade.AddIngredient(ingredient2);
			var ingredients = facade.UpdateIngredient(ingredient3);

			Assert.AreEqual(ingredient1, ingredients[0]);
			Assert.AreEqual(ingredient2, ingredients[1]);
			Assert.AreEqual(2, ingredients.Count);
		}

		[TestMethod]
		public void DeleteIngredientWorks()
		{
			var facade = CreateFacade();
			facade.LoadPositionList();
			var position = new Position(1, "1", "1");
			facade.AddPosition(position);
			var size = new Size(0, "1", new Money(1), position);
			facade.AddSize(size);
			var ingredient1 = new Ingredient(1, 1, size, new Consumable(1, "1", 1));
			var ingredient2 = new Ingredient(1, 1, size, new Consumable(2, "2", 1));
			facade.AddIngredient(ingredient1);
			var ingredients = facade.DeleteIngredient(ingredient2);

			Assert.AreEqual(0, ingredients.Count);
		}

		[TestMethod]
		public void ConsumablesWork()
		{
			var facade = CreateFacade();
			facade.LoadPositionList();
			var consumables = facade.Consumables;
			Assert.AreEqual(new Consumable(1, "1", 0), consumables[0]);
			Assert.AreEqual(new Consumable(2, "2", 0), consumables[1]);
		}

		[TestMethod]
		public void CantAddFourthSize()
		{
			var facade = CreateFacade();
			facade.LoadPositionList();
			var position = new Position(1, "1", "1");
			facade.AddPosition(position);
			var size = new Size(0, "size1", new Money(1), position);
			facade.AddSize(size);
			size = new Size(1, "size2", new Money(1), position);
			facade.AddSize(size);
			size = new Size(2, "size3", new Money(1), position);
			facade.AddSize(size);
			size = new Size(3, "size4", new Money(1), position);
			facade.AddSize(size);
			var sizeList = facade.GetSizeList(size.Position);
			Assert.AreEqual(3, sizeList.Count);
		}


		private PositionListFacade CreateFacade()
		{
			var dataHolder = new PositionListDataHolder(new PositionRepositoryEFStub());
			return new PositionListFacade(new PositionListValidator(new PositionListFeedback(dataHolder)),
				new SizeListValidator(new SizeListFeedback(dataHolder)),
				new IngredientListValidator(new IngredientListFeedback(dataHolder)));
		}

		private class PositionRepositoryEFStub : PositionRepositoryEF
		{
			public PositionRepositoryEFStub() : base(new Mapper())
			{
			}

			public override List<Domain.Model.Position> LoadPositions()
			{
				return new List<Domain.Model.Position>();
			}

			public override List<Domain.Model.Consumable> LoadConsumables()
			{
				return new List<Domain.Model.Consumable>()
				{
					new ConsumableStub(1, "1"),
					new ConsumableStub(2, "2")
				};
			}
		}

		private class ConsumableStub : Domain.Model.Consumable
		{
			public ConsumableStub(int id, string name) : base(id)
			{
				this.name = name;
			}
		}

	}
}
