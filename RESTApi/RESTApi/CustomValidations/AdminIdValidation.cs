using System;
using System.ComponentModel.DataAnnotations;

namespace RESTApi.CustomValidations
{
    public class AdminIdValidation : ValidationAttribute
    {
        // Minimum value allowed for Admin Id
        private const int MinimumAllowedValueForAdminId = 100000;

        public AdminIdValidation(string ErrorMessage) : base(ErrorMessage)
        {

        }

        public override bool IsValid(object value)
        {
            if(value == null)
            {
                return false;
            }

            int adminId = Convert.ToInt32(value.ToString());

            // checking if adminId is valid
            return adminId >= MinimumAllowedValueForAdminId;
        }
    }
}