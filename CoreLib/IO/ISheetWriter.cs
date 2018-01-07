using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IO
{
    public interface ISheetWriter
    {
        Task WriteToFile(Sheet sheet);
        Task<string> WriteToString(Sheet sheet);

        void SetFilePath(string filePath);
    }
}
