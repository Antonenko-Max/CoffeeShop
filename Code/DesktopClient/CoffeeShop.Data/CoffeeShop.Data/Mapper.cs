using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CoffeeShop.Domain.DTO;
using Domain.DataAccess;
using Domain.Model;
using Domain.Settings;

namespace CoffeeShop.Data
{
	public class Mapper : IMapper
	{
		// todo: make sure not to create redundant instances
		// todo: delete redundant methods

		public List<Domain.DTO.Position> GetDTOPositionList(ICollection<Domain.Model.Position> positions)
		{
			List<Domain.DTO.Position> result = new List<Domain.DTO.Position>();
			foreach (var position in positions)
			{
				var dtoPosition = position.DTO;
				dtoPosition.Sizes = GetDTOSizeList(position.Sizes).ToImmutableList();
				foreach (var size in dtoPosition.Sizes)
				{
					size.Ingredients = GetDTOIngredientList(position.Sizes.Find(p => p.Id == size.Id).Ingredients).ToImmutableList();
				}
				result.Add(dtoPosition);
			}
			return result;
		}

		public List<Domain.Model.Position> GetDomainPositionList(ICollection<Domain.DTO.Position> positions, ICollection<Domain.Model.Consumable> consumables)
		{
			var result = new List<Domain.Model.Position>();
			foreach (var position in positions)
			{
				if (!Domain.Model.Position.DoesPositionExist(position, result, out _) && !Domain.Model.Position.DoesPositionNameExist(position, result, out _))
				{
					var domainPosition = Domain.Model.Position.Create(position);
					domainPosition.UpdateSizes(domainPosition.ValidateSizes(position.Sizes, out _), consumables);
					result.Add(domainPosition);
					foreach (var size in domainPosition.Sizes)
					{
						size.UpdateIngredients(position.Sizes.Find(p => p.Id == size.Id).Ingredients, consumables);
					}
				}
			}
			return result;
		}

		public List<Size> GetDTOSizeList(ICollection<Domain.Model.Size> sizes)
		{
			List<Size> result = new List<Size>();
			foreach (var size in sizes)
			{
				var dtoSize = size.DTO;
				dtoSize.Ingredients = GetDTOIngredientList(size.Ingredients).ToImmutableList();
				result.Add(dtoSize);
			}
			return result;
		}

		public List<Domain.Model.Size> GetDomainSizeList(ICollection<Size> sizes, ICollection<Domain.Model.Consumable> consumables)
		{
			List<Domain.Model.Size> result = new List<Domain.Model.Size>();
			foreach (var size in sizes)
			{
				var position = Domain.Model.Position.Create(size.Position);
				var domainSize = new Domain.Model.Size(size.Id, position);
				domainSize.UpdateSize(size);
				domainSize.UpdateIngredients(size.Ingredients, consumables);
				result.Add(domainSize);
			}
			return result;
		}

		public List<Ingredient> GetDTOIngredientList(ICollection<Domain.Model.Ingredient> ingredients)
		{
			var result = new List<Ingredient>();
			foreach (var ingredient in ingredients)
			{
				result.Add(ingredient.DTO);	
			}
			return result;
		}

		public List<Consumable> GetDTOConsumableList(ICollection<Domain.Model.Consumable> consumables)
		{
			List<Consumable> result = new List<Consumable>();
			foreach (var consumable in consumables)
				result.Add(new Consumable(consumable.Id, consumable.Name, consumable.Amount));
			
			return result;
		}

		public List<Domain.Model.Ingredient> GetDomainIngredientList(ICollection<Ingredient> ingredients, ICollection<Domain.Model.Consumable> consumables)
		{
			List<Domain.Model.Ingredient> result = new List<Domain.Model.Ingredient>();
			foreach (var ingredient in ingredients)
			{
				var position = Domain.Model.Position.Create(ingredient.Position);
				var size = new Domain.Model.Size(ingredient.Size.Id, position);
				var consumable = consumables.First(p => p.Id == ingredient.Consumable.Id);
				var domainIngredient = new Domain.Model.Ingredient(ingredient.Id, size, consumable);
				size.UpdateSize(ingredient.Size);
				var success = consumable.TryUpdateConsumable(ingredient.Consumable, out _);
				if (success) success = domainIngredient.TryUpdateIngredient(ingredient, out _);
				if (success) result.Add(domainIngredient);
			}
			return result;
		}
	}
}
