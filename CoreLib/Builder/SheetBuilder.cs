﻿using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Builder
{
    public abstract class SheetBuilder
    {
        readonly protected Sheet sheet;

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

        public virtual void AddBar(List<MSNote> notes)
        {
            var bar = barFactory.GetBar();
            bar.Notes = notes;
            sheet.Bars.Add(bar);
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

        internal void AddTempto(uint bpm)
        {
            sheet.Tempo = bpm;
        }
    }
}
