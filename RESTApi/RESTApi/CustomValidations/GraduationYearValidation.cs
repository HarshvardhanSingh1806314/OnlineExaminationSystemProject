using System;
using System.ComponentModel.DataAnnotations;

namespace RESTApi.CustomValidations
{
    public class GraduationYearValidation : ValidationAttribute
    {
        // Minimum acceptable value for graduation year
        private const int MinimumGraduationYearAllowed = 1970;

        public GraduationYearValidation(string ErrorMessage) : base(ErrorMessage)
        {

        }

        public override bool IsValid(object value)
        {
            if(value == null)
            {
                return false;
            }

            int graduationYear = Convert.ToInt32(value.ToString());

            // checking if the graduation year is valid
            return graduationYear >= MinimumGraduationYearAllowed;
        }
    }
}