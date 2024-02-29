using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class User
{
    public string? userName {  get; set; }
    public int Id { get; set; }
    public int password {  get; set; }
    public bool isMennager { get; set; } = false;
    public override string ToString() => this.ToStringProperty();
}
