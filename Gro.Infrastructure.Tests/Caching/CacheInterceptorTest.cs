using System.Collections.Generic;
using Gro.Infrastructure.Data;
using Gro.Infrastructure.Data.Interceptors;
using Gro.Infrastructure.Data.Interceptors.Attributes;
using Gro.Infrastructure.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Gro.Infrastructure.Data.Caching;

namespace Gro.Infrastructure.Tests.Caching
{
    internal class DictionaryCache : IMemoryCache
    {
        private readonly Dictionary<string, object> _dict = new Dictionary<string, object>();

        public object this[string key] => _dict[key];

        public void CreateOrSet(string key, object value, CacheOptions cacheOptions) => _dict[key] = value;

        public void Remove(string key) => _dict.Remove(key);

        public bool TryGetValue(string key, out object value) => _dict.TryGetValue(key, out value);

        public bool KeyExists(string key) => _dict.ContainsKey(key);
    }

    public interface IService
    {
        object ReadNoParam();
        Task<object> ReadNoParamAsync();
        object ReadWithParams(object param1, object param2);
        void WriteNoParam();
        Task WriteNoParamAsync();
        void WriteWithParams(object param1, object param2);
        object ReadWithCustomKey(object param);
        void WriteWithCustomKey(object param);
        object ReadNoCache(object param);

        object[] ReadReturnEmptyArray();

        Task<object[]> ReadReturnEmptyArrayAsync();
    }

    public class ServiceImplementation : IService
    {
        [Cache]
        public object ReadNoParam() => nameof(ReadNoParam);

        [Cache]
        public async Task<object> ReadNoParamAsync()
        {
            await Task.Delay(1);
            return nameof(ReadNoParam);
        }

        [Cache]
        public object ReadWithParams(object param1, object param2) => $"{nameof(ReadWithParams)}{nameof(param1)}{nameof(param2)}";

        [Cache(Key = "Custom_{param}")]
        public object ReadWithCustomKey(object param) => $"{nameof(ReadWithCustomKey)}{param}";

        [CacheInvalidate("ReadNoParam")]
        public void WriteNoParam()
        {
        }

        [CacheInvalidate("ReadNoParam")]
        public async Task WriteNoParamAsync()
        {
            await Task.Delay(1);
        }

        [CacheInvalidate("ReadWithParams_{param1}_{param2}")]
        public void WriteWithParams(object param1, object param2)
        {
        }

        [CacheInvalidate("Custom_{param}")]
        public void WriteWithCustomKey(object param)
        {
        }

        [Cache]
        public object[] ReadReturnEmptyArray() => new object[0];

        [Cache]
        public async Task<object[]> ReadReturnEmptyArrayAsync()
        {
            await Task.Delay(1);
            return new object[0];
        }

        public object ReadNoCache(object param) => nameof(param);
    }

    [TestClass]
    public class CacheInterceptorTest
    {
        private readonly ServiceImplementation _service = new ServiceImplementation();
        private DictionaryCache GetMemoryCache() => new DictionaryCache();

        private IService GetServiceProxy(IMemoryCache cache)
            => new RepositoryBuilder<IService>(new ServiceImplementation())
                .AddCache(cache, new FakeLogger(), new CacheOptions())
                .Build();

        [TestMethod]
        public void ReadNoCacheTest()
        {
            var cache = GetMemoryCache();
            var service = GetServiceProxy(cache);
            service.ReadNoParam();
            const string param = "param";

            var serviceResult = _service.ReadNoCache(param);
            var proxyResult = service.ReadNoCache(param);

            Assert.AreEqual(serviceResult, proxyResult);
        }

        [TestMethod]
        public void ReadNoParamTest()
        {
            var cache = GetMemoryCache();
            var service = GetServiceProxy(cache);
            var cacheValue = service.ReadNoParam();

            Assert.AreEqual(cache[nameof(ServiceImplementation.ReadNoParam)], cacheValue);
        }

        [TestMethod]
        public async Task ReadNoParamAsyncTest()
        {
            var cache = GetMemoryCache();
            var service = GetServiceProxy(cache);
            var cacheValue = await service.ReadNoParamAsync();

            Assert.AreEqual(cache[nameof(ServiceImplementation.ReadNoParam)], cacheValue);
        }

        [TestMethod]
        public void ReadWithParamsTest()
        {
            var cache = GetMemoryCache();
            var service = GetServiceProxy(cache);
            var cacheValue = service.ReadWithParams("1", 2);

            Assert.AreEqual(cache[$"{nameof(ServiceImplementation.ReadWithParams)}_1_2"], cacheValue);
        }

        [TestMethod]
        public void WriteNoParamTest()
        {
            var cache = GetMemoryCache();
            var service = GetServiceProxy(cache);

            service.ReadNoParam();
            Assert.IsTrue(cache.KeyExists("ReadNoParam"));

            service.WriteNoParam();
            Assert.IsFalse(cache.KeyExists("ReadNoParam"));
        }

        [TestMethod]
        public async Task WriteNoParamAsyncTest()
        {
            var cache = GetMemoryCache();
            var service = GetServiceProxy(cache);

            await service.ReadNoParamAsync();
            Assert.IsTrue(cache.KeyExists("ReadNoParam"));

            await service.WriteNoParamAsync();
            Assert.IsFalse(cache.KeyExists("ReadNoParam"));
        }

        [TestMethod]
        public void WriteWithParamsTest()
        {
            var cache = GetMemoryCache();
            var service = GetServiceProxy(cache);
            service.ReadWithParams("1", 2);
            service.WriteWithParams("1", 2);

            Assert.IsFalse(cache.KeyExists("ReadWithParams_1_2"));
        }

        [TestMethod]
        public void WriteWithParamsDifferentArguments()
        {
            var cache = GetMemoryCache();
            var service = GetServiceProxy(cache);
            service.ReadWithParams("1", 2);
            service.WriteWithParams("1", 3);

            Assert.IsTrue(cache.KeyExists("ReadWithParams_1_2"));
        }

        [TestMethod]
        public void ReadWithCustomKeyTest()
        {
            var param = "param";
            var cache = GetMemoryCache();
            var service = GetServiceProxy(cache);
            var cacheValue = service.ReadWithCustomKey(param);

            var cacheKey = $"Custom_{param}";
            Assert.AreEqual(cache[cacheKey], cacheValue);
        }

        [TestMethod]
        public void WriteWithCustomKey()
        {
            var param = "param";
            var cache = GetMemoryCache();
            var service = GetServiceProxy(cache);
            var cacheValue = service.ReadWithCustomKey(param);
            var cacheKey = $"Custom_{param}";

            service.WriteWithCustomKey(param);
            Assert.IsFalse(cache.KeyExists(cacheKey));
        }

        [TestMethod]
        public void ReturnEmptyArrayTest()
        {
            var cache = GetMemoryCache();
            var service = GetServiceProxy(cache);
            var a = service.ReadReturnEmptyArray();
            var cacheKey = nameof(IService.ReadReturnEmptyArray);
            Assert.IsFalse(cache.KeyExists(cacheKey));
        }
    }
}
