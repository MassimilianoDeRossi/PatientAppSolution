using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private ISystemUtility _sysUtility = null;

        private SQLiteConnection GetConnection()
        {
            var connection = DependencyService.Get<ISQLite>().GetConnection();
            connection.BusyTimeout = new TimeSpan(0, 0, 5);
            return connection;
        }

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
        }

        /// <summary>
        /// Initialize database content: create all not existing tables 
        /// </summary>
        /// <param name="forced"></param>
        /// <returns></returns>
        public bool CreateTablesIfNotExists(bool forced = false)
        {
            using (var database = GetConnection())
            {
                if (forced || !TableExists(database, nameof(Prescription)))
                    database.CreateTable<Prescription>();

                if (forced || !TableExists(database, nameof(StrutAdjustment)))
                    database.CreateTable<StrutAdjustment>();

                if (forced || !TableExists(database, nameof(LogHistoryItem)))
                    database.CreateTable<LogHistoryItem>();

                if (forced || !TableExists(database, nameof(Reminder)))
                    database.CreateTable<Reminder>();

                if (forced || !TableExists(database, nameof(ShoppingItem)) || !AppSettings.Instance.DatabaseVersion.HasValue || AppSettings.Instance.DatabaseVersion.Value < 2)
                {
                    bool restoreChecked = false;
                    ShoppingItem[] savedCheckedItems = null;

                    if (TableExists(database, nameof(ShoppingItem)))
                    {
                        // Save old checked items
                        savedCheckedItems = database.Table<ShoppingItem>().ToArray();
                        database.DropTable<ShoppingItem>();
                        restoreChecked = true;
                    }

                    database.CreateTable<ShoppingItem>();
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

                        SaveEntity(database, shoppingList[i]);
                    }
                }

                if (forced || !TableExists(database, nameof(SurgeonContacts)))
                    database.CreateTable<SurgeonContacts>();

                if (forced || !TableExists(database, nameof(SyncResult)))
                    database.CreateTable<SyncResult>();


                AppSettings.SetDatabaseVersion(2);

                return true;
            }
        }

        /// <summary>
        /// Check for database corruption
        /// </summary>
        /// <returns></returns>
        public bool CheckIntegrity()
        {
            bool success = true;
            try
            {
                GetPrescriptions();
                GetReminders();
                GetShoppingItems();
                GetSurgeonContacts();
                GetUnsyncedHistoryLogItems();

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
        public void DeleteAllTables()
        {
            using (var database = GetConnection())
            {
                if (TableExists(database, nameof(Prescription)))
                    database.DropTable<Prescription>();

                if (TableExists(database, nameof(StrutAdjustment)))
                    database.DropTable<StrutAdjustment>();

                if (TableExists(database, nameof(LogHistoryItem)))
                    database.DropTable<LogHistoryItem>();

                if (TableExists(database, nameof(Reminder)))
                    database.DropTable<Reminder>();

                if (TableExists(database, nameof(ShoppingItem)))
                    database.DropTable<ShoppingItem>();

                if (TableExists(database, nameof(SurgeonContacts)))
                    database.DropTable<SurgeonContacts>();

                if (TableExists(database, nameof(SyncResult)))
                    database.DropTable<SyncResult>();
            }
        }

        /// <summary>
        /// Emtpy all tables resetting content to anonymous mode
        /// </summary>
        /// <returns></returns>
        public bool ResetToAnonymous()
        {
            using (var database = GetConnection())
            {
                bool result = false;

                try
                {
                    foreach (var prescription in database.Table<Prescription>())
                    {
                        var adjusts = database.Table<StrutAdjustment>().Where(s => s.PrescriptionId == prescription.Id).ToList();
                        foreach (var sa in adjusts.ToList())
                        {
                            database.Delete(sa);
                        }
                        database.Delete(prescription);
                    }

                    foreach (var syncResult in database.Table<SyncResult>())
                    {
                        database.Delete(syncResult);
                    }

                    foreach (var reminder in database.Table<Reminder>())
                    {
                        database.Delete(reminder);
                    }

                    foreach (var item in database.Table<LogHistoryItem>())
                    {
                        database.Delete(item);
                    }

                    if (database.Table<SurgeonContacts>().Any())
                    {
                        var contacts = database.Table<SurgeonContacts>().First();
                        database.Delete(contacts);
                    }
                    result = true;
                }
                catch (Exception ex)
                {
                    LastException = ex;
                }

                return result;
            }
        }

        /// <summary>
        /// Get all saved prescriptions
        /// </summary>
        /// <returns></returns>
        public List<Prescription> GetPrescriptions()
        {
            using (var database = GetConnection())
            {
                return database.Table<Prescription>().OrderBy(p => p.Id).ToList();
            }
        }

        /// <summary>
        /// Get a specific prescription 
        /// </summary>
        /// <param name="id">Unique prescription identifier</param>
        /// <returns></returns>
        public Prescription GetPrescritionById(Guid id)
        {
            using (var database = GetConnection())
            {
                return database.Table<Prescription>().FirstOrDefault(p => p.Id == id);
            }
        }

        /// <summary>
        /// Get the list of Strut Adjustments related to a specific prescription
        /// </summary>
        /// <param name="id">Unique prescription identifier</param>
        /// <param name="todoOnly">If True, returns only items not marked as done</param>
        /// <returns></returns>
        public List<StrutAdjustment> GetPrescriptionStrutAdjustments(Guid id, bool todoOnly)
        {
            using (var database = GetConnection())
            {
                if (todoOnly)
                    return database.Table<StrutAdjustment>().Where(s => s.PrescriptionId == id && !s.Done).ToList();
                else
                    return database.Table<StrutAdjustment>().Where(s => s.PrescriptionId == id).ToList();
            }
        }

        /// <summary>
        /// Get a specific strut adjustment 
        /// </summary>
        /// <param name="id">Unique strut adjustment identifier</param>
        /// <returns></returns>
        public StrutAdjustment GetStrutAdjustmentById(Guid id)
        {
            using (var database = GetConnection())
            {
                return database.Table<StrutAdjustment>().FirstOrDefault(p => p.Id == id);
            }
        }

        /// <summary>
        /// Get a list of strut adjustments scheduled on a specific date
        /// </summary>
        /// <param name="dt">Scheduling date</param>
        /// <returns></returns>
        public List<StrutAdjustment> GetStrutAdjustmentsAtDate(DateTime dt)
        {
            using (var database = GetConnection())
            {
                try
                {
                    //var result = database.Table<StrutAdjustment>().Where(a => a.DateOfAdjustment.Year == dt.Year && a.DateOfAdjustment.Month == dt.Month && a.DateOfAdjustment.Day == dt.Day).ToList();
                    var result = database.Table<StrutAdjustment>().ToList();
                    return result.Where(s => s.DateOfAdjustment.Date == dt.Date).ToList();
                }
                catch (Exception ex)
                {
                    LastException = ex;
                    return null;
                }
            }
        }

        /// <summary>
        /// Check for struts adjustment existance to a specific date
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool ExistStrutAdjustmentsToDate(DateTime dt)
        {
            using (var database = GetConnection())
            {
                return database.Table<StrutAdjustment>().Any(s => s.DateOfAdjustment.Date <= dt.Date);
            }
        }

        /// <summary>
        ///  Delete all strut adjustments scheduled until a specific date. Optionally delete related reminders
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="deleteReminders"></param>
        /// <returns></returns>
        public bool DeleteStrutAdjustmentsToDate(DateTime dt, bool deleteReminders = false)
        {
            using (var database = GetConnection())
            {
                bool result = false;
                try
                {
                    database.BeginTransaction();
                    var struts = database.Table<StrutAdjustment>().ToList().Where(s => s.DateOfAdjustment.Date <= dt.Date).ToList();
                    foreach (var strutToDelete in struts)
                    {
                        database.Delete(strutToDelete);
                        if (deleteReminders)
                        {
                            // Delete related reminders
                            var reminders = database.Table<Reminder>().Where(r => r.EntityId == strutToDelete.Id).ToList();
                            foreach (var reminder in reminders)
                                database.Delete<Reminder>(reminder.Id);
                        }
                    }
                    database.Commit();
                    result = true;
                }
                catch (Exception ex)
                {
                    database.Rollback();
                    LastException = ex;
                }

                return result;
            }
        }

        /// <summary>
        /// Save a strut adjusments item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool SaveStrutAdjustment(StrutAdjustment item)
        {
            using (var database = GetConnection())
            {
                if (SaveEntity(database, item))
                {
                    var prescription = database.Table<Prescription>().FirstOrDefault(p => p.Id == item.PrescriptionId);

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
                    SaveHistoryLogItem(historyItem);

                    // Delete related reminders                    
                    var remninders = database.Table<Reminder>().Where(r => r.EntityId == item.Id).ToList();
                    foreach (var reminder in remninders)
                        database.Delete(reminder);

                    return true;
                }
            }
            return false;
        }


        /// <summary>
        ///  Get all saved reminders
        /// </summary>
        /// <returns></returns>
        public List<Reminder> GetReminders()
        {
            using (var database = GetConnection())
            {
                return database.Table<Reminder>().OrderBy(r => r.Id).ToList();
            }
        }

        /// <summary>
        ///  Get a specific reminder
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Reminder GetReminderById(Guid id)
        {
            using (var database = GetConnection())
            {
                return database.Table<Reminder>().FirstOrDefault(r => r.Id == id);
            }
        }


        /// <summary>
        /// Get a llist of reminders related to a generic entity
        /// </summary>
        /// <param name="entityId">Entity unique identifier</param>
        /// <returns></returns>
        public List<Reminder> GetEntityReminders(Guid entityId)
        {
            using (var database = GetConnection())
            {
                return database.Table<Reminder>().Where(r => r.EntityId == entityId).ToList();
            }
        }

        /// <summary>
        /// Get a list of reminders scheduled at a specific date
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<Reminder> GetRemindersAtDate(DateTime dt)
        {
            using (var database = GetConnection())
            {
                var dateWithoutTime = dt.Date;
                var tomorrow = dateWithoutTime.AddDays(1);
                return database.Table<Reminder>().Where(x => !x.IsChecked && x.DateTime >= dateWithoutTime && x.DateTime < tomorrow).OrderBy(r => r.Id).ToList();
            }
        }

        /// <summary>
        /// Get a list of reminders of a given type scheduled at a speficic date
        /// </summary>
        /// <param name="dt">Scheduling date</param>
        /// <param name="type">Remiders type</param>
        /// <returns></returns>
        public List<Reminder> GetRemindersAtDate(DateTime dt, Reminder.ReminderType type)
        {
            using (var database = GetConnection())
            {
                var dateWithoutTime = dt.Date;
                var tomorrow = dateWithoutTime.AddDays(1);
                return database.Table<Reminder>().Where(x => !x.IsChecked && x.Type == type && x.DateTime >= dateWithoutTime && x.DateTime < tomorrow).OrderBy(r => r.Id).ToList();
            }
        }

        /// <summary>
        /// Get a list of reminders scheduled until a specific date
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<Reminder> GetRemindersToDate(DateTime dt)
        {
            using (var database = GetConnection())
            {
                return database.Table<Reminder>().Where(r => !r.IsChecked && r.DateTime <= dt).ToList();
            }
        }

        /// <summary>
        /// Get a list of reminders of a given type scheduled until a speficic date
        /// </summary>
        /// <param name="dt">Scheduling date</param>
        /// <param name="type">Remiders type</param>
        /// <returns></returns>
        public List<Reminder> GetRemindersToDate(DateTime dt, Reminder.ReminderType type)
        {
            using (var database = GetConnection())
            {
                return database.Table<Reminder>().Where(r => r.Type == type && !r.IsChecked && r.DateTime <= dt).ToList();
            }
        }

        /// <summary>
        /// Delete a reminder item
        /// </summary>
        /// <param name="reminder"></param>
        /// <returns></returns>
        public bool DeleteReminder(Reminder reminder)
        {
            using (var database = GetConnection())
            {
                try
                {
                    database.Delete(reminder);
                    return true;

                }
                catch (Exception ex)
                {
                    LastException = ex;
                    return false;
                }
            }
        }

        /// <summary>
        /// Delete a list of reminders
        /// </summary>
        /// <param name="reminders"></param>
        /// <returns></returns>
        public bool DeleteReminders(List<Reminder> reminders)
        {
            using (var database = GetConnection())
            {
                try
                {
                    database.BeginTransaction();
                    foreach (var reminder in reminders)
                        database.Delete(reminder);
                    database.Commit();
                    return true;

                }
                catch (Exception ex)
                {
                    database.Rollback();
                    LastException = ex;
                    return false;
                }
            }
        }

        /// <summary>
        /// Get the list of items in the Shopping table
        /// </summary>
        /// <returns></returns>
        public List<ShoppingItem> GetShoppingItems()
        {
            using (var database = GetConnection())
            {
                return database.Table<ShoppingItem>().ToList();
            }
        }

        /// <summary>
        /// Get a list of LogHistory items saved at a given date 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<LogHistoryItem> GetHistoryLogItemsAtDate(DateTime dt)
        {
            using (var database = GetConnection())
            {
                var items = database.Table<LogHistoryItem>().ToList();
                return items.Where(i => i.EventDateTime.Date == dt.Date).ToList();
            }
        }

        /// <summary>
        /// Get a list of LogHistory items saved at a given date and of a given type
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<LogHistoryItem> GetHistoryLogItemsAtDate(DateTime dt, LogHistoryItem.ItemTypeEnum type)
        {
            using (var database = GetConnection())
            {
                var items = database.Table<LogHistoryItem>().Where(i => i.ItemType == type).ToList();
                return items.Where(i => i.EventDateTime.Date == dt.Date).ToList();
            }
        }

        /// <summary>
        /// Get a list of LogHistory items saved at a given date related to done activities
        /// </summary>
        /// <param name="dt"></param>        
        /// <returns></returns>
        public List<LogHistoryItem> GetHistoryLogItemsDoneActivitesAtDate(DateTime dt)
        {
            using (var database = GetConnection())
            {
                var items = database.Table<LogHistoryItem>()
                    .Where(i => i.ItemType == LogHistoryItem.ItemTypeEnum.MoodSelfAssessment ||
                                i.ItemType == LogHistoryItem.ItemTypeEnum.PinSiteCareDone ||
                                i.ItemType == LogHistoryItem.ItemTypeEnum.StrutAdjustmentDone).ToList();

                return items.Where(i => i.EventDateTime.Date == dt.Date).ToList();
            }
        }

        /// <summary>
        /// Get a list of LogHistory items not marked as synced
        /// </summary>
        /// <returns></returns>
        public List<LogHistoryItem> GetUnsyncedHistoryLogItems()
        {
            using (var database = GetConnection())
            {
                var items = database.Table<LogHistoryItem>().Where(i => !i.Synced).ToList();
                return items;
            }
        }

        /// <summary>
        /// Save an item in the LogHistory table
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool SaveHistoryLogItem(LogHistoryItem item)
        {
            return SaveEntity(item);
        }

        /// <summary>
        /// Update a list of LogHistory items marking all as synced
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool MarkAsSyncedHistoryLogItems(IEnumerable<LogHistoryItem> items)
        {
            using (var database = GetConnection())
            {
                bool result = true;

                try
                {
                    database.BeginTransaction();
                    foreach (var item in items)
                    {
                        item.Synced = true;
                        result = SaveEntity<LogHistoryItem>(database, item);

                        if (!result)
                            break;
                    }
                    database.Commit();
                }
                catch (Exception ex)
                {
                    database.Rollback();
                    LastException = ex;
                    AppLoggerHelper.LogException(LastException, "Error on MarkAsSyncedHistoryLogItems operation", Newtonsoft.Json.TraceLevel.Error);
                }
                return result;
            }
        }

        /// <summary>
        /// Save an item in the reminder table
        /// </summary>
        /// <param name="reminder"></param>
        /// <returns></returns>
        public bool SaveReminder(Reminder reminder)
        {
            return SaveEntity(reminder);
        }

        /// <summary>
        /// Save a shopping list item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool SaveShoppingItem(ShoppingItem item)
        {
            return SaveEntity(item);
        }

        /// <summary>
        /// Save surgeon contacts informations
        /// </summary>
        /// <param name="contacts"></param>
        /// <returns></returns>
        public bool SaveSurgeonContacts(SurgeonContacts contacts)
        {
            return SaveEntity(contacts);
        }

        /// <summary>
        /// get the saved surgeon contacts informations
        /// </summary>
        /// <returns></returns>
        public SurgeonContacts GetSurgeonContacts()
        {
            using (var database = GetConnection())
            {
                var contacts = database.Table<SurgeonContacts>().FirstOrDefault();
                return contacts;
            }
        }

        /// <summary>
        /// Get the data about last prescription sync operation
        /// </summary>
        /// <returns></returns>
        public List<SyncResult> GetSyncResults()
        {
            using (var database = GetConnection())
            {
                return database.Table<SyncResult>().OrderBy(p => p.Id).ToList();
            }
        }

        /// <summary>
        /// Clear the data about last prescription sync operation
        /// </summary>
        /// <returns></returns>
        public bool ClearSyncResults()
        {
            using (var database = GetConnection())
            {

                var items = database.Table<SyncResult>().OrderBy(p => p.Id);
                try
                {
                    foreach (var item in items)
                        database.Delete(item);

                    return true;
                }
                catch (Exception ex)
                {
                    LastException = ex;
                    return false;
                }
            }
        }

        /// <summary>
        /// Return true if detect changes in last prescription sync operation
        /// </summary>
        /// <returns></returns>
        public bool LastSyncHasChanges()
        {
            using (var database = GetConnection())
            {
                return database.Table<SyncResult>().Any(r => r.State != SyncResultStateEnum.Unchanged);
            }
        }

        /// <summary>
        /// Delete from reminders table all items of a given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool DeleteAllReminderType(Reminder.ReminderType type)
        {
            using (var database = GetConnection())
            {
                try
                {
                    var reminders = database.Table<Reminder>().Where(r => r.Type == type);

                    foreach (var curReminder in reminders)
                    {
                        database.Delete(curReminder);
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    LastException = ex;
                    return false;
                }
            }
        }

        /// <summary>
        /// Save downloaded prescriptions to local database
        /// </summary>
        /// <param name="update">A package containing List of prescriptions DTO items and changes counter</param>
        /// <returns>True if operation is succesfully completed</returns>
        public bool SaveDownloadedPrescriptionsUpdate(MyHexPlanProxies.Models.PrescriptionUpdate update)
        {
            using (var database = GetConnection())
            {
                try
                {
                    database.BeginTransaction();
                    // Delete old prescriptions 
                    var prescriptions = database.Table<Prescription>().OrderBy(p => p.Id).ToList();
                    foreach (var oldPrescription in prescriptions)
                    {
                        var change = update.Changes.FirstOrDefault(c => c.FrameID == oldPrescription.FrameID);
                        if (change == null || !change.State.HasValue || change.State.Value != (int)MyHexPlanProxies.Models.PrescriptionState.Unchanged)
                        {
                            // Delete reminders related to the prescription
                            var reminders = database.Table<Reminder>().Where(r => r.PrescriptionId == oldPrescription.Id).ToList();
                            foreach (var reminder in reminders)
                                database.Delete(reminder);

                            // Delete the prescription and the struts adjustments
                            DeletePrescriptionCascade(database, oldPrescription.Id);
                        }
                    }
                    database.Commit();
                }
                catch (Exception ex)
                {
                    database.Rollback();
                    LastException = ex;
                    return false;
                }

                try
                {
                    database.BeginTransaction();

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

                                if (SavePrescription(database, localPrescription, localStrutAdjustments))
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
                                        if (!SaveEntity(database, reminder))
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

                    database.Commit();

                }
                catch (Exception ex)
                {
                    database.Rollback();
                    LastException = ex;
                    return false;
                }

                try
                {
                    database.BeginTransaction();

                    // Delete old sync results
                    var syncResults = database.Table<SyncResult>().OrderBy(p => p.Id).ToList();
                    foreach (var syncResultToDelete in syncResults)
                        database.Delete(syncResultToDelete);

                    database.Commit();
                }
                catch (Exception ex)
                {
                    database.Rollback();
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
                    SaveEntity(database, syncResult);
                }

                return true;
            }
        }

        /// <summary>
        /// Updates the prescriptions RemovalDate according to a list of treatments dates
        /// </summary>
        /// <param name="treatmentDates"></param>
        /// <returns></returns>
        public bool UpdatePrescriptionsTreatmentDates(IList<MyHexPlanProxies.Models.FinalTreatmentCaseDateDTO> treatmentDates)
        {
            using (var database = GetConnection())
            {
                try
                {
                    // Delete old sync results
                    foreach (var treatmentDate in treatmentDates)
                    {
                        if (treatmentDate.CaseUid.HasValue && treatmentDate.FinalTreatmentDate.HasValue)
                        {
                            var prescription = database.Table<Prescription>().FirstOrDefault(p => p.CaseId == treatmentDate.CaseUid.Value);
                            if (prescription != null)
                            {
                                // Update only if needed
                                if (!prescription.RemovalDate.HasValue || prescription.RemovalDate.Value.Date != treatmentDate.FinalTreatmentDate.Value.Date)
                                {
                                    prescription.RemovalDate = treatmentDate.FinalTreatmentDate.Value;
                                    SaveEntity(database, prescription);
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
        }

        /// <summary>
        /// Regenerate the list of pin site care daily items
        /// </summary>
        public void RebuildPinSiteCareCalendar(bool isLoggedIn, bool pinSiteCareEnabled, TimeSpan pinSiteCareTime, DateTime? pinSiteCareStartDate, bool[] pinSiteCareDaysOfWeekNotification)
        {
            using (var database = GetConnection())
            {
                //Delete and refresh PinSiteCare Reminder.
                var reminders = database.Table<Reminder>().Where(r => r.Type == Reminder.ReminderType.PinSiteCareReminder);
                foreach (var curReminder in reminders)
                {
                    database.Delete(curReminder);
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
                        var prescriptions = database.Table<Prescription>().ToList();
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
                                    var items = database.Table<LogHistoryItem>().Where(i => i.ItemType == LogHistoryItem.ItemTypeEnum.PinSiteCareDone).ToList();
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
                                        SaveEntity<Reminder>(database, reminder);
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
                        var items = database.Table<LogHistoryItem>().Where(i => i.ItemType == LogHistoryItem.ItemTypeEnum.PinSiteCareDone).ToList();
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
                            SaveEntity<Reminder>(database, reminder);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check if a table exists 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private bool TableExists(SQLiteConnection database, string tableName)
        {
            int colCount;
            try
            {
                colCount = database.GetTableInfo(tableName).Count;
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
        private bool SavePrescription(SQLiteConnection database, Prescription prescription, IEnumerable<StrutAdjustment> strutAdjustments)
        {
            var result = SaveEntity(database, prescription);
            var adjResult = false;
            if (result)
            {
                foreach (var adj in strutAdjustments)
                {
                    adj.PrescriptionId = prescription.Id;
                    adj.Done = false;
                    adjResult = SaveEntity(database, adj);
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
        private void DeletePrescriptionCascade(SQLiteConnection database, Guid id)
        {
            var adjusts = database.Table<StrutAdjustment>().Where(s => s.PrescriptionId == id).ToList();
            foreach (var sa in adjusts.ToList())
            {
                database.Delete(sa);
            }
            var prescription = database.Table<Prescription>().FirstOrDefault(p => p.Id == id);
            database.Delete(prescription);
        }

        /// <summary>
        /// This method will throw exception if the update will be massive (query contains more than 1000 characters)
        /// </summary>
        private bool SaveEntityList<T>(IEnumerable<T> entityList) where T : BaseSqlEntity
        {
            using (var database = GetConnection())
            {

                var toInsert = entityList.Where(x => x.Id == Guid.Empty).ToList();
                var toUpdate = entityList.Where(x => x.Id != Guid.Empty).ToList();
                foreach (var item in toInsert)
                {
                    item.Id = Guid.NewGuid();
                }
                try
                {
                    database.UpdateAll(toUpdate, false);
                    database.InsertAll(toInsert, false);
                    LastException = null;
                    return true;
                }
                catch (Exception ex)
                {
                    LastException = ex;
                    return false;
                }
            }
        }

        private bool SaveEntity<T>(T entity) where T : BaseSqlEntity
        {
            using (var database = GetConnection())
            {
                return SaveEntity<T>(database, entity);
            }
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


    }
}