using EPiServer.Core;
using EPiServer.SpecializedProperties;
using System;
using System.ComponentModel.DataAnnotations;

namespace Gro.Core.ContentTypes.Business
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LinkItemCollectionLimitAttribute : ValidationAttribute
    {
        public int Min { get; set; } = 0;
        public int Max { get; set; } = int.MaxValue;

        public override bool RequiresValidationContext => true;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var contentData = validationContext.ObjectInstance as IContentData;

            var linkItemCollection = value as LinkItemCollection;
            if (linkItemCollection == null) return ValidationResult.Success;

            if (linkItemCollection.Count <= Max && linkItemCollection.Count >= Min) return ValidationResult.Success;

            return new ValidationResult($"Collection must only have between {Min} and {Max} links");
        }
    }
}
