using System;
using System.Collections.Generic;
using CoffeeShop.Data.Tests;
using CoffeeShop.Domain.Model;
using Domain.DataAccess;
using Domain.UseCases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Consumable = CoffeeShop.Domain.Model.Consumable;
using DTOConsumable = CoffeeShop.Domain.DTO.Consumable;
using DTOPosition = CoffeeShop.Domain.DTO.Position;
using DTOSize = CoffeeShop.Domain.DTO.Size;
using DTOIngredient = CoffeeShop.Domain.DTO.Ingredient;

namespace CoffeeShop.Domain.Tests.UnitTests
{
	[TestClass]
	public class PositionListDataHolderTests
	{
		[TestMethod]
		public void AddPositionMethod_AddsItToPositionsProperty()
		{
			var positionList = new PositionListDataHolder(new PositionRepositoryStub());
			var dtoPosition = new DTO.Position(1, "1", "1");
			positionList.AddPosition(dtoPosition);
			Assert.AreEqual(dtoPosition, positionList.Positions[0]);
		}

		[TestMethod]
		public void LoadPositionList_fillsPositions()
		{
			var positionList = new PositionListDataHolder(new PositionRepositoryStub());
			positionList.LoadPositionList();
			Assert.AreEqual(2, positionList.Positions.Count);
			Assert.AreEqual(1, positionList.Positions[0].Id);
			Assert.AreEqual(2, positionList.Positions[1].Id);
		}

		[TestMethod]
		public void LoadPositionList_fillsConsumables()
		{
			var positionList = new PositionListDataHolderStub(new PositionRepositoryStub());
			positionList.LoadPositionList();
			Assert.AreEqual(1, positionList.Consumables.Count);
		}


		[TestMethod]
		public void DeletePositionDecreasesPositionList()
		{
			var positionList = new PositionListDataHolder(new PositionRepositoryStub());
			positionList.LoadPositionList();
			var position = new DTO.Position(1, "1", "1");
			positionList.DeletePosition(position);
			Assert.AreEqual(1, positionList.Positions.Count);
		}

		[TestMethod]
		public void UpdatePositionWorks()
		{
			var positionList = new PositionListDataHolder(new PositionRepositoryStub());
			positionList.LoadPositionList();
			var position = new DTO.Position(1, "3", "3");
			positionList.UpdatePosition(position);
			Assert.AreEqual("3", positionList.Positions[0].Name);
			Assert.AreEqual("3", positionList.Positions[0].Category);
		}

		[TestMethod]
		public void GetSizeListWorks()
		{
			var positionList = new PositionListDataHolder(new PositionRepositoryStub());
			positionList.LoadPositionList();
			var position = new DTO.Position(1, "1", "1");
			var sizeList = positionList.GetSizeList(position);
			Assert.AreEqual(new DTOSize(1, "size1", new Money(1), position), sizeList[0]);
			Assert.AreEqual(new DTOSize(2, "size2", new Money(1), position), sizeList[1]);
			Assert.AreEqual(2, sizeList.Count);
		}

		[TestMethod]
		public void GetIngredientListWorks()
		{
			var positionList = new PositionListDataHolder(new PositionRepositoryStub());
			positionList.LoadPositionList();
			var size = new DTOSize(1, "size1", new Money(1), new DTOPosition(1, "1", "1"));
			var ingredientList = positionList.GetIngredientList(size);
			var expected = new DTOIngredient(1, 1, size, new DTO.Consumable(1, "water", 1));
			Assert.AreEqual(expected, ingredientList[0]);
			Assert.AreEqual(1, ingredientList.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetSizeListReturnsExceptionIfPositionDoesntExists()
		{
			var positionList = new PositionListDataHolder(new PositionRepositoryStub());
			positionList.LoadPositionList();
			var position = new DTOPosition(3, "3", "3");
			var sizeList = positionList.GetSizeList(position);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetIngredientListReturnsExceptionIfSizeDoesntExists()
		{
			var positionList = new PositionListDataHolder(new PositionRepositoryStub());
			positionList.LoadPositionList();
			var size = new DTOSize(5, "size5", new Money(1), new DTOPosition(1, "1", "1"));
			var ingredientList = positionList.GetIngredientList(size);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetIngredientListReturnsExceptionIfPositionDoesntExists()
		{
			var positionList = new PositionListDataHolder(new PositionRepositoryStub());
			positionList.LoadPositionList();
			var size = new DTOSize(1, "size1", new Money(1), new DTOPosition(3, "3", "3"));
			var ingredientList = positionList.GetIngredientList(size);
		}

		[TestMethod]
		public void AddSizeWorks()
		{
			var positionList = new PositionListDataHolder(new PositionRepositoryStub());
			positionList.LoadPositionList();
			var size = new DTOSize(5, "size5", new Money(1), new DTOPosition(1, "1", "1"));
			positionList.AddSize(size);
			var sizeList = positionList.GetSizeList(size.Position);
			Assert.AreEqual(size, sizeList[2]);
		}

		[TestMethod]
		public void UpdateSizeWork()
		{
			var positionList = new PositionListDataHolder(new PositionRepositoryStub());
			positionList.LoadPositionList();
			var size = new DTOSize(2, "size5", new Money(1), new DTOPosition(1, "1", "1"));
			positionList.UpdateSize(size);
			var sizeList = positionList.GetSizeList(size.Position);
			Assert.AreEqual(size, sizeList[1]);
		}

		[TestMethod]
		public void DeleteSizeWorks()
		{
			var positionList = new PositionListDataHolder(new PositionRepositoryStub());
			positionList.LoadPositionList();
			var size = new DTOSize(2, "size2", new Money(1), new DTOPosition(1, "1", "1"));
			positionList.DeleteSize(size);
			var sizeList = positionList.GetSizeList(size.Position);
			Assert.AreEqual(1, sizeList.Count);
		}

		[TestMethod]
		public void AddIngredientWorks()
		{
			var positionList = new PositionListDataHolder(new PositionRepositoryStub());
			positionList.LoadPositionList();
			var ingredient = new DTOIngredient(5, 1, new DTOSize(1, "size1", new Money(1), new DTOPosition(1, "1", "1")),
				new DTO.Consumable(1, "water", 1));
			positionList.AddIngredient(ingredient);
			var ingredientList = positionList.GetIngredientList(ingredient.Size);
			Assert.AreEqual(ingredient, ingredientList[1]);
		}

		[TestMethod]
		public void UpdateIngredientWorks()
		{
			var positionList = new PositionListDataHolder(new PositionRepositoryStub());
			positionList.LoadPositionList();
			var ingredient1 = new DTOIngredient(5, 1, new DTOSize(1, "size1", new Money(1), new DTOPosition(1, "1", "1")),
				new DTO.Consumable(1, "water", 1));
			positionList.AddIngredient(ingredient1);
			var ingredient2 = new DTOIngredient(5, 2, new DTOSize(1, "size1", new Money(1), new DTOPosition(1, "1", "1")),
				new DTO.Consumable(1, "water", 1));
			positionList.UpdateIngredient(ingredient2);

			var ingredientList = positionList.GetIngredientList(ingredient2.Size);
			Assert.AreEqual(ingredient2, ingredientList[1]);
		}

		[TestMethod]
		public void DeleteIngredientWorks()
		{
			var positionList = new PositionListDataHolder(new PositionRepositoryStub());
			positionList.LoadPositionList();
			var ingredient = new DTOIngredient(1, 1, new DTOSize(1, "size1", new Money(1), new DTOPosition(1, "1", "1")),
				new DTO.Consumable(1, "water", 1));
			positionList.DeleteIngredient(ingredient);
			var ingredientList = positionList.GetIngredientList(ingredient.Size);
			Assert.AreEqual(0, ingredientList.Count);
		}

		[TestMethod]
		public void ConsumablesWork()
		{
			var positionList = new PositionListDataHolder(new PositionRepositoryStub());
			positionList.LoadPositionList();
			var consumables = positionList.Consumables;
			Assert.AreEqual(new DTOConsumable(1, "water", 0), consumables[0]);
		}

		[TestMethod]
		public void NewSizeInitializesExistedIngredients()
		{
			var positionList = new PositionListDataHolder(new PositionRepositoryStub());
			positionList.LoadPositionList();
			var size = new DTOSize(5, "size5", new Money(1), new DTOPosition(1, "1", "1"));
			positionList.AddSize(size);
			var sizeList = positionList.GetSizeList(size.Position);
			Assert.AreEqual("water", sizeList[2].Ingredients[0].Consumable.Name);
		}

		private class PositionListDataHolderStub : PositionListDataHolder
		{
			public PositionListDataHolderStub(IPositionRepository position) : base(position)
			{
				
			}

			public List<Consumable> Consumables => consumables;
		}
	}
}
