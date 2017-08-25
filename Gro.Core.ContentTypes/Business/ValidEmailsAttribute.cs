using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Gro.Business
{
    public class ValidEmailsAttribute : ValidationAttribute
    {
        private readonly EmailAddressAttribute emailAddressAttribute;
        public bool AllowEmpty { get; set; }

        public ValidEmailsAttribute()
        {
            emailAddressAttribute = new EmailAddressAttribute();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var emails = value as string;
            if (emails == null)
            {
                return AllowEmpty ? null : new ValidationResult("NullEmails");
            }

            var emailArray = emails.Split(';');
            return emailArray.Select(email => emailAddressAttribute.IsValid(email)).Any(validationResult => !validationResult) ?
                new ValidationResult("NotAnEmail") : null;
        }
    }
}
