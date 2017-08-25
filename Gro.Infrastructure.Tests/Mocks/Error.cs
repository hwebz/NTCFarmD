using System;
using System.Threading.Tasks;

namespace Gro.Infrastructure.Tests.Mocks
{
    public interface IError
    {
        void MakeError();
        Task MakeErrorAsync();
        int MakeIntError();
        Task<int> MakeIntErrorAsync();
        string MakeStringError();
        Task<string> MakeStringErrorAsync();
        int MakeNoError(int n);
        Task<int> MakeNoErrorAsync(int n);
    }

    public class Error : IError
    {
        public void MakeError()
        {
            throw new NotSupportedException();
        }

        public async Task MakeErrorAsync()
        {
            await Task.Delay(1);
            throw new NotSupportedException();
        }

        public int MakeIntError()
        {
            throw new NotSupportedException();
        }

        public async Task<int> MakeIntErrorAsync()
        {
            await Task.Delay(1);
            throw new NotSupportedException();
        }

        public int MakeNoError(int n) => n;

        public async Task<int> MakeNoErrorAsync(int n)
        {
            await Task.Delay(1);
            return n;
        }

        public string MakeStringError()
        {
            throw new NotSupportedException();
        }

        public async Task<string> MakeStringErrorAsync()
        {
            await Task.Delay(1);
            throw new NotSupportedException();
        }
    }
}
