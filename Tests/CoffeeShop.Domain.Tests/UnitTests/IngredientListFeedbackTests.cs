using System;
using System.Collections.Generic;
using CoffeeShop.Data;
using CoffeeShop.Data.Tests;
using CoffeeShop.Domain.DTO;
using CoffeeShop.Domain.Model;
using Domain.DataAccess;
using Domain.UseCases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Consumable = CoffeeShop.Domain.Model.Consumable;
using Position = CoffeeShop.Domain.Model.Position;
using DTOIngredient = CoffeeShop.Domain.DTO.Ingredient;

namespace CoffeeShop.Domain.Tests.UnitTests
{
	[TestClass]
	public class IngredientListFeedbackTests
	{
		[TestMethod]
		public void AddIngredient_Method_Invokes_IngredientAdded_Event()
		{
			var stub = new PositionListDataHolderStub();
			IIngredientList ingredientList = new IngredientListFeedback(stub);
			bool invoked = false;
			EventHandler<IngredientListChangedEventArgs> handler = (sender, e) => invoked = true;
			ingredientList.IngredientListChanged += handler;
			var dtoPosition = new DTO.Position(1, "1", "1");
			ingredientList.AddIngredient(new DTOIngredient(1, 1, new DTO.Size(1, "1", new Money(1), dtoPosition), new DTO.Consumable(1, "water", 1) ));
			Assert.IsTrue(invoked);
		}

		[TestMethod]
		public void AddIngredientInvokesDataHolderAddIngredientMethod()
		{
			var stub = new PositionListDataHolderStub();
			IIngredientList ingredientList = new IngredientListFeedback(stub);
			bool invoked = false;
			EventHandler<IngredientListChangedEventArgs> handler = (sender, e) => invoked = true;
			stub.IngredientListChanged += handler;
			var dtoPosition = new DTO.Position(1, "1", "1");
			ingredientList.AddIngredient(new DTOIngredient(1, 1, new DTO.Size(1, "1", new Money(1), dtoPosition), new DTO.Consumable(1, "water", 1)));
			Assert.IsTrue(invoked);
		}

		[TestMethod]
		public void UpdateIngredient_Method_Invokes_IngredientUpdated_Event()
		{
			var stub = new PositionListDataHolderStub();
			IIngredientList ingredientList = new IngredientListFeedback(stub);
			bool invoked = false;
			EventHandler<IngredientListChangedEventArgs> handler = (sender, e) => invoked = true;
			ingredientList.IngredientListChanged += handler;
			var dtoPosition = new DTO.Position(1, "1", "1");
			ingredientList.UpdateIngredient(new DTOIngredient(1, 1, new DTO.Size(1, "1", new Money(1), dtoPosition), new DTO.Consumable(1, "water", 1)));
			Assert.IsTrue(invoked);
		}

		[TestMethod]
		public void UpdateIngredientInvokesDataHolderUpdateIngredientMethod()
		{
			var stub = new PositionListDataHolderStub();
			IIngredientList ingredientList = new IngredientListFeedback(stub);
			bool invoked = false;
			EventHandler<IngredientListChangedEventArgs> handler = (sender, e) => invoked = true;
			stub.IngredientListChanged += handler;
			var dtoPosition = new DTO.Position(1, "1", "1");
			ingredientList.UpdateIngredient(new DTOIngredient(1, 1, new DTO.Size(1, "1", new Money(1), dtoPosition), new DTO.Consumable(1, "water", 1)));
			Assert.IsTrue(invoked);
		}

		[TestMethod]
		public void DeleteIngredient_Method_Invokes_IngredientDeleted_Event()
		{
			var stub = new PositionListDataHolderStub();
			IIngredientList ingredientList = new IngredientListFeedback(stub);
			bool invoked = false;
			EventHandler<IngredientListChangedEventArgs> handler = (sender, e) => invoked = true;
			ingredientList.IngredientListChanged += handler;
			var dtoPosition = new DTO.Position(1, "1", "1");
			ingredientList.DeleteIngredient(new DTOIngredient(1, 1, new DTO.Size(1, "1", new Money(1), dtoPosition), new DTO.Consumable(1, "water", 1)));
			Assert.IsTrue(invoked);
		}

		[TestMethod]
		public void DeleteIngredientInvokesDataHolderDeleteIngredientMethod()
		{
			var stub = new PositionListDataHolderStub();
			IIngredientList ingredientList = new IngredientListFeedback(stub);
			bool invoked = false;
			EventHandler<IngredientListChangedEventArgs> handler = (sender, e) => invoked = true;
			stub.IngredientListChanged += handler;
			var dtoPosition = new DTO.Position(1, "1", "1");
			ingredientList.DeleteIngredient(new DTOIngredient(1, 1, new DTO.Size(1, "1", new Money(1), dtoPosition), new DTO.Consumable(1, "water", 1)));
			Assert.IsTrue(invoked);
		}

		private class PositionListDataHolderStub : IPositionListDataHolder
		{
			protected readonly IPositionRepository positionRepository;
			protected List<Position> positions = new List<Position>();

			public PositionListDataHolderStub()
			{
				positionRepository = new PositionRepositoryStub();
			}

			public event EventHandler<IngredientListChangedEventArgs> IngredientListChanged;

			public List<DTO.Position> Positions => positionRepository.Mapper.GetDTOPositionList(positions);
			public List<DTO.Consumable> Consumables { get; }

			public List<DTO.Size> GetSizeList(DTO.Position position)
			{
				throw new NotImplementedException();
			}

			public List<DTO.Ingredient> GetIngredientList(DTO.Size size)
			{
				return new List<DTOIngredient>();
			}

			public void AddPosition(DTO.Position position)
			{
				throw new NotImplementedException();
			}

			public void DeletePosition(DTO.Position position)
			{
				throw new NotImplementedException();
			}

			public void LoadPositionList()
			{
				throw new NotImplementedException();
			}

			public void UpdatePosition(DTO.Position position)
			{
				throw new NotImplementedException();
			}

			public void AddSize(DTO.Size size)
			{
				throw new NotImplementedException();
			}

			public void UpdateSize(DTO.Size size)
			{
				throw new NotImplementedException();
			}

			public void DeleteSize(DTO.Size size)
			{
				throw new NotImplementedException();
			}

			public void AddIngredient(DTOIngredient ingredient)
			{
				IngredientListChanged?.Invoke(this, new IngredientListChangedEventArgs(GetIngredientList(ingredient.Size), null));
			}

			public void UpdateIngredient(DTOIngredient ingredient)
			{
				IngredientListChanged?.Invoke(this, new IngredientListChangedEventArgs(GetIngredientList(ingredient.Size), null));
			}

			public void DeleteIngredient(DTOIngredient ingredient)
			{
				IngredientListChanged?.Invoke(this, new IngredientListChangedEventArgs(GetIngredientList(ingredient.Size), null));
			}
		}
	}
}
