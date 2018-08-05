using System;
using System.Collections.Generic;
using DTOPosition = CoffeeShop.Domain.DTO.Position;
using DTOSize = CoffeeShop.Domain.DTO.Size;
using Size = CoffeeShop.Domain.Model.Size;

namespace Domain.UseCases
{
	public class SizeListValidator : ISizeList
	{
		private readonly ISizeList sizeListFeedback;


		public event EventHandler<SizeListChangedEventArgs> SizeListChanged;

		public SizeListValidator(ISizeList sizeListFeedback)
		{
			this.sizeListFeedback = sizeListFeedback;
			sizeListFeedback.SizeListChanged += OnSizeListChange;
		}

		public List<DTOSize> GetSizeList(DTOPosition position)
		{
			return sizeListFeedback.GetSizeList(position);
		}

		public void AddSize(DTOSize size)
		{
			string error;
			if (Size.DoesSizeNameExist(size, GetSizeList(size.Position), out error))
			{
				ThrowError(size, error);
				return;
			}
			if (Size.CountIsMaximum(GetSizeList(size.Position).Count, out error))
			{
				ThrowError(size, error);
				return;
			}
			sizeListFeedback.AddSize(size);
		}

		public void UpdateSize(DTOSize size)
		{
			string error;
			if (!Size.DoesSizeExist(size, GetSizeList(size.Position), out error))
			{
				ThrowError(size, error);
				return;
			}
			if (Size.DoesSizeNameExist(size, GetSizeList(size.Position), out error))
			{
				ThrowError(size, error);
				return;
			}
			sizeListFeedback.UpdateSize(size);
		}

		public void DeleteSize(DTOSize size)
		{
			string error;
			if (!Size.DoesSizeExist(size, GetSizeList(size.Position), out error))
			{
				ThrowError(size, error);
				return;
			}
			sizeListFeedback.DeleteSize(size);
		}

		private void ThrowError(DTOSize size, string error)
		{
			SizeListChanged?.Invoke(this, new SizeListChangedEventArgs(GetSizeList(size.Position), error));
		}

		private void OnSizeListChange(object sender, SizeListChangedEventArgs e)
		{
			SizeListChanged?.Invoke(this, new SizeListChangedEventArgs(e.Sizes, e.Error));
		}
	}
}
