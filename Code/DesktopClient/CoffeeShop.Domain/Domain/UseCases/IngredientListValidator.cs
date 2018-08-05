using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Domain.DTO;
using Domain.Settings;
using Position = CoffeeShop.Domain.Model.Position;
using DTOPosition = CoffeeShop.Domain.DTO.Position;
using DTOSize = CoffeeShop.Domain.DTO.Size;
using DTOIngredient = CoffeeShop.Domain.DTO.Ingredient;

namespace Domain.UseCases
{
	public class IngredientListValidator : IIngredientList
	{
		private readonly IIngredientList ingredientListFeedBack;

		public event EventHandler<IngredientListChangedEventArgs> IngredientListChanged;

		public IngredientListValidator(IIngredientList ingredientListFeedBack)
		{
			this.ingredientListFeedBack = ingredientListFeedBack;
			ingredientListFeedBack.IngredientListChanged += OnIngrtedientListChange;
		}

		public List<Ingredient> GetIngredientList(DTOSize size)
		{
			return ingredientListFeedBack.GetIngredientList(size);
		}

		public void AddIngredient(Ingredient ingredient)
		{
			if (GetIngredientList(ingredient.Size).Exists(p => p.Consumable == ingredient.Consumable))
			{
				ThrowError(ingredient, Texts.Instance.IngredientAlreadyExists(ingredient.Consumable.Name));
				return;
			}
			ingredientListFeedBack.AddIngredient(ingredient);
		}

		public void UpdateIngredient(Ingredient ingredient)
		{
			if (!GetIngredientList(ingredient.Size).Exists(p => p.Id == ingredient.Id))
			{
				ThrowError(ingredient, Texts.Instance.IngredientCannotBeFound(ingredient.Consumable.Name));
				return;
			}
			if (GetIngredientList(ingredient.Size).Exists(p => p.Consumable == ingredient.Consumable && p.Id != ingredient.Id))
			{
				ThrowError(ingredient, Texts.Instance.IngredientAlreadyExists(ingredient.Consumable.Name));
				return;
			}
			ingredientListFeedBack.UpdateIngredient(ingredient);
		}

		public void DeleteIngredient(Ingredient ingredient)
		{
			if (!GetIngredientList(ingredient.Size).Exists(p => p.Id == ingredient.Id))
			{
				ThrowError(ingredient, Texts.Instance.IngredientCannotBeFound(ingredient.Consumable.Name));
				return;
			}
			ingredientListFeedBack.DeleteIngredient(ingredient);
		}

		private void ThrowError(Ingredient ingredient, string error)
		{
			IngredientListChanged?.Invoke(this, new IngredientListChangedEventArgs(GetIngredientList(ingredient.Size), error));
		}

		private void OnIngrtedientListChange(object sender, IngredientListChangedEventArgs e)
		{
			IngredientListChanged?.Invoke(this, new IngredientListChangedEventArgs(e.Ingredients, e.Error));
		}
	}
}
