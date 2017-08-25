using Gro.Core.DataModels.MessageHubDtos;
using Gro.Infrastructure.Data.MessageHubService;
using Gro.Infrastructure.Data.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading.Tasks;

namespace Gro.Infrastructure.Tests.Repositories
{
    [TestClass]
    public class UserMessageSettingsRepositoryTests
    {
        [TestMethod]
        public async Task GetSettingDisplay_NoCategory()
        {
            var serviceMock = new Mock<IMessagehubService>();
            serviceMock
                .Setup(s => s.GetMsgSettingsDisplayAsync(1, 2))
                .Returns(Task.FromResult(new MessageSettingsDisplay
                {
                    SettingsTab = new MessageSettingTab[] {
                        new MessageSettingTab {
                            DisplayName = "Test",
                            DisplayOrder=1,
                            Category = new Category[0]
                        }
                    }
                }));

            var service = serviceMock.Object;
            var repo = new UserMessageSettingsRepository(service);
            var result = await repo.GetSettingDisplayAsync(1, 2);
            Assert.AreEqual(0, result.First().Category.Count());
        }

        [TestMethod]
        public async Task GetSettingDisplay_SingleEmptyCategory()
        {
            var serviceMock = new Mock<IMessagehubService>();
            serviceMock
                .Setup(s => s.GetMsgSettingsDisplayAsync(1, 2))
                .Returns(Task.FromResult(new MessageSettingsDisplay
                {
                    SettingsTab = new MessageSettingTab[] {
                        new MessageSettingTab {
                            DisplayName = "Test",
                            DisplayOrder=1,
                            Category = new Category[] {
                                new Category {
                                    Categoryid=1,
                                    CategoryName="TestCategory1",
                                    MessageType = new MsgType[0]
                                }
                            }
                        }
                    }
                }));

            var service = serviceMock.Object;
            var repo = new UserMessageSettingsRepository(service);
            var result = await repo.GetSettingDisplayAsync(1, 2);
            Assert.AreEqual(1, result.First().Category.Count());
        }

        [TestMethod]
        public async Task GetSettingDisplay_UniqueCategories()
        {
            var serviceMock = new Mock<IMessagehubService>();
            serviceMock
                .Setup(s => s.GetMsgSettingsDisplayAsync(1, 2))
                .Returns(Task.FromResult(new MessageSettingsDisplay
                {
                    SettingsTab = new MessageSettingTab[] {
                        new MessageSettingTab {
                            DisplayName = "Test",
                            DisplayOrder=1,
                            Category = new Category[] {
                                new Category {
                                    Categoryid=1,
                                    CategoryName="TestCategory1",
                                    MessageType = new MsgType[0]
                                },
                                new Category
                                {
                                    Categoryid=2,
                                    CategoryName="TestCategory2",
                                    MessageType = new MsgType[1]
                                    {
                                        new MsgType {Id = 1}
                                    }
                                },
                                new Category
                                {
                                    Categoryid=3,
                                    CategoryName="TestCategory3",
                                    MessageType = new MsgType[1]
                                    {
                                        new MsgType {Id = 2}
                                    }
                                },
                                new Category
                                {
                                    Categoryid=4,
                                    CategoryName="TestCategory4",
                                    MessageType = new MsgType[1]
                                    {
                                        new MsgType {Id = 3}
                                    }
                                }
                            }
                        }
                    }
                }));

            var service = serviceMock.Object;
            var repo = new UserMessageSettingsRepository(service);
            var result = await repo.GetSettingDisplayAsync(1, 2);
            Assert.AreEqual(4, result.First().Category.Count());
        }

        [TestMethod]
        public async Task GetSettingDisplay_DuplicateCategories()
        {
            var serviceMock = new Mock<IMessagehubService>();
            serviceMock
                .Setup(s => s.GetMsgSettingsDisplayAsync(1, 2))
                .Returns(Task.FromResult(new MessageSettingsDisplay
                {
                    SettingsTab = new MessageSettingTab[] {
                        new MessageSettingTab {
                            DisplayName = "Test",
                            DisplayOrder=1,
                            Category = new Category[] {
                                new Category {
                                    Categoryid=1,
                                    CategoryName="TestCategory1",
                                    MessageType = new MsgType[0]
                                },
                                new Category
                                {
                                    Categoryid=2,
                                    CategoryName="TestCategory2",
                                    MessageType = new MsgType[1]
                                    {
                                        new MsgType {Id = 1}
                                    }
                                },
                                new Category
                                {
                                    Categoryid=1,
                                    CategoryName="TestCategory1",
                                    MessageType = new MsgType[1]
                                    {
                                        new MsgType {Id = 2}
                                    }
                                },
                                new Category
                                {
                                    Categoryid=2,
                                    CategoryName="TestCategory2",
                                    MessageType = new MsgType[1]
                                    {
                                        new MsgType {Id = 3}
                                    }
                                }
                            }
                        }
                    }
                }));

            var service = serviceMock.Object;
            var repo = new UserMessageSettingsRepository(service);
            var result = await repo.GetSettingDisplayAsync(1, 2);
            Assert.AreEqual(1, result.First().Category.First().MessageType.Count());
            Assert.AreEqual(2, result.First().Category.ToArray()[1].MessageType.Count());
        }
    }
}
