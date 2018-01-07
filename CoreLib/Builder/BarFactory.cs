using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Builder
{
    class BarFactory
    {
        Func<Bar> factory;
        Repeat initialRepeat;
        uint rAmount;

        public BarFactory(uint lowerSignature, uint upperSignature)
        {
            factory = () =>
            {
                return new Bar()
                {
                    LowerSignature = lowerSignature,
                    UpperSignature = upperSignature,
                    Repeat = initialRepeat,
                    RepeatAmount = rAmount
                };
            };
        }

        public Bar GetBar(IEnumerable<MSNote> withNotes)
        {
            var temp = factory();

            initialRepeat = Repeat.None;
            rAmount = 0;

            temp.Notes.AddRange(withNotes);

            return temp;
        }

        public void StartRepeat(uint amount)
        {
            initialRepeat = Repeat.Start;
            rAmount = amount;
        }

        public void EndRepeat()
        {
            initialRepeat = Repeat.End;
        }
    }
}
