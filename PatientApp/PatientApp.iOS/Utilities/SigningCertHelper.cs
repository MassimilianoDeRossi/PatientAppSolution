using PatientApp.iOS.Helpers;
using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace PatientApp.iOS.Utilities
{
  public static class SigningCertHelper
  {
    private static readonly X509Certificate2 _defaultSigningCert;
    private static X509Certificate2 _runtimeSigningCert = null;

    static SigningCertHelper()
    {
      try
      {
        _defaultSigningCert = LoadEmbeddedCertificate();
      }
      catch (Exception ex)
      {
        throw new Exception("error in retrieving the certificate,please check the configuration patrameters", ex);
      }
    }

    public static bool SetCertificate(byte[] content)
    {
      _runtimeSigningCert = null;
      var password = PCLAppConfig.ConfigurationManager.AppSettings["SignCertificatePassword"];
      try
      {
        _runtimeSigningCert = new X509Certificate2(content, password, X509KeyStorageFlags.Exportable);
        return true;
      }
      catch
      {
      }

      return false;
    }

    public static byte[] ObjectToByteArray(object obj)
    {
      byte[] toRet = null;
      if (obj != null)
      {

        using (var ms = new MemoryStream())
        {
          try
          {
            //var bf = new BinaryFormatter();
            //bf.Serialize(ms, obj);
            //toRet = ms.ToArray();

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            var bytes = System.Text.UTF8Encoding.UTF8.GetBytes(json);
            return bytes;
          }
          catch (Exception ex)
          {
            throw new Exception("error on object to bytearray conversion", ex);
          }
        }

      }
      return toRet;
    }

    /// <summary>
    /// Sign an object with SHA algoritm
    /// </summary>
    /// <param name="input"></param>
    /// <param name="cert"></param>
    /// <returns></returns>
    public static byte[] SignWithSHA(byte[] input, X509Certificate2 cert)
    {
      ////Round-trip the key to XML and back, there might be a better way but this works
      //RSACryptoServiceProvider key = new RSACryptoServiceProvider();
      //key.FromXmlString(cert.PrivateKey.ToXmlString(true));
      ////Sign the data
      //return key.SignData(input, CryptoConfig.MapNameToOID("SHA256"));

      try
      {
        if (!cert.HasPrivateKey)
        {
          throw new Exception("Private Key not found");
        }

        var rsa = cert.PrivateKey as RSACryptoServiceProvider;
        var result = rsa.SignData(input, new SHA1CryptoServiceProvider());
        return result;
      }
      catch (Exception ex)
      {
        throw new Exception("error on signing an object", ex);
      }
    }

    /// <summary>
    /// Sign an object with SHA algoritm
    /// </summary>
    /// <param name="input">the object to serialize</param>
    /// <param name="cert">thecertificate to use, if null the default cert specified in configuration will be used</param>
    /// <returns></returns>
    public static byte[] SignWithSHA(object input, X509Certificate2 cert = null)
    {
      ////Round-trip the key to XML and back, there might be a better way but this works
      //RSACryptoServiceProvider key = new RSACryptoServiceProvider();
      //key.FromXmlString(cert.PrivateKey.ToXmlString(true));
      ////Sign the data
      //return key.SignData(input, CryptoConfig.MapNameToOID("SHA256"));

      if (cert == null) cert = _runtimeSigningCert ?? _defaultSigningCert;

      var objectTosign = ObjectToByteArray(input);
      return SignWithSHA(objectTosign, cert);
    }

    /// Sign an object with SHA algoritm
    /// Sign an object with SHA algoritm
    /// </summary>
    /// <param name="input">the object to serialize</param>
    /// <param name="cert">thecertificate to use, if null the default cert specified in configuration will be used</param>
    /// <returns>the Base64 encoded string of the signature</returns>
    public static string SignWithSHAToString(object input, X509Certificate2 cert = null)
    {
      ////Round-trip the key to XML and back, there might be a better way but this works
      //RSACryptoServiceProvider key = new RSACryptoServiceProvider();
      //key.FromXmlString(cert.PrivateKey.ToXmlString(true));
      ////Sign the data
      //return key.SignData(input, CryptoConfig.MapNameToOID("SHA256"));

      if (cert == null) cert = _runtimeSigningCert ?? _defaultSigningCert;

      var objectTosign = ObjectToByteArray(input);
      return Convert.ToBase64String(SignWithSHA(objectTosign, cert));

    }

    /// <summary>
    /// Verify if signature is correct using the default signing certificate
    /// </summary>
    /// <param name="data"></param>
    /// <param name="signature"></param>
    /// <returns>true in case of valid signature, false otherwise </returns>
    public static bool Verify(object objToVer, string sign)
    {
      var signature = Convert.FromBase64String(sign);
      var data = ObjectToByteArray(objToVer);

      if (data != null)
        return Verify(data, _runtimeSigningCert ?? _defaultSigningCert, signature);
      else
        return false;
    }

    /// <summary>
    /// Verify if signature is correct
    /// </summary>
    /// <param name="data">the data to verify </param>
    /// <param name="cert"> the certificate to use to verify the sign</param>
    /// <param name="signature">the signature to check</param>
    /// <returns></returns>
    private static bool Verify(byte[] data, X509Certificate2 cert, byte[] signature)
    {
      var key = (RSACryptoServiceProvider)cert.PublicKey.Key;
      //return key.VerifyData(data, CryptoConfig.MapNameToOID("SHA256"), signature);
      return key.VerifyData(data, new SHA1CryptoServiceProvider(), signature);
    }

    public static bool CheckValidity(X509Certificate2 cert)
    {
      return cert.GetExpirationDateString() != null && DateTime.Parse(cert.GetExpirationDateString()) >= DateTime.Now;
    }

    /// <summary>
    /// Load the certificate installed on server, in Local machine, personal account
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="certificateThumbprint"></param>
    /// <returns>The embedde (only for development scope) certificate required</returns>
    private static X509Certificate2 LoadEmbeddedCertificate()
    {

#if DEBUG
#else
#warning Invoke this method only for development scopes use a certificate from a  store location for the deployed version  
#endif
      byte[] embeddedCert;
      var assembly = typeof(ResourceLoader).GetTypeInfo().Assembly;
      var signCertificate = PCLAppConfig.ConfigurationManager.AppSettings["SignCertificate"];
      using (Stream certStream = ResourceLoader.GetEmbeddedResourceStream(assembly, signCertificate))
      {
        embeddedCert = new byte[certStream.Length];
        certStream.Read(embeddedCert, 0, (int)certStream.Length);
      }
      // 1Password
      return new X509Certificate2(embeddedCert, "1Password", X509KeyStorageFlags.Exportable);
    }

    // TODO: deprecate da rimuovere perchè non utilizzate
    public static string EncryptText(string plainText)
    {
      var publicKey = (_runtimeSigningCert ?? _defaultSigningCert).PublicKey.Key as RSACryptoServiceProvider;

      byte[] data = System.Text.Encoding.UTF8.GetBytes(plainText);
      byte[] cipherText = publicKey.Encrypt(data, false); //set true if u whant to used it

      return Convert.ToBase64String(cipherText);
    }

    // TODO: deprecate da rimuovere perchè non utilizzate
    public static string DecryptText(string encryptedData)
    {
      var privateKey = (_runtimeSigningCert ?? _defaultSigningCert).PrivateKey as RSACryptoServiceProvider;

      byte[] original = Convert.FromBase64String(encryptedData);
      byte[] data = privateKey.Decrypt(original, false); //set true if u whant to used it

      return System.Text.Encoding.UTF8.GetString(data);
    }
  }
}
