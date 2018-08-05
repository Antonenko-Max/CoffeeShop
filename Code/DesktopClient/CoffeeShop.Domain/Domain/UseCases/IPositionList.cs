using System;
using System.Collections.Generic;
using DTOPosition = CoffeeShop.Domain.DTO.Position;
using DTOConsumable = CoffeeShop.Domain.DTO.Consumable;

namespace Domain.UseCases
{
	public interface IPositionList
	{
		event EventHandler<PositionListChangedEventArgs> PositionListChanged;

		List<DTOPosition> Positions { get; }
		List<DTOConsumable> Consumables { get; }

		void LoadPositionList();

		void AddPosition(DTOPosition position);
		void UpdatePosition(DTOPosition position);
		void DeletePosition(DTOPosition position);
	}

	public class PositionListChangedEventArgs : EventArgs
	{
		private readonly List<DTOPosition> positions;

		public PositionListChangedEventArgs(ICollection<DTOPosition> positions) : this(positions, null)
		{
		}

		public PositionListChangedEventArgs(ICollection<DTOPosition> positions, string error)
		{
			this.positions = positions as List<DTOPosition>;
			if (this.positions == null && positions != null)
			{
				this.positions = new List<DTOPosition>();
				foreach (var position in positions)
				{
					this.positions.Add(position);
				}
			}
			Error = error;
		}

		public List<DTOPosition> Positions => positions;
		public string Error { get; set; }
	}
}
