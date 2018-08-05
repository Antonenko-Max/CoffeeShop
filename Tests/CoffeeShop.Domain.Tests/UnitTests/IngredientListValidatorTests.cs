using System;
using System.Collections.Generic;
using System.Linq;
using CoffeeShop.Domain.DTO;
using CoffeeShop.Domain.Model;
using Domain.UseCases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Position = CoffeeShop.Domain.Model.Position;
using DTOPosition = CoffeeShop.Domain.DTO.Position;
using DTOSize = CoffeeShop.Domain.DTO.Size;
using DTOIngredient = CoffeeShop.Domain.DTO.Ingredient;
using DTOConsumable = CoffeeShop.Domain.DTO.Consumable;
using Consumable = CoffeeShop.Domain.Model.Consumable;

namespace CoffeeShop.Domain.Tests.UnitTests
{
	[TestClass]
	public class IngredientListValidatorTests
	{
		[TestMethod]
		public void GetIngredientListReturnsListFromDependency()
		{
			var size = new DTOSize(1, "1", new Money(1), new DTOPosition(1, "1", "1") );
			var ingredientListStub = new IngredientListStub();
			var ingredientListValidator = new IngredientListValidator(ingredientListStub);
			var expected = ingredientListStub.GetIngredientList(size);
			var actual = ingredientListValidator.GetIngredientList(size);
			Assert.AreEqual(expected[0], actual[0]);
			Assert.AreEqual(expected[1], actual[1]);
			Assert.AreEqual(expected.Count, actual.Count);
		}

		[TestMethod]
		public void AddIngredientMethod_InvokesDependencyAddIngredientMethod_IfIngredientPassesValidation()
		{
			var position = new DTOPosition(1, "1", "1");
			var ingredient = new DTOIngredient(3, 1, new DTOSize(1, "1", new Money(1), position),
				new DTOConsumable(1, "1", 1));
			var ingredientListStub = new IngredientListStub();
			bool invoked = false;
			EventHandler<IngredientListChangedEventArgs> handler = (sender, e) => invoked = true;
			ingredientListStub.IngredientListChanged += handler;
			var ingredientListValidator = new IngredientListValidator(ingredientListStub);
			ingredientListValidator.AddIngredient(ingredient);
			Assert.IsTrue(invoked);
		}

		[TestMethod]
		public void CantRepeatConsumable_withAddMethod()
		{
			var position = new DTOPosition(1, "1", "1");
			var ingredient = new DTOIngredient(3, 1, new DTOSize(1, "1", new Money(1), position),
				new DTOConsumable(1, "water", 1));
			var ingredientListStub = new IngredientListStub();
			var ingredientListValidator = new IngredientListValidator(ingredientListStub);
			string error = null;
			EventHandler<IngredientListChangedEventArgs> handler = (sender, e) => error = e.Error;
			ingredientListValidator.IngredientListChanged += handler;
			ingredientListValidator.AddIngredient(ingredient);
			Assert.AreEqual("Ingredient water already exists", error);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void CantAddIngredientToNonExistingSize()
		{
			var position = new DTOPosition(1, "1", "1");
			var ingredient = new DTOIngredient(1, 1, new DTOSize(2, "1", new Money(1), position),
				new DTOConsumable(1, "water", 1));
			var ingredientListStub = new IngredientListStub();
			var ingredientListValidator = new IngredientListValidator(ingredientListStub);
			string error = null;
			EventHandler<IngredientListChangedEventArgs> handler = (sender, e) => error = e.Error;
			ingredientListValidator.IngredientListChanged += handler;
			ingredientListValidator.AddIngredient(ingredient);
		}

		[TestMethod]
		public void UpdateIngredientMethod_InvokesDependencyAddIngredientMethod_IfIngredientPassesValidation()
		{
			var position = new DTOPosition(1, "1", "1");
			var ingredient = new DTOIngredient(1, 1, new DTOSize(1, "1", new Money(1), position),
				new DTOConsumable(1, "1", 1));
			var ingredientListStub = new IngredientListStub();
			bool invoked = false;
			EventHandler<IngredientListChangedEventArgs> handler = (sender, e) => invoked = true;
			ingredientListStub.IngredientListChanged += handler;
			var ingredientListValidator = new IngredientListValidator(ingredientListStub);
			ingredientListValidator.UpdateIngredient(ingredient);
			Assert.IsTrue(invoked);
		}

		[TestMethod]
		public void CantUpdateNonExistingIngredient()
		{
			var position = new DTOPosition(1, "1", "1");
			var ingredient = new DTOIngredient(3, 1, new DTOSize(1, "1", new Money(1), position),
				new DTOConsumable(1, "water", 1));
			var ingredientListStub = new IngredientListStub();
			var ingredientListValidator = new IngredientListValidator(ingredientListStub);
			string error = null;
			EventHandler<IngredientListChangedEventArgs> handler = (sender, e) => error = e.Error;
			ingredientListValidator.IngredientListChanged += handler;
			ingredientListValidator.UpdateIngredient(ingredient);
			Assert.AreEqual("Ingredient water cannot be found", error);
		}

		[TestMethod]
		public void CantUpdateToExistingConsumable()
		{
			var position = new DTOPosition(1, "1", "1");
			var ingredient = new DTOIngredient(2, 1,  new DTOSize(1, "1", new Money(1), position),
				new DTOConsumable(1, "water", 1));
			var ingredientListStub = new IngredientListStub();
			var ingredientListValidator = new IngredientListValidator(ingredientListStub);
			string error = null;
			EventHandler<IngredientListChangedEventArgs> handler = (sender, e) => error = e.Error;
			ingredientListValidator.IngredientListChanged += handler;
			ingredientListValidator.UpdateIngredient(ingredient);
			Assert.AreEqual("Ingredient water already exists", error);
		}

		[TestMethod]
		public void CanUpdateAmountOnly()
		{
			var position = new DTOPosition(1, "1", "1");
			var ingredient = new DTOIngredient(1, 2, new DTOSize(1, "1", new Money(1), position),
				new DTOConsumable(1, "water", 1));
			var ingredientListStub = new IngredientListStub();
			var ingredientListValidator = new IngredientListValidator(ingredientListStub);
			string error = null;
			EventHandler<IngredientListChangedEventArgs> handler = (sender, e) => error = e.Error;
			ingredientListValidator.IngredientListChanged += handler;
			ingredientListValidator.UpdateIngredient(ingredient);
			Assert.IsNull(error);
		}

		[TestMethod]
		public void DeleteIngredientMethod_InvokesDependencyDeleteIngredientMethod_IfIngredientPassesValidation()
		{
			var position = new DTOPosition(1, "1", "1");
			var ingredient = new DTOIngredient(1, 1, new DTOSize(1, "1", new Money(1), position),
				new DTOConsumable(1, "1", 1));
			var ingredientListStub = new IngredientListStub();
			bool invoked = false;
			EventHandler<IngredientListChangedEventArgs> handler = (sender, e) => invoked = true;
			ingredientListStub.IngredientListChanged += handler;
			var ingredientListValidator = new IngredientListValidator(ingredientListStub);
			ingredientListValidator.DeleteIngredient(ingredient);
			Assert.IsTrue(invoked);
		}

		[TestMethod]
		public void DeleteNonExistingIngredientReturnsError()
		{
			var position = new DTOPosition(1, "1", "1");
			var ingredient = new DTOIngredient(3, 1, new DTOSize(1, "1", new Money(1), position),
				new DTOConsumable(1, "water", 1));
			var ingredientListStub = new IngredientListStub();
			var ingredientListValidator = new IngredientListValidator(ingredientListStub);
			string error = null;
			EventHandler<IngredientListChangedEventArgs> handler = (sender, e) => error = e.Error;
			ingredientListValidator.IngredientListChanged += handler;
			ingredientListValidator.DeleteIngredient(ingredient);
			Assert.AreEqual("Ingredient water cannot be found", error);
		}

		[TestMethod]
		public void ReraiseEventOfDependency()
		{
			var position = new DTOPosition(1, "1", "1");
			var ingredient = new DTOIngredient(3, 1, new DTOSize(1, "1", new Money(1), position),
				new DTOConsumable(1, "1", 1));
			var ingredientListStub = new IngredientListStub();
			var ingredientListValidator = new IngredientListValidator(ingredientListStub);
			bool invoked = false;
			EventHandler<IngredientListChangedEventArgs> handler = (sender, e) => invoked = true;
			ingredientListValidator.IngredientListChanged += handler;
			ingredientListValidator.AddIngredient(ingredient);
			Assert.IsTrue(invoked);
		}

		private class IngredientListStub : IIngredientList
		{
			public event EventHandler<IngredientListChangedEventArgs> IngredientListChanged;

			public List<DTOIngredient> GetIngredientList(DTOSize size)
			{
				var sizes = new List<DTOSize>() { new DTOSize(1, "1", new Money(1), new DTOPosition(1, "1", "1")) };
				
				var foundSize = sizes.First(p => p == size);
				return new List<DTOIngredient>()
				{
					new DTOIngredient(1, 1, size, new DTOConsumable(1, "water", 1)),
					new DTOIngredient(2, 2, size, new DTOConsumable(2, "sugar", 1))
				};
			}

			public void AddIngredient(DTOIngredient ingredient)
			{
				IngredientListChanged?.Invoke(this, new IngredientListChangedEventArgs(Ingredients));
			}

			public void UpdateIngredient(DTOIngredient ingredient)
			{
				IngredientListChanged?.Invoke(this, new IngredientListChangedEventArgs(Ingredients));
			}

			public void DeleteIngredient(DTOIngredient ingredient)
			{
				IngredientListChanged?.Invoke(this, new IngredientListChangedEventArgs(Ingredients));
			}

			public List<DTOIngredient> Ingredients { get; set; } = new List<DTOIngredient>();
		}
	}
}
