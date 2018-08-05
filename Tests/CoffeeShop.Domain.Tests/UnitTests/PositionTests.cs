using System.Collections.Generic;
using System.Collections.Immutable;
using CoffeeShop.Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Consumable = CoffeeShop.Domain.Model.Consumable;
using DTOPosition = CoffeeShop.Domain.DTO.Position;
using DTOSize = CoffeeShop.Domain.DTO.Size;
using Ingredient = CoffeeShop.Domain.Model.Ingredient;
using DTOIngredient = CoffeeShop.Domain.DTO.Ingredient;
using Position = CoffeeShop.Domain.Model.Position;
using Size = CoffeeShop.Domain.Model.Size;

namespace CoffeeShop.Domain.Tests.UnitTests
{
	[TestClass]
	public class PositionTests
	{
		[TestMethod]
		public void CreateWorks()
		{
			var dTOposition = new DTOPosition(1, "first", "category");
			var domainPosition = Position.Create(dTOposition);

			Assert.AreEqual(dTOposition.Id, domainPosition.Id);
			Assert.AreEqual(dTOposition.Name, domainPosition.Name);
			Assert.AreEqual(dTOposition.Category, domainPosition.Category);
		}


		[TestMethod]
		public void UpdatePositionWorks()
		{
			var dTOposition = new DTOPosition(1, "first", "category");
			var position = Position.Create(dTOposition);
			position.UpdatePosition(dTOposition);

			Assert.AreEqual(1, position.Id);
			Assert.AreEqual("first", position.Name);
			Assert.AreEqual("category", position.Category);
		}

		[TestMethod]
		public void UpdateSizesTestWorks()
		{
			var position = Position.Create(new DTOPosition(1, "1", "1"));
			var consumables = new List<Consumable>() {new Consumable(1)};
			var size = new DTOSize(1, "size1", new Money(1), new DTOPosition(1, "1", "1") );
			var ingredientList = new List<DTOIngredient>() { new DTOIngredient(1, 0, size, new DTO.Consumable(1, "water", 0))};
			size.Ingredients = ingredientList.ToImmutableList();
			var domainSize = new SizeStub(1, position, "size1");
			var domainIngredient = new Ingredient(1, domainSize, new ConsumableStub(1, "water"));
			var sizes = new List<DTOSize> {size};
			position.UpdateSizes(sizes, consumables);

			Assert.AreEqual(domainSize, position.Sizes[0]);
			Assert.AreEqual(domainIngredient, position.Sizes[0].Ingredients[0]);
		}

		[TestMethod]
		public void ValidateSizesReturnsError_IfPositionId_DoesntMach()
		{
			var position = Position.Create(new DTOPosition(1, "1", "1"));
			var size = new DTOSize(1, "Name", new Money(1), new DTOPosition(2, "2", "2"));
			var sizes = new List<DTOSize>() {size};

			position.ValidateSizes(sizes, out var error);
			Assert.AreEqual("Size Name corresponds to another position /n", error);
		}

		[TestMethod]
		public void ValidateSizesReturnErrorIfSizeIdRepeat()
		{
			var position = Position.Create(new DTOPosition(1, "1", "1"));

			var sizes = new List<DTOSize>()
			{
				new DTOSize(1, "size1", new Money(1), new DTOPosition(1, "1", "1")),
				new DTOSize(1, "size2", new Money(1), new DTOPosition(1, "1", "1"))
			}; ;
			position.ValidateSizes(sizes, out string error);
			Assert.AreEqual("Size size2 already exists /n", error);
		}

		[TestMethod]
		public void ValidateSizesReturnsError_IfNameRepeat()
		{
			var position = Position.Create(new DTOPosition(1, "1", "1"));

			var sizes = new List<DTOSize>()
			{
				new DTOSize(1, "size1", new Money(1), new DTOPosition(1, "1", "1")),
				new DTOSize(2, "size1", new Money(1), new DTOPosition(1, "1", "1"))
			}; ;
			position.ValidateSizes(sizes, out string error);
			Assert.AreEqual("Name size1 is already taken /n", error);
		}

		[TestMethod]
		public void DoesPositionExistSuccess()
		{
			var position1 = Position.Create(new DTOPosition(1, "1", "1"));
			var position2 = Position.Create(new DTOPosition(2, "2", "2"));
			var dTOposition = new DTOPosition(1, "2", "2");
			var positions = new List<Position>() { position1, position2 };
			var result = Position.DoesPositionExist(dTOposition, positions, out var error);
			Assert.IsTrue(result);
			Assert.AreEqual($"Position {dTOposition.Name} already exists", error);
		}

		[TestMethod]
		public void DoesPositionExistFail()
		{
			var position1 = Position.Create(new DTOPosition(1, "1", "1"));
			var position2 = Position.Create(new DTOPosition(2, "2", "2"));
			var dTOposition = new DTOPosition(3, "2", "2");
			var positions = new List<Position>() { position1, position2 };
			var result = Position.DoesPositionExist(dTOposition, positions, out var error);
			Assert.IsFalse(result);
			Assert.AreEqual($"Position {dTOposition.Name} cannot be found", error);
		}

		[TestMethod]
		public void DoesPositionNameExist()
		{
			var position1 = Position.Create(new DTOPosition(1, "1", "1"));
			var position2 = Position.Create(new DTOPosition(2, "2", "2"));
			var dTOposition = new DTOPosition(3, "2", "2");
			var	positions = new List<Position>() { position1, position2 };
			var result = Position.DoesPositionNameExist(dTOposition, positions, out var error);
			Assert.IsTrue(result);
			Assert.AreEqual($"Name {dTOposition.Name} is already taken", error);
		}

		[TestMethod]
		public void DoesPositionExistSuccess_DTOList()
		{
			var position1 = new DTOPosition(1, "1", "1");
			var position2 = new DTOPosition(2, "2", "2");
			var dTOposition = new DTOPosition(1, "2", "2");
			var positions = new List<DTOPosition>() { position1, position2 };
			var result = Position.DoesPositionExist(dTOposition, positions, out var error);
			Assert.IsTrue(result);
			Assert.AreEqual($"Position {dTOposition.Name} already exists", error);
		}

		[TestMethod]
		public void DoesPositionExistFail_DTOList()
		{
			var position1 = new DTOPosition(1, "1", "1");
			var position2 = new DTOPosition(2, "2", "2");
			var dTOposition = new DTOPosition(3, "2", "2");
			var positions = new List<DTOPosition>() { position1, position2 };
			var result = Position.DoesPositionExist(dTOposition, positions, out var error);
			Assert.IsFalse(result);
			Assert.AreEqual($"Position {dTOposition.Name} cannot be found", error);
		}

		[TestMethod]
		public void DoesPositionNameExist_DTOList()
		{
			var position1 = new DTOPosition(1, "1", "1");
			var position2 = new DTOPosition(2, "2", "2");
			var dTOposition = new DTOPosition(3, "2", "2");
			var positions = new List<DTOPosition>() { position1, position2 };
			var result = Position.DoesPositionNameExist(dTOposition, positions, out var error);
			Assert.IsTrue(result);
			Assert.AreEqual($"Name {dTOposition.Name} is already taken", error);
		}


		[TestMethod]
		public void AddSizeWorks()
		{
			var position = Position.Create(new DTOPosition(1, "1", "1"));
			var dtoSize = new DTOSize(1, "size1", new Money(1), new DTOPosition(1, "1", "1"));
			var size = new SizeStub(1, position, "size1");
			position.AddSize(dtoSize);
			Assert.AreEqual(size, position.Sizes[0]);
		}

		[TestMethod]
		public void DeleteSizeWorks()
		{
			var position = Position.Create(new DTOPosition(1, "1", "1"));
			var dtoSize = new DTOSize(1, "size1", new Money(1), new DTOPosition(1, "1", "1"));
			position.AddSize(dtoSize);
			position.DeleteSize(dtoSize);
			Assert.AreEqual(0, position.Sizes.Count);
		}

		[TestMethod]
		public void UpdateSizeWorks()
		{
			var position = Position.Create(new DTOPosition(1, "1", "1"));
			var dtoSize1 = new DTOSize(1, "size1", new Money(1), new DTOPosition(1, "1", "1"));
			position.AddSize(dtoSize1);
			var dtoSize2 = new DTOSize(1, "size2", new Money(1), new DTOPosition(1, "1", "1"));
			position.UpdateSize(dtoSize2);
			Assert.AreEqual("size2", position.Sizes[0].Name);
		}

		private class SizeStub : Size
		{
			public SizeStub(int id, Position position, string name) : base(id, position)
			{
				this.name = name;
			}
		}
		private class ConsumableStub : Consumable
		{
			public ConsumableStub(int id, string name) : base(id)
			{
				this.name = name;
			}
		}

	}
}
