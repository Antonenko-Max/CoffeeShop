using System;
using System.Collections.Generic;
using DTOPosition = CoffeeShop.Domain.DTO.Position;
using DTOSize = CoffeeShop.Domain.DTO.Size;
using DTOIngredient = CoffeeShop.Domain.DTO.Ingredient;
using Position = CoffeeShop.Domain.Model.Position;

namespace Domain.UseCases
{
	public interface IIngredientList
	{
		event EventHandler<IngredientListChangedEventArgs> IngredientListChanged;

		List<DTOIngredient> GetIngredientList(DTOSize size);

		void AddIngredient(DTOIngredient ingredient);
		void UpdateIngredient(DTOIngredient ingredient);
		void DeleteIngredient(DTOIngredient ingredient);
	}

	public class IngredientListChangedEventArgs : EventArgs
	{
		private readonly List<DTOIngredient> ingredients;

		public IngredientListChangedEventArgs(ICollection<DTOIngredient> ingredients) : this(ingredients, null)
		{
		}

		public IngredientListChangedEventArgs(ICollection<DTOIngredient> ingredients, string error)
		{
			this.ingredients = ingredients as List<DTOIngredient>;
			if (this.ingredients == null)
			{
				this.ingredients = new List<DTOIngredient>();
				foreach (var size in ingredients)
				{
					this.ingredients.Add(size);
				}
			}
			Error = error;
		}

		public List<DTOIngredient> Ingredients => ingredients;
		public string Error { get; set; }
	}

}
