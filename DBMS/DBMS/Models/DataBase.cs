using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMS.Models
{
    public class DataBase
    {
        public string Name { get; set; }
        public List<Table> Tables;
        public DataBase(string name)
        {
            Tables = new List<Table>();
            Name = name;
        }
        public Table AddTable(string name, ObservableCollection<Attribute> attributes)
        {
            if (name == null || name == "")
            {
                throw new Exception("Please write the name of table");
            }
            if (attributes.Count==0)
                throw new Exception("Please add at least one column");

            if(attributes.Any(a => a.Name == null))
                throw new Exception("Please the name of column");


            if (!IsTableAlreadyExists(name))
            {
                Table newTable = new Table(name);
                foreach (Attribute attribute in attributes)
                {
                    newTable.Attributes.Add(attribute);
                }
                Tables.Add(newTable);
                return newTable;
            }
            else throw new Exception("The table with this name already exists");
        }

        public bool IsTableAlreadyExists(string name)
        {
            if (!Tables.Any(x => x.Name==name))
            return false;
            return true;
        }
    }
}
