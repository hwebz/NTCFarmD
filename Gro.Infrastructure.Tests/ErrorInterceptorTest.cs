using Gro.Infrastructure.Data;
using Gro.Infrastructure.Data.Interceptors;
using Gro.Infrastructure.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Gro.Infrastructure.Tests
{
    [TestClass]
    public class ErrorInterceptorTest
    {
        private static IError GetErrorProxy() => new RepositoryBuilder<IError>(new Error())
            .AddErrorHandler(new FakeLogger(), new Type[0])
            .Build();

        //This should not throw anything
        [TestMethod]
        public void VoidError()
        {
            var error = GetErrorProxy();
            error.MakeError();
        }

        //This should not throw anything
        [TestMethod]
        public async Task VoidErrorAsync()
        {
            var error = GetErrorProxy();
            await error.MakeErrorAsync();
        }

        [TestMethod]
        public void ValueTypeError()
        {
            var error = GetErrorProxy();
            var r = error.MakeIntError();
            Assert.AreEqual(r, default(int));
        }

        [TestMethod]
        public async Task ValueTypeErrorAsync()
        {
            var error = GetErrorProxy();
            var r = await error.MakeIntErrorAsync();
            Assert.AreEqual(r, default(int));
        }

        [TestMethod]
        public void ReferenceTypeError()
        {
            var error = GetErrorProxy();
            var r = error.MakeStringError();
            Assert.AreEqual(r, default(string));
        }

        [TestMethod]
        public async Task ReferenceTypeErrorAsync()
        {
            var error = GetErrorProxy();
            var r = await error.MakeStringErrorAsync();
            Assert.AreEqual(r, default(string));
        }

        [TestMethod]
        public void NoError()
        {
            var rand = new Random();
            var number = rand.Next(0, 1000);
            var error = GetErrorProxy();
            var r = error.MakeNoError(number);
            Assert.AreEqual(r, number);
        }

        [TestMethod]
        public async Task NoErrorAsync()
        {
            var rand = new Random();
            var number = rand.Next(0, 1000);
            var error = GetErrorProxy();
            var r = await error.MakeNoErrorAsync(number);
            Assert.AreEqual(r, number);
        }
    }
}
