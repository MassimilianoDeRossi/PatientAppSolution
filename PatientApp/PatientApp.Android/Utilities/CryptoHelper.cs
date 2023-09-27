using System;
using System.Security.Cryptography;
using System.Text;

namespace PatientApp.Droid.Utilities
{
    public class CryptoHelper
    {
        static readonly RijndaelManaged rjm = new RijndaelManaged();

        static CryptoHelper()
        {
            rjm.KeySize = 128;
            rjm.BlockSize = 128;
            rjm.Key = Encoding.ASCII.GetBytes(chiave);
            rjm.IV = Encoding.ASCII.GetBytes(iv);
        }

        private const string chiave = "AxTsQWCvGTFRbgLL"; // Chiave di criptaggio a 16 byte 
        private const string iv = "QWExcATyUxxLOafO"; // Vettore di Inizializzazione a 16 byte

        /// <summary>
        /// Symmetric encryptor with the current System.Security.Cryptography.SymmetricAlgorithm.Key
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Encrypt(string value)
        {
            if (value == null)
                return null;

            var input = Encoding.Unicode.GetBytes(value);
            var output = rjm.CreateEncryptor().TransformFinalBlock(input, 0, input.Length);

            return Convert.ToBase64String(output);
        }

        /// <summary>
        /// Symmetric decryptor with the current System.Security.Cryptography.SymmetricAlgorithm.Key
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Decrypt(string value)
        {
            if (value == null)
                return null;

            var input = Convert.FromBase64String(value);
            var output = rjm.CreateDecryptor().TransformFinalBlock(input, 0, input.Length);

            return Encoding.Unicode.GetString(output);
        }
    }
}
