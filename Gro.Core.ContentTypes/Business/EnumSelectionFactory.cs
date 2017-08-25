using EPiServer.Shell.ObjectEditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Gro.Core.ContentTypes.Business
{
    public class EnumSelectionFactory<TEnum> : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var type = typeof(TEnum);
            var names = type.GetEnumNames();
            foreach (var name in names)
            {
                var enumField = type.GetField(name);
                var displayAttr = enumField.GetCustomAttribute<DisplayAttribute>();
                var displayText = displayAttr?.Name ?? name;

                yield return new SelectItem
                {
                    Text = displayText,
                    Value = enumField.GetValue(null)
                };
            }
        }

        public static string GetDisplayText(object value)
        {
            var name = Enum.GetName(typeof(TEnum), value);
            var enumField = typeof(TEnum).GetField(name);
            var displayAttr = enumField.GetCustomAttribute<DisplayAttribute>();
            var displayText = displayAttr?.Name ?? name;

            return displayText;
        }
    }
}
