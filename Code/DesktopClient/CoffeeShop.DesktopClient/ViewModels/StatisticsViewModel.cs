using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CoffeeShop.Windows;

namespace CoffeeShop.DesktopClient.ViewModels
{
	public class StatisticsViewModel : ITab
	{
		public StatisticsViewModel()
		{
			CloseCommand = new ActionCommand(p => CloseRequested?.Invoke(this, EventArgs.Empty));
		}

		public string Name { get; set; }
		public ICommand CloseCommand { get; }
		public event EventHandler CloseRequested;
	}
}
