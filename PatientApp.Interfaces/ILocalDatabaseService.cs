using System;
using System.Collections.Generic;

using PatientApp.DataModel.SqlEntities;

namespace PatientApp.Interfaces
{

    public interface ILocalDatabaseService
    {
        Exception LastException { get; set; }

        bool CreateTablesIfNotExists(bool forced = false);

        bool CheckIntegrity();
        void DeleteAllTables();
        bool ResetToAnonymous();
        List<Prescription> GetPrescriptions();
        Prescription GetPrescritionById(Guid id);        
        List<StrutAdjustment> GetPrescriptionStrutAdjustments(Guid id, bool todoOnly);
        bool ExistStrutAdjustmentsToDate(DateTime dt);
        bool DeleteStrutAdjustmentsToDate(DateTime dt, bool deleteReminders = false);
        StrutAdjustment GetStrutAdjustmentById(Guid id);
        List<StrutAdjustment> GetStrutAdjustmentsAtDate(DateTime dt);
        List<ShoppingItem> GetShoppingItems();
        Reminder GetReminderById(Guid id);
        List<Reminder> GetReminders();
        List<Reminder> GetRemindersAtDate(DateTime dt);
        List<Reminder> GetRemindersAtDate(DateTime dt, Reminder.ReminderType type);
        List<Reminder> GetRemindersToDate(DateTime dt);
        List<Reminder> GetRemindersToDate(DateTime dt, Reminder.ReminderType type);
        List<Reminder> GetEntityReminders(Guid entityId);
        bool DeleteReminder(Reminder reminder);
        bool DeleteReminders(List<Reminder> reminders);
        bool DeleteAllReminderType(Reminder.ReminderType type);
        bool SaveShoppingItem(ShoppingItem item);
        bool SaveStrutAdjustment(StrutAdjustment item);
        bool SaveReminder(Reminder reminder);
        List<LogHistoryItem> GetHistoryLogItemsAtDate(DateTime dt);
        List<LogHistoryItem> GetHistoryLogItemsAtDate(DateTime dt, LogHistoryItem.ItemTypeEnum type);
        List<LogHistoryItem> GetHistoryLogItemsDoneActivitesAtDate(DateTime dt);        
        List<LogHistoryItem> GetUnsyncedHistoryLogItems();
        bool MarkAsSyncedHistoryLogItems(IEnumerable<LogHistoryItem> items);
        bool SaveHistoryLogItem(LogHistoryItem item);
        SurgeonContacts GetSurgeonContacts();
        bool SaveSurgeonContacts(SurgeonContacts contacts);
        List<SyncResult> GetSyncResults();
        bool ClearSyncResults();
        bool LastSyncHasChanges();
        bool SaveDownloadedPrescriptionsUpdate(MyHexPlanProxies.Models.PrescriptionUpdate update);

        bool UpdatePrescriptionsTreatmentDates(IList<MyHexPlanProxies.Models.FinalTreatmentCaseDateDTO> treatmentDates);

        void RebuildPinSiteCareCalendar(bool isLoggedIn, bool pinSiteCareEnabled, TimeSpan pinSiteCareTime, DateTime? pinSiteCareStartDate, bool[] pinSiteCareDaysOfWeekNotification);
    }
}
