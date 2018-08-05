using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Windows
{
	public class UpdatableObservableCollection<T> : ObservableCollection<T>
	{
		public UpdatableObservableCollection() : base()
		{
			
		}

		public void UpdateCollection()
		{
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}
	}
}
