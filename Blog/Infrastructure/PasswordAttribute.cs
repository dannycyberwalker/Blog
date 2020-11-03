using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Infrastructure
{
    public class PasswordAttribute : ValidationAttribute
    {
        private string FirstPassword;
        public PasswordAttribute(string FirstPassword)
        {
            this.FirstPassword = FirstPassword;
        }
        public override bool IsValid(object value)
        {
            string SecondPassword = value?.ToString();
            if (SecondPassword != null && SecondPassword == FirstPassword)
                return true;
            return false;
        }
    }
}
