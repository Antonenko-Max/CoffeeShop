using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoffeeShop.Windows;

namespace CoffeeShop.Windows.Tests.UnitTests
{
	[TestClass]
	public class ObservableObjectTests
	{
		[TestMethod]
		public void PropertyChangedEventHandlerIsRaised()
		{
			var obj = new StabObservableObject();

			bool raised = false;

			obj.PropertyChanged += (sender, e) =>
			{
				Assert.IsTrue(e.PropertyName == "ChangedProperty");
				raised = true;
			};

			obj.ChangedProperty = "Some Value";

			if (!raised) Assert.Fail("PropertyChanged was never inviked.");

		}

		class StabObservableObject : ObservableObject
		{
			private string changedProperty;

			public string ChangedProperty
			{
				get { return changedProperty; }
				set
				{
					changedProperty = value;
					NotifyPropertyChanged();
				}
			}
		}
	}
}
