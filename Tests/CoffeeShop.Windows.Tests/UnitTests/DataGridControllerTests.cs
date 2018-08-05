using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using CoffeeShop.Data;
using CoffeeShop.Data.Tests;
using CoffeeShop.Domain.Model;
using Domain.UseCases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Consumable = CoffeeShop.Domain.DTO.Consumable;
using Ingredient = CoffeeShop.Domain.DTO.Ingredient;
using Position = CoffeeShop.Domain.DTO.Position;
using Size = CoffeeShop.Domain.DTO.Size;

namespace CoffeeShop.Windows.Tests.UnitTests
{
	[TestClass]
	public class DataGridControllerTests
	{
		[TestMethod]
		public void ClearIngredientList_Works()
		{
			var controller = new DataGridControllerStub(new PositionListFacade(new PositionListStub(), new SizeListStub(), new IngredientListStub()))
			{ IngredientRows = new List<IngredientDataGridRow>() { new IngredientDataGridRow() { Consumable = "water" }}};

			controller.ClearIngredientListTest();
			Assert.AreEqual(0, controller.IngredientRows.Count);
			Assert.AreEqual("Ingredients", controller.IngredientColumns.ToList()[0].Header);
			Assert.AreEqual(1, controller.IngredientColumns.Count);
		}

		[TestMethod]
		public void FillDataGrid_fillsColumnsCorrect()
		{
			var controller =
				new DataGridControllerStub(new PositionListFacade(new PositionListStub(), new SizeListStub(),
					new IngredientListStub()));
			controller.Position = new Position(1, "1", "1");
			controller.ClearIngredientListTest();
			var sizes = new List<Size>() { new Size(0, "1", new Money(), controller.Position)};
			controller.FillDataGridTest(sizes);
			Assert.AreEqual("1", controller.IngredientColumns.ToList()[1].Header);
			Assert.AreEqual(2, controller.IngredientColumns.Count);
		}

		[TestMethod]
		public void FillDataGrid_fillsRowsCorrect()
		{
			var facade = new PositionListFacade(new PositionListStub(), new SizeListStub(),
				new IngredientListStub());
			var controller = new DataGridControllerStub(facade);
			controller.Position = facade.LoadPositionList()[0];
			var sizes = facade.GetSizeList(controller.Position);
			controller.Position.Sizes = sizes.ToImmutableList();
			controller.ClearIngredientListTest();
			controller.FillDataGridTest(sizes);
			Assert.AreEqual(1, controller.IngredientRows.Count);
			Assert.AreEqual("1", controller.IngredientRows.ToList()[0].Consumable);
			Assert.AreEqual(1.0, controller.IngredientRows.ToList()[0].Columns["size1"]);
		}

		[TestMethod]
		public void AddNewRowWorks()
		{
			var facade = new PositionListFacade(new PositionListStub(), new SizeListStub(),
				new IngredientListStub());
			var controller = new DataGridController(facade);
			controller.Position = facade.LoadPositionList()[0];
			controller.AddRow(new Consumable(3, "2", 1));
			Assert.AreEqual(1, controller.IngredientRows.Count);
			Assert.AreEqual("2", controller.IngredientRows.ToList()[0].Consumable);
			Assert.AreEqual(0.0, controller.IngredientRows.ToList()[0].Columns["size1"]);
		}

		[TestMethod]
		public void AddExistedRowWorks()
		{
			var facade = new PositionListFacade(new PositionListStub(), new SizeListStub(),
				new IngredientListStub());
			var controller = new DataGridController(facade);
			controller.Position = facade.LoadPositionList()[0];
			controller.AddRow(new Consumable(1, "1", 1));
			Assert.AreEqual(1, controller.IngredientRows.Count);
			Assert.AreEqual("1", controller.IngredientRows.ToList()[0].Consumable);
			Assert.AreEqual(1.0, controller.IngredientRows.ToList()[0].Columns["size1"]);
		}

		[TestMethod]
		public void SynchroniseNewRow()
		{
			var facade = new PositionListFacade(new PositionListStub(), new SizeListStub(),
				new IngredientListStub());
			var controller = new DataGridControllerStub(facade);
			var consumable = new Consumable(2, "2", 1);
			var row = new IngredientDataGridRow() { Consumable = consumable.Name };
			row.Columns.Add("size1", 0.0);
			row.Columns.Add("size2", 0.0);
			controller.Position = facade.LoadPositionList()[0];
			controller.SynchroniseTest(row, consumable);
			Assert.AreEqual(0.0, row.Columns["size1"]);
			Assert.AreEqual(0.0, row.Columns["size2"]);
		}

		[TestMethod]
		public void SynchroniseExistedRow()
		{
			var facade = new PositionListFacade(new PositionListStub(), new SizeListStub(),
				new IngredientListStub());
			var controller = new DataGridControllerStub(facade);
			var consumable = new Consumable(1, "1", 1);
			var row = new IngredientDataGridRow() { Consumable = consumable.Name };
			controller.Position = facade.LoadPositionList()[0];
			controller.SynchroniseTest(row, consumable);
			Assert.AreEqual(1.0, row.Columns["size1"]);
			Assert.AreEqual(1.0, row.Columns["size2"]);
		}

		[TestMethod]
		public void AddRowMethod_AffectsDomain()
		{
			var ingredientList = new IngredientListStub();
			var facade = new PositionListFacade(new PositionListStub(), new SizeListStub(),
				ingredientList);
			var controller = new DataGridController(facade);
			controller.Position = facade.LoadPositionList()[0];
			controller.AddRow(new Consumable(2, "2", 1));
			Assert.AreEqual(4, ingredientList.Ingredients.Count);
			Assert.AreEqual("2", ingredientList.Ingredients[2].Consumable.Name);
			Assert.AreEqual(0, ingredientList.Ingredients[2].Amount);
		}

		[TestMethod]
		public void RemoveRowMethod_AffectsDomain()
		{
			var ingredientList = new IngredientListStub();
			var facade = new PositionListFacade(new PositionListStub(), new SizeListStub(),
				ingredientList);
			var controller = new DataGridController(facade);
			controller.Position = facade.LoadPositionList()[0];
			controller.AddRow(new Consumable(2, "2", 1));
			controller.SelectedRow = controller.IngredientRows.ToList()[0];
			controller.RemoveRow();
			Assert.AreEqual(2, ingredientList.Ingredients.Count);
		}

		[TestMethod]
		public void CellEditEventWorks()
		{
			//var facade = new PositionListFacade(new PositionListStub(), new SizeListStub(),
			//	new IngredientListStub());
			//var controller = new DataGridControllerStub(facade);
			//controller.Position = facade.LoadPositionList()[0];
			//var consumable = new Consumable(2, "2", 2);
			//controller.AddRow(consumable);
			//controller.IngredientRows.ToList()[0].AmountSmall = 1;
			//controller.IngredientRows.ToList()[0].AmountModerate = 1;
			//controller.CellEndEditTest(controller.IngredientRows.ToList()[0]);
			//Assert.AreEqual(1, facade.GetIngredientList(facade.GetSizeList(controller.Position)[0])[0].Amount);
			//Assert.AreEqual(1, facade.GetIngredientList(facade.GetSizeList(controller.Position)[1])[0].Amount);
		}

		private class PositionListStub : IPositionList
		{
			public event EventHandler<PositionListChangedEventArgs> PositionListChanged;

			public List<Position> Positions { get; set; }
			public List<Consumable> Consumables { get; } = new List<Consumable>() { new Consumable(0, "water", 0) };

			public PositionListStub()
			{
				Positions = new List<Position>() { new Position(1, "1", "1")};
			}

			public void LoadPositionList()
			{
				var repository = new PositionRepositoryStub();
				var mapper = new Mapper();
				Positions = mapper.GetDTOPositionList(repository.LoadPositions());
				PositionListChanged?.Invoke(this, new PositionListChangedEventArgs(Positions, null));
			}

			public void AddPosition(Domain.DTO.Position position)
			{
				Positions.Add(position);
				PositionListChanged?.Invoke(this, new PositionListChangedEventArgs(Positions, null));
			}

			public void UpdatePosition(Domain.DTO.Position position)
			{
				var updatingPosition = Positions.Find(p => p.Id == position.Id);
				var index = Positions.IndexOf(updatingPosition);
				Positions[index] = position;
				PositionListChanged?.Invoke(this, new PositionListChangedEventArgs(Positions, null));
			}

			public void DeletePosition(Domain.DTO.Position position)
			{
				Positions.Remove(position);
				PositionListChanged?.Invoke(this, new PositionListChangedEventArgs(Positions, null));
			}
		}

		private class SizeListStub : ISizeList
		{
			public event EventHandler<SizeListChangedEventArgs> SizeListChanged;

			public List<Size> GetSizeList(Position position)
			{
				return new List<Size>()
				{
					new Size(1, "size1", new Money(1), new Position(1, "1", "1") ),
					new Size(2, "size2", new Money(1), new Position(1, "1", "1"))
				};
			}

			public List<Size> Sizes { get; set; } = new List<Size>();

			public void AddSize(Size size)
			{
				Sizes.Add(size);
				SizeListChanged?.Invoke(this, new SizeListChangedEventArgs(Sizes, null));
			}

			public void UpdateSize(Size size)
			{
				var updatingSize = Sizes.Find(p => p.Id == size.Id);
				var index = Sizes.IndexOf(updatingSize);
				Sizes[index] = size;
				SizeListChanged?.Invoke(this, new SizeListChangedEventArgs(Sizes, null));
			}

			public void DeleteSize(Size size)
			{
				Sizes.Remove(size);
				SizeListChanged?.Invoke(this, new SizeListChangedEventArgs(Sizes, null));
			}
		}

		private class IngredientListStub : IIngredientList
		{
			public event EventHandler<IngredientListChangedEventArgs> IngredientListChanged;

			public List<Ingredient> GetIngredientList(Size size)
			{
				return Ingredients.FindAll(p => p.Size == size);
			}

			public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>()
				{
					new Ingredient(1, 1, new Size(1, "size1", new Money(1), new Position(1, "1", "1")), new Consumable(1, "1", 1)),
					new Ingredient(2, 1, new Size(2, "size2", new Money(1), new Position(1, "1", "1")), new Consumable(1, "1", 1))
				};

			public void AddIngredient(Ingredient ingredient)
			{
				Ingredients.Add(ingredient);
				IngredientListChanged?.Invoke(this, new IngredientListChangedEventArgs(Ingredients, null));
			}

			public void UpdateIngredient(Ingredient ingredient)
			{
				var updatingIngredient = Ingredients.Find(p => p.Id == ingredient.Id);
				var index = Ingredients.IndexOf(updatingIngredient);
				Ingredients[index] = ingredient;
				IngredientListChanged?.Invoke(this, new IngredientListChangedEventArgs(Ingredients, null));
			}

			public void DeleteIngredient(Ingredient ingredient)
			{
				Ingredients.Remove(ingredient);
				IngredientListChanged?.Invoke(this, new IngredientListChangedEventArgs(Ingredients, null));
			}
		}

		private class DataGridControllerStub : DataGridController
		{
			public DataGridControllerStub(PositionListFacade positionList) : base(positionList)
			{
				
			}

			public void CellEndEditTest(IEditableObject sender)
			{
				CellEndEdit(sender);
			}

			public void SynchroniseTest(IngredientDataGridRow row, Consumable consumable)
			{
				SynchroniseAmounts(row, consumable);
			}

			public void ClearIngredientListTest()
			{
				ClearIngredientList();
			}

			public void FillDataGridTest(List<Size> sizes)
			{
				FillDataGrid(sizes);
			}
		}
	}
}
