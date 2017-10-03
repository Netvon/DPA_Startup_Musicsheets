using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.IO
{
    public interface IMusicReader
    {
        /// <summary>
        /// Reads a Sheet from a file asynchronously.
        /// </summary>
        /// <param name="filePath">The file to read</param>
        /// <param name="name">The name to give the music sheet, leave null to use the file name.</param>
        /// <returns></returns>
        Task<Sheet> ReadFromFileAsync(string filePath, string name = null);
    }
}
