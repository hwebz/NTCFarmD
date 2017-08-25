using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gro.Core.DataModels.Machine;
using Gro.Core.DataModels.Organization;
using Gro.Core.Interfaces;
using Gro.Infrastructure.Data.MachineService;
using Gro.Infrastructure.Data.OrganisationService;
using Gro.Infrastructure.Data.MachineAddNewServer;
using System.Xml;
using Gro.Infrastructure.Data.MachineRemoveService;
using Gro.Infrastructure.Data.MachineDetailByReg;
using Gro.Infrastructure.Data.MachineDetailById;
using System.Web;

namespace Gro.Infrastructure.Data.Repositories
{
    public class MachineRepository : IMachineRepository
    {
        private readonly getAllMachinesForCustomerId_PortType _listMachineService;
        private readonly ILM2OrganisationService _organisationService;
        private readonly updateMachineForCustomerId_PortType _addMachineService;
        private readonly deleteMachineForCustomerId_PortType _deleteMachineService;
        private readonly GetMachineByRegNumber_PortType _detailMachineByRegService;
        private readonly getMachineBySysId_PortType _detailMachineBySysIdService;
        private readonly TicketProvider _ticketProvider;

        private string _ticket;
        private string Ticket => _ticket ?? (_ticket = _ticketProvider.GetTicket());

        public MachineRepository(getAllMachinesForCustomerId_PortType listMachineService,
            ILM2OrganisationService organisationService, TicketProvider ticketProvider,
            updateMachineForCustomerId_PortType addMachineService,
            deleteMachineForCustomerId_PortType deleteMachineService,
            GetMachineByRegNumber_PortType detailMachineByRegService,
            getMachineBySysId_PortType detailMachineBySysIdService)
        {
            _listMachineService = listMachineService;
            _addMachineService = addMachineService;
            _organisationService = organisationService;
            _ticketProvider = ticketProvider;
            _deleteMachineService = deleteMachineService;
            _detailMachineByRegService = detailMachineByRegService;
            _detailMachineBySysIdService = detailMachineBySysIdService;
        }

        public List<Machine> GetAllMachinesForCustomerId(string customerId)
        {
            var result = _listMachineService.GetAllMachinesForCustomerId(new GetAllMachinesForCustomerIdRequest
            {
                CUNO = customerId
            });

            var listMachine = new List<Machine>();
            if (result?.allMachinesOut == null) return listMachine;

            foreach (var t in result.allMachinesOut)
            {
                var componentAttrs = t.componentAttributes;
                if (componentAttrs == null) continue;

                var groupNew = new MachineGroup
                {
                    Id = componentAttrs.CmIndividType?.smdid ?? "0",
                    Key = componentAttrs.CmIndividType?.smdkey ?? "0",
                    Name = componentAttrs.CmIndividType?.Text?.Length > 0
                        ? componentAttrs.CmIndividType?.Text[0]
                        : "Ingen kategori"
                };

                var images = t.image;
                var machineImages = images?.Select(image => new MachineImage
                {
                    FileName = image.CmFilename?.Text?.Length > 0 ? image.CmFilename?.Text[0] : string.Empty,
                    Name = image.CmName?.Text?.Length > 0 ? image.CmName?.Text[0] : string.Empty,
                    Id = image.SysId?.Text?.Length > 0 ? image.SysId?.Text[0] : string.Empty,
                    ImageType = image.CmImageType?.Text != null && image.CmImageType.Text.Length > 0 ? image.CmImageType.Text[0] : string.Empty
                });

                var brand = new MachineBrand
                {
                    Id = componentAttrs.CmOE?.smdkey != null ? componentAttrs.CmOE?.smdid : string.Empty,
                    Key = componentAttrs.CmOE?.smdkey ?? string.Empty,
                    Name = componentAttrs.CmOE?.Text?.Length > 0
                        ? (componentAttrs.CmOE.Text[0] != "Annan" ? componentAttrs.CmOE.Text[0] : string.Empty)
                        : string.Empty
                };

                var model = new MachineModel
                {
                    Id = componentAttrs.CmModels?.smdkey != null ? componentAttrs.CmModels?.smdid : string.Empty,
                    Key = componentAttrs.CmModels?.smdkey ?? string.Empty,
                    Name = componentAttrs.CmModels?.Text?.Length > 0
                        ? (componentAttrs.CmModels?.Text[0] != "Annan" ? componentAttrs.CmModels.Text[0] : string.Empty)
                        : string.Empty
                };

                var machine = new Machine
                {
                    LegalRegNumber = componentAttrs.MvxSAREBE?.Text?.Length > 0 ? componentAttrs.MvxSAREBE?.Text[0] : string.Empty,
                    SerialNumber = componentAttrs.MvxSASERI?.Text?.Length > 0 ? componentAttrs.MvxSASERI?.Text[0] : string.Empty,
                    Id = componentAttrs.SysId?.Text?.Length > 0 ? componentAttrs.SysId.Text[0] : string.Empty,
                    ModelDescription = componentAttrs.MvxSAMODE?.Text?.Length > 0
                        ? componentAttrs.MvxSAMODE.Text[0]
                        : componentAttrs.CmSubHeader?.Text?.Length > 0
                            ? componentAttrs.CmSubHeader?.Text[0]
                            : string.Empty,
                    IndividualNumber = componentAttrs.MvxSAINNO?.Text?.Length > 0 ? componentAttrs.MvxSAINNO.Text[0] : string.Empty,
                    DescriptionBrand = componentAttrs.CmHeader?.Text?.Length > 0 ? componentAttrs.CmHeader.Text[0] : string.Empty,
                    Brand = brand,
                    Group = groupNew,
                    Model = model,
                    Images = machineImages?.ToList() ?? new List<MachineImage>()
                };

                listMachine.Add(machine);
            }
            return listMachine;
        }

        public async Task<Machine> GetDetailMachineByRegNumber(string regNumber)
        {
            var result =
                await _detailMachineByRegService.GetMachineByRegNumberAsync(new GetMachineByRegNumberRequest(regNumber));
            if (result?.oneMachineOut?.component == null) return new Machine();

            var componentAttrs = result.oneMachineOut.component.componentAttributes;
            var groupNew = new MachineGroup
            {
                Id = componentAttrs.CmIndividType?.smdid ?? "0",
                Name = componentAttrs.CmIndividType?.Text?.FirstOrDefault() ?? "Ingen kategori",
                Key = componentAttrs.CmIndividType?.smdkey ?? string.Empty
            };

            return new Machine
            {
                Id = componentAttrs.SysId?.Text?.FirstOrDefault() ?? string.Empty,
                ModelName = componentAttrs.MvxSAMODC?.Text?.FirstOrDefault() ?? string.Empty,
                ModelDescription = componentAttrs.MvxSAMODE?.Text?.FirstOrDefault() ?? string.Empty,
                IndividualNumber = componentAttrs.MvxSAINNO?.Text?.FirstOrDefault() ?? string.Empty,
                LegalRegNumber = componentAttrs.MvxSAREBE?.Text?.FirstOrDefault() ?? string.Empty,
                OwnerNumber = componentAttrs.MvxSACUOW?.Text?.FirstOrDefault() ?? string.Empty,
                SerialNumber = componentAttrs.MvxSASERI?.Text?.FirstOrDefault() ?? string.Empty,
                DescriptionBrand = componentAttrs.CmHeader?.Text?.FirstOrDefault() ?? string.Empty,
                DescriptionModel = componentAttrs.CmSubHeader?.Text?.FirstOrDefault() ?? string.Empty,
                Brand = new MachineBrand
                {
                    Id = componentAttrs.CmOE?.smdid ?? string.Empty,
                    Key = componentAttrs.CmOE?.smdkey ?? string.Empty,
                    Name = componentAttrs.CmOE?.Text?.FirstOrDefault() ?? string.Empty
                },
                Model = new MachineModel
                {
                    Id = componentAttrs.CmModels?.smdid ?? string.Empty,
                    Key = componentAttrs.CmModels?.smdkey ?? string.Empty,
                    Name = componentAttrs.CmModels?.Text?.FirstOrDefault() ?? string.Empty
                },
                Group = groupNew
            };
        }

        public bool AddNewMachine(Machine machine)
        {
            var request = new updateMachineForCustomerIdRequest()
            {
                ID = machine?.Id ?? string.Empty,
                OWNER = machine?.OwnerNumber ?? string.Empty,
                BRAND = machine?.Brand?.Key ?? string.Empty,
                DESCRBRAND = machine?.DescriptionBrand ?? string.Empty,
                MODEL = machine?.Model?.Key ?? string.Empty,
                DESCRMODEL = machine?.DescriptionModel ?? string.Empty,
                INDTYPE = machine?.IndvidualType ?? string.Empty
            };

            var updataResult = _addMachineService.updateMachineForCustomerId(request);
            return updataResult?.individSaveResponse?.IOEnvelope?.FunctionStatus?.Status == "1";
        }

        public List<MachineCategory> GetCategoryListToXml()
        {
            var doc = new XmlDocument();
            var xmlPath = HttpContext.Current.Server.MapPath("~/App_Data/MachineDataXml/intypeepi.xml");
            doc.Load(xmlPath);

            if (doc.DocumentElement == null) return new List<MachineCategory>();

            return (from XmlNode categoryNode in doc.DocumentElement.ChildNodes
                select new MachineCategory
                {
                    Id = categoryNode.ChildNodes[1].InnerText,
                    Key = categoryNode.ChildNodes[5].InnerText,
                    Name = categoryNode.ChildNodes[4].InnerText,
                }).ToList();
        }

        public List<MachineBrand> GetBrandListToXml()
        {
            var doc = new XmlDocument();
            var xmlPath = HttpContext.Current.Server.MapPath("~/App_Data/MachineDataXml/models.xml");
            doc.Load(xmlPath);

            if (doc.DocumentElement == null) return new List<MachineBrand>();

            var brandList = new List<MachineBrand>();
            foreach (XmlNode brandNode in doc.DocumentElement.ChildNodes)
            {
                var modelList = new List<MachineModel>();
                foreach (XmlNode modelNode in brandNode.ChildNodes)
                {
                    if (modelNode.Name == "Object")
                    {
                        modelList.Add(new MachineModel
                        {
                            Id = modelNode.ChildNodes[1].InnerText,
                            Key = modelNode.ChildNodes[5].InnerText,
                            Name = modelNode.ChildNodes[4].InnerText
                        });
                    }
                }
                brandList.Add(new MachineBrand
                {
                    Id = brandNode.ChildNodes[1].InnerText,
                    Key = brandNode.ChildNodes[5].InnerText,
                    Name = brandNode.ChildNodes[4].InnerText,
                    ModelList = modelList
                });
            }
            return brandList;
        }

        public async Task<OrganisationPicture> GetMachinePicUrl(int organisationId, string machineId)
        {
            var result = await _organisationService.GetMachinePicURLAsync(organisationId, machineId, _ticket);
            return result;
        }

        public async Task<bool> CreateMachinePicUrl(int organizationId, string machineId, string picUrl)
        {
            var result = await _organisationService.CreateMachinePicURLAsync(organizationId, machineId, picUrl, _ticket);
            return result != null && result.Id > 0;
        }

        public async Task<bool> DeleteMachinePicUrl(int picId)
        {
            var result = await _organisationService.DeleteMachinePicURLAsync(picId, _ticket);
            return result;
        }

        public async Task<OrganisationPicture[]> FindMachinePictures(int organizationId)
        {
            var result = await _organisationService.FindMachinePicturesAsync(organizationId, Ticket);
            return result;
        }

        public string GetMachineCategoryImage(string categoryId)
        {
            switch (categoryId)
            {
                case MachineCategoryEnum.Tool:
                    return "/Static/images/machine-default-3.jpg";
                case MachineCategoryEnum.Tractor:
                    return "/Static/images/machine-default-1.jpg";
                case MachineCategoryEnum.Tresch:
                    return "/Static/images/machine-default-2.jpg";
                default:
                    return string.Empty;
            }
        }

        public async Task<bool> RemoveMachine(string machineId)
        {
            if (string.IsNullOrEmpty(machineId))
            {
                return false;
            }

            await _deleteMachineService.deleteMachineForCustomerIdAsync(new deleteMachineForCustomerIdRequest
            {
                ID = machineId
            });
            return true;
        }

        public async Task<Machine> GetDetailMachineById(string sysId)
        {
            var result = await _detailMachineBySysIdService.GetMachineBySysIdAsync(new GetMachineBySysIdRequest(sysId));
            if (result?.oneMachineOut?.component == null) return new Machine();

            var componentAttrs = result.oneMachineOut.component.componentAttributes;
            var groupNew = new MachineGroup
            {
                Id = componentAttrs.CmIndividType?.smdid ?? "0",
                Name = componentAttrs.CmIndividType?.Text?.FirstOrDefault() ?? "Ingen kategori",
                Key = componentAttrs.CmIndividType?.smdkey ?? string.Empty
            };

            var images = result.oneMachineOut.component.image;
            var machineImages = images?.Select(i => new MachineImage
            {
                FileName = i.CmFilename?.Text?.Length > 0 ? i.CmFilename?.Text[0] : string.Empty,
                Name = i.CmName?.Text?.Length > 0 ? i.CmName?.Text[0] : string.Empty,
                Id = i.SysId?.Text?.Length > 0 ? i.SysId?.Text[0] : string.Empty,
                ImageType = i.CmImageType?.Text != null && i.CmImageType.Text.Length > 0 ? i.CmImageType.Text[0] : string.Empty
            });

            var documents = result.oneMachineOut.component.document;
            var machineDocuments = documents?.Select(md => new MachineDocument()
            {
                FileName = md.CmFilename?.Text?.Length > 0 ? md.CmFilename?.Text[0] : string.Empty,
                Name = md.CmName?.Text?.Length > 0 ? md.CmName?.Text[0] : string.Empty,
                Id = md.SysId?.Text?.Length > 0 ? md.SysId?.Text[0] : string.Empty,
                Url = md.CmUrl?.Text?.Length > 0 ? md.CmUrl?.Text[0] : string.Empty
            });

            var videos = result.oneMachineOut.component.video;
            var machineVideos = videos?.Select(i => new MachineVideo()
            {
                FileName = i.CmFilename?.Text?.Length > 0 ? i.CmFilename?.Text[0] : string.Empty,
                Name = i.CmName?.Text?.Length > 0 ? i.CmName?.Text[0] : string.Empty,
                Id = i.SysId?.Text?.Length > 0 ? i.SysId?.Text[0] : string.Empty,
                Url = i.CmUrl?.Text?.Length > 0 ? i.CmUrl?.Text[0] : string.Empty
            });

            return new Machine
            {
                Id = componentAttrs.SysId?.Text?.Length > 0 ? componentAttrs.SysId?.Text[0] : string.Empty,
                Type = result.oneMachineOut.component.type,
                Unique = result.oneMachineOut.component.unique,
                ModelName = componentAttrs.MvxSAMODC?.Text?.Length > 0 ? componentAttrs.MvxSAMODC?.Text[0] : string.Empty,
                ModelDescription = componentAttrs.MvxSAMODE?.Text?.Length > 0 ? componentAttrs.MvxSAMODE?.Text[0] : string.Empty,
                IndividualNumber = componentAttrs.MvxSAINNO?.Text?.Length > 0 ? componentAttrs.MvxSAINNO?.Text[0] : string.Empty,
                IndvidualType = componentAttrs.MvxSAINTY?.Text?.Length > 0 ? componentAttrs.MvxSAINTY?.Text[0] : string.Empty,
                LegalRegNumber = componentAttrs.MvxSAREBE?.Text?.Length > 0 ? componentAttrs.MvxSAREBE?.Text[0] : string.Empty,
                OwnerNumber = componentAttrs.MvxSACUOW?.Text?.Length > 0 ? componentAttrs.MvxSACUOW?.Text[0] : string.Empty,
                Fabric = componentAttrs.MvxSABRAN?.value,
                ModelYear = componentAttrs.MvxSAMLYR?.Text?.Length > 0 ? componentAttrs.MvxSAMLYR?.Text[0] : string.Empty,
                ItemGroup = componentAttrs.MvxSAITGR?.Text?.Length > 0 ? componentAttrs.MvxSAITGR?.Text[0] : string.Empty,
                Status = componentAttrs.MvxSAISTS?.Text?.Length > 0 ? componentAttrs.MvxSAISTS?.Text[0] : string.Empty,
                SerialNumber = componentAttrs.MvxSASERI?.Text?.Length > 0 ? componentAttrs.MvxSASERI?.Text[0] : string.Empty,
                DeliveryDate = componentAttrs.MvxSADEDA?.Text?.Length > 0 ? componentAttrs.MvxSADEDA?.Text[0] : string.Empty,
                WarrantyDateSales = componentAttrs.MvxSAGDT1?.Text?.Length > 0 ? componentAttrs.MvxSAGDT1?.Text[0] : string.Empty,
                ItemNumber = componentAttrs.MvxMMITNO?.Text?.Length > 0 ? componentAttrs.MvxMMITNO?.Text[0] : string.Empty,
                ReceiptDate = componentAttrs.MvxSAREDA?.Text?.Length > 0 ? componentAttrs.MvxSAREDA?.Text[0] : string.Empty,
                WarrantyDateSupplier = componentAttrs.MvxSAGDT2?.Text?.Length > 0 ? componentAttrs.MvxSAGDT2?.Text[0] : string.Empty,
                InstallationDate = componentAttrs.MvxSAINDA?.Text?.Length > 0 ? componentAttrs.MvxSAINDA?.Text[0] : string.Empty,
                DateDisposed = componentAttrs.MvxSADDAT?.Text?.Length > 0 ? componentAttrs.MvxSADDAT?.Text[0] : string.Empty,
                Name = componentAttrs.MvxOKCUNM?.Text?.Length > 0 ? componentAttrs.MvxOKCUNM?.Text[0] : string.Empty,
                DescriptionBrand = componentAttrs.CmHeader?.Text?.Length > 0 ? componentAttrs.CmHeader?.Text[0] : string.Empty,
                DescriptionModel = componentAttrs.CmSubHeader?.Text?.Length > 0 ? componentAttrs.CmSubHeader?.Text[0] : string.Empty,
                Images = machineImages?.ToList() ?? new List<MachineImage>(),
                Documents = machineDocuments?.ToList() ?? new List<MachineDocument>(),
                Videos = machineVideos?.ToList() ?? new List<MachineVideo>(),
                Brand = new MachineBrand
                {
                    Id = componentAttrs.CmOE?.smdid ?? string.Empty,
                    Key = componentAttrs.CmOE?.smdkey ?? string.Empty,
                    Name = componentAttrs.CmOE?.Text?.Length > 0 ? componentAttrs.CmOE?.Text[0] : string.Empty
                },
                Model = new MachineModel
                {
                    Id = componentAttrs.CmModels?.smdid ?? string.Empty,
                    Key = componentAttrs.CmModels?.smdkey ?? string.Empty,
                    Name = componentAttrs.CmModels?.Text?.Length > 0 ? componentAttrs.CmModels?.Text[0] : string.Empty
                },
                Group = groupNew
            };
        }
    }
}
