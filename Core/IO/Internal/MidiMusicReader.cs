using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Core.IO.Internal
{
    [MusicReader("(.mid|.midi)", nameof(MidiMusicReader))]
    class MidiMusicReader : IMusicReader
    {
        public Task<Sheet> ReadFromFileAsync(string filePath, string name = null)
        {
            throw new NotImplementedException();
        }
    }
}
