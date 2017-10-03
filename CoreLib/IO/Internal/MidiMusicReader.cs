using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Sanford.Multimedia.Midi;
using System.Threading;
using System.IO;
using Core.Builder.Internal;
using System.Linq;

namespace Core.IO.Internal
{
    [MusicReader("(.mid|.midi)", nameof(MidiMusicReader))]
    class MidiMusicReader : IMusicReader
    {
        private string filePath;
        private SemaphoreSlim mse = new SemaphoreSlim(0, 1);

        public async Task<Sheet> ReadFromFileAsync()
        {
            var builder = new MidiSheetBuidler();

            var sequence = await LoadMidiAsync(filePath);

            foreach (var track in sequence)
            {
                foreach (var message in track.Iterator().Select(e => e.MidiMessage))
                {
                    builder.AddMidiMessage(message);
                }
            }

            return builder.Build();
        }

        public void SetFilePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("message", nameof(path));
            if (!File.Exists(path))
                throw new ArgumentException("The given path does not exist", nameof(path));

            filePath = path;
        }

        async Task<Sequence> LoadMidiAsync(string filePath)
        {
            var sequence = new Sequence();
            sequence.LoadCompleted += (s, e) => mse.Release();

            sequence.LoadAsync(filePath);

            await mse.WaitAsync();

            // TODO: do something here...

            mse = new SemaphoreSlim(0, 1);

            return sequence;
        }
    }
}
