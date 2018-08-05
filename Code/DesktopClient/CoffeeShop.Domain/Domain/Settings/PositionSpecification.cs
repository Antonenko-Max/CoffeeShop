using System;
using System.Collections.Generic;
using System.Linq;
using Position = CoffeeShop.Domain.Model.Position;

namespace Domain.Settings
{
	public class PositionSpecification
	{
		public static string[] ValidateProperties = { "Name" };

		public static string Validate(string propertyName, string value)
		{
			switch (propertyName)
			{
				case "Name":
					return ValidatePositionName(value);
			}
			return null;
		}

		private static string ValidatePositionName(string name)
		{
			if (String.IsNullOrWhiteSpace(name))
				return Texts.Instance.NameCannotBeEmpty();
			return null;
		}

		public static string PositionValidToAdd(Position position, ICollection<Position> positions)
		{
			if (positions.Any(p => p.Id == position.Id)) return Texts.Instance.PositionAlreadyExists(position.Name);
			if (positions.Any(p => p.Name == position.Name)) return Texts.Instance.NameIsAlreadyTaken(position.Name);
			return null;
		}
	}
}
