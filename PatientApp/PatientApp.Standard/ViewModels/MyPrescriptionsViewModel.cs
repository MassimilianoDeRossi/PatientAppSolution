using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

using PatientApp.Interfaces;
using PatientApp.Settings;
using PatientApp.Views;
using PatientApp.DataModel.SqlEntities;
using PatientApp.Utilities;

namespace PatientApp.ViewModels
{

  /// <summary>
  /// ViewModel for prescription list related to the patients.
  /// </summary>
  public class MyPrescriptionsViewModel : BaseViewModel
  {
    ObservableCollection<PrescriptionBarModel> _prescriptionRecap;
    public ObservableCollection<PrescriptionBarModel> PrescriptionRecap
    {
      get { return _prescriptionRecap; }
      set { SetProperty(ref _prescriptionRecap, value); }
    }

    string _syncStateDescription;
    public string SyncStateDescription
    {
      get { return _syncStateDescription; }
      set { SetProperty(ref _syncStateDescription, value); }
    }

    bool _syncError;
    public bool SyncError
    {
      get { return _syncError; }
      set { SetProperty(ref _syncError, value); }
    }

    /// <summary>
    /// The viewmodel for a prescription viewbar
    /// </summary>
    public class PrescriptionBarModel : ObservableObject
    {
      public string FrameID { get; set; }
      public string CurrentPhase { get; set; }
      public string DebugPhaseCode { get; set; }
      public int LatencyDaysLength { get; set; }
      public int CorrectionDaysLength { get; set; }
      public int ConsolidationDaysLength { get; set; }
      public int CorrectionDaysCurrentLength { get; set; }

      public GridLength Col1Length { get; protected set; } = GridLength.Auto;
      public GridLength Col2Length { get; protected set; } = GridLength.Auto;
      public GridLength Col3Length { get; protected set; } = GridLength.Auto;
      public GridLength Col4Length { get; protected set; } = GridLength.Auto;

      public DateTime StatusDateTime { get; set; }
      public string StatusDateTimeFormatted { get { return this.StatusDateTime.ToString("dd-MMM h:mm tt"); } }

      public string Status
      {
        get
        {
          switch (PrescriptionState)
          {
            case SyncResultStateEnum.Added:
              return Resources.PatientApp.LblAddedStatusMyPrescriptionPrefix;
            case SyncResultStateEnum.Updated:
              return Resources.PatientApp.LblUpdatedStatusMyPrescriptionPrefix;
            case SyncResultStateEnum.Revoked:
              return Resources.PatientApp.LblRevokedStatusMyPrescriptionPrefix + " " + FrameID + " " + Resources.PatientApp.LblRevokedStatusMyPrescriptionSuffix;
            default:
              return App.TestModel.TestModeOn ? "UNCHANGED" : string.Empty;
          }
        }
        set { }
      }

      public bool IsLatencyStarted { get; set; }
      public bool IsCorrectionStarted { get; set; }
      public bool IsConsolidationStarted { get; set; }

      public Color Col1Color { get; set; }
      public Color Col2Color { get; set; }
      public Color Col3Color { get; set; }
      public Color Col4Color { get; set; }

      public bool Sep1Visible { get; set; }
      public bool Sep3Visible { get; set; }

      public SyncResultStateEnum PrescriptionState { get; set; }

      public bool IsRemoved
      {
        get { return PrescriptionState == SyncResultStateEnum.Revoked; }
        set { }
      }

      /// <summary>
      /// Build the graphical aspect of the bar based on prescrition phase and current date
      /// </summary>
      public void BuildViewBars()
      {
        double totalDaysLength = CorrectionDaysLength + ConsolidationDaysLength + LatencyDaysLength;

        var debugPhase = string.Empty;

        if (totalDaysLength > 0)
        {
          var latencyPerc = LatencyDaysLength > 0 ? (LatencyDaysLength / totalDaysLength) * 100.0d : 0;
          var correctionPerc = CorrectionDaysLength > 0 ? (CorrectionDaysLength / totalDaysLength) * 100.0d : 0d;
          var consolidationPerc = ConsolidationDaysLength > 0 ? (ConsolidationDaysLength / totalDaysLength) * 100.0d : 0d;

          var currentPerc = (CorrectionDaysCurrentLength / totalDaysLength) * 100.0d;

          var noColor = Color.Transparent;
          var inactiveColor = (Color)Application.Current.Resources["InactivePhaseColor"];
          var latencyColor = IsLatencyStarted ? (Color)Application.Current.Resources["ActiveLatencyColor"] : inactiveColor;
          var correctionColor = IsCorrectionStarted ? (Color)Application.Current.Resources["ActiveCorrectionColor"] : inactiveColor;
          var consolidationColor = IsConsolidationStarted ? (Color)Application.Current.Resources["ActiveConsolidationColor"] : inactiveColor;

          if (latencyPerc == 0 & correctionPerc == 0 && consolidationPerc == 0) // 000
          {
            // No phases
            Col1Length = new GridLength(0, GridUnitType.Absolute);
            Col1Color = noColor;
            Col2Length = new GridLength(0, GridUnitType.Absolute);
            Col2Color = noColor;
            Col3Length = new GridLength(0, GridUnitType.Absolute);
            Col3Color = noColor;
            Col4Length = new GridLength(0, GridUnitType.Absolute);
            Col4Color = noColor;

            Sep1Visible = Sep3Visible = false;
            debugPhase = "000";
          }
          else if (latencyPerc == 0 & correctionPerc == 0 && consolidationPerc > 0) // 001
          {
            // Consolidation only
            Col1Length = new GridLength(1, GridUnitType.Star);
            Col1Color = consolidationColor;
            Col2Length = new GridLength(0, GridUnitType.Absolute);
            Col2Color = noColor;
            Col3Length = new GridLength(0, GridUnitType.Absolute);
            Col3Color = noColor;
            Col4Length = new GridLength(1, GridUnitType.Star);
            Col4Color = consolidationColor;

            Sep1Visible = Sep3Visible = false;
            debugPhase = "001";
          }
          else if (latencyPerc == 0 & correctionPerc > 0 && consolidationPerc == 0) // 010
          {
            // Correction only
            if (currentPerc > 0)
            {
              Col1Length = new GridLength(currentPerc, GridUnitType.Star);
              Col1Color = correctionColor;
              Col2Length = new GridLength(0, GridUnitType.Absolute);
              Col2Color = noColor;
              Col3Length = new GridLength(0, GridUnitType.Absolute);
              Col3Color = noColor;
              Col4Length = new GridLength(100 - currentPerc, GridUnitType.Star);
              Col4Color = inactiveColor;

              Sep1Visible = false;
              Sep3Visible = true;
            }
            else
            {
              Col1Length = new GridLength(50, GridUnitType.Star);
              Col1Color = correctionColor;
              Col2Length = new GridLength(0, GridUnitType.Absolute);
              Col2Color = noColor;
              Col3Length = new GridLength(0, GridUnitType.Absolute);
              Col3Color = noColor;
              Col4Length = new GridLength(50, GridUnitType.Star);
              Col4Color = correctionColor;

              Sep1Visible = Sep3Visible = false;
            }

            debugPhase = "010";
          }
          else if (latencyPerc == 0 && correctionPerc > 0 && consolidationPerc > 0) // 011
          {
            // Correction and consolidation
            if (currentPerc > 0)
            {
              Col1Length = new GridLength(currentPerc, GridUnitType.Star);
              Col1Color = correctionColor;
              Col2Length = new GridLength(correctionPerc - currentPerc, GridUnitType.Star);
              Col2Color = inactiveColor;
            }
            else
            {
              Col1Length = new GridLength(correctionPerc, GridUnitType.Star);
              Col1Color = correctionColor;
              Col2Length = new GridLength(0, GridUnitType.Absolute);
              Col2Color = noColor;
            }
            Col3Length = new GridLength(0, GridUnitType.Absolute);
            Col3Color = noColor;
            Col4Length = new GridLength(consolidationPerc, GridUnitType.Star);
            Col4Color = consolidationColor;

            Sep1Visible = Sep3Visible = true;
            debugPhase = "011";
          }
          else if (latencyPerc > 0 & correctionPerc == 0 && consolidationPerc == 0) // 100
          {
            // Latency only
            Col1Length = new GridLength(1, GridUnitType.Star);
            Col1Color = latencyColor;
            Col2Length = new GridLength(0, GridUnitType.Absolute);
            Col2Color = noColor;
            Col3Length = new GridLength(0, GridUnitType.Absolute);
            Col3Color = noColor;
            Col4Length = new GridLength(1, GridUnitType.Star);
            Col4Color = latencyColor;

            Sep1Visible = Sep3Visible = false;
            debugPhase = "100";
          }

          else if (latencyPerc > 0 && correctionPerc == 0 && consolidationPerc > 0) // 101
          {
            // Latency and consolidation
            Col1Length = new GridLength(latencyPerc, GridUnitType.Star);
            Col1Color = latencyColor;
            Col2Length = new GridLength(0, GridUnitType.Absolute);
            Col2Color = noColor;
            Col3Length = new GridLength(0, GridUnitType.Absolute);
            Col3Color = noColor;
            Col4Length = new GridLength(consolidationPerc, GridUnitType.Star);
            Col4Color = consolidationColor;

            Sep1Visible = Sep3Visible = true;
            debugPhase = "101";
          }
          else if (latencyPerc > 0 && correctionPerc > 0 && consolidationPerc == 0) // 110
          {
            // Latency and correction
            Col1Length = new GridLength(latencyPerc, GridUnitType.Star);
            Col1Color = latencyColor;
            Col2Length = new GridLength(0, GridUnitType.Absolute);
            Col2Color = noColor;
            if (currentPerc > 0)
            {
              Col3Length = new GridLength(currentPerc, GridUnitType.Star);
              Col3Color = correctionColor;
              Col4Length = new GridLength(correctionPerc - currentPerc, GridUnitType.Star);
              Col4Color = inactiveColor;
            }
            else
            {
              Col3Length = new GridLength(0, GridUnitType.Absolute);
              Col3Color = noColor;
              Col4Length = new GridLength(correctionPerc, GridUnitType.Star);
              Col4Color = correctionColor;
            }

            Sep1Visible = Sep3Visible = true;
            debugPhase = "110";
          }
          else if (latencyPerc > 0 && correctionPerc > 0 && consolidationPerc > 0) // 111
          {
            // All phases
            Col1Length = new GridLength(latencyPerc, GridUnitType.Star);
            Col1Color = latencyColor;
            if (currentPerc > 0)
            {
              Col2Length = new GridLength(currentPerc - latencyPerc, GridUnitType.Star);
              Col2Color = correctionColor;
              Col3Length = new GridLength(correctionPerc - (currentPerc - latencyPerc), GridUnitType.Star);
              Col3Color = inactiveColor;
            }
            else
            {
              Col2Length = new GridLength(correctionPerc, GridUnitType.Star);
              Col2Color = correctionColor;
              Col3Length = new GridLength(0, GridUnitType.Absolute);
              Col3Color = noColor;
            }
            Col4Length = new GridLength(consolidationPerc, GridUnitType.Star);
            Col4Color = consolidationColor;

            Sep1Visible = Sep3Visible = true;
            debugPhase = "111";
          }

        }
        else
        {
          Col1Length = new GridLength(0, GridUnitType.Star);
          Col2Length = new GridLength(0, GridUnitType.Star);
          Col3Length = new GridLength(0, GridUnitType.Star);
        }

        OnPropertyChanged(nameof(Col1Length));
        OnPropertyChanged(nameof(Col2Length));
        OnPropertyChanged(nameof(Col3Length));

        OnPropertyChanged(nameof(Col1Color));
        OnPropertyChanged(nameof(Col2Color));
        OnPropertyChanged(nameof(Col3Color));

        OnPropertyChanged(nameof(IsRemoved));

        this.DebugPhaseCode = debugPhase;

      }

    }

    public MyPrescriptionsViewModel(ILocalDatabaseService dbService, ISystemUtility sysUtility) : base(dbService, null, sysUtility)
    {
      // Refresh data on view appearing
      MessagingCenter.Subscribe<BaseContentPage>(this, Messaging.Messages.VIEW_APPEARING_MESSAGE, ViewAppearing);
    }

    private async void ViewAppearing(BaseContentPage page)
    {
      //DependencyService.Get<IOrientationManager>().ForcePortrait();
      if (page is MyPrescriptionsPage)
      {
        await LoadRecapAndResetChanges();
        // Show sync status description or error
        SyncError = AppSettings.Instance.HasSyncPending;
        if (SyncError)
        {
          SyncStateDescription = Resources.PatientApp.LblLastSyncDatePending;
        }
        else if (IsLoggedIn && (!AppSettings.Instance.SyncLastDateTime.HasValue || AppSettings.Instance.SyncLastDateTime.Value < _sysUtility.Now.AddDays(-1)))
        {
          SyncError = true;
          SyncStateDescription = Resources.PatientApp.ErrorConnectionMissing;
        }
        else if (!AppSettings.Instance.SyncLastDateTime.HasValue)
        {
          SyncStateDescription = Resources.PatientApp.LblLastSyncDateMyPrescriptionPrefix + " " + Resources.PatientApp.LblLastSyncDateFailed;
        }
        else
          SyncStateDescription = Resources.PatientApp.LblLastSyncDateMyPrescriptionPrefix + " " + AppSettings.Instance.SyncLastDateTime.Value.ToString();
      }
    }

    /// <summary>
    /// Check if a date is between two date.
    /// </summary>
    /// <param name="now"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    private bool DateBetween(DateTime now, DateTime start, DateTime end)
    {
      // see if start comes before end
      if (start <= end)
        return start < now && now < end;
      // start is after end, so do the inverse comparison
      return !(end < now && now < start);
    }

    /// <summary>
    ///  Build the list of prescriptions with significant treatment times and status
    ///  and reset changes in database
    /// </summary>
    private async Task LoadRecapAndResetChanges()
    {
      //Retrieved downloaded changes.
      var changes = await _dbService.GetSyncResults();

      //Retrieve prescriptions downloaded.
      var prescriptions = (await _dbService.GetPrescriptions()).OrderBy(p => p.FrameID);

      var recapList = new List<PrescriptionBarModel>();

      foreach (var p in prescriptions)
      {
        var change = changes.FirstOrDefault(x => x.FrameId == p.FrameID);
        var bar = new PrescriptionBarModel()
        {
          FrameID = p.FrameID,
          LatencyDaysLength = (p.StartOfTreatment.Value.Date).Subtract(p.SurgeryDate.Value.Date).Days,
          CorrectionDaysLength = (p.EndOfTreatment.Value.Date).Subtract(p.StartOfTreatment.Value.Date).Days + 1,
          ConsolidationDaysLength = (p.RemovalDate.Value.Date).Subtract(p.EndOfTreatment.Value.Date).Days - 1,
          PrescriptionState = change != null ? change.State : SyncResultStateEnum.Unchanged,
          IsLatencyStarted = _sysUtility.Now.Date > p.SurgeryDate.Value.Date,
          IsCorrectionStarted = _sysUtility.Now.Date > p.StartOfTreatment.Value.Date,
          IsConsolidationStarted = _sysUtility.Now.Date > p.EndOfTreatment.Value.Date,
          StatusDateTime = p.LastSyncDate.Value
        };

        if (bar.IsConsolidationStarted && bar.ConsolidationDaysLength == 0)
        {
          bar.IsConsolidationStarted = false;
        }

        var isInLatency = DateBetween(_sysUtility.Now.Date, p.SurgeryDate.Value.Date, p.StartOfTreatment.Value.Date);
        var isInCorrection = DateBetween(_sysUtility.Now.Date, p.StartOfTreatment.Value.Date, p.EndOfTreatment.Value.Date);
        var isInConsolidation = DateBetween(_sysUtility.Now.Date, p.EndOfTreatment.Value.Date, p.RemovalDate.Value.Date);

        string phase = string.Empty;
        if (isInLatency)
        {
          phase = PatientApp.Resources.PatientApp.LblMyPrescriptionsLatencyLegend;
        }
        else if (isInCorrection)
        {
          phase = PatientApp.Resources.PatientApp.LblMyPrescriptionsCorrectionLegend;
          bar.CorrectionDaysCurrentLength = bar.LatencyDaysLength + _sysUtility.Now.Date.Subtract(p.StartOfTreatment.Value.Date).Days;
        }
        else if (isInConsolidation)
        {
          phase = PatientApp.Resources.PatientApp.LblMyPrescriptionsConsolidationLegend;
        }

        bar.CurrentPhase = phase.Length > 0 ? string.Format("{0} {1}", PatientApp.Resources.PatientApp.LblYouAreInPhasePefix, phase) : string.Empty;

        recapList.Add(bar);
      }
      //Add Revoked prescription.
      foreach (var c in changes.Where(x => x.State == SyncResultStateEnum.Revoked))
      {
        recapList.Add(new PrescriptionBarModel()
        {
          FrameID = c.FrameId,
          PrescriptionState = c.State,
          StatusDateTime = c.DateTime.Value
        });
      }

      // Recalculate all visual lenghts and colors
      foreach (var item in recapList)
        item.BuildViewBars();

      PrescriptionRecap = new ObservableCollection<PrescriptionBarModel>(recapList.OrderBy(x => x.FrameID));

      await _dbService.ClearSyncResults();
    }


  }

}
