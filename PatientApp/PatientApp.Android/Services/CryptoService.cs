using Xamarin.Forms;
using PatientApp.Services;

[assembly: Dependency(typeof(PatientApp.Droid.Services.CryptoServiceImplementation))]

namespace PatientApp.Droid.Services
{
  public class CryptoServiceImplementation : ICryptoService
  {
    public CryptoServiceImplementation()
    {
    }

    public string Encrypt(string data)
    {
      return Utilities.CryptoHelper.Encrypt(data);
    }

    public string Decrypt(string data)
    {
      return Utilities.CryptoHelper.Decrypt(data);
    }

    public bool CheckCertificate(byte[] certContent)
    {
      return Utilities.SigningCertHelper.SetCertificate(certContent);
    }

    public bool VerifySign(object data, string sign, byte[] certContent = null)
    {
      if (certContent != null)
        Utilities.SigningCertHelper.SetCertificate(certContent);

      return Utilities.SigningCertHelper.Verify(data, sign);
      //return true;
    }

    public string SignWithSHAPortableToString(object data)
    {
      return Utilities.SigningCertHelper.SignWithSHAToString(data);
    }

  }
}
