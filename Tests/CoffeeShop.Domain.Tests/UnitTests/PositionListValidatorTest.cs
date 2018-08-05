using System;
using System.Collections.Generic;
using CoffeeShop.Data;
using CoffeeShop.Data.Tests;
using CoffeeShop.Domain.Model;
using Domain.DataAccess;
using Domain.UseCases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Consumable = CoffeeShop.Domain.Model.Consumable;
using DTOPosition = CoffeeShop.Domain.DTO.Position;
using DTOSize = CoffeeShop.Domain.DTO.Size;
using Ingredient = CoffeeShop.Domain.Model.Ingredient;
using DTOIngredient = CoffeeShop.Domain.DTO.Ingredient;
using Position = CoffeeShop.Domain.Model.Position;
using Sell = CoffeeShop.Domain.Model.Sell;
using DTOSell = CoffeeShop.Domain.DTO.Sell;
using Size = CoffeeShop.Domain.Model.Size;

namespace CoffeeShop.Domain.Tests.UnitTests
{
	[TestClass]
	public class PositionListValidatorTest
	{
		[TestMethod]
		public void AddPositionMethod_InvokesDependencyAddPositionMethod_IfPositionPassesValidation()
		{
			var position = new DTOPosition(3, "3", "3");
			var positionListStub = new PositionListStub();
			bool invoked = false;
			EventHandler<PositionListChangedEventArgs> handler = (sender, e) => invoked = true;
			positionListStub.PositionListChanged += handler;
			var positionListValidator = new PositionListValidator(positionListStub);
			positionListValidator.AddPosition(position);
			Assert.IsTrue(invoked);
		}

		[TestMethod]
		public void CantRepeatPositionName_withAddMethod()
		{
			var position = new DTOPosition(3, "1", "1");
			var positionListStub = new PositionListStub();
			var positionListValidator = new PositionListValidator(positionListStub);
			string error = null;
			EventHandler<PositionListChangedEventArgs> handler = (sender, e) => error = e.Error;
			positionListValidator.PositionListChanged += handler;
			positionListValidator.LoadPositionList();
			positionListValidator.AddPosition(position);
			Assert.AreEqual("Name 1 is already taken", error);
		}

		[TestMethod]
		public void PositionsPropertyReturnsPositionsFromDependency()
		{
			var positionListStub = new PositionListStub();
			var positionListValidator = new PositionListValidator(positionListStub);
			positionListValidator.LoadPositionList();
			var expected = positionListStub.Positions;
			var actual = positionListValidator.Positions;
			Assert.AreEqual(expected[0], actual[0]);
			Assert.AreEqual(expected[1], actual[1]);
			Assert.AreEqual(expected.Count, actual.Count);
		}

		[TestMethod]
		public void UpdatePositionMethod_InvokesDependencyAddPositionMethod_IfPositionPassesValidation()
		{
			var position = new DTOPosition(1, "1", "1");
			var positionListStub = new PositionListStub();
			bool invoked = false;
			EventHandler<PositionListChangedEventArgs> handler = (sender, e) => invoked = true;
			positionListStub.PositionListChanged += handler;
			var positionListValidator = new PositionListValidator(positionListStub);
			positionListValidator.LoadPositionList();
			positionListValidator.UpdatePosition(position);
			Assert.IsTrue(invoked);
		}

		[TestMethod]
		public void CantUpdateNonExistingPosition()
		{
			var position = new DTOPosition(3, "1", "1");
			var positionListStub = new PositionListStub();
			var positionListValidator = new PositionListValidator(positionListStub);
			string error = null;
			EventHandler<PositionListChangedEventArgs> handler = (sender, e) => error = e.Error;
			positionListValidator.PositionListChanged += handler;
			positionListValidator.LoadPositionList();
			positionListValidator.UpdatePosition(position);
			Assert.AreEqual("Position 1 cannot be found", error);
		}

		[TestMethod]
		public void CantUpdateToExistingName()
		{
			var position = new DTOPosition(2, "1", "1");
			var positionListStub = new PositionListStub();
			var positionListValidator = new PositionListValidator(positionListStub);
			string error = null;
			EventHandler<PositionListChangedEventArgs> handler = (sender, e) => error = e.Error;
			positionListValidator.PositionListChanged += handler;
			positionListValidator.LoadPositionList();
			positionListValidator.UpdatePosition(position);
			Assert.AreEqual("Name 1 is already taken", error);
		}

		[TestMethod]
		public void CanUpdateCategoryOnly()
		{
			var position = new DTOPosition(2, "2", "1");
			var positionListStub = new PositionListStub();
			var positionListValidator = new PositionListValidator(positionListStub);
			string error = null;
			EventHandler<PositionListChangedEventArgs> handler = (sender, e) => error = e.Error;
			positionListValidator.PositionListChanged += handler;
			positionListValidator.LoadPositionList();
			positionListValidator.UpdatePosition(position);
			Assert.IsNull(error);
		}

		[TestMethod]
		public void DeletePositionMethod_InvokesDependencyDeletePositionMethod_IfPositionPassesValidation()
		{
			var position = new DTOPosition(1, "1", "1");
			var positionListStub = new PositionListStub();
			bool invoked = false;
			EventHandler<PositionListChangedEventArgs> handler = (sender, e) => invoked = true;
			positionListStub.PositionListChanged += handler;
			var positionListValidator = new PositionListValidator(positionListStub);
			positionListValidator.LoadPositionList();
			positionListValidator.DeletePosition(position);
			Assert.IsTrue(invoked);
		}


		[TestMethod]
		public void DeleteNonExistingPositionReturnsError()
		{
			var position = new DTOPosition(3, "1", "1");
			var positionListStub = new PositionListStub();
			var positionListValidator = new PositionListValidator(positionListStub);
			string error = null;
			EventHandler<PositionListChangedEventArgs> handler = (sender, e) => error = e.Error;
			positionListValidator.PositionListChanged += handler;
			positionListValidator.LoadPositionList();
			positionListValidator.DeletePosition(position);
			Assert.AreEqual("Position 1 cannot be found", error);
		}

		[TestMethod]
		public void ReraiseEventOfDependency()
		{
			var position = new DTOPosition(3, "3", "3");
			var positionListStub = new PositionListStub();
			var positionListValidator = new PositionListValidator(positionListStub);
			bool invoked = false;
			EventHandler<PositionListChangedEventArgs> handler = (sender, e) => invoked = true;
			positionListValidator.PositionListChanged += handler;
			positionListValidator.AddPosition(position);
			Assert.IsTrue(invoked);
		}

		[TestMethod]
		public void ConsumablesWork()
		{
			var positionListStub = new PositionListStub();
			var positionListValidator = new PositionListValidator(positionListStub);
			var consumables = positionListValidator.Consumables;
			Assert.AreEqual(new DTO.Consumable(1, "water", 0), consumables[0]);
		}

		private class PositionListStub : IPositionList
		{
			public event EventHandler<PositionListChangedEventArgs> PositionListChanged;
			public List<DTOPosition> Positions { get; private set; } = new List<DTOPosition>();
			public List<DTO.Consumable> Consumables { get; } = new List<DTO.Consumable>() { new DTO.Consumable(1, "water", 0)};

			public List<DTOSize> GetSizeList(Position position)
			{
				return new List<DTOSize>()
				{
					new DTOSize(1, "size1", new Money(1),new DTOPosition(1, "1", "1") ),
					new DTOSize(2, "size2", new Money(1),new DTOPosition(1, "1", "1") )
				};
			}

			public List<DTOIngredient> GetIngredientList(Position position)
			{
				return new List<DTOIngredient>()
				{
					new DTOIngredient(1, 0, new DTOSize(1, "size1", new Money(1), new DTOPosition(1, "1", "1")), new DTO.Consumable(1, "1", 1)),
					new DTOIngredient(2, 0, new DTOSize(2, "size2", new Money(1), new DTOPosition(1, "1", "1")), new DTO.Consumable(1, "1", 2))
				};
			}

			public void LoadPositionList()
			{
				Positions = new List<DTOPosition>()
				{
					new DTOPosition(1, "1", "1"),
					new DTOPosition(2, "2", "2")
				};
			}

			public void AddPosition(DTOPosition position)
			{
				if (PositionListChanged != null) PositionListChanged.Invoke(this, new PositionListChangedEventArgs(Positions));
			}

			public void UpdatePosition(DTOPosition position)
			{
				if (PositionListChanged != null) PositionListChanged.Invoke(this, new PositionListChangedEventArgs(Positions));
			}

			public void DeletePosition(DTOPosition position)
			{
				if (PositionListChanged != null) PositionListChanged.Invoke(this, new PositionListChangedEventArgs(Positions));
			}

			public void AddSize(DTOSize size)
			{
				throw new NotImplementedException();
			}

			public void UpdateSize(DTOSize size)
			{
				throw new NotImplementedException();
			}

			public void DeleteSize(DTOSize size)
			{
				throw new NotImplementedException();
			}

			public void AddIngredient(DTOIngredient ingredient)
			{
				throw new NotImplementedException();
			}

			public void UpdateIngredient(DTOIngredient ingredient)
			{
				throw new NotImplementedException();
			}

			public void DeleteIngredient(DTOIngredient ingredient)
			{
				throw new NotImplementedException();
			}
		}

		private class PositionStub : Position
		{
			public PositionStub(int id, string name, string category) : base(id)
			{
				this.name = name;
				this.category = category;
			}
		}
	}
}
