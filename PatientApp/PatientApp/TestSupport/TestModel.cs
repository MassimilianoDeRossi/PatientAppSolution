namespace PatientApp.TestSupport
{
    /// <summary>
    /// A model containing objects used by tests
    /// </summary>
    public class TestModel
    {
        public bool TestModeOn { get; set; }
        public string MockedScannedQrCode { get; set; }
        public string TimeToReturn { get; set; }
        public bool[] ShoppingListCheckBoxStatus { get; set; }
        public bool SendNotifications { get; set; }
        public bool ForceSkipQrScan { get; set; }
        public string ToDoList { get; set; }
        public string DoneList { get; set; }
        public string SelectedStrutDirectionImage { get; set; }
        public string SelectedStrutBackgroundImageName { get; set; }
        public string FrameStrutsAdj { get; set; }
        public string BadgeStrutsAdj { get; set; }

        public TestModel()
        {
            TestModeOn = false;
            SendNotifications = true;
            ForceSkipQrScan = false;
            ShoppingListCheckBoxStatus = new bool[6];
            BadgeStrutsAdj = "";
            FrameStrutsAdj = "";
        }

    }
}
