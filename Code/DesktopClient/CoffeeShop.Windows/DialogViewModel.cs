using System;
using System.Windows.Input;

namespace CoffeeShop.Windows
{
	public class DialogViewModel : ObservableObject, IDialogRequestClose
	{
		public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

		public ICommand OkCommand { get; private set; }
		public ICommand CancelCommand { get; private set; }

		protected void InitializeCommands()
		{
			OkCommand = new ActionCommand(p => OnOkClick());
			CancelCommand = new ActionCommand(p => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false, null)));
		}

		protected virtual void OnOkClick()
		{
			OnConfirm(null);
		}

		protected void OnConfirm(object result)
		{
			CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true, result));
		}


		protected virtual string OnValidate(string propertyName, Func<string, string, string> validateFunc)
		{
			string value = this.GetType().GetProperty(propertyName).GetValue(this) as string;
			return validateFunc(propertyName, value);
		}

	}
}
