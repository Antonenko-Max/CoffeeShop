using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Domain.DTO;

namespace Domain.UseCases
{
	public class IngredientListFeedback : IIngredientList
	{
		private readonly IPositionListDataHolder positionListDataHolder;
		public event EventHandler<IngredientListChangedEventArgs> IngredientListChanged;

		public IngredientListFeedback(IPositionListDataHolder positionListDataHolder)
		{
			this.positionListDataHolder = positionListDataHolder;
		}

		public List<Ingredient> GetIngredientList(Size size)
		{
			return positionListDataHolder.GetIngredientList(size);
		}

		public void AddIngredient(Ingredient ingredient)
		{
			positionListDataHolder.AddIngredient(ingredient);
			InvokeEvent(ingredient.Size);
		}

		public void UpdateIngredient(Ingredient ingredient)
		{
			positionListDataHolder.UpdateIngredient(ingredient);
			InvokeEvent(ingredient.Size);
		}

		public void DeleteIngredient(Ingredient ingredient)
		{
			positionListDataHolder.DeleteIngredient(ingredient);
			InvokeEvent(ingredient.Size);
		}

		private void InvokeEvent(Size size)
		{
			IngredientListChanged?.Invoke(this, new IngredientListChangedEventArgs(GetIngredientList(size), null));
		}
	}
}
