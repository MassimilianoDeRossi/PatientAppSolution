using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientApp.Services
{
  /// <summary>
  /// Dependency Service for device rooting / jailbreak check
  /// </summary>
  public interface IDeviceIntegrityService
  {
    /// <summary>
    /// Return true if device has not been corrupted (rooted or jailbroken)
    /// </summary>
    /// <returns></returns>
    bool IsSafe();
  }
}
