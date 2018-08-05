using System;
using System.Collections.Generic;
using CoffeeShop.Data;
using CoffeeShop.Data.Tests;
using CoffeeShop.Domain.Model;
using Domain.DataAccess;
using Domain.UseCases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Consumable = CoffeeShop.Domain.DTO.Consumable;
using Ingredient = CoffeeShop.Domain.DTO.Ingredient;
using Size = CoffeeShop.Domain.DTO.Size;

namespace CoffeeShop.Domain.Tests.UnitTests
{
	[TestClass]
	public class PositionListFeedbackTests
	{
		[TestMethod]
		public void LoadPositionList_Method_Invokes_PositionListLoaded_Event()
		{
			IPositionList positionList = new PositionListFeedback(new PositionListDataHolderStub());
			bool invoked = false;
			EventHandler<PositionListChangedEventArgs> handler = (sender, e) => invoked = true;
			positionList.PositionListChanged += handler;
			positionList.LoadPositionList();
			Assert.IsTrue(invoked);
		}

		[TestMethod]
		public void PositionListLoadedEvent_Returns_PositionListFromRepository()
		{
			IPositionList positionList = new PositionListFeedback(new PositionListDataHolderStub());
			List<DTO.Position> positions = null;
			EventHandler<PositionListChangedEventArgs> handler = (sender, e) => positions = e.Positions;
			positionList.PositionListChanged += handler;
			positionList.LoadPositionList();
			List<DTO.Position> expectedPositions = new List<DTO.Position>()
			{
				new DTO.Position(1, "1", "1") ,
				new DTO.Position(2, "2", "2")
			};
			Assert.AreEqual(expectedPositions[0], positions[0]);
			Assert.AreEqual(expectedPositions[1], positions[1]);
			Assert.AreEqual(expectedPositions.Count, positions.Count);
		}

		[TestMethod]
		public void AddPosition_Method_Invokes_PositionAdded_Event()
		{
			var stub = new PositionListDataHolderStub();
			IPositionList positionList = new PositionListFeedback(stub);
			bool invoked = false;
			EventHandler<PositionListChangedEventArgs> handler = (sender, e) => invoked = true;
			positionList.PositionListChanged += handler;
			positionList.AddPosition(new DTO.Position(1, "1", "1"));
			Assert.IsTrue(invoked);
		}

		[TestMethod]
		public void AddPositionInvokesDataHolderAddPositionMethod()
		{
			var stub = new PositionListDataHolderStub();
			IPositionList positionList = new PositionListFeedback(stub);
			bool invoked = false;
			EventHandler<PositionListChangedEventArgs> handler = (sender, e) => invoked = true;
			stub.PositionListChanged += handler;
			positionList.AddPosition(new DTO.Position(1, "1", "1"));
			Assert.IsTrue(invoked);
		}

		[TestMethod]
		public void UpdatePosition_Method_Invokes_PositionUpdated_Event()
		{
			var stub = new PositionListDataHolderStub();
			IPositionList positionList = new PositionListFeedback(stub);
			bool invoked = false;
			EventHandler<PositionListChangedEventArgs> handler = (sender, e) => invoked = true;
			positionList.PositionListChanged += handler;
			positionList.UpdatePosition(new DTO.Position(1, "1", "1"));
			Assert.IsTrue(invoked);
		}

		[TestMethod]
		public void UpdatePositionInvokesDataHolderUpdatePositionMethod()
		{
			var stub = new PositionListDataHolderStub();
			IPositionList positionList = new PositionListFeedback(stub);
			bool invoked = false;
			EventHandler<PositionListChangedEventArgs> handler = (sender, e) => invoked = true;
			stub.PositionListChanged += handler;
			positionList.UpdatePosition(new DTO.Position(1, "1", "1"));
			Assert.IsTrue(invoked);
		}

		[TestMethod]
		public void DeletePosition_Method_Invokes_PositionDeleted_Event()
		{
			var stub = new PositionListDataHolderStub();
			IPositionList positionList = new PositionListFeedback(stub);
			bool invoked = false;
			EventHandler<PositionListChangedEventArgs> handler = (sender, e) => invoked = true;
			positionList.PositionListChanged += handler;
			positionList.DeletePosition(new DTO.Position(1, "1", "1"));
			Assert.IsTrue(invoked);
		}

		[TestMethod]
		public void DeletePositionInvokesDataHolderDeletePositionMethod()
		{
			var stub = new PositionListDataHolderStub();
			IPositionList positionList = new PositionListFeedback(stub);
			bool invoked = false;
			EventHandler<PositionListChangedEventArgs> handler = (sender, e) => invoked = true;
			stub.PositionListChanged += handler;
			positionList.DeletePosition(new DTO.Position(1, "1", "1"));
			Assert.IsTrue(invoked);
		}

		[TestMethod]
		public void ConsumablesWork()
		{
			var stub = new PositionListDataHolderStub();
			IPositionList positionList = new PositionListFeedback(stub);
			var consumables = positionList.Consumables;
			Assert.AreEqual(new Consumable(1, "water", 0), consumables[0]);
		}

		private class PositionListDataHolderStub : IPositionListDataHolder
		{
			protected readonly IPositionRepository positionRepository;
			protected List<Position> positions = new List<Position>();

			public PositionListDataHolderStub()
			{
				positionRepository = new PositionRepositoryStub();
			}

			public event EventHandler<PositionListChangedEventArgs> PositionListChanged;

			public List<DTO.Position> Positions => positionRepository.Mapper.GetDTOPositionList(positions);
			public List<Consumable> Consumables { get; } = new List<Consumable>() { new Consumable(1, "water", 0)};

			public List<Size> GetSizeList(DTO.Position position)
			{
				throw new NotImplementedException();
			}

			public List<Ingredient> GetIngredientList(Size size)
			{
				throw new NotImplementedException();
			}

			public void AddPosition(DTO.Position position)
			{
				PositionListChanged?.Invoke(this, new PositionListChangedEventArgs(Positions, null));
			}

			public void DeletePosition(DTO.Position position)
			{
				PositionListChanged?.Invoke(this, new PositionListChangedEventArgs(Positions, null));
			}

			public void LoadPositionList()
			{
				positions = positionRepository.LoadPositions();
			}

			public void UpdatePosition(DTO.Position position)
			{
				PositionListChanged?.Invoke(this, new PositionListChangedEventArgs(Positions, null));
			}

			public void AddSize(Size size)
			{
				throw new NotImplementedException();
			}

			public void UpdateSize(Size size)
			{
				throw new NotImplementedException();
			}

			public void DeleteSize(Size size)
			{
				throw new NotImplementedException();
			}

			public void AddIngredient(Ingredient ingredient)
			{
				throw new NotImplementedException();
			}

			public void UpdateIngredient(Ingredient ingredient)
			{
				throw new NotImplementedException();
			}

			public void DeleteIngredient(Ingredient ingredient)
			{
				throw new NotImplementedException();
			}
		}
	}
}
