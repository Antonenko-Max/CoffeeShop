using System;

namespace Domain.Settings
{
	public class Texts
	{
		private static readonly Lazy<Texts> instance = new Lazy<Texts>(() => new Texts());

		private Texts()
		{
		}

		public static Texts Instance => instance.Value;

		public string IngredientAlreadyExists(string name)
		{
			return $"Ingredient {name} already exists";
		}

		public string IngredientCannotBeFound(string name)
		{
			return $"Ingredient {name} cannot be found";
		}

		public string IngredientIsAlreadyPresent(string name)
		{
			return $"Ingredient {name} is already present";
		}

		public string PositionAlreadyExists(string name)
		{
			return $"Position {name} already exists";
		}

		public string PositionCannotBeFound(string name)
		{
			return $"Position {name} cannot be found";
		}

		public string SizeAlreadyExists(string name)
		{
			return $"Size {name} already exists";
		}

		public string SizeCannotBeFound(string name)
		{
			return $"Size {name} cannot be found";
		}

		public string NameIsAlreadyTaken(string name)
		{
			return $"Name {name} is already taken";
		}

		public string SizeCorrespondsToAnotherPosition(string name)
		{
			return $"Size {name} corresponds to another position";
		}

		public string IngredientCorrespondsToAnotherPosition(string name)
		{
			return $"Ingredient {name} corresponds to another size";
		}

		public string NameCannotBeEmpty()
		{
			return "Name cannot be empty";
		}

		public string PriceMustBeNumber()
		{
			return "Price must be a number";
		}

		public string PriceCannotBeNegative()
		{
			return "Price cannot be negative";
		}

		public string AmountMustBeNumeric()
		{
			return "Amount must be numeric";
		}

		public string CannotAddMeSizesThan(string maximumSizes)
		{
			return $"Cannot add more than {maximumSizes} sizes";
		}
	}
}
