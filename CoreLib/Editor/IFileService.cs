using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Editor
{
    public interface IFileService
    {
        string RequestReadPath(string filter);
        string RequestWritePath(string filter);
    }
}
