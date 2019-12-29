using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChatTU.DTOs.ValidationAttributes
{
    public class NameValidationAttribute : ValidationAttribute
    {
        private const string forbidenChars = "' ',%,&";
        public override bool IsValid(object value)
        {
            string val = value.ToString().Trim();

            foreach (var character in forbidenChars.Split(','))
            {
                if(val.Contains(character))
                {
                    ErrorMessage = $"Name fields cannot contain any of the following charaster: {forbidenChars}";
                    return false;
                }
            }

            return true;
        }
    }
}