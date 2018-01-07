using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Builder
{
    public abstract class SheetBuilder
    {
        readonly protected Sheet sheet;

        BarFactory barFactory;
        readonly List<Action<NoteBuilder>> builderMods = new List<Action<NoteBuilder>>();

        public int AlternativeCount { get; set; }

        public SheetBuilder()
        {
            sheet = new Sheet();
        }

        public abstract Sheet Build();

        public virtual void AddKey(SheetKey key)
        {
            sheet.Key = key;
        }

        public virtual void AddRepeat(uint amount)
        {
            barFactory.StartRepeat(amount);
        }

        public virtual void AddStopRepeat()
        {
            barFactory.EndRepeat();
        }

        public virtual void AddBar(List<MSNote> notes)
        {
            var bar = barFactory.GetBar(notes);

            if (AlternativeCount > 0)
            {
                var list = sheet.Bars.Last().Alternatives.ElementAtOrDefault(AlternativeCount);

                if (list == null)
                    sheet.Bars.Last().Alternatives.Add(new List<Bar> { bar });
                else
                    list.Add(bar);
            }
            else
            {
                sheet.Bars.Add(bar);
            }
        }

        public virtual void AddTimeSignature(uint upper, uint lower)
        {
            barFactory = new BarFactory(lower, upper);
        }

        internal void AddName(string value)
        {
            sheet.Name = value;
        }

        internal void AddGlobalNoteOctave(int octave)
        {
            builderMods.Add(nb =>
            {
                nb.AddOctave(octave);
            });
        }

        internal NoteBuilder GetNoteBuilder()
        {
            var temp = new NoteBuilder();
            builderMods.ForEach(x => x(temp));

            return temp;
        }

        internal void AddTempto(uint bpm)
        {
            sheet.Tempo = bpm;
        }
    }
}
