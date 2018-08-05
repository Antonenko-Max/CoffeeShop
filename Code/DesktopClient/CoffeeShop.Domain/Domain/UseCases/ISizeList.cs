using System;
using System.Collections.Generic;
using DTOPosition = CoffeeShop.Domain.DTO.Position;
using DTOSize = CoffeeShop.Domain.DTO.Size;
using DTOIngredient = CoffeeShop.Domain.DTO.Ingredient;
using Position = CoffeeShop.Domain.Model.Position;

namespace Domain.UseCases
{
	public interface ISizeList
	{
		event EventHandler<SizeListChangedEventArgs> SizeListChanged;

		List<DTOSize> GetSizeList(DTOPosition position);

		void AddSize(DTOSize size);
		void UpdateSize(DTOSize size);
		void DeleteSize(DTOSize size);
	}

	public class SizeListChangedEventArgs : EventArgs
	{
		private readonly List<DTOSize> sizes;

		public SizeListChangedEventArgs(ICollection<DTOSize> sizes) : this(sizes, null)
		{
		}

		public SizeListChangedEventArgs(ICollection<DTOSize> sizes, string error)
		{
			this.sizes = sizes as List<DTOSize>;
			if (this.sizes == null)
			{
				this.sizes = new List<DTOSize>();
				foreach (var size in sizes)
				{
					this.sizes.Add(size);
				}
			}
			Error = error;
		}

		public List<DTOSize> Sizes => sizes;
		public string Error { get; set; }
	}
}
