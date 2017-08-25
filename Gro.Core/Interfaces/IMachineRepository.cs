using System.Collections.Generic;
using System.Threading.Tasks;
using Gro.Core.DataModels.Machine;
using Gro.Core.DataModels.Organization;

namespace Gro.Core.Interfaces
{
    public interface IMachineRepository
    {
        List<Machine> GetAllMachinesForCustomerId(string customerId);
        Task<Machine> GetDetailMachineByRegNumber(string regNumber);
        Task<Machine> GetDetailMachineById(string sysId);
        Task<OrganisationPicture> GetMachinePicUrl(int organisationId, string machineId);
        Task<bool> CreateMachinePicUrl(int organizationId, string machineId, string picUrl);
        Task<bool> DeleteMachinePicUrl(int picId);
        Task<OrganisationPicture[]> FindMachinePictures(int organizationId);
        List<MachineCategory> GetCategoryListToXml();
	    bool AddNewMachine(Machine machine);
        List<MachineBrand> GetBrandListToXml();
        string GetMachineCategoryImage(string categoryId);
        Task<bool> RemoveMachine(string machineId);
    }
}
