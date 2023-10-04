using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DBMS.Models
{
    public class Parser
    {
        private string _condition;
        private List<string> operators;
        private List<string> parsedCondition = new List<string>();

        public Parser()
        {
            operators = new List<string> { ">=", "<=", "==", ">", "<"};
        }

        public List<string> ParseCondition(string condition)
        {
            parsedCondition.Clear();
            //надо распарсить "id>=5" на "id" ">=" "5"
            foreach (string op in operators)
            {
                if (condition.Contains(op))
                {
                    string[] parts = condition.Split(new[] { op }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2)
                    {
                        parsedCondition.Add(parts[0].Trim());
                        parsedCondition.Add(op);
                        parsedCondition.Add(parts[1].Trim());
                        return parsedCondition;
                    }
                }
            }
            return null;
        }


    
}
}
