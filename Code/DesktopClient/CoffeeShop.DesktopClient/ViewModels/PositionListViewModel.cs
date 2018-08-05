using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using CoffeeShop.Domain.DTO;
using CoffeeShop.Windows;
using Ingredient = CoffeeShop.Domain.DTO.Ingredient;
using Position = CoffeeShop.Domain.DTO.Position;
using Size = CoffeeShop.Domain.DTO.Size;

namespace CoffeeShop.DesktopClient.ViewModels
{
	public class PositionListViewModel : ViewModel, ITab
	{
		private Position selectedPosition;
		private Size selectedSize;
		private readonly IDialogService dialogService;
		private readonly PositionListFacade positionList;
		protected readonly DataGridController dataGridController;

		public PositionListViewModel(IDialogService dialogService, PositionListFacade positionList)
		{
			this.dialogService = dialogService;
			this.positionList = positionList;
			positionList.LoadPositionList();
			Name = "Position List";

			#region Command Initialization
			CloseCommand = new ActionCommand(p => CloseRequested?.Invoke(this, EventArgs.Empty));
			AddPositionCommand = new ActionCommand(p => AddPosition());
			DeletePositionCommand = new ActionCommand(p => DeletePosition());
			EditPositionCommand = new ActionCommand(p => EditPosition());
			AddSizeCommand = new ActionCommand(p => AddSize());
			DeleteSizeCommand = new ActionCommand(p => DeleteSize());
			EditSizeCommand = new ActionCommand(p => EditSize());
			AddIngredientCommand = new ActionCommand(p => AddIngredient());
			DeleteIngredientCommand = new ActionCommand(p => DeleteIngredient());
			#endregion

			dataGridController = new DataGridController(positionList);

			#region Observable Collections Initialization
			Positions = new ObservableCollection<Position>();
			Sizes = new ObservableCollection<Size>();
			#endregion
		}
		
		public event EventHandler CloseRequested;
		public string Name { get; set; }

		#region Command Definition
		public ICommand CloseCommand { get; }
		public ICommand AddPositionCommand { get; }
		public ICommand DeletePositionCommand { get; }
		public ICommand EditPositionCommand { get; }
		public ICommand AddSizeCommand { get; }
		public ICommand DeleteSizeCommand { get; }
		public ICommand EditSizeCommand { get; }
		public ICommand AddIngredientCommand { get; }
		public ICommand DeleteIngredientCommand { get; }
		public ICommand EditIngredientCommand { get; }
		#endregion

		#region Properties
		public ICollection<Position> Positions { get; set; }
		public ICollection<Size> Sizes { get; set; }

		public bool CanModify
		{
			get { return SelectedPosition != null; }
		}
		public Position SelectedPosition
		{
			get { return selectedPosition; }
			set
			{
				selectedPosition = value;
				dataGridController.Position = value;
				NotifyPropertyChanged();
				NotifyPropertyChanged("PositionSelected");
				NotifyPropertyChanged("CanModify");
				NotifyPropertyChanged("IsAddingSizeEnabled");
				UpdateSizeListAndDataGrid(positionList.GetSizeList(SelectedPosition));
			}
		}
		public Size SelectedSize
		{
			get { return selectedSize; }
			set
			{
				selectedSize = value;
				NotifyPropertyChanged();
				NotifyPropertyChanged("SizeSelected");
				NotifyPropertyChanged("CanModify");
			}
		}
		public IngredientDataGridRow SelectedRow
		{
			get => dataGridController.SelectedRow;
			set
			{
				dataGridController.SelectedRow = value;
				NotifyPropertyChanged();
				NotifyPropertyChanged("IngredientSelected");
				NotifyPropertyChanged("CanModify");
			}
		}

		public DataGridController DataGridController => dataGridController;

		public bool PositionSelected => SelectedPosition != null;
		public bool SizeSelected => SelectedSize != null;
		public bool IngredientSelected => SelectedRow != null;
		public bool IsValid { get; }
		public bool IsAddingSizeEnabled => (PositionSelected && !positionList.IsMaximumCountOfSizes(SelectedPosition));
		#endregion

		#region List Update Utilities

		private void UpdatePositionList(List<Position> positions)
		{
			Positions.Clear();
			positions.ForEach(p => Positions.Add(p));
		}

		private void UpdateSizeListAndDataGrid(List<Size> sizes)
		{
			Sizes.Clear();
			dataGridController.Update(sizes);
			FillSizeList(sizes);
		}


		private void FillSizeList(List<Size> sizes)
		{
			foreach (var size in sizes) Sizes.Add(size);
		}
		#endregion

		#region Position Utilities
		private void AddPosition()
		{
			var position = ShowPositionDialog(null);
			if (position == null) return;
			UpdatePositionList(positionList.AddPosition(position));
			SelectedPosition = Positions.FirstOrDefault(p => p.Name == position.Name);
		}

		private void DeletePosition()
		{
			var position = SelectedPosition;
			if (position == null) return;
			UpdatePositionList(positionList.DeletePosition(position));
		}

		private void EditPosition()
		{
			var position = ShowPositionDialog(SelectedPosition);
			if (position == null) return;
			UpdatePositionList(positionList.UpdatePosition(position));
			SelectedPosition = Positions.FirstOrDefault(p => p.Name == position.Name);
		}
		#endregion

		#region Size Utilities
		private void AddSize()
		{
			var size = ShowSizeDialog(null);
			if (size == null) return;
			UpdateSizeListAndDataGrid(positionList.AddSize(size));
			SelectedSize = Sizes.FirstOrDefault(s => s.Name == size.Name);
			NotifyPropertyChanged("IsAddingSizeEnabled");
		}

		private void EditSize()
		{
			var size = ShowSizeDialog(SelectedSize);
			if (size == null) return;
			UpdateSizeListAndDataGrid(positionList.UpdateSize(size));
			SelectedSize = Sizes.FirstOrDefault(s => s.Name == size.Name);
		}

		private void DeleteSize()
		{
			var size = SelectedSize;
			if (size == null) return;
			UpdateSizeListAndDataGrid(positionList.DeleteSize(size));
			NotifyPropertyChanged("IsAddingSizeEnabled");
		}


		#endregion

		#region Ingredient Utilities
		private void AddIngredient()
		{
			var consumable = ShowIngredientDialog();
			dataGridController.AddRow(consumable);
		}

		private void DeleteIngredient()
		{
			dataGridController.RemoveRow();
		}
		#endregion

		#region Dialog Utilities
		private Position ShowPositionDialog(Position position)
		{
			var viewModel = new PositionDialogViewModel(position);
			var result = dialogService.ShowDialog(viewModel);
			Position returnedPosition = null;
			if (result.Result.HasValue)
				if (result.Result.Value)
					returnedPosition = (Position)result.Value;
			return returnedPosition;
		}

		private Size ShowSizeDialog(Size size)
		{
			var viewModel = new SizeDialogViewModel(size);
			var result = dialogService.ShowDialog(viewModel);
			Size returnedSize = null;
			if (result.Result.HasValue)
				if (result.Result.Value)
				{
					returnedSize = (Size) result.Value;
					return new Size(0, returnedSize.Name, returnedSize.Price, SelectedPosition);
				}
			return null;
		}

		private Consumable ShowIngredientDialog()
		{

			var consumables = positionList.Consumables;

			var viewModel = new IngredientDialogViewModel(consumables);
			var result = dialogService.ShowDialog(viewModel);
			Consumable consumable = null;
			if (result.Result.HasValue)
				if (result.Result.Value)
					return (Consumable)result.Value;
			return null;
		}
		#endregion
	}
}
