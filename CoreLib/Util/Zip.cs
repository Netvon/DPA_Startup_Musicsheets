using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;

namespace Core.Util
{
    [Serializable]
    public class NoSuchFileException : Exception
    {
        public NoSuchFileException() { }
        public NoSuchFileException(string message) : base(message) { }
        public NoSuchFileException(string message, Exception inner) : base(message, inner) { }
        protected NoSuchFileException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    internal static class Zip
    {
        public static Stream OpenStreamFromArchiveWhere(string path, Func<string, bool> predicate)
        {
            var stream = new FileStream(path, FileMode.Open);
            var archive = new ZipArchive(stream, ZipArchiveMode.Read);
            var entry = archive.Entries.First(x => predicate(x.FullName));

            if (entry == null)
                throw new NoSuchFileException();

            return entry.Open();
        }
    }
}
