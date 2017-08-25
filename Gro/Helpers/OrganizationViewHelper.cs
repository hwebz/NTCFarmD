using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Gro.Core.DataModels.Organization;
using Gro.Core.DataModels.Security;
using Gro.ViewModels.Pages.Organization;
using System.Threading.Tasks;
using Gro.Controllers;
using EPiServer.Core;
using Newtonsoft.Json;
using Gro.Core.Interfaces;
using Gro.ViewModels.Organization;

namespace Gro.Helpers
{
    public static class OrganizationViewHelper
    {
        public static AddressViewModel PopulateAdressModels(CustomerDeliveryAddress address, DeliveryReceiver[] notificationRecceivers, User[] allReceivers)
        {
            var model = new AddressViewModel
            {
                Silos = address.Silos.IsNullOrEmpty() ? new List<SiloItem>() : address.Silos.ToList(),
                AdressStreet = address.AddressRow1,
                PhoneNumber = address.PhoneNumber,
                ZipCode = address.ZipCode,
                Place = address.AddressRow3,
                MobileNumber = address.MobileNumber,
                Longitude = address.Longitude,
                Latitude = address.Latitude,
                Direction = address.Directions,
                AddressNumber = address.AddressNumber,
                CustomerName = address.CustomerName,
                CustomerNumber = address.CustomerNumber,
                FaxNumber = address.FaxNumber,
                AddressRow2 = address.AddressRow2,
                Country = address.Country,
                AddressRow4 = address.AddressRow4,
                Notifications = notificationRecceivers.IsNullOrEmpty() ? new List<DeliveryReceiver>() : notificationRecceivers.ToList(),
                Receivers = allReceivers.IsNullOrEmpty() ? new List<User>() : allReceivers.ToList()
            };
            return model;
        }

        public static CustomerDeliveryAddress PopulateCustomerDeliveryAddress(AddressViewModel address)
        {
            return new CustomerDeliveryAddress()
            {
                Silos = PopulateSilor(address.Silos),
                Latitude = address.Latitude,
                Longitude = address.Longitude,
                AddressNumber = address.AddressNumber,
                AddressRow1 = address.AdressStreet,
                AddressRow2 = address.AddressRow2,
                AddressRow3 = address.Place,
                AddressRow4 = address.AddressRow4,
                Country = address.Country,
                MobileNumber = address.MobileNumber,
                PhoneNumber = address.PhoneNumber,
                ZipCode = address.ZipCode,
                CustomerName = address.CustomerName,
                CustomerNumber = address.CustomerNumber,
                FaxNumber = address.FaxNumber,
                Directions = address.Direction
            };
        }

        private static SiloItem[] PopulateSilor(List<SiloItem> silos)
        {
            if (silos.IsNullOrEmpty())
            {
                return new SiloItem[0];
            }
            var result = new List<SiloItem>();
            foreach (var siloItem in silos)
            {
                siloItem.Description = $"Silo {siloItem.Number}";
                result.Add(siloItem);
            }
            return result.ToArray();
        }

        public static List<int> GetListChoosenReceiverIds(List<NotificationReceiverModel> notificationReceiverModels)
        {
            var listChoosenIds = new List<int>();
            if (notificationReceiverModels.IsNullOrEmpty())
            {
                return listChoosenIds;
            }
            foreach (var receiver in notificationReceiverModels.Where(receiver => !string.IsNullOrEmpty(receiver.Choosen)))
            {
                bool choosen;
                if (bool.TryParse(receiver.Choosen, out choosen) && choosen)
                {
                    listChoosenIds.Add(receiver.UserId);
                }
            }
            return listChoosenIds;
        }

        public static List<SiloItem> PopulateSilos(List<SiloItem> silos)
        {
            if (silos.IsNullOrEmpty()) return silos;
            foreach (var siloItem in silos)
            {
                siloItem.Description = $"Silo {siloItem.Number}";
            }
            return silos;
        }

        public static async Task GetAllRolesTask<T>(this SiteControllerBase<T> siteController,
            ISecurityRepository securityRepository) where T : PageData
        {
            var roles = await securityRepository.GetAllRolesAsync();
            siteController.ViewData["roles"] = JsonConvert.SerializeObject(roles
                .Where(r => r.Sysrole == false));
        }

        public static async Task<RoleProfileViewModel[]> GetRolesAndProfiles<T>(this SiteControllerBase<T> siteController, ISecurityRepository securityRepository)
            where T : PageData
        {
            var profiles = await securityRepository.GetRoleProfilesAsync() ?? new Core.DataModels.Security.Profile[0];

            var getRolesTasks = profiles.Select(async p =>
            {
                var rls = await securityRepository.GetRolesOfProfileAsync(p.Id);

                //put 'basic' right on top
                ProfileRole basicRight = null;
                var basicRightIndex = -1;
                for (var i = 0; i < rls.Length; i++)
                {
                    if (rls[i].RoleName != "Basfunktioner") continue;
                    basicRight = rls[i];
                    basicRightIndex = i;
                    break;
                }
                if (basicRight == null)
                {
                    return new RoleProfileViewModel
                    {
                        Id = p.Id,
                        Description = p.Description,
                        ProfileRoles = rls
                    };
                }

                rls[basicRightIndex] = rls[0];
                rls[0] = basicRight;

                return new RoleProfileViewModel
                {
                    Id = p.Id,
                    Description = p.Description,
                    ProfileRoles = rls
                };
            });

            var profileRoles = await Task.WhenAll(getRolesTasks);
            siteController.ViewData["profileRoles"] = JsonConvert.SerializeObject(profileRoles);

            return profileRoles;
        }
    }
}
