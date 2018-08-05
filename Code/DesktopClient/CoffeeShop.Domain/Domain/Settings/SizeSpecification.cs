using System;

namespace Domain.Settings
{
	public class SizeSpecification
	{
		public static string[] ValidateProperties = { "Name", "Price" };

		public static string Validate(string propertyName, string value)
		{
			switch (propertyName)
			{
				case "Name":
					return ValidateSizeName(value);
				case "Price":
					return ValidateSizePrice(value);
			}
			return null;
		}

		private static string ValidateSizePrice(string price)
		{
			decimal d;
			if (!decimal.TryParse(price, out d)) return Texts.Instance.PriceMustBeNumber();
			else if (d < 0) return Texts.Instance.PriceCannotBeNegative();
			return null;
		}

		private static string ValidateSizeName(string name)
		{
			if (String.IsNullOrWhiteSpace(name))
				return Texts.Instance.NameCannotBeEmpty();
			return null;
		}
	}
}
