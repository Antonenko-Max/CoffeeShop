using System;
using CoffeeShop.DesktopClient.ViewModels;
using CoffeeShop.Domain.Model;
using CoffeeShop.Windows;
using Domain.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Position = CoffeeShop.Domain.DTO.Position;
using Size = CoffeeShop.Domain.DTO.Size;

namespace CoffeeShop.DesktopClient.Tests.UnitTests
{
	[TestClass]
	public class SizeDialogTests
	{
		[TestMethod]
		public void IdDoesntChanged()
		{
			var position = new Position(1, "1", "1");
			var size = new Size(1, "1", new Money(1), position);
			var viewModel = new SizeDialogViewModel(size);
			Size returnedSize = null;
			EventHandler<DialogCloseRequestedEventArgs> handler = (sender, e) => returnedSize = (Size)e.Value;
			viewModel.CloseRequested += handler;
			viewModel.OkCommand.Execute(null);

			Assert.AreEqual(size, returnedSize);
		}

		[TestMethod]
		public void PriceCannotBeNegative()
		{
			var viewModel = new SizeDialogViewModelStub {Price = "-1"};

			var msg = viewModel["Price"];

			Assert.AreEqual(msg, "Price cannot be negative");
		}

		[TestMethod]
		public void NameCannotBeEmpty()
		{
			var viewModel = new SizeDialogViewModelStub();

			var msg = viewModel["Name"];

			Assert.AreEqual(msg, "Name cannot be empty");
		}

		[TestMethod]
		public void IsValid()
		{
			var viewModel = new SizeDialogViewModel()
			{
				Name = "Test",
				Price = "10"
			};

			Assert.IsTrue(viewModel.IsValid);
		}

		[TestMethod]
		public void NegativePriceNotAllowed()
		{
			var viewModel = new SizeDialogViewModel()
			{
				Name = "Test",
				Price = "-1"
			};

			Assert.IsFalse(viewModel.IsValid);
		}

		[TestMethod]
		public void CloseCommandTriggersCloseEvent()
		{
			var viewModel = new SizeDialogViewModel();
			bool triggered = false;
			EventHandler< DialogCloseRequestedEventArgs> handler = (sender, e) => triggered = true;

			viewModel.CloseRequested += handler;
			viewModel.OkCommand.Execute(null);

			Assert.IsTrue(triggered);
		}

		[TestMethod]
		public void ReflectionGetsCorrectValue()
		{
			var viewModel = new SizeDialogViewModel()
			{
				Name = "name", Price = "1"
			};

			string name = viewModel.GetType().GetProperty("Name").GetValue(viewModel).ToString();
			string price = viewModel.GetType().GetProperty("Price").GetValue(viewModel).ToString();

			Assert.AreEqual("name", name);
			Assert.AreEqual("1", price);
		}

		private class SizeDialogViewModelStub : SizeDialogViewModel
		{
			public string this[string propertyName]
			{
				get { return OnValidate(propertyName, SizeSpecification.Validate); }
			}

		}

	}
}
