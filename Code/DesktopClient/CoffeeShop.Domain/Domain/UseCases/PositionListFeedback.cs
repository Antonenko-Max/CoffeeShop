using System;
using System.Collections.Generic;
using CoffeeShop.Domain.DTO;

namespace Domain.UseCases
{
	public class PositionListFeedback : IPositionList
	{
		private readonly IPositionListDataHolder positionListDataHolder;
		public event EventHandler<PositionListChangedEventArgs> PositionListChanged;
		public List<Position> Positions => positionListDataHolder.Positions;
		public List<Consumable> Consumables => positionListDataHolder.Consumables;

		public PositionListFeedback(IPositionListDataHolder positionListDataHolder)
		{
			this.positionListDataHolder = positionListDataHolder;
		}

		public void LoadPositionList()
		{
			positionListDataHolder.LoadPositionList();
			InvokeEvent();
		}

		public void AddPosition(Position position)
		{
			positionListDataHolder.AddPosition(position);
			InvokeEvent();
		}

		public void UpdatePosition(Position position)
		{
			positionListDataHolder.UpdatePosition(position);
			InvokeEvent();
		}

		public void DeletePosition(Position position)
		{
			positionListDataHolder.DeletePosition(position);
			InvokeEvent();
		}

		private void InvokeEvent()
		{
			PositionListChanged?.Invoke(this, new PositionListChangedEventArgs(Positions, null));
		}
	}
}
