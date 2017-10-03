using System.Collections.Generic;

namespace Core.Models
{
    public class Staff
    {
        public IEnumerable<Bar> Bars { get; set; } = new List<Bar>();
    }
}