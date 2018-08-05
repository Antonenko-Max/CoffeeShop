using System;
using System.Collections.Generic;
using CoffeeShop.Data;
using CoffeeShop.Data.Tests;
using CoffeeShop.Domain.Model;
using Domain.DataAccess;
using Domain.UseCases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Consumable = CoffeeShop.Domain.DTO.Consumable;
using Position = CoffeeShop.Domain.Model.Position;
using DTOSize = CoffeeShop.Domain.DTO.Size;
using Ingredient = CoffeeShop.Domain.DTO.Ingredient;

namespace CoffeeShop.Domain.Tests.UnitTests
{
	[TestClass]
	public class SizeListFeedBackTests
	{
		[TestMethod]
		public void AddSize_Method_Invokes_SizeAdded_Event()
		{
			var stub = new PositionListDataHolderStub();
			ISizeList sizeList = new SizeListFeedback(stub);
			bool invoked = false;
			EventHandler<SizeListChangedEventArgs> handler = (sender, e) => invoked = true;
			sizeList.SizeListChanged += handler;
			sizeList.AddSize(new DTOSize(1, "1", new Money(1), new DTO.Position(1, "1", "1") ));
			Assert.IsTrue(invoked);
		}

		[TestMethod]
		public void AddSizeInvokesDataHolderAddSizeMethod()
		{
			var stub = new PositionListDataHolderStub();
			ISizeList sizeList = new SizeListFeedback(stub);
			bool invoked = false;
			EventHandler<SizeListChangedEventArgs> handler = (sender, e) => invoked = true;
			stub.SizeListChanged += handler;
			sizeList.AddSize(new DTOSize(1, "1", new Money(1), new DTO.Position(1, "1", "1")));
			Assert.IsTrue(invoked);
		}

		[TestMethod]
		public void UpdateSize_Method_Invokes_SizeUpdated_Event()
		{
			var stub = new PositionListDataHolderStub();
			ISizeList sizeList = new SizeListFeedback(stub);
			bool invoked = false;
			EventHandler<SizeListChangedEventArgs> handler = (sender, e) => invoked = true;
			sizeList.SizeListChanged += handler;
			sizeList.UpdateSize(new DTOSize(1, "1", new Money(1), new DTO.Position(1, "1", "1")));
			Assert.IsTrue(invoked);
		}

		[TestMethod]
		public void UpdateSizeInvokesDataHolderUpdateSizeMethod()
		{
			var stub = new PositionListDataHolderStub();
			ISizeList sizeList = new SizeListFeedback(stub);
			bool invoked = false;
			EventHandler<SizeListChangedEventArgs> handler = (sender, e) => invoked = true;
			stub.SizeListChanged += handler;
			sizeList.UpdateSize(new DTOSize(1, "1", new Money(1), new DTO.Position(1, "1", "1")));
			Assert.IsTrue(invoked);
		}

		[TestMethod]
		public void DeleteSize_Method_Invokes_SizeDeleted_Event()
		{
			var stub = new PositionListDataHolderStub();
			ISizeList sizeList = new SizeListFeedback(stub);
			bool invoked = false;
			EventHandler<SizeListChangedEventArgs> handler = (sender, e) => invoked = true;
			sizeList.SizeListChanged += handler;
			sizeList.DeleteSize(new DTOSize(1, "1", new Money(1), new DTO.Position(1, "1", "1")));
			Assert.IsTrue(invoked);
		}

		[TestMethod]
		public void DeleteSizeInvokesDataHolderDeleteSizeMethod()
		{
			var stub = new PositionListDataHolderStub();
			ISizeList sizeList = new SizeListFeedback(stub);
			bool invoked = false;
			EventHandler<SizeListChangedEventArgs> handler = (sender, e) => invoked = true;
			stub.SizeListChanged += handler;
			sizeList.DeleteSize(new DTOSize(1, "1", new Money(1), new DTO.Position(1, "1", "1")));
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

			public event EventHandler<SizeListChangedEventArgs> SizeListChanged;

			public List<DTO.Position> Positions => positionRepository.Mapper.GetDTOPositionList(positions);
			public List<Consumable> Consumables { get; }

			public List<DTOSize> GetSizeList(DTO.Position position)
			{
				return new List<DTOSize>();
			}

			public List<DTO.Ingredient> GetIngredientList(DTOSize size)
			{
				throw new NotImplementedException();
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
				SizeListChanged?.Invoke(this, new SizeListChangedEventArgs(GetSizeList(size.Position), null));
			}

			public void DeleteSize(DTO.Size size)
			{
				SizeListChanged?.Invoke(this, new SizeListChangedEventArgs(GetSizeList(size.Position), null));
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

			public void UpdateSize(DTO.Size size)
			{
				SizeListChanged?.Invoke(this, new SizeListChangedEventArgs(GetSizeList(size.Position), null));
			}
		}
	}
}
