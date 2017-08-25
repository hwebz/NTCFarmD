using System.Collections.Generic;
using System.Linq;

namespace Gro.Infrastructure.Data
{
    public class MediaConfig
    {
        private readonly string _customerFolder;
        private readonly string _userFolder;
        private readonly string _machineFolder;
        private readonly string _migrateFolderPath;

        public MediaConfig(
            string customerFolder,
            string userFolder,
            string machineFolder,
            string migrateFolderPath,
            string imageTypes,
            string documentTypes)
        {
            _customerFolder = customerFolder;
            _userFolder = userFolder;
            _machineFolder = machineFolder;
            _migrateFolderPath = migrateFolderPath;
            ImageTypes = string.IsNullOrEmpty(imageTypes) ? new List<string>() {"jpg","png"} : imageTypes.Split('|').ToList();
            DocumentTypes = string.IsNullOrEmpty(documentTypes) ? new List<string>() {"pdf"} : documentTypes.Split('|').ToList();
        }

        public string CustomerFolder => string.IsNullOrEmpty(_customerFolder) ? "Customers" : _customerFolder;
        public string UserFolder => string.IsNullOrEmpty(_userFolder) ? "Users" : _userFolder;
        public string MachineFolder => string.IsNullOrEmpty(_machineFolder) ? "Machines" : _machineFolder;
        public string MigrateFolderPath => string.IsNullOrEmpty(_migrateFolderPath) ? "~/App_Data/MigrateFolder/" : _migrateFolderPath;
        public List<string> ImageTypes { get; }

        public List<string> DocumentTypes { get; }

        public string HttpPostedFileKey => "UploadedImage";
        public string GlobalAssets => "globalassets";
    }
}
