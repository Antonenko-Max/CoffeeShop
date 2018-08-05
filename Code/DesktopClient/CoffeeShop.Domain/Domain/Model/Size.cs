using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Domain.Settings;
using DTOIngredient = CoffeeShop.Domain.DTO.Ingredient;
using DTOSize = CoffeeShop.Domain.DTO.Size;

namespace CoffeeShop.Domain.Model
{
	public class Size
	{
		protected readonly int id;
		protected string name;
		protected Money price = new Money();
		protected readonly Position position;
		protected List<Ingredient> ingredients = new List<Ingredient>();
		private const int maximumSizes = 3;

		public Size(int id, Position position)
		{
			this.id = id;
			this.position = position;
		}

		public int Id => id;
		public string Name => name;
		public Money Price => price;
		public Position Position => position;

		public List<Ingredient> Ingredients => ingredients;

		public DTOSize DTO => new DTOSize(id, name, price, position.DTO);

		public void UpdateSize(DTOSize size)
		{
			this.name = size.Name;
			this.price = size.Price;
		}

		public void UpdateIngredients(ICollection<DTOIngredient> ingredients, ICollection<Consumable> consumables)
		{
			var checkedIngredients = new List<Ingredient>();
			ingredients = ValidateIngredients(ingredients, out _);
			foreach (var ingredient in ingredients)
			{
				var domainIngredient = new Ingredient(ingredient.Id, this, consumables.First(p => p.Id == ingredient.Consumable.Id));
				checkedIngredients.Add(domainIngredient);
			}
			this.ingredients = checkedIngredients;
		}

		public List<DTOIngredient> ValidateIngredients(ICollection<DTOIngredient> ingredients, out string error)
		{
			var result = new List<DTOIngredient>();
			error = null;
			foreach (var ingredient in ingredients)
			{
				if (ingredient.Size.Id != this.Id)
				{
					error += Texts.Instance.IngredientCorrespondsToAnotherPosition(ingredient.Consumable.Name) + " /n";
					continue;
				}
				if (Ingredient.DoesIngredientExist(ingredient, result, out string existError))
				{
					error += existError + " /n";
					continue;
				}
				if (Ingredient.DoesConsumableExist(ingredient, result, out existError))
				{
					error += existError + " /n";
					continue;
				}
				result.Add(ingredient);
			}
			return result;

		}

		public static bool DoesSizeExist(DTOSize size, ICollection<DTOSize> sizes, out string error)
		{
			if (sizes.Any(p => p.Id == size.Id))
			{
				error = Texts.Instance.SizeAlreadyExists(size.Name);
				return true;
			}
			error = Texts.Instance.SizeCannotBeFound(size.Name);
			return false;
		}

		public static bool DoesSizeNameExist(DTOSize size, ICollection<DTOSize> sizes, out string error)
		{
			if (sizes.Any(p => p.Name == size.Name && p.Id != size.Id))
			{
				error = Texts.Instance.NameIsAlreadyTaken(size.Name);
				return true;
			}
			error = null;
			return false;
		}

		public static bool CountIsMaximum(int count, out string error)
		{
			error = Texts.Instance.CannotAddMeSizesThan(maximumSizes.ToString());
			return (count >= maximumSizes);
		}

		public void AddIngredient(DTOIngredient ingredient, ICollection<Consumable> consumables)
		{
			var domainIngredient = new Ingredient(ingredient.Id, this, consumables.First(p => p.Id == ingredient.Consumable.Id));
			ingredients.Add(domainIngredient);
		}

		public void UpdateIngredient(DTOIngredient ingredient)
		{
			ingredients.First(p => p.Id == ingredient.Id).TryUpdateIngredient(ingredient, out _);
		}

		public void DeleteIngredient(DTOIngredient ingredient)
		{
			ingredients.Remove(ingredients.First(p => p.Id == ingredient.Id));
		}

		public override bool Equals(object obj)
		{
			Size another = obj as Size;
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
