using System;
using CoffeeShop.DesktopClient.ViewModels;
using CoffeeShop.Domain.DTO;
using CoffeeShop.Windows;
using Domain.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeShop.DesktopClient.Tests.UnitTests
{
	[TestClass]
	public class PositionDialogTests
	{
		[TestMethod]
		public void IdDoesntChanged()
		{
			var position = new Position(1, "1", "1");
			var viewModel = new PositionDialogViewModel(position);
			Position returnedPosition = null;
			EventHandler<DialogCloseRequestedEventArgs> handler = (sender, e) => returnedPosition = (Position)e.Value;
			viewModel.CloseRequested += handler;
			viewModel.OkCommand.Execute(null);

			Assert.AreEqual(position, returnedPosition);
		}

		[TestMethod]
		public void NameCannotBeEmpty()
		{
			var viewModel = new PositionDialogViewModelStub();

			var msg = viewModel["Name"];

			Assert.AreEqual("Name cannot be empty", msg);
		}

		[TestMethod]
		public void IsValid()
		{
			var viewModel = new PositionDialogViewModel()
			{
				Name = "Test",
				Category = "Test"
			};

			Assert.IsTrue(viewModel.IsValid);
		}

		[TestMethod]
		public void CloseCommandTriggersCloseEvent()
		{
			var viewModel = new PositionDialogViewModel();
			bool triggered = false;
			EventHandler<DialogCloseRequestedEventArgs> handler = (sender, e) => triggered = true;

			viewModel.CloseRequested += handler;
			viewModel.OkCommand.Execute(null);

			Assert.IsTrue(triggered);
		}

		[TestMethod]
		public void ReflectionGetsCorrectValue()
		{
			var viewModel = new PositionDialogViewModel()
			{
				Name = "name",
				Category = "category"
			};

			string name = viewModel.GetType().GetProperty("Name").GetValue(viewModel).ToString();
			string category = viewModel.GetType().GetProperty("Category").GetValue(viewModel).ToString();

			Assert.AreEqual("name", name);
			Assert.AreEqual("category", category);
		}

		private class PositionDialogViewModelStub : PositionDialogViewModel
		{
			public string this[string propertyName]
			{
				get { return OnValidate(propertyName, PositionSpecification.Validate); }
			}
		}
	}
}
