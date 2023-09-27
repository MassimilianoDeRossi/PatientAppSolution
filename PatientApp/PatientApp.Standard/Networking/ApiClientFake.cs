using MyHexPlanProxies.Models;
using Newtonsoft.Json;
using PatientApp.DataModel.Networking;
using PatientApp.Interfaces;
using PatientApp.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PatientApp.Networking
{
#pragma warning disable
  /*
  * Prescription1:
  * 3 frames (same start date and all not expired)
  * Frame A : 4 times - 65 days (01, 02, 04, 16)
  * Frame B : 2 times - 20 days (including direction change) (12, 16)
  * Frame C : 2 times - 20 days (8, 16)
  *
  * Prescription2:
  * 3 frames (same start date)
  * Frame A : 4 times - 65 days
  * Frame B : 2 times - 20 days (including direction change)
  * Frame C : (UPDATED) 2 times - 20 days
  *
  * Prescription3:
  * 3 frames (same start date)
  * Frame A : 4 times - 65 days
  * Frame B : (REVOKED) 2 times - 20 days (including direction change)
  * Frame C : 2 times - 20 days
  *
  * Prescription4:
  * 3 frames (same start date)
  * Frame A : (REVOKED) 4 times - 65 days
  * Frame B : (REVOKED) 2 times - 20 days (including direction change)
  * Frame C : (REVOKED) 2 times - 20 days
  */
  public class PrescriptionsMockedIndices
  {
    // Use 'QrCode1' if you don't use ApiClientFake but ApiClient

    public string QrCode1 { get => @"{'Chk':'skw0W3Ffl6EJthrM5JWF7J7tFvt9rfThKRXLAc\/3LQUJw0iOiYzVJkxMuuEQgIdY3Z4nh46TC8kYssu\/jq\/OVtQ4V89ksnqipx8NAH+Wu9GCuWiy1Sln6E3t7LQnhvx3ja3P5X6f79yjJPE5BPtpC+IFZK3PzyLsVfkWjCBFal5RN\/I1Lvkttjlka5njQwUF3ezqEEJjiTGJusO8T0wPiNmz1HnWojTP76pSgIq9Cb6ee5SkcBEza507fVLS5Ru1NxCJuQCheFIAUhhyd9WgYcAucXYtN4w\/flS349j1I7iwDBY9Sk5t6YE63c2YyvjCMB89pfFLG26xcnS08CeRtw==','Info':{'C':'28f7c4bd-4041-4b2d-9c04-f6868794c3bb','P':'b0170b24-62af-44e9-81fd-c342606c12c7'}}"; }
    public string QrCode2 { get => @"{'Chk':'','Info':{'C':'00000000-0000-0000-0000-000000000000','P':'00000000-0000-0000-0000-000000000002'}}"; }
    public string QrCode3 { get => @"{'Chk':'','Info':{'C':'00000000-0000-0000-0000-000000000000','P':'00000000-0000-0000-0000-000000000003'}}"; }
    public string QrCode4 { get => @""; }

    public string QrCodeDevOld { get => @"{'Chk':'J44Z31inH2QHB9vDUjCuutyM0C6RcZflhoEv9p895xgvsyJoUhu7EPjOJfMhOrndPTZO/xeLKEGjLU29aPSvvy9mX//T/ocmOm3t7k2nMNhX35OzXlQF3OGmlGduvUcuhM9fqIx6KgckBrLoOAalAWQ3MdHH\u002BFxN8rheJpbTCUa2utkCZIMnjE\u002BTRasG5SG4wFy\u002BNH1A1\u002BsCDRbMNa8bBZYQp5BoMupcniYiPbDTNxQdq0h7x\u002BAC04rIlOc7IM3qEXLEMyvsi9vctTDFmvG3fx3XUGeYbFZeXmSIsisjbHd0ZoF3NEulycU9qJRF/ZwmTTn1vPpKUml\u002BZoP8qItZkg==','Info':{'C':'f795ccf1-9dcc-44a4-9c3a-62637b52b2f1','P':'999cbe33-20e0-4778-82f7-01ba65fde722'}}"; }
    public string QrCodeDev { get => @"{'Chk':'ZlCVoI+tU4jJt4+t4x02Zr0jzCcavQPY57Rh6vc/T3FhBlCtkFO27/rZC/bzzWa+T6+0C78a4XLAugFDA70kp1VgM2w/z99Djkbh1PZPXR8QSbtR/To/b3gSZY0Yy5bGdpPfY/EXIYp+7+gaLF/MiBy1Dgp3CT+EuWlYH1BEtF3WuHvgPbgfQgoSj3Vh3F3X6YrCwquEoeprlCg564Ps6f4UPnmajZ9/4zhvFRoFRoQhWTbN4YziL9dnKj91kplKtMfCKetIYeYQ/n4oMN9wfS6Qs2SWFNsQALlc5ml7UjfcX0/vLt3KpR43J/rqxmiTcdBn9206wEgS5vuAWOBgYw==','Info':{'C':'438b0853-4055-4b3d-8fdb-f1a83014dd00','P':'938f512c-236c-47c3-a6a4-c404f7ce0a4b'}}"; }

    public string QrCodeVer { get => @"{'Chk':'w3\u002BC/uXzujhdb45EHNtBMr52IjuZWduu/Mtk\u002BsDozD1niqgQmJCnEewA/A81Q92A1xcR7BjGFYJ0vZTt3dIlcRvvpUdp9hVfUJhDQVn/Fh5PaAqwO/I\u002BavhU5D7hfp4YuJm9F3pv/EfU4RxJ1SFOx6pAx4677sQN0y5xtNrfm3eAqjIJtSk6hkCE1t9K3/p/uln2giJl0xe75mHgiALJrxKdgMxul59\u002B7io/9X6X2SOnj835yDv0QnEL6AVz6fogTDvFGCC/SKXZIyUjuwCrxdZMXvn2ekV1fcsvlwTU5ILg55AUMnWv2QprOnDYczFBoQwSFnJN35ujvuKyzoZhtw==','Info':{'C':'d068caa0-24ee-4b7e-80cb-7a90c3b3a2a0','P':'0fc5905a-6956-488f-9690-9441caa29552'}}"; }
    public string QrCodeInvalidSign { get => @"{'Chk':'fakeSign','Info':{'C':'05f8a79e-035f-4df2-a6f7-df19fed9dd87','P':'33c221e6-75a6-4486-b32b-ff998e66aaef'}}"; }
    public string QrCodeSingleADJ0 { get => @"{'Chk':'XtxWtIVMjQSaj1DceR9adTrGNlBl09\/V9wE1C3xUlGBvUyEqyKicyWG9j2T\/LpHjTR7KjR4sU5bxd6lCm35YCrYcKNb8lWUD8GnKiGCGK5GKyWnFeCWQoODGeOG7QKwa\/G9ukWDMg9bIxUVAr\/\/z9uPMJATT9ryfI\/oOf59XTaIxjfkEs2d+Z9jB4YtTcTpExDpnWi8WJ5d2XOPjD68WQzuaIRNWSlBL6Jo2fh4UoCaBh9KS6PqWOQkHdo6AYJd+VocJMgTlZBCnpcI4xIP6oBdVf6W9Qk9Vz97U0tD7o\/pjBcBPCATxrHRBMOGojSYfPZ7iKJulq3ydloD9SNLIQA==','Info':{'C':'d663ca62-5390-4568-bb8a-de96131c63e7','P':'5ee1bc03-42fd-4b52-8fa4-05555bde5eef'}}"; }
    public string QrCodeSingleADJ1 { get => @"{'Chk':'x4\/q+yGwtkOeoVRgEPNCnUYNUa7dYFt5sL+5VxgsyloebScl\/8lcl86Z\/Dhsm\/pNnJMITUWZ8HEmvPjOY47E2LMdavo138293LX4lRiFyHjEmQIXPQAx3SPHVnywUSPfSftb0S8fe6vi\/D07QlqP8tApLrsRq2w51k9o9Xcz1r8PtyQLyWoOpLchEg4fc8SlSMGcAfY3vSLDzYxsVePZHaV5f8AIWd4USWPy9X1m+cfbs1x3yMABKLIT5mDBcrGLBPs+8IpqYQQqXvJeB96WpHz\/7dr+U\/JMxaxoYYZRgmN90vBmGZlL1HQ0AhAAJOaxOUZfy+D+Bx1bDu\/169hIog==','Info':{'C':'8c93ae50-48d7-4136-a027-c257aa2f2d4e','P':'2ee7de51-b03d-46ae-895e-d516d0b269ad'}}"; }
    public string QrCodeSingleADJ2 { get => @"{'Chk':'OL23ulXEyTERodHlrD4MDo9h7M1IKar0RwLgFK9\/rcScDTLBUlDTzhpdDVtW6i\/KzUmMMtkyudyTG9lpKIWXkHDs14FYsocdUIKSMLwJLN5HO4rGfPECQkR9PmcmmrATWgeXabV86kL+Nn0jkoKtSKFmHr7rLocGvkr8TRJr6Q+yThx\/I5l0MEelTq7kSsmsvv9imIln1D0VNXgAG78QalFEAxlubdi8igHkUe2fZayq3QNP1MU6FsjMbBQDFT\/BI9NWJWRmQwccMETtruhweYfhGs1B3ll7gdR7SzCzMZ7H8vlfbyfjp520AuIDXcgIrGEgTUKtMw3mG\/DxjHrFyw==','Info':{'C':'ba7df8e5-14af-44ce-ac4a-a26a91b785c9','P':'5244d36b-0f38-4320-b93f-c24ad7ed083a'}}"; }
    public string QrCodeSingleADJ3 { get => @"{'Chk':'NHW59CaBWZlgfxxjYqc3LdhnOxwZRrS0zojsqBcOtToX1oCk9i0qmUXvhkhhS2EXYZ2TS\/uhTcl1ub6seX0ZtJATFex0NCvp\/n8igbJrLAVX7rdH+FG9Cj6Tu+oCVofCtb1HzpJeFmhHuYnQPL0VRHUS0Aj+T+1VY4DV\/s72M\/fxKqCncAfW7bsdgbaubY5TdMSpEaTWuZGct8tN3KqSHDCM63sRZbuxTlnzj2QSiQbuoRdOtM1MqvEUzmdEQEa5uPJ+9V1g1VbwqO1pGskz8txrcSTI9\/6ZgT\/qUbT5fCZ8t1emkMO2ro1lTPg4NA3V\/HVkQpHVpqGyG1vtg48DUg==','Info':{'C':'3dfeb1dc-5a39-47de-baad-05b85de42db0','P':'723a58f6-8146-4f5a-825d-b41a08924c03'}}"; }
    //public string QrCodeToImplement { get => @"{'Chk':'LaCOG9IbRGM+IHOwDSqQMt8+AJ\/gA0j9xvFNU7oL+2kT0qzppw7sUex4dsKUtdSaM\/\/etZfZNKWs5mLJsXT9GJXQbYvh1Y8JZHnrSwoSVFcyx1NitlIMZR17wKBG\/oKxUvNH7My6n6mzzQZyz57Otb3ZjRlf7tFdfyyheANsV8BSz0dEohF4hez7G5GWaTceF4xiNa0XzNKWamtmcERheykTB6MOz\/A9IvutFnDo05Hp+lSkVWhooAwaQej1k+GuUvAdcxOEV22Ae6QiKwzxhvCpx\/8q+qlsdA6\/dKCCCZKEEUeCuFNd2cyEPre\/iPegiogdmWYaCpE\/lZgBSKe8VQ==','Info':{'C':'05f8a79e-035f-4df2-a6f7-df19fed9dd87','P':'33c221e6-75a6-4486-b32b-ff998e66aaef'}}"; }
    //public string QrCodeToImplement { get => @"{'Chk':'NmEwE6ni3mEBtKTde5skzbhuEBxc4CkmnWjdzgaQTDa34df58ffxoG\/XoL2UVPcI\/c422IBeaFx3tXnSom1AQkmGoPDS0JtFYn\/3QpvhE1JK80BTsT\/vbSDGd4Izm5ZCrvxuWxSah1m+S7vBueZjpwTbhBkZMBFrwjNtlFv50ysTkuPYOPDfIzZBQLcFCeNzTBRw436ca\/tAhCXoSUrkFqNdePEP8qXAkpO\/ybITk4Zmqn31HYRDmz5nF3z+Kk6qL+v1zJBSwBLXvpDHFKTyGhiLg1jc9NlqmrVWs2ADYGmOpYGikqSKh3yODaBLSsD3t+ZZe04Xe1+LfGhILlhV9Q==','Info':{'C':'a9a7ed6b-a46b-4204-92f5-aba75b2673b7','P':'ce218514-abee-4e84-8ff4-6a32c5b8f383'}}"; }
    //public string QrCode1 { get => @"{'Chk':'IXPGFawDk0d+qLSEKuNnBrnGz\/hwPe88wHj0gyyr+v9WJqpoPMgOkGUPCPBfvbzgZTDseN\/aas70Dt7kivXBtjL+brawfepVKOhOk\/G+ESvQDvinJOZKjzZBnbku05r26W6ymhXJhcm651mJ1mlnb0Xx6ikzoIgdVyxu08zfS9eRh+EOSdSNGvC7NAR25xeTTUcGVzRYmaLS4E+lnEmdzPL\/ZhgExIVgytwgU66W7pwA9X7cOcz6n8b0KkySHDD3qgv6WPxITfrWgWZeRrnkor9AoDgg2kWLrjKsYxHopDFR79sBypWXyOKzhSbu3WNx5i1wWhoA0ShSrU6moeK9CA==','Info':{'C':'404416a7-71d5-4604-95c7-e344a8c03317','P':'204e2be8-0473-4cee-9b93-9f50ab1b2c8f'}}"; }
  }

  /// <summary>
  /// Fake Client for REST API consuming (used for testing)
  /// </summary>
  public class ApiClientFake : IApiClient
  {
    private string _prescriptionString;
    private static bool _isFirstCall = true;

    public async Task<BaseResponse<bool>> Login(string usernName, string password)
    {
      _prescriptionString = "Prescription" + GetPrescriptionNumberFromStrGuid(usernName.Split('|')[1]);

      return new BaseResponse<bool>()
      {
        Success = true
      };
    }

    public async Task<AssociateDeviceResponse> AssociateDevice()
    {
      return new AssociateDeviceResponse()
      {
        Data = true,
        Success = true
      };
    }

    public async Task<AssociateDeviceResponse> ChangeAssociatedDevice()
    {
      return null; //not used for test
    }

    public async Task<GetPrescriptionResponse> GetPrescription(string caseId, string patientId)
    {
      return null; //not used
    }

    public async Task<UpdatePackageResponse> DownloadUpdatePackage()
    {
      var _mockedPrescriptionDeserialized = Resources.MockedData.ResourceManager.GetString(_prescriptionString);

      IList<PrescriptionExtended> prescriptions = null;
      UpdatePackageResponse prescriptionResponse = null;
      if (_mockedPrescriptionDeserialized != null)
      {
        prescriptionResponse = JsonConvert.DeserializeObject<UpdatePackageResponse>(_mockedPrescriptionDeserialized);
        prescriptions = prescriptionResponse.Data.Updates.Prescriptions;
      }

      var today = DateTime.Today;
      var tomorrow = today.AddDays(1);
      var caseGuids = new Dictionary<string, Guid>();
      switch (_prescriptionString)
      {
        case "Prescription1": //QrCode1
          caseGuids["A"] = Guid.NewGuid();
          caseGuids["B"] = Guid.NewGuid();
          caseGuids["C"] = Guid.NewGuid();
          prescriptionResponse = CreateUpdatePackageResponse(caseGuids);
          if (_isFirstCall)
          {
            prescriptionResponse.Data.Updates.Prescriptions = new List<PrescriptionExtended>() {
                            CreateMockedPrescriptionWithNoClicks("A", createCorrectionTimes(new int[]{ 1, 2, 4, 16 }), today, (65*4)+1, caseGuids["A"]),
                            CreateMockedPrescriptionWithNoClicks("B", createCorrectionTimes(new int[]{ 12, 16 }), today, (20*2)+1, caseGuids["B"]),
                            CreateMockedPrescriptionWithNoClicks("C", createCorrectionTimes(new int[]{ 8, 16 }), today, (20*2)+1, caseGuids["C"])
                        };
          }
          break;
        case "Prescription2": //QrCode2
                              //UpdateAllStartDates(prescriptions, today);
          break;
        case "Prescription3": //QrCode3
                              //UpdateStartDates(prescriptions[0], today);
                              //index 1 has been revoked
                              //UpdateStartDates(prescriptions[2], today);
          break;
        case "Prescription4": //QrCode4
                              //all indices have been revoked
          break;
        case "Prescription5": //QrCodeSingleADJ1
          caseGuids["A"] = Guid.NewGuid();
          prescriptionResponse = CreateUpdatePackageResponse(caseGuids);
          if (_isFirstCall)
          {
            prescriptionResponse.Data.Updates.Prescriptions = new List<PrescriptionExtended>() {
                            CreateMockedPrescriptionWithNoClicks("A", createCorrectionTimes(new int[]{ 11 }), today, 1, caseGuids["A"])
                        };
          }
          break;
        case "Prescription6": //QrCodeSingleADJ2
          caseGuids["A"] = Guid.NewGuid();
          caseGuids["B"] = Guid.NewGuid();
          caseGuids["C"] = Guid.NewGuid();
          caseGuids["D"] = Guid.NewGuid();
          caseGuids["E"] = Guid.NewGuid();
          caseGuids["F"] = Guid.NewGuid();
          caseGuids["G"] = Guid.NewGuid();
          caseGuids["H"] = Guid.NewGuid();
          caseGuids["I"] = Guid.NewGuid();
          prescriptionResponse = CreateUpdatePackageResponse(caseGuids);
          if (_isFirstCall)
          {
            prescriptionResponse.Data.Updates.Prescriptions = new List<PrescriptionExtended>() {
                            CreateMockedPrescriptionWithNoClicks("A", createCorrectionTimes(new int[]{ 1,11 }), today, 2, caseGuids["A"]),
                            CreateMockedPrescriptionWithNoClicks("B", createCorrectionTimes(new int[]{ 11 }), today, 1, caseGuids["B"]),
                            CreateMockedPrescriptionWithNoClicks("C", createCorrectionTimes(new int[]{ 11 }), today, 1, caseGuids["C"]),
                            CreateMockedPrescriptionWithNoClicks("D", createCorrectionTimes(new int[]{ 11 }), today, 1, caseGuids["D"]),
                            CreateMockedPrescriptionWithNoClicks("E", createCorrectionTimes(new int[]{ 11 }), today, 1, caseGuids["E"]),
                            CreateMockedPrescriptionWithNoClicks("F", createCorrectionTimes(new int[]{ 11 }), today, 1, caseGuids["F"]),
                            CreateMockedPrescriptionWithNoClicks("G", createCorrectionTimes(new int[]{ 11 }), today, 1, caseGuids["G"]),
                            CreateMockedPrescriptionWithNoClicks("H", createCorrectionTimes(new int[]{ 11 }), today, 1, caseGuids["H"]),
                            CreateMockedPrescriptionWithNoClicks("I", createCorrectionTimes(new int[]{ 11 }), today, 1, caseGuids["I"])
                        };
          }
          break;
        case "Prescription7": //QrCodeSingleADJ3
          caseGuids["A"] = Guid.NewGuid();
          prescriptionResponse = CreateUpdatePackageResponse(caseGuids);
          if (_isFirstCall)
          {
            prescriptionResponse.Data.Updates.Prescriptions = new List<PrescriptionExtended>() {
                            CreateMockedPrescriptionWithNoClicks("A", createCorrectionTimes(new int[]{ 1,11 }), today, 2, caseGuids["A"])
                        };
          }
          break;
        case "Prescription8": //QrCodeSingleADJ0
          caseGuids["A"] = Guid.NewGuid();
          caseGuids["B"] = Guid.NewGuid();
          caseGuids["C"] = Guid.NewGuid();
          caseGuids["D"] = Guid.NewGuid();
          caseGuids["E"] = Guid.NewGuid();
          caseGuids["F"] = Guid.NewGuid();
          caseGuids["G"] = Guid.NewGuid();
          caseGuids["H"] = Guid.NewGuid();
          caseGuids["I"] = Guid.NewGuid();
          prescriptionResponse = CreateUpdatePackageResponse(caseGuids);
          if (_isFirstCall)
          {
            prescriptionResponse.Data.Updates.Prescriptions = new List<PrescriptionExtended>() {
                            CreateMockedPrescriptionWithNoClicks("A", createCorrectionTimes(new int[]{ 11 }), tomorrow, 2, caseGuids["A"]),
                            CreateMockedPrescriptionWithNoClicks("B", createCorrectionTimes(new int[]{ 11 }), tomorrow, 2, caseGuids["B"]),
                            CreateMockedPrescriptionWithNoClicks("C", createCorrectionTimes(new int[]{ 11 }), tomorrow, 2, caseGuids["C"]),
                            CreateMockedPrescriptionWithNoClicks("D", createCorrectionTimes(new int[]{ 11 }), tomorrow, 2, caseGuids["D"]),
                            CreateMockedPrescriptionWithNoClicks("E", createCorrectionTimes(new int[]{ 11 }), tomorrow, 2, caseGuids["E"]),
                            CreateMockedPrescriptionWithNoClicks("F", createCorrectionTimes(new int[]{ 11 }), tomorrow, 2, caseGuids["F"]),
                            CreateMockedPrescriptionWithNoClicks("G", createCorrectionTimes(new int[]{ 11 }), tomorrow, 2, caseGuids["G"]),
                            CreateMockedPrescriptionWithNoClicks("H", createCorrectionTimes(new int[]{ 11 }), tomorrow, 2, caseGuids["H"]),
                            CreateMockedPrescriptionWithNoClicks("I", createCorrectionTimes(new int[]{ 11 }), tomorrow, 2, caseGuids["I"])
                        };
          }
          break;
      }
      prescriptionResponse.Data.Sign = DependencyService.Get<ICryptoService>().SignWithSHAPortableToString(prescriptionResponse.Data.Updates);
      _isFirstCall = false;
      return prescriptionResponse;
    }

    /// <summary>
    /// Translate
    /// </summary>
    /// <param name="FakeGuid"></param>
    /// <returns></returns>
    private string GetPrescriptionNumberFromStrGuid(string FakeGuid)
    {
      switch (FakeGuid)
      {
        case "28f7c4bd-4041-4b2d-9c04-f6868794c3bb":
          return "1";
        case "8c93ae50-48d7-4136-a027-c257aa2f2d4e":
          return "5";
        case "ba7df8e5-14af-44ce-ac4a-a26a91b785c9":
          return "6";
        case "3dfeb1dc-5a39-47de-baad-05b85de42db0":
          return "7";
        case "d663ca62-5390-4568-bb8a-de96131c63e7":
          return "8";
          //case "05f8a79e-035f-4df2-a6f7-df19fed9dd87":
          //    return "TOIMPLEMENT";
          //case "a9a7ed6b-a46b-4204-92f5-aba75b2673b7":
          //   return "TOIMPLEMENT";
          //case "404416a7-71d5-4604-95c7-e344a8c03317":
          //    return "1";
      }
      throw new Exception("Fake prescription doesn't exist. Check Case QrCode Guid.");
    }

    /// <summary>
    /// Update all dates (struts clicks included) considering the new start treatment date
    /// </summary>
    /// <param name="prescription">Prescription to modify</param>
    /// <param name="startDate">New treatment start date</param>
    private void UpdateStartDates(Prescription prescription, DateTime startDate)
    {
      TimeSpan timeToAdd = startDate.Subtract(prescription.ScheduleInfo.DateOfTreatmentStart.Value);

      var schedule = prescription.ScheduleInfo;
      schedule.SurgeryDate = schedule.SurgeryDate.Value.Add(timeToAdd);
      schedule.FinalTreatmentDate = schedule.FinalTreatmentDate.Value.Add(timeToAdd);
      schedule.DateOfTreatmentStart = schedule.DateOfTreatmentStart.Value.Add(timeToAdd);

      var strutsClick = prescription.PrescriptionData.Parts;
      foreach (var click in strutsClick)
      {
        click.DateOfAdjustment = click.DateOfAdjustment.Value.Add(timeToAdd);
      }
    }

    /// <summary>
    /// Update all dates (struts clicks included) considering the new start treatment date, of all prescriptions
    /// </summary>
    /// <param name="prescriptions">Prescription list to modify</param>
    /// <param name="startDate">New treatment start date</param>
    private void UpdateAllStartDates(IList<Prescription> prescriptions, DateTime startDate)
    {
      foreach (var prescription in prescriptions)
      {
        UpdateStartDates(prescription, startDate);
      }
    }

    /// <summary>
    /// TEMPORARY --- Set only a single struct click for a prescription
    /// </summary>
    private void SetOnly1StrutsClicks(Prescription prescription)
    {
      prescription.PrescriptionData.Parts = new List<PrescriptionClick>() { prescription.PrescriptionData.Parts[0] };
    }

    private PrescriptionExtended CreateMockedPrescriptionWithNoClicks(string frameID, bool[] correctionTimes, DateTime startTreatmentDate, int correctionClicksCount, Guid caseGuid)
    {
      Dictionary<string, bool?> correctionTimesConverted = new Dictionary<string, bool?>();
      for (int i = 0; i < correctionTimes.Length; i++)
      {
        correctionTimesConverted[i.ToString()] = correctionTimes[i];
      }

      var partsPreCalcolated = createPrescriptionsClick(startTreatmentDate, correctionTimes, correctionClicksCount);

      return new PrescriptionExtended()
      {
        Prescription = new Prescription()
        {
          CaseData = new CaseData()
          {
            AnatomiesType = 10,
            BoneTypeSegment = 10,
            CaseGuid = caseGuid,
            FrameID = frameID,
            Name = "testName",
            Number = "testNumber"
          },
          ScheduleInfo = new Schedule()
          {
            CorrectionTimes = correctionTimesConverted,
            DateOfTreatmentStart = startTreatmentDate,
            FinalTreatmentDate = partsPreCalcolated[partsPreCalcolated.Count - 1].DateOfAdjustment.Value.Date,
            IsApplyLengtheningShorteningFirst = false,
            LatencyDays = 0,
            SurgeryDate = startTreatmentDate.AddDays(-1)
          },
          PrescriptionData = new PrescriptionData()
          {
            PrescriptionNotes = null,
            Parts = partsPreCalcolated
          }
        },
        RemovalDate = DateTime.Now
      };
    }

    private List<PrescriptionClick> createPrescriptionsClick(DateTime startTreatmentDate, bool[] correctionTimes, int correctionClicksCount)
    {
      var response = new List<PrescriptionClick>();
      List<int> correctionTimesConverted = new List<int>();
      for (int i = 0; i < correctionTimes.Length; i++)
      {
        if (correctionTimes[i])
        {
          correctionTimesConverted.Add(i);
        }
      }

      for (int i = 0; i < correctionClicksCount; i++)
      {
        var prescriptionClick = new PrescriptionClick()
        {
          DateOfAdjustment = startTreatmentDate.AddHours(correctionTimesConverted[i % correctionTimesConverted.Count]).AddDays(i / correctionTimesConverted.Count),
          TreatmentStepNumber = i,
          StageType = 10,
          Sequence = i,
          Struts = new List<Strut>()
                    {
                        CreateStrut(1),
                        CreateStrut(2),
                        CreateStrut(3),
                        CreateStrut(4,-1),
                        CreateStrut(5),
                        CreateStrut(6)
                    }
        };
        response.Add(prescriptionClick);
      }
      return response;
    }

    private Strut CreateStrut(int index, int clickSign = 1)
    {
      return new Strut()
      {
        Acute = 5,
        Click = 1 * clickSign,
        Gradual = 35,
        StrutNumber = index,
        StrutSize = 20,
        Total = 120 //give it a sense?
      };
    }

    private UpdatePackageResponse CreateUpdatePackageResponse(Dictionary<string, Guid> caseGuids)
    {
      var state = _isFirstCall ? (int)PrescriptionState.Added : (int)PrescriptionState.Unchanged;
      var result = new UpdatePackageResponse()
      {
        Data = new PackageUpdate()
        {
          Updates = new PrescriptionUpdate()
          {
            Prescriptions = null,
            Changes = new List<Change>()
          },
          Sign = null
        },
        Success = true,
        ErrorCode = 0,
        ErrorMessage = null
      };

      foreach (KeyValuePair<string, Guid> entry in caseGuids)
      {
        result.Data.Updates.Changes.Add(new Change()
        {
          CaseId = entry.Value,
          FrameID = entry.Key,
          Datetime = DateTime.Now,
          State = state
        });
      }
      return result;
    }

    private bool[] createCorrectionTimes(int[] times)
    {
      bool[] correctionTimes = new bool[24];
      for (int i = 0; i < times.Length; i++)
      {
        correctionTimes[times[i]] = true;
      }
      return correctionTimes;
    }

    public async Task<GetSettingsResponse> GetSettings()
    {
      var GuidAddress = Guid.NewGuid();
      var GuidCase = Guid.NewGuid();
      return new GetSettingsResponse()
      {
        Data = new PortalSettingsDTO()
        {
          PinSiteCareSettings = new PinSiteCare()
          {
            Enabled = true,
            FirstDay = DateTime.Today,
            Frequency = new List<bool?>() { true, false, false, false, false, false, false },
            Id = 1,
            PatientTime = "PatientTime", //NEED TO BE IMPLEMENT?
            Time = new List<bool?>() { true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
          },
          SurgeonAddressInfo = new SurgeonAddress()
          {
            AddressLine1 = "AddressLine1",
            AddressLine2 = "AddressLine2",
            City = "City",
            Country = new Country()
            {
              Description = "Description",
              Id = 1
            },
            FirstName = "FirstName",
            GuidAddress = GuidAddress,
            Hospital = "Hospital",
            LastName = "LastName",
            MobilePhone = "+391234567890",
            OfficePhone = "+391234567890",
            PostalCode = "PostalCode",
            StateProvince = "StateProvince"
          },
          FinalTreatmentDateList = new List<FinalTreatmentCaseDateDTO>() //NEED TO HAVE MORE ELEMENTS?
                    {
                        new FinalTreatmentCaseDateDTO()
                        {
                            CaseUid = GuidCase,
                            FinalTreatmentDate = DateTime.Today
                        }
                    }
        },
        Success = true,
        ErrorCode = 0,
        ErrorMessage = null
      };
    }

    public async Task<SetSettingsResponse> UploadSettings(List<PatientDiaryEvent> events, TimeSpan pinSiteCareTime, bool isGoalEnabled, bool isMotivationalMessageEnabled, TimeSpan motivationalMessageTime, bool isPushEnabled, string appVersion)
    {
      return new SetSettingsResponse()
      {
        Data = null, //not used for now
        Success = true,
        ErrorCode = 0,
        ErrorMessage = null
      };
    }

    public async Task<SetSyncCompletedResponse> SetSyncCompleted()
    {
      return new SetSyncCompletedResponse()
      {
        Success = true,
        Data = DateTime.Now,
        ErrorCode = 0,
        ErrorMessage = null
      };
    }

    public Task<bool> IsServerReachable()
    {
      return Task.FromResult(true);
    }

    public Task<GetSigningCertificateResponse> GetSigningCertificate(DateTime? refDate)
    {
      var bytes = new byte[1024];

      return Task.FromResult(new GetSigningCertificateResponse()
      {
        Success = true,
        Data = new CertificateBinDTO()
        {
          Exists = true,
          Certificate = bytes
        }
      });
    }
  }
}