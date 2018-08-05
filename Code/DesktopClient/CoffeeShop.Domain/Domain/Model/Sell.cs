using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Domain.Model
{
	public class Sell
	{
		public int Id { get; set; }
		public int PositionId { get; set; }
		public int PositionSizeId { get; set; }
		public decimal Price { get; set; }

		public Position Position { get; set; }
		public Size Size { get; set; }
	}
}
