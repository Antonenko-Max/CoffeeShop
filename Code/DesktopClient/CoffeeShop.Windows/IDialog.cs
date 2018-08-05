using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CoffeeShop.Windows
{
	public interface IDialog
	{
		object DataContext { get; set; }
		bool? DialogResult { get; set; }
		Window Owner { get; set; }
		bool? ShowDialog();
		void Close();
	}

	public interface IDialogService
	{
		void Register<TViewModel, TView>() where TViewModel : IDialogRequestClose
			where TView : IDialog;

		DialogResult ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogRequestClose;
	}

	public interface IDialogRequestClose
	{
		event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;
	}

	public class DialogCloseRequestedEventArgs : EventArgs
	{
		public object Value { get; }
		public bool? DialogResult { get; }

		public DialogCloseRequestedEventArgs(bool? dialogResult, object value)
		{
			DialogResult = dialogResult;
			Value = value;
		}

	}

	public class DialogResult
	{
		public bool? Result { get; set; }
		public object Value { get; set; }
	}
}
