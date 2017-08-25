using System.Collections.Generic;

namespace Gro.Core.DataModels.Machine
{
    public class Machine
    {
        public Machine()
        {
            Images = new List<MachineImage>();
            Documents = new List<MachineDocument>();
            Videos = new List<MachineVideo>();
            Brand= new MachineBrand();
            Group= new MachineGroup();
            Model = new MachineModel();
        }
        public string Id { get; set; }
        public string Type { get; set; }
        public string Unique { get; set; }
        public string ModelName { get; set; }
        public string ModelDescription { get; set; }
        public string IndividualNumber { get; set; }
        public string IndvidualType { get; set; }
        public string LegalRegNumber { get; set; }
        public string OwnerNumber { get; set; }
        public string Fabric { get; set; }
        public string ModelYear { get; set; }
        public string ItemGroup { get; set; }
        public string Status { get; set; }
        public string SerialNumber { get; set; }
        public string DeliveryDate { get; set; }
        public string WarrantyDateSales { get; set; }
        public string ItemNumber { get; set; }
        public string ReceiptDate { get; set; }
        public string WarrantyDateSupplier { get; set; }
        public string InstallationDate { get; set; }
        public string DateDisposed { get; set; }
        public string Name { get; set; }
        public string DescriptionBrand { get; set; }
        public string DescriptionModel { get; set; }
        public string ImageUrl { get; set; }
        public MachineBrand Brand { get; set; }
        public MachineGroup Group { get; set; }
        public List<MachineImage> Images { get; set; }
        public List<MachineDocument> Documents { get; set; }
        public List<MachineVideo> Videos { get; set; }
        public MachineModel Model { get; set; }

        public string GetModelDescription()
        {
            return string.IsNullOrEmpty(ModelDescription) ? DescriptionModel : ModelDescription;
        }
    }
}
