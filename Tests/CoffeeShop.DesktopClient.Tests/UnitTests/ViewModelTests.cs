using System;
using System.ComponentModel.DataAnnotations;
using CoffeeShop.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeShop.DesktopClient.Tests.UnitTests
{
	[TestClass]
	public class ViewModelTests
	{
		[TestMethod]
		public void IndexerPropertyValidatesPropertyNameWithInvalidValue()
		{
			var viewModel = new StubViewModel();
			Assert.IsNotNull(viewModel["RequiredProperty"]);
		}
		[TestMethod]
		public void IndexerPropertyValidatesPropertyNameWithValue()
		{
			var viewModel = new StubViewModel()
								{
									RequiredProperty = "Some Value"
								};

			Assert.IsNull(viewModel["RequiredProperty"]);
		}

		[TestMethod]
		public void IndexerReturnsErrorMessageForRequestedInvalidProperty()
		{
			var viewModel = new StubViewModel()
			{
				RequiredProperty = null,
				SomeProperty = null
			};

			var msg = viewModel["SomeProperty"];

			Assert.AreEqual("Требуется поле SomeProperty.", msg);
		}
	}

	class StubViewModel : ViewModel
	{
		[Required]
		public string RequiredProperty { get; set; }
		[Required]
		public object SomeProperty { get; set; }
	}
}
