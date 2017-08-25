using System.IO;
using System.Threading.Tasks;

namespace Gro.Core.Interfaces
{
    public interface IFileRepository
    {
        /// <summary>
        /// Create a file and return an url
        /// </summary>
        /// <param name="fileStream">The file content stream</param>
        /// <param name="fileExtension">File extension with dot</param>
        /// <param name="fileGroups"></param>
        /// <param name="owner"></param>
        /// <returns>File url</returns>
        Task<string> SaveAsync(Stream fileStream, string fileExtension, string fileGroups, string owner = "system");

        /// <summary>
        /// Delete a file
        /// </summary>
        Task DeleteAsync(string url);
    }
}
