using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Builder
{
    class NoteBuilder
    {
        List<Action<MSNote>> setters = new List<Action<MSNote>>();
        Func<MSNote> instanciator;

        public void AddPitch(NotePitch pitch)
        {
            IsPitchNote();

            setters.Add(n =>
            {
                if (n is PitchNote pn)
                    pn.Pitch = pitch;
            });
        }

        public void AddModifier(NoteModifier modifier)
        {
            IsPitchNote();

            setters.Add(n =>
            {
                if (n is PitchNote pn)
                    pn.Modifier = modifier;
            });
        }

        public void AddDotts(uint amount)
        {
            setters.Add(n => n.Dotts += amount);
        }

        public void AddBaseLength(uint length)
        {
            setters.Add(n => n.BaseLength = length);
        }

        public void AddOctave(int octave)
        {
            IsPitchNote();

            setters.Add(n =>
            {
                if (n is PitchNote pn)
                    pn.Octave += octave;
            });
        }

        public void AddRest()
        {
            IsRestNote();
        }

        public MSNote Build()
        {
            var instance = instanciator();
            setters.ForEach(x => x(instance));

            return instance;
        }

        private void IsPitchNote()
        {
            instanciator = () => new PitchNote();
        }

        private void IsRestNote()
        {
            instanciator = () => new RestNote();
        }
    }
}
