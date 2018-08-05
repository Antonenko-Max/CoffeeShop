using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using Domain.Settings;

namespace CoffeeShop.Windows
{
	public class IngredientDataGridRow : DynamicObject, IEditableObject
	{
		private Dictionary<string, object> columns = new Dictionary<string, object>();
		public Dictionary<string, object> Columns
		{
			get => columns;
			set => columns = value;
		}

		public string Consumable { get; set; }

		public override bool TryGetMember(
			GetMemberBinder binder, out object result)
		{
			return columns.TryGetValue(binder.Name, out result);
		}

		public override bool TrySetMember(
			SetMemberBinder binder, object value)
		{
			try
			{
				columns[binder.Name.ToLower()] = Double.Parse(value.ToString());
			}
			catch (FormatException)
			{
				CellCancelEdit?.Invoke();
				return false;
			}
			return true;
		}

		public delegate void CellEndEditEventHandler(IEditableObject sender);
		public delegate void CellCancelEditEventHandler();

		public event CellEndEditEventHandler CellEndEdit;
		public event CellCancelEditEventHandler CellCancelEdit;

		public void BeginEdit()
		{
		}

		public void EndEdit()
		{
			CellEndEdit?.Invoke(this);
		}

		public void CancelEdit()
		{
			CellCancelEdit?.Invoke();
		}
	}
}
