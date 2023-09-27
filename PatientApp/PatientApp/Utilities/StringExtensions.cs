namespace PatientApp.Utilities
{
  public static class StringExtensions
  {
    public static string ToFirstLetterUpper(this string str)
    {
      if (str == null)
        return null;

      if (str.Length > 1)
        return char.ToUpper(str[0]) + str.Substring(1);

      return str.ToUpper();
    }
  }
}
