using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CoffeeShop.Windows;

namespace CoffeeShop.DesktopClient.ViewModels
{
	public class  ConnectionViewModel : IDialogRequestClose
	{
		public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

		public ICommand OkCommand { get; }
		public ICommand CanselCommand { get; }

		public string DataSource { get; set; }
		public string InitialCatalog { get; set; }

		public ConnectionViewModel()
		{
			OkCommand = new ActionCommand(p => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true, null)));
			CanselCommand = new ActionCommand(p => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false, null)));
		}
	}
}
