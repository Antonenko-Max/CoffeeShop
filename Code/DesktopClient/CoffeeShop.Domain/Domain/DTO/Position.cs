using System.Collections.Immutable;
using System.ComponentModel;
using Domain.Model;

namespace CoffeeShop.Domain.DTO
{
	public class Position
	{
		public Position(int id, string name, string category)
		{
			this.Id = id;
			this.Name = name;
			this.Category = category;
		}


		public int Id { get; }
		public string Name { get; }
		public string Category { get; }

		public ImmutableList<Size> Sizes { get; set; } = ImmutableList<Size>.Empty;

		public override bool Equals(object obj)
		{
			Position another = obj as Position;
			if (another == null) return false;
			if (Id == another.Id && Name == another.Name && Category == another.Category) return true;
			return false;
		}

		public static bool operator ==(Position position1, Position position2)
		{
			if (object.ReferenceEquals(position1, null))
			{
				return object.ReferenceEquals(position2, null);
			}
			return position1.Equals(position2);
		}

		public static bool operator !=(Position position1, Position position2)
		{
			if (object.ReferenceEquals(position1, null))
			{
				return !object.ReferenceEquals(position2, null);
			}
			return !position1.Equals(position2);
		}

		public override int GetHashCode()
		{
			return Id;
		}
	}
}
