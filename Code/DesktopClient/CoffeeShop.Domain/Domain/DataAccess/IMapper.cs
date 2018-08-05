using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Domain.Model;
using CoffeeShop.Domain.DTO;

using Consumable = CoffeeShop.Domain.Model.Consumable;
using DTOConsumable = CoffeeShop.Domain.DTO.Consumable;
using DTOPosition = CoffeeShop.Domain.DTO.Position;
using Position = CoffeeShop.Domain.Model.Position;
using DTOSize = CoffeeShop.Domain.DTO.Size;
using Size = CoffeeShop.Domain.Model.Size;
using DTOIngredient = CoffeeShop.Domain.DTO.Ingredient;
using Ingredient = CoffeeShop.Domain.Model.Ingredient;

namespace Domain.DataAccess
{
	public interface IMapper
	{
		List<DTOPosition> GetDTOPositionList(ICollection<Position> positions);
		List<Position> GetDomainPositionList(ICollection<DTOPosition> positions, ICollection<Consumable> consumables);
		List<DTOSize> GetDTOSizeList(ICollection<Size> sizes);
		List<DTOIngredient> GetDTOIngredientList(ICollection<Ingredient> ingredients);
		List<DTOConsumable> GetDTOConsumableList(ICollection<Consumable> consumables);
	}
}
