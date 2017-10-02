using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    class Sheet
    {
        public string Name { get; set; }
        public SheetKey Key { get; set; }

        public List<Staff> Staffs { get; set; }
    }
}
