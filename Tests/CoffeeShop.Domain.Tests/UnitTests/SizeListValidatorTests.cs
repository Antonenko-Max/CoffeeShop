using System;
using System.Collections.Generic;
using System.Linq;
using CoffeeShop.Data.Tests;
using CoffeeShop.Domain.Model;
using Domain.UseCases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Position = CoffeeShop.Domain.Model.Position;
using DTOPosition = CoffeeShop.Domain.DTO.Position;
using DTOSize = CoffeeShop.Domain.DTO.Size;
using DTOIngredient = CoffeeShop.Domain.DTO.Ingredient;

namespace CoffeeShop.Domain.Tests.UnitTests
{
	[TestClass]
	public class SizeListValidatorTests
	{
		[TestMethod]
		public void GetSizeListReturnsListFromDependency()
		{
			var position = new DTOPosition(1, "1", "1");
			var sizeListStub = new SizeListStub();
			var sizeListValidator = new SizeListValidator(sizeListStub);
			var expected = sizeListStub.GetSizeList(position);
			var actual = sizeListValidator.GetSizeList(position);
			Assert.AreEqual(expected[0], actual[0]);
			Assert.AreEqual(expected[1], actual[1]);
			Assert.AreEqual(expected.Count, actual.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetSizeListReturnsExceptionIfPositionDoesntExists()
		{
			var position = new DTOPosition(2, "2", "2");
			var sizeListStub = new SizeListStub();
			var sizeListValidator = new SizeListValidator(sizeListStub);
			var actual = sizeListValidator.GetSizeList(position);
		}

		[TestMethod]
		public void AddSizeMethod_InvokesDependencyAddSizeMethod_IfSizenPassesValidation()
		{
			var size = new DTOSize(3, "3", new Money(1), new DTOPosition(1, "1", "1") );
			var sizeListStub = new SizeListStub();
			bool invoked = false;
			EventHandler<SizeListChangedEventArgs> handler = (sender, e) => invoked = true;
			sizeListStub.SizeListChanged += handler;
			var sizeListValidator = new SizeListValidator(sizeListStub);
			sizeListValidator.AddSize(size);
			Assert.IsTrue(invoked);
		}

		[TestMethod]
		public void CantRepeatSizeName_withAddMethod()
		{
			var size = new DTOSize(3, "1", new Money(1), new DTOPosition(1, "1", "1"));
			var sizeListStub = new SizeListStub();
			var sizeListValidator = new SizeListValidator(sizeListStub);
			string error = null;
			EventHandler<SizeListChangedEventArgs> handler = (sender, e) => error = e.Error;
			sizeListValidator.SizeListChanged += handler;
			sizeListValidator.AddSize(size);
			Assert.AreEqual("Name 1 is already taken", error);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void CantAddSizeToNonExistingPosition()
		{
			var size = new DTOSize(3, "1", new Money(1), new DTOPosition(2, "2", "2"));
			var sizeListStub = new SizeListStub();
			var sizeListValidator = new SizeListValidator(sizeListStub);
			string error = null;
			EventHandler<SizeListChangedEventArgs> handler = (sender, e) => error = e.Error;
			sizeListValidator.SizeListChanged += handler;
			sizeListValidator.AddSize(size);
		}

		[TestMethod]
		public void UpdateSizeMethod_InvokesDependencyAddSizeMethod_IfSizePassesValidation()
		{
			var size = new DTOSize(1, "3", new Money(1), new DTOPosition(1, "1", "1"));
			var sizeListStub = new SizeListStub();
			bool invoked = false;
			EventHandler<SizeListChangedEventArgs> handler = (sender, e) => invoked = true;
			sizeListStub.SizeListChanged += handler;
			var sizeListValidator = new SizeListValidator(sizeListStub);
			sizeListValidator.UpdateSize(size);
			Assert.IsTrue(invoked);
		}

		[TestMethod]
		public void CantUpdateNonExistingSize()
		{
			var size = new DTOSize(3, "1", new Money(1), new DTOPosition(1, "1", "1"));
			var sizeListStub = new SizeListStub();
			var sizeListValidator = new SizeListValidator(sizeListStub);
			string error = null;
			EventHandler<SizeListChangedEventArgs> handler = (sender, e) => error = e.Error;
			sizeListValidator.SizeListChanged += handler;
			sizeListValidator.UpdateSize(size);
			Assert.AreEqual("Size 1 cannot be found", error);
		}

		[TestMethod]
		public void CantUpdateToExistingName()
		{
			var size = new DTOSize(2, "1", new Money(1), new DTOPosition(1, "1", "1"));
			var sizeListStub = new SizeListStub();
			var sizeListValidator = new SizeListValidator(sizeListStub);
			string error = null;
			EventHandler<SizeListChangedEventArgs> handler = (sender, e) => error = e.Error;
			sizeListValidator.SizeListChanged += handler;
			sizeListValidator.UpdateSize(size);
			Assert.AreEqual("Name 1 is already taken", error);
		}

		[TestMethod]
		public void CanUpdateMoneyOnly()
		{
			var size = new DTOSize(1, "1", new Money(2), new DTOPosition(1, "1", "1"));
			var sizeListStub = new SizeListStub();
			var sizeListValidator = new SizeListValidator(sizeListStub);
			string error = null;
			EventHandler<SizeListChangedEventArgs> handler = (sender, e) => error = e.Error;
			sizeListValidator.SizeListChanged += handler;
			sizeListValidator.UpdateSize(size);
			Assert.IsNull(error);
		}

		[TestMethod]
		public void DeleteSizeMethod_InvokesDependencyDeleteSizeMethod_IfSizePassesValidation()
		{
			var size = new DTOSize(1, "3", new Money(1), new DTOPosition(1, "1", "1"));
			var sizeListStub = new SizeListStub();
			bool invoked = false;
			EventHandler<SizeListChangedEventArgs> handler = (sender, e) => invoked = true;
			sizeListStub.SizeListChanged += handler;
			var sizeListValidator = new SizeListValidator(sizeListStub);
			sizeListValidator.DeleteSize(size);
			Assert.IsTrue(invoked);
		}

		[TestMethod]
		public void DeleteNonExistingSizeReturnsError()
		{
			var size = new DTOSize(3, "1", new Money(1), new DTOPosition(1, "1", "1"));
			var sizeListStub = new SizeListStub();
			var sizeListValidator = new SizeListValidator(sizeListStub);
			string error = null;
			EventHandler<SizeListChangedEventArgs> handler = (sender, e) => error = e.Error;
			sizeListValidator.SizeListChanged += handler;
			sizeListValidator.DeleteSize(size);
			Assert.AreEqual("Size 1 cannot be found", error);
		}

		[TestMethod]
		public void ReraiseEventOfDependency()
		{
			var size = new DTOSize(3, "3", new Money(1), new DTOPosition(1, "1", "1"));
			var sizeListStub = new SizeListStub();
			var sizeListValidator = new SizeListValidator(sizeListStub);
			bool invoked = false;
			EventHandler<SizeListChangedEventArgs> handler = (sender, e) => invoked = true;
			sizeListValidator.SizeListChanged += handler;
			sizeListValidator.AddSize(size);
			Assert.IsTrue(invoked);
		}

		private class SizeListStub : ISizeList
		{
			public event EventHandler<SizeListChangedEventArgs> SizeListChanged;
			
			public List<DTOSize> GetSizeList(DTOPosition position)
			{
				var positions = new List<DTOPosition>() {new DTOPosition(1, "1", "1")};
				var pos = positions.First(p => p == position);
				return new List<DTOSize>()
				{
					new DTOSize(1, "1", new Money(1),new DTOPosition(1, "1", "1") ),
					new DTOSize(2, "2", new Money(1),new DTOPosition(1, "1", "1") )
				};
			}

			public List<DTOSize> Sizes { get; set; } = new List<DTOSize>();

			public void AddSize(DTOSize size)
			{
				if (SizeListChanged != null) SizeListChanged.Invoke(this, new SizeListChangedEventArgs(Sizes));
			}

			public void UpdateSize(DTOSize size)
			{
				if (SizeListChanged != null) SizeListChanged.Invoke(this, new SizeListChangedEventArgs(Sizes));
			}

			public void DeleteSize(DTOSize size)
			{
				if (SizeListChanged != null) SizeListChanged.Invoke(this, new SizeListChangedEventArgs(Sizes));
			}
		}
	}
}
