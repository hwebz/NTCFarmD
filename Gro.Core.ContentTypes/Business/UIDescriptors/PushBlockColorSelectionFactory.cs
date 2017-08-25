using System.Collections.Generic;
using EPiServer.Shell.ObjectEditing;
using Gro.Core.ContentTypes.Utils;

namespace Gro.Core.ContentTypes.Business.UIDescriptors
{
    public class PushBlockColorSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem {Text = "Green", Value = PushColor.Green},
                new SelectItem {Text = "Blue", Value = PushColor.Blue},
                new SelectItem {Text = "Orange", Value = PushColor.Orange},
                new SelectItem {Text = "Purple", Value = PushColor.Purple}
            };
        }
    }
}
