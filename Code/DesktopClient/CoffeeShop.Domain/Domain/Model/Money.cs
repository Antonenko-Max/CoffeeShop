using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Domain.Model
{
	public enum Currency
	{
		Руб,
		Usd
	}

	public class Money
	{
		private Currency currency = Currency.Руб;

		public Currency Currency { get { return currency; } set { currency = value; } }
		public decimal Amount { get; set; }

		public Money()
		{
		}

		public Money(decimal amount)
		{
			Amount = amount;
		}

		public override string ToString()
		{
			return string.Format("{0:0.00}", Amount) + $" {currency}.";
		}
	}
}
