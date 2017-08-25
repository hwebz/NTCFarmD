using System.Collections.Generic;

namespace Gro.Core.DataModels.Machine
{
    public class MachineCategory
    {
        public MachineCategory()
        {
            brandList = new List<MachineBrand>();
        }

        public string Id { get; set; }
        public string Key { get; set; }
        
        public string Name { get; set; }

        public List<MachineBrand> brandList;
    }
}
