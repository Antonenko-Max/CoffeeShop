using System;
using System.Collections.Generic;
using System.Linq;
using CoffeeShop.Data;
using CoffeeShop.DesktopClient.ViewModels;
using CoffeeShop.Domain.Model;
using CoffeeShop.Windows;
using Domain.UseCases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Consumable = CoffeeShop.Domain.DTO.Consumable;
using Position = CoffeeShop.Domain.DTO.Position;
using Size = CoffeeShop.Domain.DTO.Size;

namespace CoffeeShop.DesktopClient.Tests.FunctionalTests
{
	[TestClass]
	public class PositionListViewModelTests
	{
		[TestMethod]
		public void IngredientsUpdates_Works()
		{
			var dataHolder = new PositionListDataHolder(new PositionRepositoryEFStub());
			var facade = new PositionListFacade(new PositionListValidator(new PositionListFeedback(dataHolder)),
				new SizeListValidator(new SizeListFeedback(dataHolder)),
				new IngredientListValidator(new IngredientListFeedback(dataHolder)));

			var viewModel = new PositionListViewModelStub(facade);
			var position = new Position(1, "1", "1");
			var positions = facade.AddPosition(position);
			viewModel.SelectedPosition = positions[0];
			var size = new Size(0, "1", new Money(1), viewModel.SelectedPosition);
			facade.AddSize(size);
			var consumable = new Consumable(1, "1", 0);
			viewModel.DataGridController.AddRow(consumable);
			Assert.IsNotNull(viewModel.SelectedPosition);
			var row = viewModel.DataGridController.IngredientRows.ToList()[0];
			row.Columns["1"] = 1.0;
			row.EndEdit();
			Assert.AreEqual(1, dataHolder.GetIngredientList(dataHolder.GetSizeList(viewModel.SelectedPosition)[0])[0].Amount);
		}
		
		private PositionListFacade CreateFacade()
		{
			var dataHolder = new PositionListDataHolder(new PositionRepositoryEFStub());
			return new PositionListFacade(new PositionListValidator(new PositionListFeedback(dataHolder)),
				new SizeListValidator(new SizeListFeedback(dataHolder)),
				new IngredientListValidator(new IngredientListFeedback(dataHolder)));
		}

		private class PositionListViewModelStub : PositionListViewModel
		{
			public PositionListViewModelStub(PositionListFacade facade) : base(new DialogService(), facade)
			{

			}

			public DataGridController DataGridController => dataGridController;
		}

		private class PositionRepositoryEFStub : PositionRepositoryEF
		{
			public PositionRepositoryEFStub() : base(new Mapper())
			{
			}

			public override List<Domain.Model.Position> LoadPositions()
			{
				return new List<Domain.Model.Position>();
			}

			public override List<Domain.Model.Consumable> LoadConsumables()
			{
				return new List<Domain.Model.Consumable>()
				{
					new ConsumableStub(1, "1"),
					new ConsumableStub(2, "2")
				};
			}
		}
		private class ConsumableStub : Domain.Model.Consumable
		{
			public ConsumableStub(int id, string name) : base(id)
			{
				this.name = name;
			}
		}

	}
}
