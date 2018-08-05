using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeShop.Windows.Tests.UnitTests
{
	[TestClass]
	public class ExpandoObjectTests
	{
		[TestMethod]
		public void ExpandoObjectTest()
		{
			var expando = new ExpandoTest();
			var dict = (IDictionary<string, object>) expando.Columns;
			dict["Test"] = 1;
			Assert.AreEqual(1, ((dynamic)(expando.Columns)).Test);
		}

		[TestMethod]
		public void CustomDynamicObjectTest()
		{
			var custom = new DynamicTest();
			custom.Dictionary["Test"] = 1;
			Assert.AreEqual(1, ((dynamic)custom).Test);
		}


		private class ExpandoTest
		{
			public ExpandoObject Columns = new ExpandoObject();
		}

		private class DynamicTest : DynamicObject
		{
			private Dictionary<string, object> dictionary = new Dictionary<string, object>();

			public Dictionary<string, object> Dictionary
			{
				get => dictionary;
				set => dictionary = value;
			}

			public override bool TryGetMember(
				GetMemberBinder binder, out object result)
			{
				return dictionary.TryGetValue(binder.Name, out result);
			}

			public override bool TrySetMember(
				SetMemberBinder binder, object value)
			{
				dictionary[binder.Name.ToLower()] = value;
				return true;
			}
		}
	}
}
