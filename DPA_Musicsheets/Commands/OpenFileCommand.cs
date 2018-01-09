using Core.Commands;
using Core.Editor;
using Core.IO;
using Core.Memento;
using DPA_Musicsheets.Messages;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands
{
    [CommandBinding(Name = Editor.Commands.OpenFileCommandName)]
    class OpenFileCommand : ICommand
    {
        public bool CanInvoke<T>(CareTaker<T> careTaker) => true;

        public void Invoke<T>(CareTaker<T> careTaker)
        {
            var fact = new SheetReaderFactory();
            var extensions = fact.GetAllSupportedExtension().Select(x => {
                var e = $"*{x.ext}";

                if (x.ext.StartsWith("(", StringComparison.InvariantCultureIgnoreCase))
                    e = x.ext.Replace("(", "").Replace(")", "").Replace("|", ";").Replace(".", "*.");

                return $"{x.name}|{e}";
            });

            var fileService = ServiceLocator.Current.GetInstance<IFileService>();
            var path = fileService.RequestReadPath(string.Join("|", extensions.ToArray()));

            if (!string.IsNullOrWhiteSpace(path))
            {
                Messenger.Default.Send(new CurrentPathMessage() { FilePath = path });
            }
        }
    }
}
