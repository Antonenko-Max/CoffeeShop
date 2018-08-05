using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.Entity.Core.Metadata.Edm;
using CoffeeShop.Domain.DTO;
using CoffeeShop.Domain.Model;
using Domain.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Consumable = CoffeeShop.Domain.Model.Consumable;
using Ingredient = CoffeeShop.Domain.Model.Ingredient;
using Size = CoffeeShop.Domain.Model.Size;

namespace CoffeeShop.Data.Tests.UnitTests
{
	[TestClass]
	public class MapperTests
	{
		[TestMethod]
		public void GetDTOPositionsList_Works()
		{
			IMapper mapper = new Mapper();
			IPositionRepository repository = new PositionRepositoryStub();
			ICollection<Domain.Model.Position> source = repository.LoadPositions();
			List<Domain.DTO.Position> testList = mapper.GetDTOPositionList(source);

			var size1 = new Domain.DTO.Size(1, "size1", new Money(1), new Domain.DTO.Position(1, "1", "1"));
			var size2 = new Domain.DTO.Size(2, "size2", new Money(1), new Domain.DTO.Position(1, "1", "1"));
			var size3 = new Domain.DTO.Size(3, "size1", new Money(1), new Domain.DTO.Position(2, "2", "2"));
			var size4 = new Domain.DTO.Size(4, "size2", new Money(1), new Domain.DTO.Position(2, "2", "2"));
			var water = new Domain.DTO.Consumable(1, "water", 0);

			Assert.AreEqual(new Domain.DTO.Position(1, "1", "1"), testList[0]);
			Assert.AreEqual(new Domain.DTO.Position(2, "2", "2"), testList[1]);
			Assert.AreEqual(size1, testList[0].Sizes[0]);
			Assert.AreEqual(size2, testList[0].Sizes[1]);
			Assert.AreEqual(size3, testList[1].Sizes[0]);
			Assert.AreEqual(size4, testList[1].Sizes[1]);
			Assert.AreEqual(new Domain.DTO.Ingredient(1, 2, size1, water), testList[0].Sizes[0].Ingredients[0]);
			Assert.AreEqual(new Domain.DTO.Ingredient(2, 2, size2, water), testList[0].Sizes[1].Ingredients[0]);
			Assert.AreEqual(new Domain.DTO.Ingredient(3, 2, size3, water), testList[1].Sizes[0].Ingredients[0]);
			Assert.AreEqual(new Domain.DTO.Ingredient(4, 2, size4, water), testList[1].Sizes[1].Ingredients[0]);
			Assert.AreEqual(2, testList.Count);
		}

		[TestMethod]
		public void GetDomainPositionsList_Works()
		{
			IMapper mapper = new Mapper();
			IPositionRepository repository = new PositionRepositoryStub();
			List<Consumable> consumables = new List<Consumable>() { new ConsumableStub(1, "water")};

			List<Domain.DTO.Position> source = new List<Domain.DTO.Position>
			{
				new Domain.DTO.Position(1, "1", "1"),
				new Domain.DTO.Position(2, "2", "2")
			};
			var size1 = new Domain.DTO.Size(1, "size1", new Money(1), new Domain.DTO.Position(1, "1", "1"));
			var size2 = new Domain.DTO.Size(2, "size2", new Money(1), new Domain.DTO.Position(1, "1", "1"));
			var size3 = new Domain.DTO.Size(3, "size1", new Money(1), new Domain.DTO.Position(2, "2", "2"));
			var size4 = new Domain.DTO.Size(4, "size2", new Money(1), new Domain.DTO.Position(2, "2", "2"));
			source[0].Sizes = ImmutableList.CreateRange(new List<Domain.DTO.Size>() { size1, size2 });
			source[1].Sizes = ImmutableList.CreateRange(new List<Domain.DTO.Size>() { size3, size4 });
			var water = new Domain.DTO.Consumable(1, "water", 0);
			source[0].Sizes[0].Ingredients = ImmutableList.CreateRange(new List<Domain.DTO.Ingredient>()
			{
				new Domain.DTO.Ingredient(1, 2, size1, water),
			});
			source[0].Sizes[1].Ingredients = ImmutableList.CreateRange(new List<Domain.DTO.Ingredient>()
			{
				new Domain.DTO.Ingredient(2, 2, size2, water)
			});
			source[1].Sizes[0].Ingredients = ImmutableList.CreateRange(new List<Domain.DTO.Ingredient>()
			{
				new Domain.DTO.Ingredient(3, 2, size3, water), 
			});
			source[1].Sizes[1].Ingredients = ImmutableList.CreateRange(new List<Domain.DTO.Ingredient>()
			{
				new Domain.DTO.Ingredient(4, 2, size4, water)
			});

			List<Domain.Model.Position> sample = repository.LoadPositions();
			List<Domain.Model.Position> testList = mapper.GetDomainPositionList(source, consumables);

			Assert.AreEqual(sample[0], testList[0]);
			Assert.AreEqual(sample[1], testList[1]);
			Assert.AreEqual(sample.Count, testList.Count);
			Assert.AreEqual(sample[0].Sizes[0], testList[0].Sizes[0]);
			Assert.AreEqual(sample[0].Sizes[1], testList[0].Sizes[1]);
			Assert.AreEqual(sample[1].Sizes[0], testList[1].Sizes[0]);
			Assert.AreEqual(sample[1].Sizes[1], testList[1].Sizes[1]);
			Assert.AreEqual(sample[0].Sizes.Count, testList[0].Sizes.Count);
			Assert.AreEqual(sample[1].Sizes.Count, testList[1].Sizes.Count);
			Assert.AreEqual(sample[0].Sizes[0].Ingredients[0], testList[0].Sizes[0].Ingredients[0]);
			Assert.AreEqual(sample[0].Sizes[1].Ingredients[0], testList[0].Sizes[1].Ingredients[0]);
			Assert.AreEqual(sample[1].Sizes[0].Ingredients[0], testList[1].Sizes[0].Ingredients[0]);
			Assert.AreEqual(sample[1].Sizes[1].Ingredients[0], testList[1].Sizes[1].Ingredients[0]);
		}

		[TestMethod]
		public void GetDTOSizeList_Works()
		{
			var water = new ConsumableStub(1, "water");
			IMapper mapper = new Mapper();
			List<Consumable> consumables = new List<Consumable>() { water };

			Domain.Model.Size size1 = new SizeStub(1, "size1", new Money(1), new PositionStub(1, "1", "1"));
			Domain.Model.Size size2 = new SizeStub(2, "size2", new Money(1), new PositionStub(2, "2", "2"));
			var dtoSize1 = new Domain.DTO.Size(1, "size1", new Money(1), new Domain.DTO.Position(1, "1", "1"));
			var dtoSize2 = new Domain.DTO.Size(2, "size2", new Money(1), new Domain.DTO.Position(2, "2", "2"));

			List<Domain.Model.Size> source = new List<Size>() { size1, size2 };
			var ingredients1 = new List<Domain.DTO.Ingredient>()
			{ 
				new Domain.DTO.Ingredient(1, 0, dtoSize1, new Domain.DTO.Consumable(1, "water", 0))
			};
			size1.UpdateIngredients(ingredients1, consumables);
			var ingredients2 = new List<Domain.DTO.Ingredient>()
			{
				new Domain.DTO.Ingredient(2, 0, dtoSize2, new Domain.DTO.Consumable(1, "water", 0))
			};
			size2.UpdateIngredients(ingredients2, consumables);

			List<Domain.DTO.Size> sample = new List<Domain.DTO.Size>() {dtoSize1, dtoSize2};
			sample[0].Ingredients = ImmutableList.CreateRange( new List<Domain.DTO.Ingredient>()
			{
				new Domain.DTO.Ingredient(1, 1, dtoSize1, new Domain.DTO.Consumable(1, "water", 0))
			});
			sample[1].Ingredients = ImmutableList.CreateRange(new List<Domain.DTO.Ingredient>()
			{
				new Domain.DTO.Ingredient(2, 1, dtoSize2, new Domain.DTO.Consumable(1, "water", 0))
			});

			List<Domain.DTO.Size> testList = mapper.GetDTOSizeList(source);

			Assert.AreEqual(sample[0], testList[0]);
			Assert.AreEqual(sample[1], testList[1]);
			Assert.AreEqual(2, testList.Count);
			Assert.AreEqual(sample[0].Ingredients[0], testList[0].Ingredients[0]);
			Assert.AreEqual(sample[1].Ingredients[0], testList[1].Ingredients[0]);
			Assert.AreEqual(sample[0].Ingredients.Count, testList[0].Ingredients.Count);
			Assert.AreEqual(sample[1].Ingredients.Count, testList[1].Ingredients.Count);
		}

		[TestMethod]
		public void GetDomainSizeList_Works()
		{
			Mapper mapper = new Mapper();
			List<Consumable> consumables = new List<Consumable>() { new ConsumableStub(1, "water") };

			var water = new Domain.DTO.Consumable(1, "water", 0);
			var dtoSize1 = new Domain.DTO.Size(1, "size1", new Money(1), new Domain.DTO.Position(1, "1", "1") );
			var dtoSize2 = new Domain.DTO.Size(2, "size2", new Money(1), new Domain.DTO.Position(2, "2", "2"));
			var source = new List<Domain.DTO.Size>() { dtoSize1, dtoSize2 };
			source[0].Ingredients = ImmutableList.Create<Domain.DTO.Ingredient>(new Domain.DTO.Ingredient[]
			{ new Domain.DTO.Ingredient(1, 1, dtoSize1, water) });
			source[1].Ingredients = ImmutableList.Create<Domain.DTO.Ingredient>(new Domain.DTO.Ingredient[]
			{ new Domain.DTO.Ingredient(2, 1, dtoSize2, water) });

			var size1 = new SizeStub(1, "size1", new Money(1), new PositionStub(1, "1", "1"));
			var size2 = new SizeStub(2, "size2", new Money(1), new PositionStub(2, "2", "2"));
			var sample = new List<Domain.Model.Size>() { size1, size2 };
			size1.UpdateIngredients(new List<Domain.DTO.Ingredient>()
				{ new Domain.DTO.Ingredient(1, 0, dtoSize1, new Domain.DTO.Consumable(1, "water", 0) ) }, consumables);
			size2.UpdateIngredients(new List<Domain.DTO.Ingredient>()
				{ new Domain.DTO.Ingredient(2, 0, dtoSize2, new Domain.DTO.Consumable(1, "water", 0) ) }, consumables);
			List<Domain.Model.Size> testList = mapper.GetDomainSizeList(source, consumables);

			Assert.AreEqual(sample[0], testList[0]);
			Assert.AreEqual(sample[1], testList[1]);
			Assert.AreEqual(sample.Count, testList.Count);
			Assert.AreEqual(sample[0].Ingredients[0], testList[0].Ingredients[0]);
			Assert.AreEqual(sample[1].Ingredients[0], testList[1].Ingredients[0]);
			Assert.AreEqual(sample[0].Ingredients.Count, testList[0].Ingredients.Count);
			Assert.AreEqual(sample[1].Ingredients.Count, testList[1].Ingredients.Count);
		}

		[TestMethod]
		public void GetGTOIngredientList_Works()
		{
			IMapper mapper = new Mapper();
			var position = new PositionStub(1, "1", "1");
			List<Domain.Model.Ingredient> source = new List<Ingredient>()
			{
				new Ingredient(1, new SizeStub(1, "size1", new Money(1), position), new ConsumableStub(1, "water") ),
				new Ingredient(2, new SizeStub(2, "size2", new Money(1), position), new ConsumableStub(1, "water") ),
				new Ingredient(3, new SizeStub(1, "size1", new Money(1), position), new ConsumableStub(2, "sugar") ),
				new Ingredient(4, new SizeStub(2, "size2", new Money(1), position), new ConsumableStub(2, "sugar") )
			};

			var dtoPosition = new Domain.DTO.Position(1, "1", "1");
			List<Domain.DTO.Ingredient> sample = new List<Domain.DTO.Ingredient>()
			{
				new Domain.DTO.Ingredient(1, 0, new Domain.DTO.Size(1, "size1", new Money(1), dtoPosition ), new Domain.DTO.Consumable(1, "water", 0)),
				new Domain.DTO.Ingredient(2, 0, new Domain.DTO.Size(2, "size2", new Money(1), dtoPosition ), new Domain.DTO.Consumable(1, "water", 0)),
				new Domain.DTO.Ingredient(3, 0, new Domain.DTO.Size(1, "size1", new Money(1), dtoPosition ), new Domain.DTO.Consumable(2, "sugar", 0)),
				new Domain.DTO.Ingredient(4, 0, new Domain.DTO.Size(2, "size2", new Money(1), dtoPosition ), new Domain.DTO.Consumable(2, "sugar", 0)),
			};

			List<Domain.DTO.Ingredient> testList = mapper.GetDTOIngredientList(source);

			Assert.AreEqual(sample[0], testList[0]);
			Assert.AreEqual(sample[1], testList[1]);
			Assert.AreEqual(sample[2], testList[2]);
			Assert.AreEqual(sample[3], testList[3]);
			Assert.AreEqual(sample.Count, testList.Count);
		}

		[TestMethod]
		public void GetDomainIngredientList_Works()
		{
			Mapper mapper = new Mapper();
			List<Consumable> consumables = new List<Consumable>() { new ConsumableStub(1, "water"), new ConsumableStub(2, "sugar") };

			var dtoPosition = new Domain.DTO.Position(1, "1", "1");
			List<Domain.DTO.Ingredient> source = new List<Domain.DTO.Ingredient>()
			{
				new Domain.DTO.Ingredient(1, 0, new Domain.DTO.Size(1, "size1", new Money(1), dtoPosition ), new Domain.DTO.Consumable(1, "water", 0)),
				new Domain.DTO.Ingredient(2, 0, new Domain.DTO.Size(2, "size2", new Money(1), dtoPosition ), new Domain.DTO.Consumable(1, "water", 0)),
				new Domain.DTO.Ingredient(3, 0, new Domain.DTO.Size(1, "size1", new Money(1), dtoPosition ), new Domain.DTO.Consumable(2, "sugar", 0)),
				new Domain.DTO.Ingredient(4, 0, new Domain.DTO.Size(2, "size2", new Money(1), dtoPosition ), new Domain.DTO.Consumable(2, "sugar", 0)),
			};

			var position = new PositionStub(1, "1", "1");
			List<Domain.Model.Ingredient> sample = new List<Ingredient>()
			{
				new Ingredient(1, new Size(1, position), new ConsumableStub(1, "water") ),
				new Ingredient(2, new Size(2, position), new ConsumableStub(1, "water") ),
				new Ingredient(3, new Size(1, position), new ConsumableStub(2, "sugar") ),
				new Ingredient(4, new Size(2, position), new ConsumableStub(2, "sugar") )
			};

			List<Domain.Model.Ingredient> testList = mapper.GetDomainIngredientList(source, consumables);

			Assert.AreEqual(sample[0], testList[0]);
			Assert.AreEqual(sample[1], testList[1]);
			Assert.AreEqual(sample[2], testList[2]);
			Assert.AreEqual(sample[3], testList[3]);
			Assert.AreEqual(sample.Count, testList.Count);
		}

		[TestMethod]
		public void GetDTOConsumableList_Works()
		{
			Mapper mapper = new Mapper();
			List<Consumable> consumables = new List<Consumable>() { new ConsumableStub(1, "water"), new ConsumableStub(2, "sugar") };
			var dtoConsumables = mapper.GetDTOConsumableList(consumables);
			Assert.AreEqual(new Domain.DTO.Consumable(1, "water", 0), dtoConsumables[0]);
			Assert.AreEqual(new Domain.DTO.Consumable(2, "sugar", 0), dtoConsumables[1]);
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

		private class SizeStub : Domain.Model.Size
		{
			public SizeStub(int id, string name, Money price, Domain.Model.Position position) : base(id, position)
			{
				this.name = name;
				this.price = price;
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
