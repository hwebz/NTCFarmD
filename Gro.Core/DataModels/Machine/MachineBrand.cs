using System.Collections.Generic;

namespace Gro.Core.DataModels.Machine
{
    public class MachineBrand
    {
        public MachineBrand()
        {
            ModelList = new List<MachineModel>();
        }

        public string Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }

        public List<MachineModel> ModelList;
    }
}
