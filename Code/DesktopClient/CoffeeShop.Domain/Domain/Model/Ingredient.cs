using System.Collections.Generic;
using System.Linq;
using Domain.Settings;
using DTOIngredient = CoffeeShop.Domain.DTO.Ingredient;

namespace CoffeeShop.Domain.Model
{
	public class Ingredient
	{
		protected readonly int id;
		protected double amount;
		protected readonly Position position;
		protected readonly Size size;
		protected readonly Consumable consumable;

		public Ingredient(int id, Size size, Consumable consumable)
		{
			this.id = id;
			this.position = size.Position;
			this.size = size;
			this.consumable = consumable;
		}

		public int Id => id;
		public double Amount => amount;
		public Position Position => position;
		public Size Size => size;
		public Consumable Consumable => consumable;

		public DTOIngredient DTO => new DTOIngredient(id, amount, size.DTO, consumable.DTO);

		public bool TryUpdateIngredient(DTOIngredient ingredient, out string error)
		{
			this.amount = ingredient.Amount;
			error = null;
			return true;
		}

		public static bool DoesIngredientExist(DTOIngredient ingredient, ICollection<DTOIngredient> ingredients, out string error)
		{
			if (ingredients.Any(p => p.Id == ingredient.Id))
			{
				error = Texts.Instance.IngredientAlreadyExists(ingredient.Consumable.Name);
				return true;
			}
			error = Texts.Instance.IngredientCannotBeFound(ingredient.Consumable.Name);
			return false;
		}

		public static bool DoesConsumableExist(DTOIngredient ingredient, ICollection<DTOIngredient> ingredients, out string error)
		{
			if (ingredients.Any(p => p.Consumable.Id == ingredient.Consumable.Id && p.Id != ingredient.Id))
			{
				error = Texts.Instance.IngredientIsAlreadyPresent(ingredient.Consumable.Name);
				return true;
			}
			error = null;
			return false;
		}


		public override bool Equals(object obj)
		{
			Ingredient another = obj as Ingredient;
			if (another == null) return false;
			if (Id == another.Id) return true;
			return false;
		}

		public override int GetHashCode()
		{
			return Id;
		}

	}
}
