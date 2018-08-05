using System;
using System.Collections.Generic;
using CoffeeShop.Domain.Model;
using Domain.DataAccess;
using Domain.UseCases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DTOPosition = CoffeeShop.Domain.DTO.Position;
using DTOSize = CoffeeShop.Domain.DTO.Size;
using DTOIngredient = CoffeeShop.Domain.DTO.Ingredient;
using Position = CoffeeShop.Domain.Model.Position;
using Size = CoffeeShop.Domain.Model.Size;
using ngredient = CoffeeShop.Domain.Model.Ingredient;


namespace CoffeeShop.Data.Tests
{

	public class PositionRepositoryStub : IPositionRepository
	{
		public List<Domain.Model.Position> LoadPositions()
		{
			var water = new ConsumableStub(1, "water");
			var consumables = new List<Consumable>() {water};
			var position1 = new PositionStub(1, "1", "1");
			var position2 = new PositionStub(2, "2", "2");
			List<Size> sizes1 = new List<Size>()
			{
				new SizeStub(1, "size1", new Money(1), position1),
				new SizeStub(2, "size2", new Money(1), position1)
			};
			position1.Sizes = sizes1;
			List<Size> sizes2 = new List<Size>()
			{
				new SizeStub(3, "size1", new Money(1), position2),
				new SizeStub(4, "size2", new Money(1), position2)
			};
			position2.Sizes = sizes2;
			List<DTOIngredient> ingredients1 = new List<DTOIngredient>()
			{
				new DTOIngredient(1, 0, new DTOSize(1, "1", new Money(1), new DTOPosition(1, "1", "1")), new Domain.DTO.Consumable(1, "water", 0)),
				new DTOIngredient(2, 0, new DTOSize(2, "2", new Money(1), new DTOPosition(1, "1", "1")), new Domain.DTO.Consumable(1, "water", 0)),
			};
			position1.Sizes[0].UpdateIngredients(ingredients1, consumables);
			position1.Sizes[1].UpdateIngredients(ingredients1, consumables);
			List<DTOIngredient> ingredients2 = new List<DTOIngredient>()
			{
				new DTOIngredient(3, 0, new DTOSize(3, "3", new Money(1), new DTOPosition(2, "2", "2")), new Domain.DTO.Consumable(1, "water", 0)),
				new DTOIngredient(4, 0, new DTOSize(4, "4", new Money(1), new DTOPosition(2, "2", "2")), new Domain.DTO.Consumable(1, "water", 0)),
			};
			position2.Sizes[0].UpdateIngredients(ingredients2, consumables);
			position2.Sizes[1].UpdateIngredients(ingredients2, consumables);
			List<Domain.Model.Position> result = new List<Domain.Model.Position>() {position1, position2};
			return result;
		}

		public List<Consumable> LoadConsumables()
		{
			return new List<Consumable>() { new ConsumableStub(1, "water") };
		}

		public IMapper Mapper { get; } = new Mapper();


		private class PositionStub : Domain.Model.Position
		{
			public PositionStub(int id, string name, string category) : base(id)
			{
				this.name = name;
				this.category = category;
			}

			public new List<Domain.Model.Size> Sizes
			{
				get => sizes;
				set => sizes = value;
			}
		}

		private class SizeStub : Size
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
