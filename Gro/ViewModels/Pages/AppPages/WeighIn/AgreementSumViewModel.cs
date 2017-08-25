using System.Collections.Generic;

namespace Gro.ViewModels.Pages.AppPages.WeighIn
{
    public class AgreementSumViewModel
    {
        public string Sort { get; set; }
        public int Quantity { get; set; }
        public int QuantitySpont { get; set; }

    }

    public class AgreementSumComparer : IEqualityComparer<AgreementSumViewModel>
    {
        public bool Equals(AgreementSumViewModel x, AgreementSumViewModel y)
        {
            return x.Sort == y.Sort && x.Quantity == y.Quantity && x.QuantitySpont == y.QuantitySpont;
        }

        public int GetHashCode(AgreementSumViewModel obj)
        {
            unchecked
            {
                var hash = 17;

                hash = hash * 23 + obj.Sort.GetHashCode();
                hash = hash * 23 + obj.Quantity.GetHashCode();
                hash = hash * 23 + obj.QuantitySpont.GetHashCode();
                return hash;
            }
        }
    }
}
