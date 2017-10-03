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
        public Task<Sheet> ReadFromFileAsync()
        {
            throw new NotImplementedException();
        }

        public void SetFilePath(string path)
        {
            throw new NotImplementedException();
        }
    }
}
