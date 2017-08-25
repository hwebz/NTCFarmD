using System.Collections.Generic;

namespace Gro.Core.DataModels.Boka
{
    public class SearchResultDto
    {
        public SearchResultDto()
        {
            Customers = new List<CustomerDto>();
            Ios = new List<IODto>();
        }

        public string RegNo { get; set; }
        public int Status { get; set; }
        public string ErrorMessage { get; set; }
        public string LinkUrl { get; set; }
        public BookingOrder BookingOrder { get; set; }
        public List<CustomerDto> Customers { get; set; }
        public List<IODto> Ios { get; set; }
    }
}
