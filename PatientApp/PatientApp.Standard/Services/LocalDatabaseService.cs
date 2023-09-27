using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

using Xamarin.Forms;
using SQLite;

using PatientApp.Interfaces;
using PatientApp.DataModel.SqlEntities;
using PatientApp.Utilities;
using PatientApp.Settings;

namespace PatientApp.Services
{
  /// <summary>
  /// SqlLite local database management
  /// </summary>
  public class LocalDatabaseService : ILocalDatabaseService
  {
    private static readonly string a = "PRAGMA key=xXNodNA3avqS65Do1W36";

    private readonly ISystemUtility _sysUtility = null;

    static readonly Lazy<SQLiteAsyncConnection> _connectionHolder = new Lazy<SQLiteAsyncConnection>(() => DependencyService.Get<ISQLite>().GetAsyncConnection());

    private static SQLiteAsyncConnection ConnectionInstance => _connectionHolder.Value;

    /// <summary>
    /// The last exception thrown in methods returning false result
    /// </summary>
    public Exception LastException { get; set; }


    /// <summary>
    /// Instantiate a new database manager
    /// </summary>
    /// <param name="sysUtility"></param>
    public LocalDatabaseService(ISystemUtility sysUtility)
    {
      _sysUtility = sysUtility;
      Console.WriteLine("MYHEXPLAN: Database service constructor");
      InitConnection();
    }

    public async Task InitConnection()
    {
      await ConnectionInstance.SetBusyTimeoutAsync(new TimeSpan(0, 0, 5)).ConfigureAwait(false);
      await ConnectionInstance.QueryAsync<int>(a).ConfigureAwait(false);     
    }

    /// <summary>
    /// Initialize database content: create all not existing tables 
    /// </summary>
    /// <param name="forced"></param>
    /// <returns></returns>
    public async Task<bool> CreateTablesIfNotExists(bool forced = false)
    {      
      if (forced || !await TableExists(ConnectionInstance, nameof(Prescription)))
        await ConnectionInstance.CreateTableAsync<Prescription>();

      if (forced || !await TableExists(ConnectionInstance, nameof(StrutAdjustment)))
        await ConnectionInstance.CreateTableAsync<StrutAdjustment>();

      if (forced || !await TableExists(ConnectionInstance, nameof(LogHistoryItem)))
        await ConnectionInstance.CreateTableAsync<LogHistoryItem>();

      if (forced || !await TableExists(ConnectionInstance, nameof(Reminder)))
        await ConnectionInstance.CreateTableAsync<Reminder>();

      if (forced || !await TableExists(ConnectionInstance, nameof(ShoppingItem)) || !AppSettings.Instance.DatabaseVersion.HasValue || AppSettings.Instance.DatabaseVersion.Value < 2)
      {
        bool restoreChecked = false;
        ShoppingItem[] savedCheckedItems = null;

        if (await TableExists(ConnectionInstance, nameof(ShoppingItem)))
        {
          // Save old checked items
          savedCheckedItems = await ConnectionInstance.Table<ShoppingItem>().ToArrayAsync();
          await ConnectionInstance.DropTableAsync<ShoppingItem>();
          restoreChecked = true;
        }

        await ConnectionInstance.CreateTableAsync<ShoppingItem>();
        //Fill shoppinglist with default values
        var shoppingList = new ShoppingItem[]
        {
                    new ShoppingItem
                    {
                        //Description = Resources.PatientApp.ShoppingList_ShoppingItem_CleansingSolution,
                        Description = "ShoppingList_ShoppingItem_CleansingSolution",
                        IsChecked = false
                    },
                    new ShoppingItem
                    {
                        //Description =   Resources.PatientApp.ShoppingList_ShoppingItem_DisposableCups,
                        Description =   "ShoppingList_ShoppingItem_DisposableCups",
                        IsChecked = false
                    },
                    new ShoppingItem
                    {
                        //Description = Resources.PatientApp.ShoppingList_ShoppingItem_SterileGauze22,
                        Description = "ShoppingList_ShoppingItem_SterileGauze22",
                        IsChecked = false
                    },
                    new ShoppingItem
                    {
                        //Description =  Resources.PatientApp.ShoppingList_ShoppingItem_SterileGauze44,
                        Description =  "ShoppingList_ShoppingItem_SterileGauze44",
                        IsChecked = false
                    },
                    new ShoppingItem
                    {
                        //Description = Resources.PatientApp.ShoppingList_ShoppingItem_SterileCottonSwabs,
                        Description = "ShoppingList_ShoppingItem_SterileCottonSwabs",
                        IsChecked = false
                    },
                    new ShoppingItem
                    {
                        //Description = Resources.PatientApp.ShoppingList_ShoppingItem_BagForWaste,
                        Description = "ShoppingList_ShoppingItem_BagForWaste",
                        IsChecked = false
                    }
        };

        for (int i = 0; i < shoppingList.Length; i++)
        {
          // Restore checked if upgrading
          if (restoreChecked)
            shoppingList[i].IsChecked = savedCheckedItems[i].IsChecked;

          await SaveEntityAsync(ConnectionInstance, shoppingList[i]);
        }
      }

      if (forced || !await TableExists(ConnectionInstance, nameof(SurgeonContacts)))
        await ConnectionInstance.CreateTableAsync<SurgeonContacts>();

      if (forced || !await TableExists(ConnectionInstance, nameof(SyncResult)))
        await ConnectionInstance.CreateTableAsync<SyncResult>();


      AppSettings.SetDatabaseVersion(2);

      return true;
    }

    /// <summary>
    /// Check for database corruption
    /// </summary>
    /// <returns></returns>
    public async Task<bool> CheckIntegrity()
    {
      bool success = true;
      try
      {
        await GetPrescriptions();
        await GetReminders();
        await GetShoppingItems();
        await GetSurgeonContacts();
        await GetUnsyncedHistoryLogItems();

      }
      catch (Exception ex)
      {
        LastException = ex;
        success = false;
      }

      return success;
    }

    /// <summary>
    /// Reset database content deleting all tables
    /// </summary>
    public async Task DeleteAllTables()
    {      
      if (await TableExists(ConnectionInstance, nameof(Prescription)))
        await ConnectionInstance.DropTableAsync<Prescription>();

      if (await TableExists(ConnectionInstance, nameof(StrutAdjustment)))
        await ConnectionInstance.DropTableAsync<StrutAdjustment>();

      if (await TableExists(ConnectionInstance, nameof(LogHistoryItem)))
        await ConnectionInstance.DropTableAsync<LogHistoryItem>();

      if (await TableExists(ConnectionInstance, nameof(Reminder)))
        await ConnectionInstance.DropTableAsync<Reminder>();

      if (await TableExists(ConnectionInstance, nameof(ShoppingItem)))
        await ConnectionInstance.DropTableAsync<ShoppingItem>();

      if (await TableExists(ConnectionInstance, nameof(SurgeonContacts)))
        await ConnectionInstance.DropTableAsync<SurgeonContacts>();

      if (await TableExists(ConnectionInstance, nameof(SyncResult)))
        await ConnectionInstance.DropTableAsync<SyncResult>();
    }

    /// <summary>
    /// Emtpy all tables resetting content to anonymous mode
    /// </summary>
    /// <returns></returns>
    public async Task<bool> ResetToAnonymous()
    {
      bool result = false;

      try
      {
        foreach (var prescription in await ConnectionInstance.Table<Prescription>().ToListAsync())
        {
          var adjusts = await ConnectionInstance.Table<StrutAdjustment>().Where(s => s.PrescriptionId == prescription.Id).ToListAsync();
          foreach (var sa in adjusts)
          {
            await ConnectionInstance.DeleteAsync(sa);
          }
          await ConnectionInstance.DeleteAsync(prescription);
        }

        foreach (var syncResult in await ConnectionInstance.Table<SyncResult>().ToListAsync())
        {
          await ConnectionInstance.DeleteAsync(syncResult);
        }

        foreach (var reminder in await ConnectionInstance.Table<Reminder>().ToListAsync())
        {
          await ConnectionInstance.DeleteAsync(reminder);
        }

        foreach (var item in await ConnectionInstance.Table<LogHistoryItem>().ToListAsync())
        {
          await ConnectionInstance.DeleteAsync(item);
        }

        SurgeonContacts contacts = await ConnectionInstance.Table<SurgeonContacts>().FirstOrDefaultAsync();
        if (contacts != null)
        {
          await ConnectionInstance.DeleteAsync(contacts);
        }
        result = true;
      }
      catch (Exception ex)
      {
        LastException = ex;
      }

      return result;
    }

    /// <summary>
    /// Get all saved prescriptions
    /// </summary>
    /// <returns></returns>
    public async Task<List<Prescription>> GetPrescriptions()
    {
      return await ConnectionInstance.Table<Prescription>().OrderBy(p => p.Id).ToListAsync();
    }

    /// <summary>
    /// Get a specific prescription 
    /// </summary>
    /// <param name="id">Unique prescription identifier</param>
    /// <returns></returns>
    public async Task<Prescription> GetPrescritionById(Guid id)
    {
      return await ConnectionInstance.Table<Prescription>().FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <summary>
    /// Get the list of Strut Adjustments related to a specific prescription
    /// </summary>
    /// <param name="id">Unique prescription identifier</param>
    /// <param name="todoOnly">If True, returns only items not marked as done</param>
    /// <returns></returns>
    public async Task<List<StrutAdjustment>> GetPrescriptionStrutAdjustments(Guid id, bool todoOnly)
    {
      if (todoOnly)
        return await ConnectionInstance.Table<StrutAdjustment>().Where(s => s.PrescriptionId == id && !s.Done).ToListAsync();
      else
        return await ConnectionInstance.Table<StrutAdjustment>().Where(s => s.PrescriptionId == id).ToListAsync();
    }

    /// <summary>
    /// Get a specific strut adjustment 
    /// </summary>
    /// <param name="id">Unique strut adjustment identifier</param>
    /// <returns></returns>
    public async Task<StrutAdjustment> GetStrutAdjustmentById(Guid id)
    {
      return await ConnectionInstance.Table<StrutAdjustment>().FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <summary>
    /// Get a list of strut adjustments scheduled on a specific date
    /// </summary>
    /// <param name="dt">Scheduling date</param>
    /// <returns></returns>
    public async Task<List<StrutAdjustment>> GetStrutAdjustmentsAtDate(DateTime dt)
    {
      try
      {
        //var result = ConnectionInstance.Table<StrutAdjustment>().Where(a => a.DateOfAdjustment.Year == dt.Year && a.DateOfAdjustment.Month == dt.Month && a.DateOfAdjustment.Day == dt.Day).ToListAsync();
        var result = await ConnectionInstance.Table<StrutAdjustment>().ToListAsync();
        return result.Where(s => s.DateOfAdjustment.Date == dt.Date).ToList();
      }
      catch (Exception ex)
      {
        LastException = ex;
        return null;
      }
    }

    /// <summary>
    /// Check for struts adjustment existance to a specific date
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public async Task<bool> ExistStrutAdjustmentsToDate(DateTime dt)
    {
      var struts = await ConnectionInstance.Table<StrutAdjustment>().ToListAsync();
      var exists = struts.Any(s => s.DateOfAdjustment.Date <= dt.Date);
      return exists;
    }

    /// <summary>
    ///  Delete all strut adjustments scheduled until a specific date. Optionally delete related reminders
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="deleteReminders"></param>
    /// <returns></returns>
    public async Task<bool> DeleteStrutAdjustmentsToDate(DateTime dt, bool deleteReminders = false)
    {
      bool result = false;
      try
      {
        await ConnectionInstance.RunInTransactionAsync((transactionConnection) =>
        {
          var struts = transactionConnection.Table<StrutAdjustment>().ToList().Where(s => s.DateOfAdjustment.Date <= dt.Date).ToList();
          foreach (var strutToDelete in struts)
          {
            transactionConnection.Delete(strutToDelete);
            if (deleteReminders)
            {
                // Delete related reminders
                var reminders = transactionConnection.Table<Reminder>().Where(r => r.EntityId == strutToDelete.Id).ToList();
              foreach (var reminder in reminders)
                transactionConnection.Delete<Reminder>(reminder.Id);
            }
          }
        });
        result = true;
      }
      catch (Exception ex)
      {
        LastException = ex;
      }

      return result;

    }

    /// <summary>
    /// Save a strut adjusments item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public async Task<bool> SaveStrutAdjustment(StrutAdjustment item)
    {
      if (await SaveEntityAsync(ConnectionInstance, item))
      {
        var prescription = await ConnectionInstance.Table<Prescription>().FirstOrDefaultAsync(p => p.Id == item.PrescriptionId);

        if (prescription != null)
        {
          // Add new item in log history
          var historyItem = new LogHistoryItem()
          {
            ExpectedDateTime = item.DateOfAdjustment,
            EventDateTime = _sysUtility.Now,
            ItemType = LogHistoryItem.ItemTypeEnum.StrutAdjustmentDone,
            Description = string.Format("Strut Adjustment Done on Frame ID {0}", prescription.FrameID),
            LocalEntityId = item.Id,
            ServerEntityId = item.TreatmentStepNumber
          };
          await SaveHistoryLogItem(historyItem);

          // Delete related reminders                    
          var remninders = await ConnectionInstance.Table<Reminder>().Where(r => r.EntityId == item.Id).ToListAsync();
          foreach (var reminder in remninders)
            await ConnectionInstance.DeleteAsync(reminder);

          return true;
        }
      }
      return false;
    }


    /// <summary>
    ///  Get all saved reminders
    /// </summary>
    /// <returns></returns>
    public async Task<List<Reminder>> GetReminders()
    {
      return await ConnectionInstance.Table<Reminder>().OrderBy(r => r.Id).ToListAsync();
    }

    /// <summary>
    ///  Get a specific reminder
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Reminder> GetReminderById(Guid id)
    {
      return await ConnectionInstance.Table<Reminder>().FirstOrDefaultAsync(r => r.Id == id);
    }


    /// <summary>
    /// Get a llist of reminders related to a generic entity
    /// </summary>
    /// <param name="entityId">Entity unique identifier</param>
    /// <returns></returns>
    public async Task<List<Reminder>> GetEntityReminders(Guid entityId)
    {
      return await ConnectionInstance.Table<Reminder>().Where(r => r.EntityId == entityId).ToListAsync();
    }

    /// <summary>
    /// Get a list of reminders scheduled at a specific date
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public async Task<List<Reminder>> GetRemindersAtDate(DateTime dt)
    {
      var dateWithoutTime = dt.Date;
      var tomorrow = dateWithoutTime.AddDays(1);
      return await ConnectionInstance.Table<Reminder>().Where(x => !x.IsChecked && x.DateTime >= dateWithoutTime && x.DateTime < tomorrow).OrderBy(r => r.Id).ToListAsync();
    }

    /// <summary>
    /// Get a list of reminders of a given type scheduled at a speficic date
    /// </summary>
    /// <param name="dt">Scheduling date</param>
    /// <param name="type">Remiders type</param>
    /// <returns></returns>
    public async Task<List<Reminder>> GetRemindersAtDate(DateTime dt, Reminder.ReminderType type)
    {
      var dateWithoutTime = dt.Date;
      var tomorrow = dateWithoutTime.AddDays(1);
      return await ConnectionInstance.Table<Reminder>().Where(x => !x.IsChecked && x.Type == type && x.DateTime >= dateWithoutTime && x.DateTime < tomorrow).OrderBy(r => r.Id).ToListAsync();
    }

    /// <summary>
    /// Get a list of reminders scheduled until a specific date
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public async Task<List<Reminder>> GetRemindersToDate(DateTime dt)
    {
      //var test1 = await ConnectionInstance.Table<Reminder>().Where(r => !r.IsChecked).ToListAsync();
      //var test2 = await ConnectionInstance.Table<Reminder>().Where(r => !r.IsChecked && r.DateTime <= dt).ToListAsync();

      var reminders = await ConnectionInstance.Table<Reminder>().Where(r => !r.IsChecked && r.DateTime <= dt).ToListAsync();
      return reminders;
    }

    /// <summary>
    /// Get a list of reminders of a given type scheduled until a speficic date
    /// </summary>
    /// <param name="dt">Scheduling date</param>
    /// <param name="type">Remiders type</param>
    /// <returns></returns>
    public async Task<List<Reminder>> GetRemindersToDate(DateTime dt, Reminder.ReminderType type)
    {
      return await ConnectionInstance.Table<Reminder>().Where(r => r.Type == type && !r.IsChecked && r.DateTime <= dt).ToListAsync();
    }

    /// <summary>
    /// Delete a reminder item
    /// </summary>
    /// <param name="reminder"></param>
    /// <returns></returns>
    public async Task<bool> DeleteReminder(Reminder reminder)
    {
      try
      {
        await ConnectionInstance.DeleteAsync(reminder);
        return true;

      }
      catch (Exception ex)
      {
        LastException = ex;
        return false;
      }
    }

    /// <summary>
    /// Delete a list of reminders
    /// </summary>
    /// <param name="reminders"></param>
    /// <returns></returns>
    public async Task<bool> DeleteReminders(List<Reminder> reminders)
    {
      try
      {
        await ConnectionInstance.RunInTransactionAsync(async (connection) =>
        {
          foreach (var reminder in reminders)
            await ConnectionInstance.DeleteAsync(reminder);
        });
        return true;

      }
      catch (Exception ex)
      {
        LastException = ex;
        return false;
      }
    }

    /// <summary>
    /// Get the list of items in the Shopping table
    /// </summary>
    /// <returns></returns>
    public async Task<List<ShoppingItem>> GetShoppingItems()
    {
      return await ConnectionInstance.Table<ShoppingItem>().ToListAsync();
    }

    /// <summary>
    /// Get a list of LogHistory items saved at a given date 
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public async Task<List<LogHistoryItem>> GetHistoryLogItemsAtDate(DateTime dt)
    {
      var items = await ConnectionInstance.Table<LogHistoryItem>().ToListAsync();
      return items.Where(i => i.EventDateTime.Date == dt.Date).ToList();
    }

    /// <summary>
    /// Get a list of LogHistory items saved at a given date and of a given type
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public async Task<List<LogHistoryItem>> GetHistoryLogItemsAtDate(DateTime dt, LogHistoryItem.ItemTypeEnum type)
    {
      var items = await ConnectionInstance.Table<LogHistoryItem>().Where(i => i.ItemType == type).ToListAsync();
      return items.Where(i => i.EventDateTime.Date == dt.Date).ToList();
    }

    /// <summary>
    /// Get a list of LogHistory items saved at a given date related to done activities
    /// </summary>
    /// <param name="dt"></param>        
    /// <returns></returns>
    public async Task<List<LogHistoryItem>> GetHistoryLogItemsDoneActivitesAtDate(DateTime dt)
    {
      var items = await ConnectionInstance.Table<LogHistoryItem>()
          .Where(i => i.ItemType == LogHistoryItem.ItemTypeEnum.MoodSelfAssessment ||
                      i.ItemType == LogHistoryItem.ItemTypeEnum.PinSiteCareDone ||
                      i.ItemType == LogHistoryItem.ItemTypeEnum.StrutAdjustmentDone).ToListAsync();

      return items.Where(i => i.EventDateTime.Date == dt.Date).ToList();
    }

    /// <summary>
    /// Get a list of LogHistory items not marked as synced
    /// </summary>
    /// <returns></returns>
    public async Task<List<LogHistoryItem>> GetUnsyncedHistoryLogItems()
    {
      var items = await ConnectionInstance.Table<LogHistoryItem>().Where(i => !i.Synced).ToListAsync();
      return items;
    }

    /// <summary>
    /// Save an item in the LogHistory table
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public async Task<bool> SaveHistoryLogItem(LogHistoryItem item)
    {
      return await SaveEntityAsync(item);
    }

    /// <summary>
    /// Update a list of LogHistory items marking all as synced
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public async Task<bool> MarkAsSyncedHistoryLogItems(IEnumerable<LogHistoryItem> items)
    {
      bool result = true;

      
      try
      {
        await ConnectionInstance.RunInTransactionAsync((transactionConnection) =>
        {
          foreach (var item in items)
          {
            item.Synced = true;
            result = SaveEntity<LogHistoryItem>(transactionConnection, item);

            if (!result)
              break;
          }
        });

      }
      catch (Exception ex)
      {
        LastException = ex;
        AppLoggerHelper.LogException(LastException, "Error on MarkAsSyncedHistoryLogItems operation", TraceLevel.Error);
      }
      return result;
    }

    /// <summary>
    /// Save an item in the reminder table
    /// </summary>
    /// <param name="reminder"></param>
    /// <returns></returns>
    public async Task<bool> SaveReminder(Reminder reminder)
    {
      return await SaveEntityAsync(reminder);
    }

    /// <summary>
    /// Save a shopping list item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public async Task<bool> SaveShoppingItem(ShoppingItem item)
    {
      return await SaveEntityAsync(item);
    }

    /// <summary>
    /// Save surgeon contacts informations
    /// </summary>
    /// <param name="contacts"></param>
    /// <returns></returns>
    public async Task<bool> SaveSurgeonContacts(SurgeonContacts contacts)
    {
      return await SaveEntityAsync(contacts);
    }

    /// <summary>
    /// get the saved surgeon contacts informations
    /// </summary>
    /// <returns></returns>
    public async Task<SurgeonContacts> GetSurgeonContacts()
    {
      var contacts = await ConnectionInstance.Table<SurgeonContacts>().FirstOrDefaultAsync();
      return contacts;
    }

    /// <summary>
    /// Get the data about last prescription sync operation
    /// </summary>
    /// <returns></returns>
    public async Task<List<SyncResult>> GetSyncResults()
    {
      return await ConnectionInstance.Table<SyncResult>().OrderBy(p => p.Id).ToListAsync();
    }

    /// <summary>
    /// Clear the data about last prescription sync operation
    /// </summary>
    /// <returns></returns>
    public async Task<bool> ClearSyncResults()
    {
      var items = await ConnectionInstance.Table<SyncResult>().OrderBy(p => p.Id).ToListAsync();
      try
      {
        foreach (var item in items)
          await ConnectionInstance.DeleteAsync(item);

        return true;
      }
      catch (Exception ex)
      {
        LastException = ex;
        return false;
      }
    }

    /// <summary>
    /// Return true if detect changes in last prescription sync operation
    /// </summary>
    /// <returns></returns>
    public async Task<bool> LastSyncHasChanges()
    {
      return await ConnectionInstance.Table<SyncResult>().FirstOrDefaultAsync(r => r.State != SyncResultStateEnum.Unchanged) != null;
    }

    /// <summary>
    /// Delete from reminders table all items of a given type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public async Task<bool> DeleteAllReminderType(Reminder.ReminderType type)
    {
      try
      {
        var reminders = await ConnectionInstance.Table<Reminder>().Where(r => r.Type == type).ToListAsync();

        foreach (var curReminder in reminders)
        {
          await ConnectionInstance.DeleteAsync(curReminder);
        }

        return true;
      }
      catch (Exception ex)
      {
        LastException = ex;
        return false;
      }
    }

    /// <summary>
    /// Save downloaded prescriptions to local database
    /// </summary>
    /// <param name="update">A package containing List of prescriptions DTO items and changes counter</param>
    /// <returns>True if operation is succesfully completed</returns>
    public async Task<bool> SaveDownloadedPrescriptionsUpdate(MyHexPlanProxies.Models.PrescriptionUpdate update)
    {
      try
      {
        await ConnectionInstance.RunInTransactionAsync((transactionConnection) =>
        {
            // Delete old prescriptions 
            var prescriptions = transactionConnection.Table<Prescription>().OrderBy(p => p.Id).ToList();
          foreach (var oldPrescription in prescriptions)
          {
            var change = update.Changes.FirstOrDefault(c => c.FrameID == oldPrescription.FrameID);
            if (change == null || !change.State.HasValue || change.State.Value != (int)MyHexPlanProxies.Models.PrescriptionState.Unchanged)
            {
                // Delete reminders related to the prescription
                var reminders = transactionConnection.Table<Reminder>().Where(r => r.PrescriptionId == oldPrescription.Id).ToList();
              foreach (var reminder in reminders)
                transactionConnection.Delete(reminder);

                // Delete the prescription and the struts adjustments
                DeletePrescriptionCascade(transactionConnection, oldPrescription.Id);
            }
          }
        });
      }
      catch (Exception ex)
      {
        LastException = ex;
        return false;
      }

      try
      {
        await ConnectionInstance.RunInTransactionAsync((transactionConnection) =>
        {
          if (update.Prescriptions != null)
          {
            foreach (var prescription in update.Prescriptions)
            {
              if (!prescription.Prescription.CaseData.CaseGuid.HasValue)
                continue;

              var change = update.Changes.FirstOrDefault(c =>
                                                         c.FrameID == prescription.Prescription.CaseData.FrameID &&
                                                         (c.State.Value == (int)MyHexPlanProxies.Models.PrescriptionState.Added || c.State.Value == (int)MyHexPlanProxies.Models.PrescriptionState.Updated)
                                                         );
              if (change != null)
              {
                  // Create new prescription
                  var localPrescription = new Prescription
                {
                  CaseId = prescription.Prescription.CaseData.CaseGuid.Value,
                  CaseName = prescription.Prescription.CaseData.Name,
                  CaseNumber = prescription.Prescription.CaseData.Number,
                  FrameID = prescription.Prescription.CaseData.FrameID,
                  AnatomiesType = prescription.Prescription.CaseData.AnatomiesType,
                  BoneTypeSegment = prescription.Prescription.CaseData.BoneTypeSegment,
                  Notes = prescription.Prescription.PrescriptionData.PrescriptionNotes,
                  SurgeryDate = prescription.Prescription.ScheduleInfo.SurgeryDate,
                  StartOfTreatment = prescription.Prescription.ScheduleInfo.DateOfTreatmentStart,
                  EndOfTreatment = prescription.Prescription.ScheduleInfo.FinalTreatmentDate,
                  RemovalDate = prescription.RemovalDate,
                  LastSyncDate = change.Datetime?.ToLocalTime()
                };

                  // Map server entities to local entities
                  var localStrutAdjustments = new List<StrutAdjustment>();

                foreach (var part in prescription.Prescription.PrescriptionData.Parts.Where(p => p.DateOfAdjustment.HasValue))
                {
                  var localStrutAdjustment = new StrutAdjustment()
                  {
                    PrescriptionId = localPrescription.Id,
                    DateOfAdjustment = part.DateOfAdjustment.Value,
                    TreatmentStepNumber = part.TreatmentStepNumber ?? -1
                  };
                  foreach (var strut in part.Struts)
                  {
                    switch (strut.StrutNumber)
                    {
                      case 1:
                        localStrutAdjustment.Click1 = strut.Click;
                        localStrutAdjustment.Length1 = strut.Gradual;
                        break;
                      case 2:
                        localStrutAdjustment.Click2 = strut.Click;
                        localStrutAdjustment.Length2 = strut.Gradual;
                        break;
                      case 3:
                        localStrutAdjustment.Click3 = strut.Click;
                        localStrutAdjustment.Length3 = strut.Gradual;
                        break;
                      case 4:
                        localStrutAdjustment.Click4 = strut.Click;
                        localStrutAdjustment.Length4 = strut.Gradual;
                        break;
                      case 5:
                        localStrutAdjustment.Click5 = strut.Click;
                        localStrutAdjustment.Length5 = strut.Gradual;
                        break;
                      case 6:
                        localStrutAdjustment.Click6 = strut.Click;
                        localStrutAdjustment.Length6 = strut.Gradual;
                        break;
                    }
                  }

                  localStrutAdjustments.Add(localStrutAdjustment);
                }

                if (SavePrescription(transactionConnection, localPrescription, localStrutAdjustments))
                {
                    // Create new reminders
                    foreach (var localStrutAdjustment in localStrutAdjustments)
                  {
                    var reminder = new Reminder()
                    {
                      DateTime = localStrutAdjustment.DateOfAdjustment,
                      EntityId = localStrutAdjustment.Id,
                      PrescriptionId = localPrescription.Id,
                      Type = Reminder.ReminderType.StrutAdjustmentReminder,
                      IsChecked = false,
                    };
                    if (!SaveEntity(transactionConnection, reminder))
                    {
                      throw LastException ?? new Exception("Error saving reminder");
                    }
                  }
                }
                else
                {
                  throw LastException ?? new Exception("Error saving prescription");
                }
              }
            }
          }

        });

      }
      catch (Exception ex)
      {
        LastException = ex;
        return false;
      }

      try
      {
        await ConnectionInstance.RunInTransactionAsync((transactionConnection) =>
        {
            // Delete old sync results
            var syncResults = transactionConnection.Table<SyncResult>().OrderBy(p => p.Id).ToList();
          foreach (var syncResultToDelete in syncResults)
            transactionConnection.Delete(syncResultToDelete);
        });

      }
      catch (Exception ex)
      {
        LastException = ex;
        return false;
      }

      foreach (var change in update.Changes)
      {
        // Map proxy entity to local sql entity
        var syncResult = new SyncResult()
        {
          CaseId = change.CaseId,
          DateTime = change.Datetime?.ToLocalTime(),
          FrameId = change.FrameID,
        };
        switch ((MyHexPlanProxies.Models.PrescriptionState)change.State)
        {
          case MyHexPlanProxies.Models.PrescriptionState.Unchanged:
            syncResult.State = SyncResultStateEnum.Unchanged;
            break;
          case MyHexPlanProxies.Models.PrescriptionState.Added:
            syncResult.State = SyncResultStateEnum.Added;
            break;
          case MyHexPlanProxies.Models.PrescriptionState.Updated:
            syncResult.State = SyncResultStateEnum.Updated;
            break;
          case MyHexPlanProxies.Models.PrescriptionState.Revoked:
            syncResult.State = SyncResultStateEnum.Revoked;
            break;
        }
        // Save new sync results
        await SaveEntityAsync(ConnectionInstance, syncResult);
      }

      return true;

    }

    /// <summary>
    /// Updates the prescriptions RemovalDate according to a list of treatments dates
    /// </summary>
    /// <param name="treatmentDates"></param>
    /// <returns></returns>
    public async Task<bool> UpdatePrescriptionsTreatmentDates(IList<MyHexPlanProxies.Models.FinalTreatmentCaseDateDTO> treatmentDates)
    {
      try
      {
        // Delete old sync results
        foreach (var treatmentDate in treatmentDates)
        {
          if (treatmentDate.CaseUid.HasValue && treatmentDate.FinalTreatmentDate.HasValue)
          {
            var prescription = await ConnectionInstance.Table<Prescription>().FirstOrDefaultAsync(p => p.CaseId == treatmentDate.CaseUid.Value);
            if (prescription != null)
            {
              // Update only if needed
              if (!prescription.RemovalDate.HasValue || prescription.RemovalDate.Value.Date != treatmentDate.FinalTreatmentDate.Value.Date)
              {
                prescription.RemovalDate = treatmentDate.FinalTreatmentDate.Value;
                await SaveEntityAsync(ConnectionInstance, prescription);
              }
            }
          }
        }
        return true;
      }
      catch (Exception ex)
      {
        LastException = ex;
        return false;
      }

    }

    /// <summary>
    /// Regenerate the list of pin site care daily items
    /// </summary>
    public async Task RebuildPinSiteCareCalendar(bool isLoggedIn, bool pinSiteCareEnabled, TimeSpan pinSiteCareTime, DateTime? pinSiteCareStartDate, bool[] pinSiteCareDaysOfWeekNotification)
    {
      //Delete and refresh PinSiteCare Reminder.
      var reminders = await ConnectionInstance.Table<Reminder>().Where(r => r.Type == Reminder.ReminderType.PinSiteCareReminder).ToListAsync();
      foreach (var curReminder in reminders)
      {
        await ConnectionInstance.DeleteAsync(curReminder);
      }

      //Prepare structure to cycle on a interval date range.
      IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
      {
        for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
          yield return day;
      }

      if (isLoggedIn)//Get pinsitecare information setup by surgeon on the portal.
      {
        if (pinSiteCareEnabled)
        {
          var prescriptions = await ConnectionInstance.Table<Prescription>().ToListAsync();
          if (prescriptions.Any())
          {
            DateTime fromDate;
            if (pinSiteCareStartDate.HasValue)
            {
              fromDate = pinSiteCareStartDate.Value;
              if (fromDate < _sysUtility.Now.Date)
                fromDate = _sysUtility.Now.Date;
            }
            else
              fromDate = _sysUtility.Now.Date;

            var toDate = prescriptions.Where(p => p.RemovalDate.HasValue).OrderByDescending(p => p.RemovalDate.Value).First().RemovalDate.Value.AddDays(-1);
            foreach (var day in EachDay(fromDate, toDate))
            {
              // if surgeon has enabled pinsitecare for this day of week                                                       
              if (pinSiteCareDaysOfWeekNotification == null
                 || pinSiteCareDaysOfWeekNotification.Length < (int)day.DayOfWeek
                 || pinSiteCareDaysOfWeekNotification[(int)day.DayOfWeek])
              {
                // If not already done and scheduled time is higher than now
                var items = await ConnectionInstance.Table<LogHistoryItem>().Where(i => i.ItemType == LogHistoryItem.ItemTypeEnum.PinSiteCareDone).ToListAsync();
                var pinSiteCareDone = items.Where(i => i.EventDateTime.Date == day.Date).FirstOrDefault();
                var pinSiteCareDt = day + pinSiteCareTime;
                if (pinSiteCareDone == null && pinSiteCareDt > _sysUtility.Now)
                {
                  var reminder = new Reminder()
                  {
                    DateTime = pinSiteCareDt,
                    Type = Reminder.ReminderType.PinSiteCareReminder,
                    IsChecked = false,
                  };
                  await SaveEntityAsync<Reminder>(ConnectionInstance, reminder);
                }
              }
            }
          }
        }

      }
      else//UserAnonymous 60 days default.
      {
        foreach (var day in EachDay(_sysUtility.Now, _sysUtility.Now.AddDays(60)))
        {
          // If not already done and scheduled time is higher than now
          var items = await ConnectionInstance.Table<LogHistoryItem>().Where(i => i.ItemType == LogHistoryItem.ItemTypeEnum.PinSiteCareDone).ToListAsync();
          var pinSiteCareDone = items.Where(i => i.EventDateTime.Date == day.Date).FirstOrDefault();
          var pinSiteCareDt = day + pinSiteCareTime;
          if (pinSiteCareDone == null && pinSiteCareDt > _sysUtility.Now)
          {
            var reminder = new Reminder()
            {
              DateTime = pinSiteCareDt,
              Type = Reminder.ReminderType.PinSiteCareReminder,
              IsChecked = false,
            };
            await SaveEntityAsync<Reminder>(ConnectionInstance, reminder);
          }
        }
      }

    }

    /// <summary>
    /// Check if a table exists 
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    private async Task<bool> TableExists(SQLiteAsyncConnection database, string tableName)
    {
      int colCount;
      try
      {
        colCount = (await database.GetTableInfoAsync(tableName)).Count;
      }
      catch (Exception ex)
      {
        return false;
      }
      return colCount > 0;
    }

    /// <summary>
    /// Save a prescription and its related struts adjustments
    /// </summary>
    /// <param name="prescription"></param>
    /// <param name="strutAdjustments"></param>
    /// <returns></returns>
    private bool SavePrescription(SQLiteConnection connection, Prescription prescription, IEnumerable<StrutAdjustment> strutAdjustments)
    {
      var result = SaveEntity(connection, prescription);
      var adjResult = false;
      if (result)
      {
        foreach (var adj in strutAdjustments)
        {
          adj.PrescriptionId = prescription.Id;
          adj.Done = false;
          adjResult = SaveEntity(connection, adj);
          if (!adjResult)
            break;
        }
      }
      return result && adjResult;
    }


    /// <summary>
    /// Delete a prescription and optionally its related data
    /// </summary>
    /// <param name="id">Unique prescription identifier</param>
    /// <returns></returns>
    private void DeletePrescriptionCascade(SQLiteConnection connection, Guid id)
    {
      var adjusts = connection.Table<StrutAdjustment>().Where(s => s.PrescriptionId == id).ToList();
      foreach (var sa in adjusts)
      {
        connection.Delete(sa);
      }
      var prescription = connection.Table<Prescription>().FirstOrDefault(p => p.Id == id);
      connection.Delete(prescription);
    }

    /// <summary>
    /// This method will throw exception if the update will be massive (query contains more than 1000 characters)
    /// </summary>
    private async Task<bool> SaveEntityList<T>(IEnumerable<T> entityList) where T : BaseSqlEntity
    {
      var toInsert = entityList.Where(x => x.Id == Guid.Empty).ToList();
      var toUpdate = entityList.Where(x => x.Id != Guid.Empty).ToList();
      foreach (var item in toInsert)
      {
        item.Id = Guid.NewGuid();
      }
      try
      {
        await ConnectionInstance.UpdateAllAsync(toUpdate, false);
        await ConnectionInstance.InsertAllAsync(toInsert, false);
        LastException = null;
        return true;
      }
      catch (Exception ex)
      {
        LastException = ex;
        return false;
      }
    }

    private bool SaveEntity<T>(T entity) where T : BaseSqlEntity
    {
      return SaveEntity<T>(entity);
    }

    private bool SaveEntity<T>(SQLiteConnection database, T entity) where T : BaseSqlEntity
    {
      if (entity.Id != Guid.Empty)
      {
        try
        {
          database.Update(entity);
          LastException = null;
          return true;
        }
        catch (Exception ex)
        {
          LastException = ex;
          return false;
        }

      }
      else
      {
        entity.Id = Guid.NewGuid();
        try
        {
          var res = database.Insert(entity);
          LastException = null;
          return res > 0;
        }
        catch (Exception ex)
        {
          LastException = ex;
          return false;
        }
      }
    }

    private async Task<bool> SaveEntityAsync<T>(T entity) where T : BaseSqlEntity
    {
      return await SaveEntityAsync<T>(ConnectionInstance, entity);
    }

    private async Task<bool> SaveEntityAsync<T>(SQLiteAsyncConnection database, T entity) where T : BaseSqlEntity
    {
      if (entity.Id != Guid.Empty)
      {
        try
        {
          await database.UpdateAsync(entity);
          LastException = null;
          return true;
        }
        catch (Exception ex)
        {
          LastException = ex;
          return false;
        }

      }
      else
      {
        entity.Id = Guid.NewGuid();
        try
        {
          var res = await ConnectionInstance.InsertAsync(entity);
          LastException = null;
          return res > 0;
        }
        catch (Exception ex)
        {
          LastException = ex;
          return false;
        }
      }
    }

  }
}