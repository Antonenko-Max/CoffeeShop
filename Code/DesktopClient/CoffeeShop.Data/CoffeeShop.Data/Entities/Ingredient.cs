using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Data.Entities
{
	class Ingredient
	{
		public int Id { get; set; }
		public int PositionId { get; set; }
		public int PositionSizeId { get; set; }
		public int ConsumableId { get; set; }
		public double Amount { get; set; }

		public Position Position { get; set; }
		public Size Size { get; set; }
		public Consumable Consumable { get; set; }
	}
}
