using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RESTApi.CustomValidations
{
    public class PasswordValidation : ValidationAttribute
    {
        // Regular expression for password validation
        private const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";

        public PasswordValidation(string ErrorMessage) : base(ErrorMessage)
        {

        }

        public override bool IsValid(object value)
        {
            if(value == null)
            {
                return false;
            }

            string password = value.ToString();

            // checking the password against the regex
            return Regex.IsMatch(password, PasswordPattern);
        }
    }
}