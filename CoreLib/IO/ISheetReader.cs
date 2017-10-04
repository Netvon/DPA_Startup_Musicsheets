using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.IO
{
    public interface ISheetReader
    {
        /// <summary>
        /// Reads a Sheet from a file asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<Sheet> ReadFromFileAsync();

        void SetFilePath(string path);
    }
}
