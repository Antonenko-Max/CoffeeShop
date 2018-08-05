using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Domain.DTO
{
	public class Consumable
	{
		public Consumable(int id, string name, double amount)
		{
			this.Id = id;
			this.Name = name;
			this.Amount = amount;
		}

		public int Id { get; }
		public string Name { get; }
		public double Amount { get; }

		public override bool Equals(object obj)
		{
			Consumable another = obj as Consumable;
			if (another == null) return false;
			if (Id == another.Id && Name == another.Name) return true;
			return false;
		}

		public override int GetHashCode()
		{
			return Id;
		}

		public static bool operator ==(Consumable consumable1, Consumable consumable2)
		{
			if (object.ReferenceEquals(consumable1, null))
			{
				return object.ReferenceEquals(consumable2, null);
			}
			return consumable1.Equals(consumable2);
		}

		public static bool operator !=(Consumable consumable1, Consumable consumable2)
		{
			if (object.ReferenceEquals(consumable1, null))
			{
				return !object.ReferenceEquals(consumable2, null);
			}
			return !consumable1.Equals(consumable2);
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
