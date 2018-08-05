using System;
using System.Collections.Generic;
using CoffeeShop.Domain.DTO;

namespace Domain.UseCases
{
	public class SizeListFeedback : ISizeList
	{
		private readonly IPositionListDataHolder positionListDataHolder;
		public event EventHandler<SizeListChangedEventArgs> SizeListChanged;

		public SizeListFeedback(IPositionListDataHolder positionListDataHolder)
		{
			this.positionListDataHolder = positionListDataHolder;
		}

		public List<Size> GetSizeList(Position position)
		{
			return positionListDataHolder.GetSizeList(position);
		}

		public void AddSize(Size size)
		{
			positionListDataHolder.AddSize(size);
			InvokeEvent(size.Position);
		}

		public void UpdateSize(Size size)
		{
			positionListDataHolder.UpdateSize(size);
			InvokeEvent(size.Position);
		}

		public void DeleteSize(Size size)
		{
			positionListDataHolder.DeleteSize(size);
			InvokeEvent(size.Position);
		}
		private void InvokeEvent(Position position)
		{
			SizeListChanged?.Invoke(this, new SizeListChangedEventArgs(GetSizeList(position), null));
		}

	}
}
