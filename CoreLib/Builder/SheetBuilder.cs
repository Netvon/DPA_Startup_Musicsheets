using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Builder
{
    public abstract class SheetBuilder
    {
        readonly Sheet sheet;

        Func<Bar> barBuilder;

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
            if (barBuilder != null)
                return;

            barBuilder = () =>
            {
                return new Bar()
                {
                    LowerSignature = lower,
                    UpperSignature = upper
                };
            };
        }

        internal void AddTempto(uint bpm)
        {
            sheet.Tempo = bpm;
        }
    }
}
