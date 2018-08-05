using System.Collections.Immutable;
using CoffeeShop.Domain.Model;

namespace CoffeeShop.Domain.DTO
{
	public class Size
	{
		public Size(int id, string name, Money price, Position position)
		{
			this.Id = id;
			this.Name = name;
			this.Price = price;
			this.Position = position;
		}
		public int Id { get; }
		public string Name { get; }
		public Money Price { get; }
		public Position Position { get; }

		public ImmutableList<Ingredient> Ingredients { get; set; } = ImmutableList<Ingredient>.Empty;

		public override bool Equals(object obj)
		{
			Size another = obj as Size;
			if (another == null) return false;
			if (Id == another.Id && Name == another.Name && Position == another.Position) return true;
			return false;
		}

		public override int GetHashCode()
		{
			return Id;
		}

		public static bool operator ==(Size size1, Size size2)
		{
			if (object.ReferenceEquals(size1, null))
			{
				return object.ReferenceEquals(size2, null);
			}
			return size1.Equals(size2);
		}

		public static bool operator !=(Size size1, Size size2)
		{
			if (object.ReferenceEquals(size1, null))
			{
				return !object.ReferenceEquals(size2, null);
			}
			return !size1.Equals(size2);
		}
	}
}
