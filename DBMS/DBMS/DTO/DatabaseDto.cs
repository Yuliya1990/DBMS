using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMS.DTO
{
    public class DatabaseDto
    {
        public string Name { get; set; }
        public List<TableDto> Tables { get; set; }
    }
}
