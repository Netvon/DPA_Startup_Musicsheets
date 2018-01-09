using Core.IO;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Managers
{
    [SheetWriter(".pdf", "Lilypond PDF")]
    class PDFSheetWriter : ISheetWriter
    {
        string filePath;
        public void SetFilePath(string filePath)
        {
            this.filePath = filePath;
        }

        public async Task WriteToFile(Sheet sheet)
        {
            var factory = new SheetWriterFactory();

            string directory = Directory.GetCurrentDirectory();
            string date = DateTime.Now.ToString();
            string tempFileName = $"{directory}\temp -{date}.ly";
            string lilypondLocation = @"C:\Program Files (x86)\LilyPond\usr\bin\lilypond.exe";

            if(!File.Exists(lilypondLocation))
                return;

            var writer = factory.GetWriter(tempFileName);

            if(writer != null)
            {
                await writer.WriteToFile(sheet);

                
                string sourceFolder = Path.GetDirectoryName(tempFileName);
                string sourceFileName = Path.GetFileNameWithoutExtension(tempFileName);
                string targetFolder = Path.GetDirectoryName(filePath);
                string targetFileName = Path.GetFileNameWithoutExtension(filePath);

                var process = new Process
                {
                    StartInfo =
                    {
                        WorkingDirectory = directory,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Arguments = string.Format("--pdf \"{0}\\{1}.ly\"", sourceFolder, sourceFileName),
                        FileName = lilypondLocation
                    }
                };

                process.Start();
                while (!process.HasExited)
                { /* Wait for exit */
                }
                if (sourceFolder != targetFolder || sourceFileName != targetFileName)
                {
                    File.Move(sourceFolder + "\\" + sourceFileName + ".pdf", targetFolder + "\\" + targetFileName + ".pdf");
                    File.Delete(tempFileName);
                }
            }
        }

        public Task<string> WriteToString(Sheet sheet)
        {
            throw new NotSupportedException();
        }
    }
}
