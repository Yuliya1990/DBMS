using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DBMS.Models
{
    public class Table
    {
        public string Name { get; set; }
        public List<Attribute> Attributes { get; set; }
        public List<Row> Rows { get; set; }
        private ValidationParser _parser;
        private Parser _conditionParser;

        public Table(string name)
        {
            Name = name;
            Attributes = new List<Attribute>();
            Rows = new List<Row>();
            _parser = new ValidationParser();
            _conditionParser = new Parser();
        }

        public Table(string tableName, List<Attribute> tableColumns)
        {
            _parser = new ValidationParser();
            Name = tableName;
            if (!tableColumns.Any())
            {
                throw new ArgumentException("Can not create table without columns");
            }
            Attributes = tableColumns;
            Rows = new List<Row>();
        }

        public void AddRow(Row row)
        {
            if (isRowValid(row))
            {
                Rows.Add(row);
            }
            else throw new Exception("The row isn`t valid");
        }

        public void EditRow(int rowIndex, List<object> updatedRow)
        {
            var newRow = new Row(updatedRow);
            if (isRowValid(newRow))
            {
                Rows[rowIndex] = newRow;
            }
            else throw new Exception("The row isn`t valid");
        }

        public void DeleteRow(int rowIndex)
        {

        }

        private bool isRowValid (Row row)
        {
            for (int i = 0; i < Attributes.Count; i++)
            {
                ColumnType typeofColumn = Attributes[i].Type;
                switch (typeofColumn)
                {
                    case ColumnType.Integer:
                        {
                            if (!_parser.ValidateInteger(row.Records[i]))
                            {
                                return false;
                            }
                            break;
                        }
                    case ColumnType.Real:
                        {
                            if (!_parser.ValidateReal(row.Records[i]))
                            {
                                return false;
                            }
                            break;
                        }
                    case ColumnType.Time:
                        {
                            if (!_parser.ValidateTime(row.Records[i]))
                            {
                                return false;
                            }
                            break;
                        }
                    case ColumnType.Char:
                        {
                            if (!_parser.ValidateChar(row.Records[i]))
                            {
                                return false;
                            }
                            break;
                        }
                    case ColumnType.Timelnvl:
                        {
                            if (!_parser.ValidateTimelnvl(row.Records[i]))
                            {
                                return false;
                            }
                            break;
                         }
                }
            }
            return true;
           
        }
       

        public List<Row> SearchRows(string condition)
        {
            List<string> logicOp = FindLogicalOperators(condition);
            string[] conditions = condition.Split(new[] { "&&", "||" }, StringSplitOptions.RemoveEmptyEntries);
            if (conditions.Length - 1 != logicOp.Count)
            {
                throw new Exception("Wrong conditon");
            }

            List<List<Row>> results = new List<List<Row>>();
            foreach (string c in conditions)
            {
                var result = _conditionParser.ParseCondition(c);
                if (result == null)
                    throw new Exception("Wrong conditon");

                string columnName = result[0];
                string op = result[1];
                string value = result[2];

                var attribute = Attributes.Where(a => a.Name == columnName).FirstOrDefault();

                if (attribute == null || !ValidateValue(attribute.Type, value))
                {
                    throw new Exception("Wrong conditon");
                }

                var columnIndex = Attributes.FindIndex(a => a.Name == columnName);

                if (columnIndex == -1)
                    throw new Exception("Wrong conditon");


                var res = MakeRowsSearch(attribute.Type, columnIndex, op, value);
                results.Add(res);
        }
            List<Row> RowResult = new List<Row>();

            for (int i = 0; i < logicOp.Count; i++){
                RowResult = results[i];
                if (logicOp[i] == "&&")
                {
                    var ins = RowResult.Intersect(results[i + 1]);
                    RowResult = ins.ToList();
                }
                else
                {
                    var un = RowResult.Union(results[i + 1]);
                    RowResult = un.ToList();
                }
            }
                
            return RowResult;

        }


        private List<Row> MakeRowsSearch(ColumnType typeofColumn, int columnIndex, string op, string value)
        {
            List<Row> filteredRows = Rows.ToList();
            switch (typeofColumn)
            {
                case ColumnType.String:
                    {
                        switch (op)
                        {
                            
                            case "==":
                                {
                                    filteredRows = filteredRows.Where(row =>
                                    {
                                        string stringRowValue = row.Records[columnIndex].ToString();
                                        return stringRowValue == value;
                                    }).ToList();
                                    break;
                                }
                            default:
                                {
                                    throw new Exception("You can`t use this operator with string");
                                }
                        }
                        break;
                    }
                case ColumnType.Integer:
                    {
                            switch (op)
                            {
                                case ">=":
                                    {
                                        int intvalue = int.Parse(value);

                                    filteredRows = filteredRows.Where(row =>
                                    {
                                        string stringRowValue = row.Records[columnIndex].ToString();
                                        int intRowValue = int.Parse(stringRowValue);

                                        return intRowValue >= intvalue;
                                    }).ToList();

                                        break;
                                    }

                            case ">":
                                {
                                    int intvalue = int.Parse(value);

                                    filteredRows = filteredRows.Where(row =>
                                    {
                                        string stringRowValue = row.Records[columnIndex].ToString();
                                        int intRowValue = int.Parse(stringRowValue);

                                        return intRowValue > intvalue;
                                    }).ToList();
                                    break;
                                }
                            case "<=":
                                {
                                    int intvalue = int.Parse(value);

                                    filteredRows = filteredRows.Where(row =>
                                    {
                                        string stringRowValue = row.Records[columnIndex].ToString();
                                        int intRowValue = int.Parse(stringRowValue);

                                        return intRowValue <= intvalue;
                                    }).ToList();
                                    break;
                                }
                            case "<":
                                {
                                    int intvalue = int.Parse(value);

                                    filteredRows = filteredRows.Where(row =>
                                    {
                                        string stringRowValue = row.Records[columnIndex].ToString();
                                        int intRowValue = int.Parse(stringRowValue);

                                        return intRowValue < intvalue;
                                    }).ToList();
                                    break;
                                }
                            case "==":
                                {
                                    int intvalue = int.Parse(value);

                                    filteredRows = filteredRows.Where(row =>
                                    {
                                        string stringRowValue = row.Records[columnIndex].ToString();
                                        int intRowValue = int.Parse(stringRowValue);

                                        return intRowValue == intvalue;
                                    }).ToList();
                                    break;
                                }
                            }
                        
                        break;
                    }
                case ColumnType.Real:
                    {
                        switch (op)
                        {
                            case ">=":
                                {
                                    int intvalue = int.Parse(value);

                                    filteredRows = filteredRows.Where(row =>
                                    {
                                        string stringRowValue = row.Records[columnIndex].ToString();
                                        int intRowValue = int.Parse(stringRowValue);

                                        return intRowValue >= intvalue;
                                    }).ToList();

                                    break;
                                }

                            case ">":
                                {
                                    int intvalue = int.Parse(value);

                                    filteredRows = filteredRows.Where(row =>
                                    {
                                        string stringRowValue = row.Records[columnIndex].ToString();
                                        int intRowValue = int.Parse(stringRowValue);

                                        return intRowValue > intvalue;
                                    }).ToList();
                                    break;
                                }
                            case "<=":
                                {
                                    int intvalue = int.Parse(value);

                                    filteredRows = filteredRows.Where(row =>
                                    {
                                        string stringRowValue = row.Records[columnIndex].ToString();
                                        int intRowValue = int.Parse(stringRowValue);

                                        return intRowValue <= intvalue;
                                    }).ToList();
                                    break;
                                }
                            case "<":
                                {
                                    int intvalue = int.Parse(value);

                                    filteredRows = filteredRows.Where(row =>
                                    {
                                        string stringRowValue = row.Records[columnIndex].ToString();
                                        int intRowValue = int.Parse(stringRowValue);

                                        return intRowValue < intvalue;
                                    }).ToList();
                                    break;
                                }
                            case "==":
                                {
                                    int intvalue = int.Parse(value);

                                    filteredRows = filteredRows.Where(row =>
                                    {
                                        string stringRowValue = row.Records[columnIndex].ToString();
                                        int intRowValue = int.Parse(stringRowValue);

                                        return intRowValue == intvalue;
                                    }).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case ColumnType.Time:
                    {
                        switch (op)
                        {
                            case ">=":
                                {
                                    DateTime timeValue = DateTime.Parse(value);

                                    filteredRows = filteredRows.Where(row =>
                                    {
                                        DateTime rowTimeValue = DateTime.Parse(row.Records[columnIndex].ToString());
                                        return rowTimeValue >= timeValue;
                                    }).ToList();
                                    break;
                                }
                            case ">":
                                {
                                    DateTime timeValue = DateTime.Parse(value);

                                    filteredRows = filteredRows.Where(row =>
                                    {
                                        DateTime rowTimeValue = DateTime.Parse(row.Records[columnIndex].ToString());
                                        return rowTimeValue > timeValue;
                                    }).ToList();
                                    break;
                                }
                            case "<=":
                                {
                                    DateTime timeValue = DateTime.Parse(value);

                                    filteredRows = filteredRows.Where(row =>
                                    {
                                        DateTime rowTimeValue = DateTime.Parse(row.Records[columnIndex].ToString());
                                        return rowTimeValue <= timeValue;
                                    }).ToList();
                                    break;
                                }
                            case "<":
                                {
                                    DateTime timeValue = DateTime.Parse(value);

                                    filteredRows = filteredRows.Where(row =>
                                    {
                                        DateTime rowTimeValue = DateTime.Parse(row.Records[columnIndex].ToString());
                                        return rowTimeValue < timeValue;
                                    }).ToList();
                                    break;
                                }
                            case "==":
                                {
                                    DateTime timeValue = DateTime.Parse(value);

                                    filteredRows = filteredRows.Where(row =>
                                    {
                                        DateTime rowTimeValue = DateTime.Parse(row.Records[columnIndex].ToString());
                                        return rowTimeValue == timeValue;
                                    }).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case ColumnType.Char:
                    {
                        switch (op)
                        {

                            case "==":
                                {
                                    filteredRows = filteredRows.Where(row =>
                                    {
                                        string stringRowValue = row.Records[columnIndex].ToString();
                                        return stringRowValue == value;
                                    }).ToList();
                                    break;
                                }
                            default:
                                {
                                    throw new Exception("You can`t use this operator with char");
                                }
                        }
                        break;
                    }
                case ColumnType.Timelnvl:
                    {
                        switch (op)
                        {
                            case "==":
                                {
                                    string[] rangeParts = value.Split('-');
                                    if (rangeParts.Length != 2)
                                    {
                                        throw new Exception("Invalid time range format");
                                    }

                                    TimeSpan startTime = TimeSpan.Parse(rangeParts[0]);
                                    TimeSpan endTime = TimeSpan.Parse(rangeParts[1]);

                                    filteredRows = filteredRows.Where(row =>
                                    {
                                        // Parse the row's time range value into start and end times
                                        string rowValue = row.Records[columnIndex].ToString();
                                        string[] rowRangeParts = rowValue.Split('-');
                                        if (rowRangeParts.Length != 2)
                                        {
                                            return false;
                                        }

                                        TimeSpan rowStartTime = TimeSpan.Parse(rowRangeParts[0]);
                                        TimeSpan rowEndTime = TimeSpan.Parse(rowRangeParts[1]);

                                        // Check if the row's time range falls within the specified range
                                        return rowStartTime >= startTime && rowEndTime <= endTime;
                                    }).ToList();
                                    break;
                                }
                            case ">=":
                                {
                                    // Parse the time range value into start and end times
                                    string[] rangeParts = value.Split('-');
                                    if (rangeParts.Length != 2)
                                    {
                                        throw new Exception("Invalid time range format");
                                    }

                                    TimeSpan startTime = TimeSpan.Parse(rangeParts[0]);
                                    TimeSpan endTime = TimeSpan.Parse(rangeParts[1]);

                                    filteredRows = filteredRows.Where(row =>
                                    {
                                        // Parse the row's time range value into start and end times
                                        string rowValue = row.Records[columnIndex].ToString();
                                        string[] rowRangeParts = rowValue.Split('-');
                                        if (rowRangeParts.Length != 2)
                                        {
                                            return false;
                                        }

                                        TimeSpan rowStartTime = TimeSpan.Parse(rowRangeParts[0]);
                                        TimeSpan rowEndTime = TimeSpan.Parse(rowRangeParts[1]);

                                        // Check if the row's time range starts after or at the same time as the specified range
                                        return rowStartTime >= startTime;
                                    }).ToList();
                                    break;
                                }
                            case ">":
                                {
                                    // Similar to ">=" logic, but without including the exact same start time
                                    string[] rangeParts = value.Split('-');
                                    if (rangeParts.Length != 2)
                                    {
                                        throw new Exception("Invalid time range format");
                                    }

                                    TimeSpan startTime = TimeSpan.Parse(rangeParts[0]);
                                    TimeSpan endTime = TimeSpan.Parse(rangeParts[1]);

                                    filteredRows = filteredRows.Where(row =>
                                    {
                                        string rowValue = row.Records[columnIndex].ToString();
                                        string[] rowRangeParts = rowValue.Split('-');
                                        if (rowRangeParts.Length != 2)
                                        {
                                            return false;
                                        }

                                        TimeSpan rowStartTime = TimeSpan.Parse(rowRangeParts[0]);

                                        return rowStartTime > startTime;
                                    }).ToList();
                                    break;
                                }
                            case "<=":
                                {
                                    // Similar to ">=" logic but for the end time of the range
                                    string[] rangeParts = value.Split('-');
                                    if (rangeParts.Length != 2)
                                    {
                                        throw new Exception("Invalid time range format");
                                    }

                                    TimeSpan startTime = TimeSpan.Parse(rangeParts[0]);
                                    TimeSpan endTime = TimeSpan.Parse(rangeParts[1]);

                                    filteredRows = filteredRows.Where(row =>
                                    {
                                        string rowValue = row.Records[columnIndex].ToString();
                                        string[] rowRangeParts = rowValue.Split('-');
                                        if (rowRangeParts.Length != 2)
                                        {
                                            return false;
                                        }

                                        TimeSpan rowEndTime = TimeSpan.Parse(rowRangeParts[1]);

                                        return rowEndTime <= endTime;
                                    }).ToList();
                                    break;
                                }
                            case "<":
                                {
                                    // Similar to "<=" logic but excluding the exact end time
                                    string[] rangeParts = value.Split('-');
                                    if (rangeParts.Length != 2)
                                    {
                                        throw new Exception("Invalid time range format");
                                    }

                                    TimeSpan startTime = TimeSpan.Parse(rangeParts[0]);
                                    TimeSpan endTime = TimeSpan.Parse(rangeParts[1]);

                                    filteredRows = filteredRows.Where(row =>
                                    {
                                        string rowValue = row.Records[columnIndex].ToString();
                                        string[] rowRangeParts = rowValue.Split('-');
                                        if (rowRangeParts.Length != 2)
                                        {
                                            return false;
                                        }

                                        TimeSpan rowEndTime = TimeSpan.Parse(rowRangeParts[1]);

                                        return rowEndTime < endTime;
                                    }).ToList();
                                    break;
                                }
                            default:
                                {
                                    throw new Exception("Invalid operator for ColumnType.Timelnvl");
                                }
                        }
                        break;


                    }
            }

            return filteredRows;
        }   

        private bool ValidateValue(ColumnType typeofColumn, object columnValue)
        {
            switch (typeofColumn)
            {
                case ColumnType.Integer:
                    {
                        if (!_parser.ValidateInteger(columnValue))
                        {
                            return false;
                        }
                        break;
                    }
                case ColumnType.Real:
                    {
                        if (!_parser.ValidateReal(columnValue))
                        {
                            return false;
                        }
                        break;
                    }
                case ColumnType.Time:
                    {
                        if (!_parser.ValidateTime(columnValue))
                        {
                            return false;
                        }
                        break;
                    }
                case ColumnType.Char:
                    {
                        if (!_parser.ValidateChar(columnValue))
                        {
                            return false;
                        }
                        break;
                    }
                case ColumnType.Timelnvl:
                    {
                        if (!_parser.ValidateTimelnvl(columnValue))
                        {
                            return false;
                        }
                        break;
                    }
            }
            return true;
           
        }


       private List<string> FindLogicalOperators(string condition)
        {
            List<string> logicalOperators = new List<string>();

            // Паттерн для поиска операторов "&&" и "||" с учетом порядка
            string pattern = @"\s*(\&\&|\|\|)\s*";

            foreach (Match match in Regex.Matches(condition, pattern))
            {
                logicalOperators.Add(match.Value.Trim());
            }

            return logicalOperators;
        }

    }
}

