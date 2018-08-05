using System;
using System.Collections.Generic;
using CoffeeShop.Data;
using CoffeeShop.Data.Tests;
using CoffeeShop.Domain.Model;
using CoffeeShop.Windows;
using Domain.UseCases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ingredient = CoffeeShop.Domain.DTO.Ingredient;
using Position = CoffeeShop.Domain.DTO.Position;
using Size = CoffeeShop.Domain.DTO.Size;
using Consumable = CoffeeShop.Domain.DTO.Consumable;

namespace CoffeeShop.DesktopClient.Tests.UnitTests
{
	[TestClass]
	public class PositionListFacadeTests
	{
		[TestMethod]
		public void LoadPositionListWorks()
		{
			var facade = new PositionListFacade(new PositionListStub(), new SizeListStub(), new IngredientListStub());
			var list = facade.LoadPositionList();
			Assert.AreEqual(new Position(1, "1", "1"), list[0]);
			Assert.AreEqual(new Position(2, "2", "2"), list[1]);
		}

		[TestMethod]
		public void GetSizeListWorks()
		{
			var facade = new PositionListFacade(new PositionListStub(), new SizeListStub(), new IngredientListStub());
			var position = new Position(1, "1", "1");
			var list = facade.GetSizeList(position);
			Assert.AreEqual(new Size(1, "size1", new Money(1), position), list[0]);
			Assert.AreEqual(new Size(2, "size2", new Money(1), position), list[1]);
		}

		[TestMethod]
		public void GetIngredient()
		{
			var facade = new PositionListFacade(new PositionListStub(), new SizeListStub(), new IngredientListStub());
			var size = new Size(1, "size1", new Money(1), new Position(1, "1", "1") );
			var list = facade.GetIngredientList(size);
			Assert.AreEqual(new Ingredient(1, 1, size, new Consumable(1, "1", 1)), list[0]);
		}

		[TestMethod]
		public void AddPositionWorks()
		{
			var facade = new PositionListFacade(new PositionListStub(), new SizeListStub(), new IngredientListStub());
			var position = new Position(1, "1", "1");
			var list = facade.AddPosition(position);
			Assert.AreEqual(position, list[0]);
		}

		[TestMethod]
		public void UpdatePositionWorks()
		{
			var facade = new PositionListFacade(new PositionListStub(), new SizeListStub(), new IngredientListStub());
			var position = new Position(1, "1", "1");
			facade.AddPosition(position);
			position = new Position(1, "2", "2");
			var list = facade.UpdatePosition(position);
			Assert.AreEqual(position, list[0]);
		}

		[TestMethod]
		public void DeletePositionWorks()
		{
			var facade = new PositionListFacade(new PositionListStub(), new SizeListStub(), new IngredientListStub());
			var position = new Position(1, "1", "1");
			facade.AddPosition(position);
			var list = facade.DeletePosition(position);
			Assert.AreEqual(0, list.Count);
		}

		[TestMethod]
		public void AddSizeWorks()
		{
			var facade = new PositionListFacade(new PositionListStub(), new SizeListStub(), new IngredientListStub());
			var size = new Size(1, "1", new Money(1), new Position(1, "1", "1") );
			var list = facade.AddSize(size);
			Assert.AreEqual(size, list[0]);
		}

		[TestMethod]
		public void UpdateSizeWorks()
		{
			var facade = new PositionListFacade(new PositionListStub(), new SizeListStub(), new IngredientListStub());
			var size = new Size(1, "1", new Money(1), new Position(1, "1", "1"));
			facade.AddSize(size);
			size = new Size(1, "2", new Money(1), new Position(1, "1", "1"));
			var list = facade.UpdateSize(size);
			Assert.AreEqual(size, list[0]);
		}

		[TestMethod]
		public void DeleteSizeWorks()
		{
			var facade = new PositionListFacade(new PositionListStub(), new SizeListStub(), new IngredientListStub());
			var size = new Size(1, "1", new Money(1), new Position(1, "1", "1"));
			facade.AddSize(size);
			var list = facade.DeleteSize(size);
			Assert.AreEqual(0, list.Count);
		}

		[TestMethod]
		public void AddIngredientWorks()
		{
			var facade = new PositionListFacade(new PositionListStub(), new SizeListStub(), new IngredientListStub());
			var ingredient = new Ingredient(1, 1, new Size(1, "1", new Money(1), new Position(1, "1", "1")),
				new Consumable(1, "water", 0));
			var list = facade.AddIngredient(ingredient);
			Assert.AreEqual(ingredient, list[0]);
		}

		[TestMethod]
		public void UpdateIngredientWorks()
		{
			var facade = new PositionListFacade(new PositionListStub(), new SizeListStub(), new IngredientListStub());
			var ingredient = new Ingredient(1, 1, new Size(1, "1", new Money(1), new Position(1, "1", "1")),
				new Consumable(1, "water", 0));
			facade.AddIngredient(ingredient);
			ingredient = new Ingredient(1, 2, new Size(1, "1", new Money(1), new Position(1, "1", "1")),
				new Consumable(1, "water", 0));
			var list = facade.UpdateIngredient(ingredient);
			Assert.AreEqual(ingredient, list[0]);
		}

		[TestMethod]
		public void DeleteIngredientWorks()
		{
			var facade = new PositionListFacade(new PositionListStub(), new SizeListStub(), new IngredientListStub());
			var ingredient = new Ingredient(1, 1, new Size(1, "1", new Money(1), new Position(1, "1", "1")),
				new Consumable(1, "water", 0));
			facade.AddIngredient(ingredient);
			var list = facade.DeleteIngredient(ingredient);
			Assert.AreEqual(0, list.Count);
		}

		[TestMethod]
		public void ReturnEmptySizeListWhenPositionIsNull()
		{
			var facade = new PositionListFacade(new PositionListStub(), new SizeListStub(), new IngredientListStub());
			var list = facade.GetSizeList(null);
			Assert.AreEqual(0, list.Count);
		}

		[TestMethod]
		public void ConsumablesWorks()
		{
			var facade = new PositionListFacade(new PositionListStub(), new SizeListStub(), new IngredientListStub());
			var consumables = facade.Consumables;

			Assert.AreEqual(new Consumable(0, "water", 0), consumables[0]);
		}

		[TestMethod]
		public void MaximumSizeCountAccessorWorks()
		{
			var facade = new PositionListFacade(new PositionListStub(), new SizeListStub(), new IngredientListStub());
			var position = new Position(1, "1", "1");
			facade.GetSizeList(position);
			var isMaximumCountOfSizes = facade.IsMaximumCountOfSizes(position);
			Assert.AreEqual(false, isMaximumCountOfSizes);
		}

		private class PositionListStub : IPositionList
		{
			public event EventHandler<PositionListChangedEventArgs> PositionListChanged;

			public List<Position> Positions { get; set;  }
			public List<Consumable> Consumables { get; } = new List<Consumable>() { new Consumable(0, "water", 0)};

			public PositionListStub()
			{
				Positions = new List<Position>();
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
				return new List<Ingredient>()
				{
					new Ingredient(1, 1, size, new Consumable(1, "1", 1))
				};
			}

			public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

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
	}
}
