using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMS.Models
{
    public class Attribute
    {
        public string Name { get; set; }
        public ColumnType Type { get; set; }
        public Attribute(){}
        public Attribute(string name, ColumnType type)
        {
            Name = name;
            Type = type;
        }
    }
}
