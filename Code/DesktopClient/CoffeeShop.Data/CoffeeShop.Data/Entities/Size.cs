using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Data.Entities
{
	class Size
	{
		public int Id { get; set; }
		public int PositionId { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }

		public Position Position { get; set; }
	}
}
