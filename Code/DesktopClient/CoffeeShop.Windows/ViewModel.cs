using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using  System.Collections.ObjectModel;
using System.Linq;

namespace CoffeeShop.Windows
{
	public abstract class ViewModel : ObservableObject, IDataErrorInfo
	{
		// todo: rework class

		public string this[string propertyName]
		{
			get { return OnValidate(propertyName); }
		}

		[Obsolete]
		public string Error
		{
			get { throw new NotSupportedException(); }
		}

		protected virtual string OnValidate(string propertyName)
		{
			var contex = new ValidationContext(this)
			{
				MemberName = propertyName
			};

			var results = new Collection<ValidationResult>();
			var isValid = Validator.TryValidateObject(this, contex, results, true);

			if (!isValid)
			{
				ValidationResult result = results.SingleOrDefault(p =>
					p.MemberNames.Any(memberName => memberName == propertyName));
				return result?.ErrorMessage;
			}

			return null;
		}
	}
}
