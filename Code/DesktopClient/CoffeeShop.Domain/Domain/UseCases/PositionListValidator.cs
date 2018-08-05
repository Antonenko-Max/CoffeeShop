using System;
using System.Collections.Generic;
using CoffeeShop.Domain.Model;
using Consumable = CoffeeShop.Domain.DTO.Consumable;
using Position = CoffeeShop.Domain.Model.Position;
using DTOPosition = CoffeeShop.Domain.DTO.Position;

namespace Domain.UseCases
{
	public class PositionListValidator : IPositionList
	{
		protected readonly IPositionList positionListFeedBack;
		protected List<Position> positions = new List<Position>();
		protected List<Size> sizes = new List<Size>();
		protected List<Ingredient> ingredients = new List<Ingredient>();

		public PositionListValidator(IPositionList positionListFeedback)
		{
			this.positionListFeedBack = positionListFeedback;
			positionListFeedback.PositionListChanged += OnPositionListChange;
		}

		public event EventHandler<PositionListChangedEventArgs> PositionListChanged;

		public List<DTOPosition> Positions => positionListFeedBack.Positions;
		public List<Consumable> Consumables => positionListFeedBack.Consumables;

		public void LoadPositionList()
		{
			positionListFeedBack.LoadPositionList();
		}

		public void AddPosition(DTOPosition position)
		{
			string error;
			if (Position.DoesPositionNameExist(position, Positions, out error))
			{
				ThrowError(error);
				return;
			}
			positionListFeedBack.AddPosition(position);
		}

		public void UpdatePosition(DTOPosition position)
		{
			string error;
			if (!Position.DoesPositionExist(position, Positions, out error))
			{
				ThrowError(error);
				return;
			}
			if (Position.DoesPositionNameExist(position, Positions, out error))
			{
				ThrowError(error);
				return;
			}
			positionListFeedBack.UpdatePosition(position);
		}

		public void DeletePosition(DTOPosition position)
		{
			string error;
			if (!Position.DoesPositionExist(position, Positions, out error))
			{
				ThrowError(error);
				return;
			}
			positionListFeedBack.DeletePosition(position);
		}

		private void ThrowError(string error)
		{
			PositionListChanged?.Invoke(this, new PositionListChangedEventArgs(Positions, error));
		}

		private void OnPositionListChange(object sender, PositionListChangedEventArgs e)
		{
			PositionListChanged?.Invoke(this, new PositionListChangedEventArgs(e.Positions, e.Error));
		}
	}
}
