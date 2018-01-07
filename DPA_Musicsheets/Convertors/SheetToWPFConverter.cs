using Core.Models;
using PSAMControlLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Convertors
{
    class SheetToWPFConverter
    {
        public IEnumerable<MusicalSymbol> ConvertSheet(Sheet sheet)
        {
            var visitor = new Visitor.Visitor();

            List<MusicalSymbol> symbols = new List<MusicalSymbol>();

            var clef = new Clef((ClefType)sheet.Key, 2);
            symbols.Add(clef);
            var timeSignature = new TimeSignature(TimeSignatureType.Numbers, sheet.Bars[0].UpperSignature, sheet.Bars[0].LowerSignature);
            symbols.Add(timeSignature);

            foreach (var bar in sheet.Bars)
            {
                foreach (var note in bar.Notes)
                {
                    note.Visit(visitor);
                    symbols.Add(visitor.Symbol);
                }
                symbols.Add(new Barline());
            }

            return symbols;
        }

    }
}
