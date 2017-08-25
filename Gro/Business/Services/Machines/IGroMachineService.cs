using System.Collections.Generic;
using System.Web;
using Gro.Core.DataModels.Machine;

namespace Gro.Business.Services.Machines
{
    public interface IGroMachineService
    {
        string GetMachineListUrl(HttpContextBase httpContext);

        string GetVirtualPath(string vppFolder, string nameWithExtension);

        string GetPhysicalPath(string vppFolder, string nameWithExtension);
        string GetMachineImageUrl(List<MachineImage> images);
        List<MachineBrand> GetBrandListFromXml();
        List<MachineCategory> GetCategoryListFromXml();
    }
}
