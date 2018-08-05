using System.Collections.Generic;

namespace CoffeeShop.Domain.Model
{
	public class Consumable
	{
		protected readonly int id;
		protected string name;
		protected double amount;
		protected List<Ingredient> ingredients;

		public Consumable(int id)
		{
			this.id = id;
		}

		public static Consumable Create(DTO.Consumable consumable)
		{
			var result = new Consumable(consumable.Id)
			{
				name = consumable.Name,
				amount = consumable.Amount
			};
			return result;
		}

		public DTO.Consumable DTO => new DTO.Consumable(id, name, amount);

		public int Id => id;
		public string Name => name;
		public double Amount => amount;
		public ICollection<Ingredient> Ingredients => ingredients;

		public bool TryUpdateConsumable(DTO.Consumable consumable, out string error)
		{
			this.name = consumable.Name;
			this.amount = consumable.Amount;
			error = null;
			return true;
		}
		public override bool Equals(object obj)
		{
			Consumable another = obj as Consumable;
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
