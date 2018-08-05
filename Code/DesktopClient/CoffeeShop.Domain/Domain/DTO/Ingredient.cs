using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Domain.DTO
{
	public class Ingredient
	{
		public Ingredient(int id, double amount, Size positionSize, Consumable consumable)
		{
			this.Id = id;
			this.Amount = amount;
			this.Position = positionSize.Position;
			this.Size = positionSize;
			this.Consumable = consumable;
		}

		public int Id { get; }
		public double Amount { get; }
		public Position Position { get; }
		public Size Size { get; }
		public Consumable Consumable { get; }

		public override bool Equals(object obj)
		{
			Ingredient another = obj as Ingredient;
			if (another == null) return false;
			if (Id == another.Id && Position == another.Position && Size == another.Size && Consumable == another.Consumable) return true;
			return false;
		}

		public override int GetHashCode()
		{
			return Id;
		}

		public static bool operator ==(Ingredient ingredient1, Ingredient ingredient2)
		{
			if (object.ReferenceEquals(ingredient1, null))
			{
				return object.ReferenceEquals(ingredient2, null);
			}
			return ingredient1.Equals(ingredient2);
		}

		public static bool operator !=(Ingredient ingredient1, Ingredient ingredient2)
		{
			if (object.ReferenceEquals(ingredient1, null))
			{
				return !object.ReferenceEquals(ingredient2, null);
			}
			return !ingredient1.Equals(ingredient2);
		}
	}
}
