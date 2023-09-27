using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PatientApp.DataModel.SqlEntities;

namespace PatientApp.Interfaces
{

  public interface ILocalDatabaseService
  {
    Exception LastException { get; set; }

    Task InitConnection();

    Task<bool> CreateTablesIfNotExists(bool forced = false);

    Task<bool> CheckIntegrity();
    Task DeleteAllTables();
    Task<bool> ResetToAnonymous();
    Task<List<Prescription>> GetPrescriptions();
    Task<Prescription> GetPrescritionById(Guid id);
    Task<List<StrutAdjustment>> GetPrescriptionStrutAdjustments(Guid id, bool todoOnly);
    Task<bool> ExistStrutAdjustmentsToDate(DateTime dt);
    Task<bool> DeleteStrutAdjustmentsToDate(DateTime dt, bool deleteReminders = false);
    Task<StrutAdjustment> GetStrutAdjustmentById(Guid id);
    Task<List<StrutAdjustment>> GetStrutAdjustmentsAtDate(DateTime dt);
    Task<List<ShoppingItem>> GetShoppingItems();
    Task<Reminder> GetReminderById(Guid id);
    Task<List<Reminder>> GetReminders();
    Task<List<Reminder>> GetRemindersAtDate(DateTime dt);
    Task<List<Reminder>> GetRemindersAtDate(DateTime dt, Reminder.ReminderType type);
    Task<List<Reminder>> GetRemindersToDate(DateTime dt);
    Task<List<Reminder>> GetRemindersToDate(DateTime dt, Reminder.ReminderType type);
    Task<List<Reminder>> GetEntityReminders(Guid entityId);
    Task<bool> DeleteReminder(Reminder reminder);
    Task<bool> DeleteReminders(List<Reminder> reminders);
    Task<bool> DeleteAllReminderType(Reminder.ReminderType type);
    Task<bool> SaveShoppingItem(ShoppingItem item);
    Task<bool> SaveStrutAdjustment(StrutAdjustment item);
    Task<bool> SaveReminder(Reminder reminder);
    Task<List<LogHistoryItem>> GetHistoryLogItemsAtDate(DateTime dt);
    Task<List<LogHistoryItem>> GetHistoryLogItemsAtDate(DateTime dt, LogHistoryItem.ItemTypeEnum type);
    Task<List<LogHistoryItem>> GetHistoryLogItemsDoneActivitesAtDate(DateTime dt);
    Task<List<LogHistoryItem>> GetUnsyncedHistoryLogItems();
    Task<bool> MarkAsSyncedHistoryLogItems(IEnumerable<LogHistoryItem> items);
    Task<bool> SaveHistoryLogItem(LogHistoryItem item);
    Task<SurgeonContacts> GetSurgeonContacts();
    Task<bool> SaveSurgeonContacts(SurgeonContacts contacts);
    Task<List<SyncResult>> GetSyncResults();
    Task<bool> ClearSyncResults();
    Task<bool> LastSyncHasChanges();
    Task<bool> SaveDownloadedPrescriptionsUpdate(MyHexPlanProxies.Models.PrescriptionUpdate update);

    Task<bool> UpdatePrescriptionsTreatmentDates(IList<MyHexPlanProxies.Models.FinalTreatmentCaseDateDTO> treatmentDates);

    Task RebuildPinSiteCareCalendar(bool isLoggedIn, bool pinSiteCareEnabled, TimeSpan pinSiteCareTime, DateTime? pinSiteCareStartDate, bool[] pinSiteCareDaysOfWeekNotification);
  }
}
