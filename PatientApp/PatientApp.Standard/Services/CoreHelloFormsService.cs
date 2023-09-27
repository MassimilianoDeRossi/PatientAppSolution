using PatientApp.Interfaces;

namespace PatientApp
{
  public class CoreHelloFormsService : IHelloFormsService
  {

    public string GetHelloFormsText()
    {
      return "Hello From the Core!";
    }
  }
}