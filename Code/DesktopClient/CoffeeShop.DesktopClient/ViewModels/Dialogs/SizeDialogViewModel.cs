using System;
using System.Windows.Input;
using CoffeeShop.Domain.Model;
using CoffeeShop.Windows;
using Domain.Settings;
using Size = CoffeeShop.Domain.DTO.Size;
using Position = CoffeeShop.Domain.DTO.Position;

namespace CoffeeShop.DesktopClient.ViewModels
{
	public class SizeDialogViewModel : DialogViewModel
	{
		protected int id;
		protected string name;
		protected string price = "0";
		protected Position position;

		public SizeDialogViewModel() : this(null)
		{
		}

		public SizeDialogViewModel(Size size)
		{
			InitializeCommands();
			if (size != null)
			{
				this.id = size.Id;
				this.name = size.Name;
				this.price = size.Price.Amount.ToString();
				this.position = size.Position;
			}
			else
			{
				id = 0;
				position = null;
			}
		}

	    public string Name
		{
			get { return name; }
			set
			{
				name = value;
				NotifyPropertyChanged();
				NotifyPropertyChanged("IsValid");
			}
		}

		public string Price
		{
			get { return price; }
			set
			{
				price = value;
				NotifyPropertyChanged();
				NotifyPropertyChanged("IsValid");
			}
		}

		public bool IsValid
		{
			get
			{
				foreach (var property in SizeSpecification.ValidateProperties)
					if (OnValidate(property, SizeSpecification.Validate) != null)
						return false;
				return true;
			}
		}


		protected override void OnOkClick()
		{
			Money price = new Money(decimal.Parse(Price));
			Size size = new Size(id, Name, price, position);
			OnConfirm(size);
		}
	}
}
