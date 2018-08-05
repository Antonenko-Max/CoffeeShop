using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Domain.Model;

namespace Domain.Model
{
	public class NullPosition : Position
	{
		public NullPosition() : base(0)
		{
		}

		//public override bool TryUpdatePosition(CoffeeShop.Domain.DTO.Position position, out string error)
		//{
		//	error = "This position doesnt exist.";
		//	return false;
		//}

	}
}
