namespace PatientApp.Services
{
    /// <summary>
    /// Dependency Service for cryptography (implemented in native layer)
    /// </summary>
    public interface ICryptoService
    {
        /// <summary>
        /// Enrcrypt and return the provided data 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        string Encrypt(string data);

        /// <summary>
        /// Dercrypt and return the provided data 
        /// </summary>
        string Decrypt(string data);

        /// <summary>
        /// Verify the sign of a crypted package
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        bool VerifySign(object data, string sign, byte[] certContent = null);

        string SignWithSHAPortableToString(object data);
    }
}
