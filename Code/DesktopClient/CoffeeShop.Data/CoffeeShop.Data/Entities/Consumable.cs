using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Data.Entities
{
	class Consumable
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public double Amount { get; set; }

		public ICollection<Ingredient> Ingredients { get; set; }

	}
}
