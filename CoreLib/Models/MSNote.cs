using Core.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;


namespace Core.Models
{
    public abstract class MSNote
    {
        private uint length;

        public uint Dotts { get; set; }
        public uint BaseLength
        {
            get => length;
            set
            {
                if (value % 2 == 0 || value == 1)
                    length = value;
                else
                    throw new ArgumentException($"{nameof(BaseLength)} must be even or 1");
            }
        }

        public abstract void Visit(IVisitor visitor);

        public double Length => BaseLength * ( 2.0 - ( 1.0 / Pow(2.0, Dotts ) ));
    }
}
