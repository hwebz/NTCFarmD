using System.Collections.Generic;
using System.Linq;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using Gro.Core.ContentTypes.Utils;

namespace Gro.Core.ContentTypes.CustomProperties
{
    [ServiceConfiguration(typeof(ISelectionQuery))]
    public class AgreementTypeSelectionQuery : ISelectionQuery
    {
        protected List<SelectItem> Items;

        public AgreementTypeSelectionQuery()
        {
            Items = new List<SelectItem>
            {
                new SelectItem {Value = AgreementType.Depaavtal, Text = "Depåavtal"},
                new SelectItem {Value = AgreementType.Poolavtal, Text = "Poolavtal"},
                new SelectItem { Value = AgreementType.SportAndForwardAvtal, Text = "Spotprisavtal and Terminsavtal"},
                new SelectItem() {Value = AgreementType.PrissakringDepaavtal, Text = "Prissäkring depåavtal"}
            };
        }

        public ISelectItem GetItemByValue(string value)
        {
            return Items != null ? Items.FirstOrDefault(i => ((string)i.Value).Contains(value)) : new SelectItem();
        }

        public IEnumerable<ISelectItem> GetItems(string query)
        {
            return Items ?? new List<SelectItem>();
        }
    }
}