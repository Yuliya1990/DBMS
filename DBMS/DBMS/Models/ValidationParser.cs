using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DBMS.Models
{
    internal class ValidationParser
    {
        public bool ValidateInteger(object value)
        {
            if (value == null || value == "")
            {
                throw new ArgumentNullException("value");
            }
            else
            {
                if (int.TryParse(value.ToString(), out int result))
                {
                    return true;
                }
                else return false;
            }

        }
        public bool ValidateReal(object value)
        {
            {
                if (value == null || value == "")
                {
                    throw new ArgumentNullException("value");
                }
                else
                {
                    if (double.TryParse(value.ToString(), out double result))
                    {
                        return true;
                    }
                    else return false;
                }
            }
        }
        public bool ValidateChar(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            else
            {
                string strValue = value.ToString();
                // You can add custom validation logic for char here, e.g., check the length or character set.
                if (!string.IsNullOrEmpty(strValue) && strValue.Length == 1)
                {
                    return true;
                }
                else return false;
            }
        }

        public bool ValidateTime(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            else
            {
                string strValue = value.ToString();
                if (Regex.IsMatch(strValue, @"^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$"))
                {
                    return true; // Valid time format
                }
                else
                {
                    return false; // 
                }
            }
        }

        public bool ValidateTimelnvl(object value)
        {
             if (value == null)
    {
        throw new ArgumentNullException("value");
    }
    else
    {
        // Custom validation logic for timelnvl format
        string strValue = value.ToString();

        // Example custom validation logic:
        // In this example, we assume a valid "timelnvl" format is "hh:mm:ss".

        if (Regex.IsMatch(strValue, @"^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]-(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$"))
        {
            return true; // Valid "timelnvl" format for time duration
        }
        else
        {
            return false; // Invalid "timelnvl" format for time duration
        }
    }
        }
    }
}
