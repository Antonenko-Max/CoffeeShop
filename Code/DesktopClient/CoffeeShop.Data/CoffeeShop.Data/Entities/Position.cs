using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Data.Entities;

namespace CoffeeShop.Data
{
	class Position
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Category { get; set; }

		public ICollection<Size> Sizes { get; set; }
		public ICollection<Ingredient> Ingredients { get; set; }
		public ICollection<Sell> Sells { get; set; }
	}
}
