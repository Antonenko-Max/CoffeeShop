using System.Collections.Generic;
using CoffeeShop.Domain.DTO;

namespace Domain.UseCases
{
	public interface IPositionListDataHolder
	{
		List<Position> Positions { get; }
		List<Consumable> Consumables { get; }
		List<Size> GetSizeList(Position position);
		List<Ingredient> GetIngredientList(Size size);

		void AddPosition(Position position);
		void DeletePosition(Position position);
		void LoadPositionList();
		void UpdatePosition(Position position);

		void AddSize(Size size);
		void UpdateSize(Size size);
		void DeleteSize(Size size);

		void AddIngredient(Ingredient ingredient);
		void UpdateIngredient(Ingredient ingredient);
		void DeleteIngredient(Ingredient ingredient);
	}
}