using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Sanford.Multimedia.Midi;
using System.Threading;

namespace Core.IO.Internal
{
    [MusicReader("(.mid|.midi)", nameof(MidiMusicReader))]
    class MidiMusicReader : IMusicReader
    {
        private SemaphoreSlim mse = new SemaphoreSlim(0, 1);

        public async Task<Sheet> ReadFromFileAsync(string filePath, string name = null)
        {
            var sequence = new Sequence();
            sequence.LoadCompleted += (s, e) => mse.Release();

            sequence.LoadAsync(filePath);

            await mse.WaitAsync();

            // TODO: do something here...

            mse = new SemaphoreSlim(0, 1);

            return new Sheet();
        }
    }
}
