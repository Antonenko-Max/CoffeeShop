using System.Collections.Generic;
using System.Linq;
using Domain.Settings;
using DTOPosition = CoffeeShop.Domain.DTO.Position;
using DTOSize = CoffeeShop.Domain.DTO.Size;

namespace CoffeeShop.Domain.Model
{
	public class Position
	{
		protected readonly int id;
		protected string name;
		protected string category;
		protected List<Size> sizes = new List<Size>(); 
		protected List<Sell> sells = new List<Sell>();

		protected Position(int id)
		{
			this.id = id;
		}

		public DTOPosition DTO
		{
			get { return new DTOPosition(id, name, category); }
		}
		
		public int Id => id;
		public string Name => name;
		public string Category => category;
		public List<Size> Sizes => sizes;
		public List<Sell> Sells => sells;

		public static Position Create(DTOPosition position)
		{
			var result = new Position(position.Id)
			{
				name = position.Name,
				category = position.Category
			};
			return result;
		}

		public static bool DoesPositionExist(DTOPosition position, ICollection<Position> positions, out string error)
		{
			if (positions.Any(p => p.Id == position.Id))
			{
				error = Texts.Instance.PositionAlreadyExists(position.Name);
				return true;
			}
			error = Texts.Instance.PositionCannotBeFound(position.Name);
			return false;
		}

		public static bool DoesPositionExist(DTOPosition position, ICollection<DTOPosition> positions, out string error)
		{
			if (positions.Any(p => p.Id == position.Id))
			{
				error = Texts.Instance.PositionAlreadyExists(position.Name);
				return true;
			}
			error = Texts.Instance.PositionCannotBeFound(position.Name);
			return false;
		}

		public static bool DoesPositionNameExist(DTOPosition position, ICollection<Position> positions, out string error)
		{
			if (positions.Any(p => p.Name == position.Name && p.Id != position.Id))
			{
				error = Texts.Instance.NameIsAlreadyTaken(position.Name);
				return true;
			}
			error = null;
			return false;
		}
		
		public static bool DoesPositionNameExist(DTOPosition position, ICollection<DTOPosition> positions, out string error)
		{
			if (positions.Any(p => p.Name == position.Name && p.Id != position.Id))
			{
				error = Texts.Instance.NameIsAlreadyTaken(position.Name);
				return true;
			}
			error = null;
			return false;
		}

		public void UpdatePosition(DTOPosition position)
		{
			name = position.Name;
			category = position.Category;
		}


		public void UpdateSizes(ICollection<DTOSize> sizes, ICollection<Consumable> consumables)
		{
			this.sizes.Clear();
			sizes = ValidateSizes(sizes, out _);
			foreach (var dtoSize in sizes)
			{
				var size = new Size(dtoSize.Id, this);
				size.UpdateSize(dtoSize);
				size.UpdateIngredients(dtoSize.Ingredients, consumables);
				this.sizes.Add(size);
			}
		}

		public List<DTOSize> ValidateSizes(ICollection<DTOSize> sizes, out string error)
		{
			var result = new List<DTOSize>();
			error = null;
			foreach (var size in sizes)
			{
				if (size.Position.Id != this.Id)
				{
					error += Texts.Instance.SizeCorrespondsToAnotherPosition(size.Name) +" /n";
					continue;
				}
				if (Size.DoesSizeExist(size, result, out string existError))
				{
					error += existError + " /n";
					continue;
				}
				if (Size.DoesSizeNameExist(size, result, out existError))
				{
					error += existError + " /n";
					continue;
				}
					result.Add(size);
			}
			return result;
		}

		public void AddSize(DTOSize size)
		{
			var domainSize = new Size(size.Id, this);
			domainSize.UpdateSize(size);
			sizes.Add(domainSize);
		}

		public void DeleteSize(DTOSize size)
		{
			sizes.Remove(sizes.First(p => p.Id == size.Id));
		}

		public void UpdateSize(DTOSize size)
		{
			sizes.First(p => p.Id == size.Id).UpdateSize(size);
		}

		public override bool Equals(object obj)
		{
			Position another = obj as Position;
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
