using Core.Commands;
using Core.Editor;
using Core.IO;
using Core.Memento;
using DPA_Musicsheets.Editor;
using DPA_Musicsheets.Messages;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands
{
    [CommandBinding(Name = Editor.Commands.SaveLilypondFileCommandName)]
    class SaveToLilypondCommand : ICommand
    {
        public bool CanInvoke<T>(CareTaker<T> careTaker) => true;

        public void Invoke<T>(CareTaker<T> careTaker)
        {
            if (careTaker is EditorCareTaker ect)
            {
                var fact = new SheetWriterFactory();
                fact.AddAssembly(Assembly.GetAssembly(typeof(SaveToPdfCommand)));

                var extensions = fact.GetAllSupportedExtension().Where(x => x.ext.Contains("ly")).Select(x =>
                {
                    var e = $"*{x.ext}";

                    if (x.ext.StartsWith("(", StringComparison.InvariantCultureIgnoreCase))
                        e = x.ext.Replace("(", "").Replace(")", "").Replace("|", ";").Replace(".", "*.");

                    return $"{x.name}|{e}";
                });

                var fileService = ServiceLocator.Current.GetInstance<IFileService>();

                var path = fileService.RequestWritePath(string.Join("|", extensions.ToArray()));

                if (!string.IsNullOrWhiteSpace(path))
                {
                    var writer = fact.GetWriter(path);

                    try
                    {
                        writer.WriteToFile(ect.Current.MusicSheet);
                    }
                    catch (Exception ex)
                    {
                        Messenger.Default.Send(new ErrorMessage() { Exception = ex });
                    }
                    
                }
            }
        }
    }
}
