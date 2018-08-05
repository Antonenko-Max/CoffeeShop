using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeShop.Domain.Tests.UnitTests
{
	[TestClass]
	public class DomainModelTests
	{
		[TestMethod]
		public void MoneyFormat()
		{
			Money money = new Money() { Amount = 100 };
			Assert.AreEqual(money.ToString(), "100,00 Руб.");
		}

	}
}
