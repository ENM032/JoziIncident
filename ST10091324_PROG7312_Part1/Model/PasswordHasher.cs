using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ST10091324_PROG7312_Part1.Model
{
    // The code for the class below was taken from StackOverflow
    // Author: Arad
    // Link: https://stackoverflow.com/questions/2138429/hash-and-salt-passwords-in-c-sharp
    internal class PasswordHasher
    {
        public static string GenerateHash(string password)
        {
            // Create the salt value with a cryptographic PRNG
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            // Create the Rfc2898DeriveBytes and get the hash value
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            // Combine the salt and password bytes for later use
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            // Turn the combined salt+hash into a string for storage
            string passwordHash = Convert.ToBase64String(hashBytes);

            return passwordHash;
        }

        public static bool VerifyPasswordHash(string username, string password, string savedPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(savedPasswordHash))
            {
                Console.WriteLine("Error during password verification: Password is missing or corrupted for this user.");
                return false;
            }

            // Extract the bytes from the stored password hash
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);

            // Extract the salt (first 16 bytes)
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Hash the entered password using the same salt and number of iterations
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            // Compare the hash from the entered password with the stored hash
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                    return false; // Password does not match
            }
            return true; // Password matches
        }
    }
}
