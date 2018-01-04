using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Builder
{
    public abstract class SheetBuilder
    {
        readonly protected Sheet sheet;

        BarFactory barFactory;
        readonly List<Action<NoteBuilder>> builderMods = new List<Action<NoteBuilder>>();

        public SheetBuilder()
        {
            sheet = new Sheet();
        }

        public abstract Sheet Build();

        public virtual void AddKey(SheetKey key)
        {
            sheet.Key = key;
        }

        public virtual void AddTimeSignature(uint upper, uint lower)
        {
            if (barFactory != null)
                return;

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
