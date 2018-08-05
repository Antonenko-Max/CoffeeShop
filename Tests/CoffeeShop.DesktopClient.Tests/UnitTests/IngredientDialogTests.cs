using System;
using System.Collections.Generic;
using CoffeeShop.DesktopClient.ViewModels;
using CoffeeShop.Domain.DTO;
using CoffeeShop.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeShop.DesktopClient.Tests.UnitTests
{
	[TestClass]
	public class IngredientDialogTests
	{

		[TestMethod]
		public void IsValid()
		{
			var consumables = new List<Consumable>() { new Consumable(1, "water", 0)};
			var viewModel = new IngredientDialogViewModel(consumables);
			viewModel.ConsumableName = consumables[0].Name;
			Assert.IsTrue(viewModel.IsValid);
		}

		[TestMethod]
		public void CloseCommandTriggersCloseEvent()
		{
			var consumables = new List<Consumable>() { new Consumable(1, "water", 0) };
			var viewModel = new IngredientDialogViewModel(consumables);
			bool triggered = false;
			EventHandler<DialogCloseRequestedEventArgs> handler = (sender, e) => triggered = true;

			viewModel.CloseRequested += handler;
			viewModel.OkCommand.Execute(null);

			Assert.IsTrue(triggered);
		}

	}
}
