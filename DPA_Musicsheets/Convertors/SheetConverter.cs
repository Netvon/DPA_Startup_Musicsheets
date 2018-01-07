using Core.Models;
using PSAMControlLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Convertors
{
    class SheetConverter
    {
        private string type;

        public SheetConverter(string pType)
        {
            type = pType;
        }

        public string getType()
        {
            return type;
        }
    }
}
