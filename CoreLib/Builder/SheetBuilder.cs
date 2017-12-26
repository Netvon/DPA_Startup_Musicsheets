using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Builder
{
    public abstract class SheetBuilder
    {
        readonly Sheet sheet;

        BarFactory barFactory;

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

        internal void AddTempto(uint bpm)
        {
            sheet.Tempo = bpm;
        }
    }
}
