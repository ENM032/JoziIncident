using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ST10091324_PROG7312_Part1.Model
{
    // The code for this class was modified and implemented from StackOverflow
    // Author: Anurag
    // Link: https://stackoverflow.com/questions/34715501/validating-password-using-regex-c-sharp
    internal class PasswordChecker
    {
        // Method to check if the password meets the requirements
        public static bool IsPasswordValid(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password), "Password cannot be null");
            }

            // Check if the password length is at least 8 characters
            bool isLengthValid = password.Length >= 8;

            // Check if the password contains at least one digit
            bool hasDigit = password.Any(char.IsDigit);

            // Check if the password contains at least one uppercase letter
            bool hasUppercase = password.Any(char.IsUpper);

            // Check if the password contains at least one lowercase letter
            bool hasLowercase = password.Any(char.IsLower);

            // Check if the password contains at least one special character
            // Define special characters as any character not alphanumeric
            bool hasSpecialCharacter = password.Any(ch => !char.IsLetterOrDigit(ch));

            // Combine all conditions to determine if the password is valid
            return isLengthValid && hasDigit && hasUppercase && hasLowercase && hasSpecialCharacter;
        }
    }
}
