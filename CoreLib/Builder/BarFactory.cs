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

        public BarFactory(uint lowerSignature, uint upperSignature)
        {
            factory = () =>
            {
                return new Bar()
                {
                    LowerSignature = lowerSignature,
                    UpperSignature = upperSignature
                };
            };
        }

        public Bar GetBar()
        {
            return factory();
        }
    }
}
