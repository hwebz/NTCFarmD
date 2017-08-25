namespace Gro.Business.DataProtection
{
    /// <summary>
    /// Generating a token representing protected data
    /// </summary>
    public interface ITokenGenerator
    {
        string Encrypt<T>(T data);
        T Decrypt<T>(string token);
    }
}
