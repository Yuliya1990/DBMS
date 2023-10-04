using DBMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMS.DTO
{
    public class TableDto
    {
        public string Name { get; set; }
        public List<Models.Attribute> Columns { get; set; }
        public List<Row> Rows { get; set; }
    }
}
