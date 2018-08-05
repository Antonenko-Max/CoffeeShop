using System.Collections.Generic;
using System.Linq;
using CoffeeShop.Domain.Model;
using Domain.DataAccess;
using DTOPosition = CoffeeShop.Domain.DTO.Position;
using DTOSize = CoffeeShop.Domain.DTO.Size;
using DTOIngredient = CoffeeShop.Domain.DTO.Ingredient;

namespace Domain.UseCases
{
	public class PositionListDataHolder : IPositionListDataHolder
	{
		protected readonly IPositionRepository positionRepository;
		protected List<Position> positions = new List<Position>();
		protected List<Consumable> consumables = new List<Consumable>();

		public PositionListDataHolder(IPositionRepository positionRepository)
		{
			this.positionRepository = positionRepository;
		}

		public List<DTOPosition> Positions => positionRepository.Mapper.GetDTOPositionList(positions);

		public List<CoffeeShop.Domain.DTO.Consumable> Consumables =>
			positionRepository.Mapper.GetDTOConsumableList(consumables);

		public List<DTOSize> GetSizeList(DTOPosition position)
		{
			var sizeList = positions.First(p => p.Id == position.Id).Sizes;
			return positionRepository.Mapper.GetDTOSizeList(sizeList);
		}

		public List<DTOIngredient> GetIngredientList(DTOSize size)
		{
			var ingredientList = positions.First(p => p.Id == size.Position.Id).Sizes.First(p => p.Id == size.Id).Ingredients;
			return positionRepository.Mapper.GetDTOIngredientList(ingredientList);
		}

		public void AddPosition(DTOPosition position)
		{
			positions.Add(Position.Create(new DTOPosition(GetNewPositionId(position.Id), position.Name, position.Category)));
		}

		public void LoadPositionList()
		{
			positions = positionRepository.LoadPositions();
			consumables = positionRepository.LoadConsumables();
		}

		public void DeletePosition(DTOPosition position)
		{
			positions.Remove(positions.First(p => p.Id == position.Id));
		}

		public void UpdatePosition(DTOPosition position)
		{
			positions.First(p => p.Id == position.Id).UpdatePosition(position);
		}

		public void AddSize(DTOSize size)
		{
			var position = positions.First(p => p.Id == size.Position.Id);
			var id = GetNewSizeId(size.Id);
			position.AddSize(new DTOSize(id, size.Name, size.Price, size.Position));
			var ingredientList = position.Sizes[0].Ingredients.ToList();
			foreach (var ingredient in ingredientList)
			{
				position.Sizes.First(p => p.Id == id).AddIngredient(new DTOIngredient(GetNewIngredientId(0), 0, new DTOSize(id, size.Name, size.Price, position.DTO), ingredient.Consumable.DTO), consumables);
			}
		}

		public void UpdateSize(DTOSize size)
		{
			positions.First(p => p.Id == size.Position.Id).UpdateSize(size);
		}

		public void DeleteSize(DTOSize size)
		{
			positions.First(p => p.Id == size.Position.Id).DeleteSize(size);
		}

		public void AddIngredient(DTOIngredient ingredient)
		{
			positions.First(p => p.Id == ingredient.Size.Position.Id).Sizes.First(p => p.Id == ingredient.Size.Id)
				.AddIngredient(new DTOIngredient(GetNewIngredientId(ingredient.Id), ingredient.Amount, ingredient.Size, ingredient.Consumable), 
								consumables);
		}

		public void UpdateIngredient(DTOIngredient ingredient)
		{
			positions.First(p => p.Id == ingredient.Size.Position.Id).Sizes.First(p => p.Id == ingredient.Size.Id)
				.UpdateIngredient(ingredient);
		}

		public void DeleteIngredient(DTOIngredient ingredient)
		{
			positions.First(p => p.Id == ingredient.Size.Position.Id).Sizes.First(p => p.Id == ingredient.Size.Id)
				.DeleteIngredient(ingredient);
		}

		private int GetNewPositionId(int id)
		{
			if (positions.Exists(p => p.Id == id))
				return positions.Select(p => p.Id).Max() + 1;
			return id;
		}

		private int GetNewSizeId(int id)
		{
			var sizes = new List<Size>();
			positions.ForEach(p=> sizes.AddRange(p.Sizes));
			if (sizes.Exists(p => p.Id == id))
				return sizes.Select(p => p.Id).Max() + 1;
			return id;
		}

		private int GetNewIngredientId(int id)
		{
			var ingredients = new List<Ingredient>();
			var sizes = new List<Size>();
			positions.ForEach(p => sizes.AddRange(p.Sizes));
			sizes.ForEach(p => ingredients.AddRange(p.Ingredients));
			if (ingredients.Exists(p => p.Id == id))
				return ingredients.Select(p => p.Id).Max() + 1;
			return id;
		}


	}
}
