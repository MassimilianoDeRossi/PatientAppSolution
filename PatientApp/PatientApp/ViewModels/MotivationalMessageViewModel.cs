using System;
using PatientApp.DataModel.SqlEntities;
using PatientApp.Interfaces;
using PatientApp.Services;
using Xamarin.Forms;

namespace PatientApp.ViewModels
{
    /// <summary>
    /// Handling display of remote motivational message received.
    /// </summary>
    public class MotivationalMessageViewModel : BaseViewModel
    {

        /// <summary>
        /// Message category Icon Img Name
        /// </summary>
        public string MessageTypeIconImgName
        {
            get
            {
                //Map category to correct icon name.
                switch (MessageCategory)
                {
                    case MotivationalMessageCategory.Generic:
                        return "ico_generic";
                    case MotivationalMessageCategory.StrutsAdjustment:
                        return "ico_strut adj";
                    case MotivationalMessageCategory.PhysicalTherapy:
                        return "ico_physical";
                    case MotivationalMessageCategory.PinSiteCare:
                        return "ico_pinsitecare";
                    default:
                        return "";
                }

            }
        }

        private MotivationalMessageCategory _messageCategory;

        /// <summary>
        /// Message category.
        /// </summary>
        public MotivationalMessageCategory MessageCategory
        {
            get { return _messageCategory; }
            set { SetProperty(ref _messageCategory, value); }
        }

        private string _messageBody;

        /// <summary>
        /// Message body text.
        /// </summary>
        public string MessageBody
        {
            get { return _messageBody; }
            set { SetProperty(ref _messageBody, value); }
        }

        /// <summary>
        /// Message title associated to the category.
        /// </summary>
        public string MessageTitle
        {
            get
            {
                switch (MessageCategory)
                {
                    case MotivationalMessageCategory.PinSiteCare:
                        return Resources.PatientApp.MotivationalMessagePinSite;
                    case MotivationalMessageCategory.StrutsAdjustment:
                        return Resources.PatientApp.MotivationalMessageStrAdj;
                    case MotivationalMessageCategory.PhysicalTherapy:
                        return Resources.PatientApp.MotivationalMessagePhisicalTherapy;
                    case MotivationalMessageCategory.Generic:
                        return Resources.PatientApp.MotivationalMessageGeneric;
                    default:
                        return "Not Supported Category";
                }
            }
        }

        public Command UserClosePopupCommand { get; set; }

        public MotivationalMessageViewModel(ILocalDatabaseService dbService, ISystemUtility sysUtility) : base(dbService, null, sysUtility)
        {
            UserClosePopupCommand = new Command(UserClosePopupCommandExecute);
        }

        private async void UserClosePopupCommandExecute(object obj)
        {
            // Add new item in log history to mark this message as read by user.
            var historyItem = new LogHistoryItem()
            {
                LocalEntityId = null,
                EventDateTime = _sysUtility.Now,
                ExpectedDateTime = null,
                ItemType = LogHistoryItem.ItemTypeEnum.MotivationalMesssageRead,
                Description = MessageBody, 
                ServerEntityId = null,
            };
            _dbService.SaveHistoryLogItem(historyItem);


            await App.NavigationController.ClosePopupAsync();
        }
    }
    
}
