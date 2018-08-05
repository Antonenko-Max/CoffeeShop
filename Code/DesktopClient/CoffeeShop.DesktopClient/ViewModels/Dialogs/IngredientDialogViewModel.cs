using System;
using System.Collections.Generic;
using System.Linq;
using CoffeeShop.Domain.DTO;
using CoffeeShop.Windows;

namespace CoffeeShop.DesktopClient.ViewModels
{
	public class IngredientDialogViewModel : DialogViewModel
	{
		protected Consumable consumable;
		protected List<Consumable> consumables;

		public IngredientDialogViewModel(ICollection<Consumable> consumables)
		{
			InitializeCommands();
			this.consumables = consumables.ToList();
			ConsumableNames = consumables.Select(i => i.Name).ToList();
		}

		public List<string> ConsumableNames { get; set; }

		public string ConsumableName
		{
			get { return consumable?.Name; }
			set
			{
				consumable = consumables.Find(p => p.Name == value);
				NotifyPropertyChanged();
				NotifyPropertyChanged("IsValid");
			}
		}

		public bool IsValid
		{
			get { return consumable != null; }
		}


		protected override void OnOkClick()
		{
			OnConfirm(consumable);
		}

	}
}
