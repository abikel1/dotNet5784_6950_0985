using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class User
    {
        public int Id { get; set; }
        public int password {  get; set; }
        public bool IsMennager { get; set; }=false;
    }
}
