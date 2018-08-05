using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CoffeeShop.Windows
{
	public class DialogService : IDialogService
	{
		//private readonly Window owner;

		public IDictionary<Type, Type> Mappings { get; }

		//		public DialogService(Window owner)
		public DialogService()
		{
			//this.owner = owner;
			Mappings = new Dictionary<Type, Type>();
		}

		public void Register<TViewModel, TView>() where TViewModel : IDialogRequestClose where TView : IDialog
		{
			if (Mappings.ContainsKey(typeof(TViewModel)))
			{
				throw new ArgumentException($"Type {typeof(TViewModel)} is already mapped to type {typeof(TView)}");
			}

			Mappings.Add(typeof(TViewModel), typeof(TView));
		}

		public DialogResult ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogRequestClose
		{
			Type viewType = Mappings[typeof(TViewModel)];
			var dialogResult = new DialogResult();

			IDialog dialog = (IDialog)Activator.CreateInstance(viewType);

			EventHandler<DialogCloseRequestedEventArgs> handler = null;

			handler = (sender, e) =>
			{
				viewModel.CloseRequested -= handler;

				if (e.DialogResult.HasValue)
				{
					dialog.DialogResult = e.DialogResult;
					dialogResult.Value = e.Value;
				}
				else
				{
					dialog.Close();
				}
			};

			viewModel.CloseRequested += handler;
			dialog.DataContext = viewModel;
			//dialog.Owner = owner;

			dialogResult.Result = dialog.ShowDialog();

			return dialogResult;
		}
	}
}
