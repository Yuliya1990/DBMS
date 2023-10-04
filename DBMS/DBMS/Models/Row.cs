using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMS.Models
{
    public class Row : IEnumerable<object>
    {
        public List<object> Records { get; set; }
        public Row() 
        {
            Records = new List<object>();
        }
        public Row(List<object> records)
        {
            Records = records;
        }
        public IEnumerator<object> GetEnumerator()
        {
            return Records.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
