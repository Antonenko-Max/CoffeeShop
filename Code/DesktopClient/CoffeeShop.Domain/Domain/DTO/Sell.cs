using System;
using CoffeeShop.Domain.Model;

namespace CoffeeShop.Domain.DTO
{
	public class Sell
	{
		public Sell(DateTime dateTime, Money price, Position position, Size size)
		{
			//this.id = id;
			this.DateTime = dateTime;
			this.Price = price;
			this.Position = position;
			this.Size = size;
		}

		//public int Id { get;  }
		public DateTime DateTime { get; }
		public Money Price { get; }
		public Position Position { get; }
		public Size Size { get; }
	}
}
