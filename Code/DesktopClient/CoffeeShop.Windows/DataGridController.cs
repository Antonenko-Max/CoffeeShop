using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using Consumable = CoffeeShop.Domain.DTO.Consumable;
using Ingredient = CoffeeShop.Domain.DTO.Ingredient;
using Position = CoffeeShop.Domain.DTO.Position;
using Size = CoffeeShop.Domain.DTO.Size;

namespace CoffeeShop.Windows
{
	public class DataGridController
	{
		private readonly PositionListFacade positionList;
		private IngredientDataGridRow selectedRow;

		public DataGridController(PositionListFacade positionList)
		{
			this.positionList = positionList;
			IngredientColumns = new ObservableCollection<DataGridColumn>();
			IngredientRows = new UpdatableObservableCollection<IngredientDataGridRow>();
		}

		public Position Position { get; set; }
		public ICollection<DataGridColumn> IngredientColumns { get; set; }
		public ICollection<IngredientDataGridRow> IngredientRows { get; set; }
		public IngredientDataGridRow SelectedRow {get => selectedRow; set => selectedRow = value; }

		public void Update(List<Size> sizes)
		{
			ClearIngredientList();
			FillDataGrid(sizes);
		}

		protected void ClearIngredientList()
		{
			var rows = IngredientRows.ToList();
			for (int i = rows.Count - 1; i >= 0; i--)
				IngredientRows.Remove(rows[i]);

			var list = IngredientColumns.ToList();
			for (int i = list.Count - 1; i >= 0; i--)
				IngredientColumns.Remove(list[i]);
			IngredientColumns.Add(GetIngredientColumn());
		}

		protected void FillDataGrid(List<Size> sizes)
		{
			FillColumns(sizes);
			FillRows();
		}

		public void AddRow(Consumable consumable)
		{
			if (Position == null) return;
			var sizes = positionList.GetSizeList(Position);
			var row = new IngredientDataGridRow() {Consumable = consumable.Name};
			foreach (var size in sizes) row.Columns.Add(size.Name, 0.0);
			SynchroniseAmounts(row, consumable);
			row.CellEndEdit += CellEndEdit;
			row.CellCancelEdit += CellCancelEdit;
			IngredientRows.Add(row);
		}
		
		public void RemoveRow()
		{
			if (selectedRow == null) return;
			RemoveIngredient(selectedRow);
			IngredientRows.Remove(selectedRow);
		}

		private void RemoveIngredient(IngredientDataGridRow row)
		{
			var sizes = positionList.GetSizeList(Position);
			foreach (var size in sizes)
			{
				if (positionList.GetIngredientList(size).Any(p => p.Consumable.Name == row.Consumable))
				{
					var ingredient = positionList.GetIngredientList(size).First(p => p.Consumable.Name == row.Consumable);
					positionList.DeleteIngredient(ingredient);
				}
			}
		}

		private void AddIngredient(IngredientDataGridRow row, Consumable consumable)
		{
			if (Position == null) return;
			var sizes = positionList.GetSizeList(Position);
			foreach (var size in sizes)
			{
				var ingredient = new Ingredient(0, (double)row.Columns[size.Name], size, consumable);
				positionList.AddIngredient(ingredient);
			}
		}

		private void CellCancelEdit()
		{
			((UpdatableObservableCollection<IngredientDataGridRow>)IngredientRows).UpdateCollection();
		}

		protected void CellEndEdit(IEditableObject sender)
		{
			var row = sender as IngredientDataGridRow;
			if (positionList.Consumables.Any(p => p.Name == row.Consumable))
			{
				var consumable = positionList.Consumables.First(p => p.Name == row.Consumable);
				if (row != null) UpdateAmounts(row, consumable);
			}
		}

		private void UpdateAmounts(IngredientDataGridRow row, Consumable consumable)
		{
			if (Position == null) return;
			var sizes = positionList.GetSizeList(Position);
			foreach (var size in sizes)
			{
				if (positionList.GetIngredientList(size).Any(p => p.Consumable.Name == row.Consumable))
				{
					var ingredient = positionList.GetIngredientList(size).First(p => p.Consumable.Name == row.Consumable);
					positionList.UpdateIngredient(new Ingredient(ingredient.Id, (double)row.Columns[size.Name], ingredient.Size, ingredient.Consumable));
				}
			}
		}

		protected void SynchroniseAmounts(IngredientDataGridRow row, Consumable consumable)
		{
			if (Position == null) return;
			if (IngredientIsNew(consumable))
			{
				AddIngredient(row, consumable);
			}
			else
			{
				var sizes = positionList.GetSizeList(Position);
				foreach (var size in sizes)
				{
					row.Columns[size.Name] = positionList.GetIngredientList(size).First(p => p.Consumable.Name == row.Consumable).Amount;
				}
			}
		}

		private bool IngredientIsNew(Consumable consumable)
		{
			var sizes = positionList.GetSizeList(Position);
			return positionList.GetIngredientList(sizes[0]).All(p => p.Consumable != consumable);
		}

		private DataGridTextColumn GetIngredientColumn()
		{
			var column = new DataGridTextColumn() { Header = "Ingredients" };
			column.Binding = new Binding("Consumable");
			column.IsReadOnly = true;
			return column;
		}

		private void FillColumns(List<Size> sizes)
		{
			foreach (var size in sizes)
			{
				var column = new DataGridTextColumn() { Header = size.Name };
				column.Binding = new Binding(size.Name);
				IngredientColumns.Add(column);
			}
		}

		private void FillRows()
		{
			foreach (Ingredient ingredient in positionList.GetIngredientListDefaultSize(Position))
			{
				AddRow(ingredient.Consumable);
			}
		}
	}

}
