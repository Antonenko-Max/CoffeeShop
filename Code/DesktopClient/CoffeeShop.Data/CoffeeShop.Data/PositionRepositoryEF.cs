using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CoffeeShop.Data.Entities;
using Domain.DataAccess;

namespace CoffeeShop.Data
{

	public class PositionRepositoryEF : IPositionRepository
	{
		protected readonly IMapper mapper;
		protected readonly DbContext context;

		public PositionRepositoryEF(IMapper mapper)
		{
			this.mapper = mapper;
		}

		private Context GetContext()
		{
			return new Context();
		}

		// virtual for test's means
		public virtual List<Domain.Model.Consumable> LoadConsumables()
		{
			// for test
			var water = Domain.Model.Consumable.Create(new Domain.DTO.Consumable(1, "water", 0));
			var sugar = Domain.Model.Consumable.Create(new Domain.DTO.Consumable(2, "sugar", 0));
			return new List<Domain.Model.Consumable>() {water, sugar};

			//using (Context context = GetContext())
			//{
			//	var consumables = context.Set<Consumable>().ToList();
			//	return GetDomainConsumableList(consumables);
			//}
		}

		public IMapper Mapper => mapper;

		// virtual for test's means
		public virtual List<Domain.Model.Position> LoadPositions()
		{
			return new List<Domain.Model.Position>();
			//using (Context context = GetContext())
			//{
			//	var positions = context.Set<Position>().ToList();
			//	var consumables = context.Set<Consumable>().ToList();
			//	return mapper.GetDomainPositionList(GetDTOPositionList(positions), GetDomainConsumableList(consumables));
			//}
		}

		private List<Domain.DTO.Position> GetDTOPositionList(ICollection<Position> positions)
		{
			throw new NotImplementedException();
		}

		private List<Domain.Model.Consumable> GetDomainConsumableList(ICollection<Consumable> consumables)
		{
			throw  new NotImplementedException();
		}
	}
}
