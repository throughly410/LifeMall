using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace LifeMall.Attribute
{
    public class CellPhoneAttribute:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return false;
            if (value is string)
            {
                return Regex.IsMatch(value.ToString(), "^09[0-9]{8}");
            }
            else return false;


        }


    }
}