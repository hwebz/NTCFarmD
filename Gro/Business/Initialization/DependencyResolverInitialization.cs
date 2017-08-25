using System;
using System.Configuration;
using System.Web.Mvc;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Gro.Business.Caching;
using Gro.Business.DataProtection;
using Gro.Core.Interfaces;
using Gro.Infrastructure.Data;
using Gro.Infrastructure.Data.AgreementService;
using Gro.Infrastructure.Data.DeliveryAssuranceService;
using Gro.Infrastructure.Data.DeliveryNoteService;
using Gro.Infrastructure.Data.GrobarhetService;
using Gro.Infrastructure.Data.Interceptors;
using Gro.Infrastructure.Data.MessageHubService;
using Gro.Infrastructure.Data.PrisgrafService;
using Gro.Infrastructure.Data.Repositories;
using Gro.Infrastructure.Data.SecurityService;
using Gro.Infrastructure.Data.ShippingService;
using Gro.Infrastructure.Data.WeighInService;
using Gro.Business.Services.News;
using Gro.Business.Services.Users;
using Gro.Infrastructure.Data.PersonService;
using Gro.Infrastructure.Data.SessionService;
using log4net;
using StructureMap;
using System.Web.Configuration;
using System.ServiceModel;
using Gro.Infrastructure.Data.UserTermsOfUseService;
using Gro.Infrastructure.Data.EmailService;
using Gro.Infrastructure.Data.GrainService;
using Gro.Infrastructure.Data.MachineService;
using StructureMap.Web.Pipeline;
using Gro.Infrastructure.Data.OrganisationService;
using Gro.Infrastructure.Data.MachineAddNewServer;
using Gro.Infrastructure.Data.Caching;
using Gro.Infrastructure.Data.MachineRemoveService;
using Gro.Infrastructure.Data.MachineDetailByReg;
using Gro.Core.ContentTypes.Media;
using Gro.Core.ContentTypes.Business;
using Gro.Infrastructure.Data.RequestService;
using Gro.Infrastructure.Data.MachineDetailById;
using Gro.Infrastructure.Data.PurchasingMobileService;
using Newtonsoft.Json;
using System.Collections.Generic;
using Gro.Business.Services.Machines;
using Gro.Infrastructure.Data.LogiWebService;
using Gro.Infrastructure.Data.BokaService;
using Gro.Infrastructure.Data.Boka;
using Gro.Infrastructure.Data.Centralen_Customer;
using Gro.Infrastructure.Data.Centralen_Items;
using Gro.Infrastructure.Data.PurchasingAgreementService;
using Gro.Infrastructure.Data.Lm2CustomerSupportService;
using Gro.Infrastructure.Data.LegacyCustomerSupportService;

namespace Gro.Business.Initialization
{
    [InitializableModule]
    public class DependencyResolverInitialization : IConfigurableModule
    {
        public static string EndpointsSerialized;

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Container.Configure(ConfigureContainer);
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(context.Container));
        }

        private static void ConfigureContainer(ConfigurationExpression container)
        {
            container.For<INewsService>().Use<NewsService>();
            container.For<IGroContentDataService>().Use<GroContentDataService>();
            container.For<IGroMachineService>().Use<GroMachineService>();

            //wcf services
            container
                .UseService<WSSessionService, WSSessionServiceClient>()
                .UseService<WSRequestService, WSRequestServiceClient>()
                .UseService<WSPersonService, WSPersonServiceClient>()
                .UseService<IWeighInService, WeighInServiceClient>()
                .UseService<IAgreementService, AgreementServiceClient>()
                .UseService<IFoljesedelService, FoljesedelServiceClient>()
                .UseService<IGrobarhetService, GrobarhetServiceClient>()
                .UseService<IPrisgrafService, PrisgrafServiceClient>()
                .UseService<IDeliveryAssuranceService, DeliveryAssuranceServiceClient>()
                .UseService<IFraktService, FraktServiceClient>()
                .UseService<IMessagehubService, MessagehubServiceClient>()
                .UseService<ISecurityService, SecurityServiceClient>()
                .UseService<IUserTermsOfUseService, UserTermsOfUseServiceClient>()
                .UseService<ILM2OrganisationService, LM2OrganisationServiceClient>()
                .UseService<IEmailService, EmailServiceClient>()
                .UseService<IGrainService, GrainServiceClient>()
                .UseService<IPurchasingMobileService, PurchasingMobileServiceClient>()
                .UseService<getAllMachinesForCustomerId_PortType, getAllMachinesForCustomerId_PortTypeClient>()
                .UseService<updateMachineForCustomerId_PortType, updateMachineForCustomerId_PortTypeClient>()
                .UseService<GetMachineByRegNumber_PortType, GetMachineByRegNumber_PortTypeClient>()
                .UseService<deleteMachineForCustomerId_PortType, deleteMachineForCustomerId_PortTypeClient>()
                .UseService<getMachineBySysId_PortType, getMachineBySysId_PortTypeClient>()
                .UseService<ILogiWebService, LogiWebServiceClient>()
                .UseService<BokaSoap, BokaSoapClient>()
                .UseService<BokaServiceSoap, BokaServiceSoapClient>()
                .UseService<CustomerSoap, CustomerSoapClient>()
                .UseService<ItemsSoap, ItemsSoapClient>()
                .UseService<IPurchasingAgreementService, PurchasingAgreementServiceClient>()
                .UseService<ILM2CustomerSupportService, LM2CustomerSupportServiceClient>()
                .UseService<ICustomerSupportService, CustomerSupportServiceClient>();
            //ticket provider
            var ticketCache = new GroRuntimeCache(nameof(TicketProvider));
            container
                .For<TicketProvider>()
                .Use<TicketProvider>()
                .Ctor<IMemoryCache>().Is(ticketCache)
                .Ctor<string>("ticketUserName").Is(ConfigurationManager.AppSettings["TicketUserName"])
                .Ctor<string>("ticketPassword").Is(ConfigurationManager.AppSettings["TicketPassword"])
                .Ctor<string>("ticketPrivateKey").Is(ConfigurationManager.AppSettings["TicketPrivateKey"])
                .Ctor<string>("wsAdminName").Is(ConfigurationManager.AppSettings["PersonSession.AdminName"])
                .Ctor<string>("wsAdminPassword").Is(ConfigurationManager.AppSettings["PersonSession.Password"])
                .Singleton();

            //data protection
            container
                .For<ITokenGenerator>()
                .Use<JwtTokenGenerator>()
                .Ctor<string>("key").Is((ConfigurationManager.GetSection("system.web/machineKey") as MachineKeySection)?.DecryptionKey)
                .Singleton();

            //media config
            container
                .For<MediaConfig>()
                .Use<MediaConfig>()
                .Ctor<string>("customerFolder").Is(ConfigurationManager.AppSettings["CustomerFolder"] ?? string.Empty)
                .Ctor<string>("userFolder").Is(ConfigurationManager.AppSettings["UserFolder"] ?? string.Empty)
                .Ctor<string>("machineFolder").Is(ConfigurationManager.AppSettings["MachineFolder"] ?? string.Empty)
                .Ctor<string>("migrateFolderPath").Is(ConfigurationManager.AppSettings["MigrateFolderPath"] ?? string.Empty)
                .Ctor<string>("imageTypes").Is(ConfigurationManager.AppSettings["ImageTypes"] ?? string.Empty)
                .Ctor<string>("documentTypes").Is(ConfigurationManager.AppSettings["DocumentTypes"] ?? string.Empty)
                .Singleton();

            container.UseRepository<IPriceGraphRepository, PriceGraphRepository>(new CacheOptions
            {
                Strategy = CacheStrategy.Absolute,
                ExpirationTimeFromNow = TimeSpan.FromMinutes(20)
            });

            //repos
            container.For<IUserManagementService>().Use<UserManagementService>();
            container.For<IAccountRepository>().Use<AccountRepository>();
            container.For<IFileRepository>().Use<EpiserverFileRepository>();
            container.UseRepository<ISecurityRepository, SecurityRepository>(typeof(FaultException));
            container.UseRepository<IWeighInRepository, WeighInRepository>();
            container.UseRepository<IAgreementRepository, AgreementRepository>();
            container.UseRepository<IDeliveryNoteRepository, DeliveryNoteRepository>();
            container.UseRepository<IGrobarhetRepository, GrobarhetRepository>();
            container.UseRepository<IDeliveryAssuranceRepository, DeliveryAssuranceRepository>();
            container.UseRepository<IShippingRepository, ShippingRepository>();
            container.UseRepository<IUserMessageSettingsRepository, UserMessageSettingsRepository>();
            container.UseRepository<IMessageAdministrationRepository, MessageAdministrationRepository>();
            container.UseRepository<IUserMessageRepository, UserMessageRepository>();
            container.UseRepository<IUserTermsOfUseRepository, UserTermsOfUseRepository>();
            container.UseRepository<IOrganizationRepository, OrganizationRepository>();
            container.UseRepository<IGrainRepository, GrainRepository>();
            container.UseRepository<IMachineRepository, MachineRepository>();
            container.UseRepository<IMediaRepository, FileBlobRepository>();
            container.UseRepository<IBookingContactRepository, BookingContactRepository>();
            container.UseRepository<IOrganizationUserRepository, OrganizationUserRepository>(typeof(Exception));
            container.UseRepository<ISearchTransportRepository, SearchTransportRepository>();
            container.UseRepository<IBokaRepository, BokaRepository>();
            container.UseRepository<IPurchasingAgreementRepository, PurchasingAgreementRepository>();
            container.UseRepository<ICustomerSupportRepository, CustomerSupportRepository>(typeof(Exception));
        }

        public void Initialize(InitializationEngine context)
        {
            var endpoints = new Dictionary<string, string>();
            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                if (!key.StartsWith("Endpoint")) continue;

                endpoints[key.Substring(key.IndexOf("Endpoint", StringComparison.Ordinal))] = ConfigurationManager.AppSettings[key];
            }
            EndpointsSerialized = JsonConvert.SerializeObject(endpoints);
        }

        public void Uninitialize(InitializationEngine context)
        {
        }

        public void Preload(string[] parameters)
        {
        }
    }

    public static class RepositoryConfigurationExtension
    {
        private static IMemoryCache GetGroMemoryCache(string name) => new GroRuntimeCache(name);
        private static CacheOptions GetDefaultCacheOptions() => new CacheOptions
        {
            Strategy = CacheStrategy.Absolute,
            ExpirationTimeFromNow = TimeSpan.FromMinutes(20)
        };

        public static void UseRepository<TInterface, TImplementation>(this ConfigurationExpression container, params Type[] thrownableExceptions)
            where TInterface : class
            where TImplementation : TInterface
            => UseRepository<TInterface, TImplementation>(container, GetDefaultCacheOptions(), thrownableExceptions);


        public static void UseRepository<TInterface, TImplementation>(this ConfigurationExpression container, CacheOptions cacheOptions, params Type[] thrownableExceptions)
            where TInterface : class
            where TImplementation : TInterface
        {
            var log = LogManager.GetLogger(typeof(TImplementation));
            container.For<TInterface>().Use(typeof(TInterface).Name, c =>
            {
                var builder = new RepositoryBuilder<TInterface>(c.GetInstance<TImplementation>());
                builder.AddCache(GetGroMemoryCache(typeof(TImplementation).Name), log, cacheOptions);
                builder.AddErrorHandler(log, thrownableExceptions);
                var result = builder.Build();
                return result;
            }).LifecycleIs<HttpContextLifecycle>();
        }
    }

    public static class ServiceConfigurationExtensions
    {
        public static ConfigurationExpression UseService<TInterface, TService>(this ConfigurationExpression container)
            where TService : TInterface
        {
            container.For<TInterface>().Use("services", () =>
            {
                var interfaceName = typeof(TInterface).Name;
                var endpoint = ConfigurationManager.AppSettings[$"Endpoint.{interfaceName}"];
                bool https;

                if (endpoint.StartsWith("https")) https = true;
                else if (endpoint.StartsWith("http")) https = false;
                else throw new ArgumentException($"Invalid transport protocol: {endpoint}");

                var basicbinding = new BasicHttpBinding
                {
                    Security = new BasicHttpSecurity
                    {
                        Mode = https ? BasicHttpSecurityMode.Transport : BasicHttpSecurityMode.None
                    },
                    MaxReceivedMessageSize = 20000000,
                    ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas
                    {
                        MaxDepth = 2000000000,
                        MaxArrayLength = 2000000000,
                        MaxBytesPerRead = 2000000000,
                        MaxStringContentLength = 2000000000,
                        MaxNameTableCharCount = 2000000000
                    }
                };

                // bypass proxy.
                //basicbinding.ProxyAddress = new Uri("http://127.0.0.1:3128");
                //basicbinding.UseDefaultWebProxy = false;
                var address = new EndpointAddress(endpoint);
                var instance = (TService)Activator.CreateInstance(typeof(TService), basicbinding, address);
                return instance;
            }).LifecycleIs<HttpContextLifecycle>();

            return container;
        }
    }
}
