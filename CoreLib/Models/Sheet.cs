using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Sheet
    {
        public string Name { get; set; }
        public SheetKey Key { get; set; }
        public uint Tempo { get; set; }

        public IEnumerable<Staff> Staffs { get; set; } = new List<Staff>();
    }
}
