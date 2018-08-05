using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Domain.Model;

namespace Domain.DataAccess
{
	public interface IPositionRepository
	{
		List<Position> LoadPositions();

		List<Consumable> LoadConsumables();

		IMapper Mapper { get; }
	}
}
