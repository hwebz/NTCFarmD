using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gro.Core.ContentTypes.Business
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ValidFileAttribute : ValidationAttribute
    {
        public string[] Accept { get; set; }

        public int MaxSize { get; set; }

        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;
            if (file == null) return false;

            if (MaxSize > 0 && file.ContentLength > MaxSize) return false;

            var accept = Accept ?? new string[0];
            return accept.Any(a => a == file.ContentType);
        }
    }
}
