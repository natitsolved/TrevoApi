using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Trevo.API.Helper
{
    public class PasswordAndTrevoHelper
    {
        public const int Pbkdf2Iterations = 1000;
        public const int HashByteSize = 20;
        public const int SaltByteSize = 24;
        public const int IterationIndex = 0;
        public const int SaltIndex = 1;
        public const int Pbkdf2Index = 2;
        private static readonly string RandomPass = ConfigurationManager.AppSettings["RandomPassword"].Trim();

        /// <summary>
        /// Get a random set of numbers based on the length
        /// </summary>
        /// <param name="lenght"></param>
        /// <returns></returns>
        public static string CreateRandomNumber(int lenght)
        {
            string allowedChars = "0123456789";
            char[] chars = new char[lenght];
            Random rd = new Random();

            for (int i = 0; i < lenght; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }


        public static string GenerateTrevoId(string name)
        {
            string trevoId = string.Empty;
            string randomChar = CreateRandomNumber(3);
            if (!string.IsNullOrEmpty(name))
            {
                name = name.Substring(0, 3);
            }
            trevoId = name + randomChar;
            return trevoId;
        }

        public static string GeneratePassword(int length) //length of salt    
        {
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            var randNum = new Random();
            var chars = new char[length];
            var allowedCharCount = allowedChars.Length;
            for (var i = 0; i <= length - 1; i++)
            {
                chars[i] = allowedChars[Convert.ToInt32((allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }

        public static string EncodePassword(string pass, string salt) //encrypt password    
        {
            byte[] bytes = Encoding.Unicode.GetBytes(pass);
            byte[] src = Encoding.Unicode.GetBytes(salt);
            byte[] dst = new byte[src.Length + bytes.Length];
            System.Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            System.Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            byte[] inArray = algorithm.ComputeHash(dst);
            //return Convert.ToBase64String(inArray);    
            return EncodePasswordMd5(Convert.ToBase64String(inArray));
        }
        public static string EncodePasswordMd5(string pass) //Encrypt using MD5    
        {
            //Byte[] originalBytes;
            //Byte[] encodedBytes;
            //MD5 md5;
            ////Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)    
            //md5 = new MD5CryptoServiceProvider();
            //originalBytes = ASCIIEncoding.Default.GetBytes(pass);
            //encodedBytes = md5.ComputeHash(originalBytes);
            ////Convert encoded bytes back to a 'readable' string    
            //return BitConverter.ToString(encodedBytes);
            string hash = "";
            using (MD5 md5Hash = MD5.Create())
            {
                hash = GetMd5Hash(md5Hash, pass);
            };
            return hash;
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static string HashPassword(string password)
        {
            var cryptoProvider = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SaltByteSize];
            cryptoProvider.GetBytes(salt);

            var hash = GetPbkdf2Bytes(password, salt, Pbkdf2Iterations, HashByteSize);
            return Pbkdf2Iterations + ":" +
                   Convert.ToBase64String(salt) + ":" +
                   Convert.ToBase64String(hash);
        }

        private static byte[] GetPbkdf2Bytes(string password, byte[] salt, int iterations, int outputBytes)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = iterations;
            return pbkdf2.GetBytes(outputBytes);
        }
        public static bool ValidatePassword(string password, string correctHash)
        {
            char[] delimiter = { ':' };
            var split = correctHash.Split(delimiter);
            var iterations = Int32.Parse(split[IterationIndex]);
            var salt = Convert.FromBase64String(split[SaltIndex]);
            var hash = Convert.FromBase64String(split[Pbkdf2Index]);

            var testHash = GetPbkdf2Bytes(password, salt, iterations, hash.Length);
            return SlowEquals(hash, testHash);
        }
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            var diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }


        public static string GetSaltString()
        {
            // Lets create a byte array to store the salt bytes
            byte[] saltBytes = new byte[SaltByteSize];

            RNGCryptoServiceProvider encrypter = new RNGCryptoServiceProvider();
            // lets generate the salt in the byte array
            encrypter.GetNonZeroBytes(saltBytes);

            // Let us get some string representation for this salt
            string saltString = GetString(saltBytes);

            // Now we have our salt string ready lets return it to the caller
            return saltString;
        }


        public static string GetPasswordHashAndSalt(string message)
        {
            // Let us use SHA256 algorithm to 
            // generate the hash from this salted password
            SHA256 sha = new SHA256CryptoServiceProvider();
            byte[] dataBytes = GetBytes(message);
            byte[] resultBytes = sha.ComputeHash(dataBytes);

            // return the hash string to the caller
            return GetString(resultBytes);
        }


        public static byte[] GetBytes(string message)
        {

            return Encoding.ASCII.GetBytes(message);

        }
        public static string GetString(byte[] arrayOfBytes)
        {

            return Encoding.ASCII.GetString(arrayOfBytes);

        }

        /// <summary>
        /// Gets a random password based on the key in config file
        /// </summary>
        /// <returns></returns>
        public static void GetRandomPassword(ref string password, ref string salt,ref string ranPass)
        {
            string ranPassword = RandomPass;
            var keyNew = GeneratePassword(30);
            ranPassword = PasswordAndTrevoHelper.EncodePassword(ranPassword, keyNew);
            password = ranPassword;
            salt = keyNew;
            ranPass = RandomPass;
        }

    }
}