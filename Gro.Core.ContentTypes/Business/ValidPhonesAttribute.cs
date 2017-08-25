using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Gro.Business
{
    public class ValidPhonesAttribute : ValidationAttribute
    {
        private readonly PhoneAttribute _phoneAttribute;

        public ValidPhonesAttribute()
        {
            _phoneAttribute = new PhoneAttribute();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var phones = value as string;
            if (phones == null) { return null; }

            var phonesArray = phones.Split(';');
            return phonesArray.Select(phone => _phoneAttribute.IsValid(phone)).Any(validationResult => !validationResult) ?
                new ValidationResult("NotAPhoneNumber") : null;
        }
    }
}
