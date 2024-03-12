using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Gant
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Dependencies { get; set; }
        public int StartOffset { get; set; }
        public int TasksDays { get; set; }
        public int EndOffset { get; set; }
        public BO.Status Status { get; set; }
        public override string ToString() => this.ToStringProperty();
    }
}
