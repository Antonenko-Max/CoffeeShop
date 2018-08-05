using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Data.Entities;

namespace CoffeeShop.Data
{
	class Context : DbContext
	{
		public Context() : base("SQL Server"){}

		public DbSet<Position> Positions { get; set; }
		public DbSet<Size> Sizes { get; set; }
		public DbSet<Ingredient> Ingredients { get; set; }
		public DbSet<Consumable> Consumables { get; set; }
		public DbSet<Sell> Sells { get; set; }
	}
}
