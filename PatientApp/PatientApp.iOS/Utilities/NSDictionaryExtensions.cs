using Foundation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PatientApp.iOS.Utilities
{
    public static class NSDictionaryExtensions
    {
        public static Dictionary<string, string> ToDictionary(this NSDictionary source)
        {
            var result = new Dictionary<string, string>();

            foreach (NSString key in source.Keys)
            {
                result.Add(key.ToString(), source.ValueForKey(key).ToString());                
            }

            return result;
        }

    }
}
