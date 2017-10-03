using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Core.IO.Internal
{
    [MusicReader(".ly", nameof(LilipondMusicReader))]
    class LilipondMusicReader : IMusicReader
    {
        public Task<Sheet> ReadFromFileAsync(string filePath, string name = null)
        {
            throw new NotImplementedException();
        }
    }
}
