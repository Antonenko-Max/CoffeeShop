using CoffeeShop.Windows;
using Domain.Settings;
using Position = CoffeeShop.Domain.DTO.Position;

namespace CoffeeShop.DesktopClient.ViewModels
{
	public class PositionDialogViewModel : DialogViewModel
	{
		protected int id;
		protected string name;
		protected string category;

		public PositionDialogViewModel() : this(null)
		{
		}

		public PositionDialogViewModel(Position position)
		{
			InitializeCommands();
			if (position != null)
			{
				this.id = position.Id;
				this.name = position.Name;
				this.category = position.Category;
			}
			else id = 0;
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

		public string Category
		{
			get { return category; }
			set
			{
				category = value;
				NotifyPropertyChanged();
				NotifyPropertyChanged("IsValid");
			}
		}

		public bool IsValid
		{
			get
			{
				foreach (var property in PositionSpecification.ValidateProperties)
					if (OnValidate(property, PositionSpecification.Validate) != null)
						return false;
				return true;
			}
		}


		protected override void OnOkClick()
		{
			OnConfirm(new Position(id, name, category));
		}
	}
}
