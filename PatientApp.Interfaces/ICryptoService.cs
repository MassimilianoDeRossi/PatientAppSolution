namespace PatientApp.Interfaces
{
    /// <summary>
    /// Service used for crypting and decrypting string using base64 rules.
    /// </summary>
    public interface ICryptoService
    {
        /// <summary>
        /// Symmetric encryptor with the current System.Security.Cryptography.SymmetricAlgorithm.Key
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        //string Encrypt(object value);

        /// <summary>
        /// Symmetric decryptor with the current System.Security.Cryptography.SymmetricAlgorithm.Key
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        //string Decrypt(string value);
    }
}
