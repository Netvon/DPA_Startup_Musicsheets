using Core.Editor;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Editor
{
    class WPFFileService : IFileService
    {
        public string RequestReadPath(string filter)
        {
            var openFileDialog = new OpenFileDialog() { Filter = filter };

            if(openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }

            return "";
        }

        public string RequestWritePath(string filter)
        {
            var saveFileDialog = new SaveFileDialog() { Filter = filter };
            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }

            return "";
        }
    }
}
