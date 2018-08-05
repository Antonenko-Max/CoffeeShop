using System.Collections.Generic;
using CoffeeShop.Domain.DTO;
using Domain.UseCases;
using Size = CoffeeShop.Domain.DTO.Size;
using Position = CoffeeShop.Domain.DTO.Position;
using Ingredient = CoffeeShop.Domain.DTO.Ingredient;


namespace CoffeeShop.Windows
{
	public class PositionListFacade
	{
		private readonly IPositionList positionList;
		private readonly ISizeList sizeList;
		private readonly IIngredientList ingredientList;

		private List<Position> positions = new List<Position>();
		private List<Size> sizes = new List<Size>();
		private List<Ingredient> ingredients = new List<Ingredient>();
		
		public PositionListFacade(IPositionList positionList, ISizeList sizeList, IIngredientList ingredientList)
		{
			this.positionList = positionList;
			this.sizeList = sizeList;
			this.ingredientList = ingredientList;
			positionList.PositionListChanged += UpdatePositions;
			sizeList.SizeListChanged += UpdateSizes;
			ingredientList.IngredientListChanged += UpdateIngredients;
		}

		public List<Consumable> Consumables => positionList.Consumables;

		public List<Position> LoadPositionList()
		{
			positionList.LoadPositionList();
			return positions;
		}

		public List<Size> GetSizeList(Position position)
		{
			if (position == null) return new List<Size>();
			return sizeList.GetSizeList(position);
		}

		public List<Ingredient> GetIngredientList(Size size)
		{
			return ingredientList.GetIngredientList(size);
		}

		public List<Ingredient> GetIngredientListDefaultSize(Position position)
		{
			if (position == null || GetSizeList(position).Count == 0 || GetIngredientList(GetSizeList(position)[0]).Count == 0)
				return new List<Ingredient>();
			return GetIngredientList(GetSizeList(position)[0]);
		}

		public List<Position> AddPosition(Position position)
		{
			positionList.AddPosition(position);
			return positions;
		}

		public List<Position> UpdatePosition(Position position)
		{
			positionList.UpdatePosition(position);
			return positions;
		}

		public List<Position> DeletePosition(Position position)
		{
			positionList.DeletePosition(position);
			return positions;
		}

		public List<Size> AddSize(Size size)
		{
			sizeList.AddSize(size);
			return sizes;
		}

		public List<Size> UpdateSize(Size size)
		{
			sizeList.UpdateSize(size);
			return sizes;
		}

		public List<Size> DeleteSize(Size size)
		{
			sizeList.DeleteSize(size);
			return sizes;
		}

		public List<Ingredient> AddIngredient(Ingredient ingredient)
		{
			ingredientList.AddIngredient(ingredient);
			return ingredients;
		}

		public List<Ingredient> UpdateIngredient(Ingredient ingredient)
		{
			ingredientList.UpdateIngredient(ingredient);
			return ingredients;
		}

		public List<Ingredient> DeleteIngredient(Ingredient ingredient)
		{
			ingredientList.DeleteIngredient(ingredient);
			return ingredients;
		}

		public bool IsMaximumCountOfSizes(Position position)
		{
			return Domain.Model.Size.CountIsMaximum(GetSizeList(position).Count, out _);
		}

		private void UpdatePositions(object sender, PositionListChangedEventArgs e)
		{
			positions = e.Positions;
		}

		private void UpdateSizes(object sender, SizeListChangedEventArgs e)
		{
			sizes = e.Sizes;
		}

		private void UpdateIngredients(object sender, IngredientListChangedEventArgs e)
		{
			ingredients = e.Ingredients;
		}
	}
}