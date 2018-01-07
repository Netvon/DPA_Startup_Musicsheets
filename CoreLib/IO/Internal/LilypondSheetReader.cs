using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using System.Linq;
using Core.Builder.Internal;

namespace Core.IO.Internal
{
    [SheetReader(".ly", nameof(LilypondSheetReader))]
    public class LilypondSheetReader : ISheetReader
    {
        string filePath;
        public async Task<Sheet> ReadFromFileAsync()
        {
            string lines = "";
            using (var reader = File.OpenText(filePath))
            {
                lines = await reader.ReadToEndAsync();
            }

            var tokens = lines.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x));

            var builder = new LilypondSheetBuilder(tokens);
            return builder.Build();
        }

        public void SetFilePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            if (!File.Exists(path))
                throw new ArgumentException("The given path does not exist", nameof(path));

            filePath = path;
        }
    }
}
