using System.Collections.Generic;
using System.Linq;
using Gro.Core.ContentTypes.Pages.StartPages;
using Gro.Core.DataModels.Machine;

namespace Gro.ViewModels.Pages.Machines
{
    public class MaskinStartPageViewModel : PageViewModel<MaskinStartPage>
    {
        public MaskinStartPageViewModel(MaskinStartPage currentPage) : base(currentPage)
        {
            ListMachine = new List<Machine>();
        }

        public List<Machine> ListMachine { get; set; }

        public Dictionary<string, List<Machine>> GroupMachines => ListMachine?.Where(m => m.Group != null)
                                                                      .OrderBy(m => m.Group.Id)
                                                                      .GroupBy(m => new {m.Group.Id, m.Group.Name})
                                                                      .ToDictionary(s => s.Key.Name,
                                                                          t => t.OrderBy(x => x.Brand.Name)
                                                                              .ThenBy(x => x.ModelDescription)
                                                                              .ToList()) ?? new Dictionary<string, List<Machine>>();

        public string DetailMachineUrl { get; set; }

        public string BuyUrl => "https://ehandel.lantmannenmaskin.se/lmma_e-Sales/esa/zItemList.jsp?mark={0}&model={1}";
    }
}
