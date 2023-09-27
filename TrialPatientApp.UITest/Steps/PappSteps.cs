using System;
using TechTalk.SpecFlow;
using Xamarin.UITest;
using NUnit.Framework;
using System.Data;
using System.Linq;
using TechTalk.SpecFlow.Assist;
using System.Globalization;
using System.Resources;

namespace PatientApp.UITest.Features
{
    public partial class ShoppingList_Item_3700Feature : FeatureBase
    {
        public ShoppingList_Item_3700Feature(Platform platform) : base(platform)
        { }
    }

    public partial class PinSiteCare_Item_3710Feature : FeatureBase
    {
        public PinSiteCare_Item_3710Feature(Platform platform) : base(platform)
        { }
    }

    public partial class AccessWithAtLeastAPrescriptionLoaded_Item_3775Feature : FeatureBase
    {
        public AccessWithAtLeastAPrescriptionLoaded_Item_3775Feature(Platform platform) : base(platform)
        { }
    }

    public partial class SetUserProfileAnonymous_Item_3776Feature : FeatureBase
    {
        public SetUserProfileAnonymous_Item_3776Feature(Platform platform) : base(platform)
        { }
    }

    public partial class FirstAccessAsAnonymousUser_Item_3778Feature : FeatureBase
    {
        public FirstAccessAsAnonymousUser_Item_3778Feature(Platform platform) : base(platform)
        { }
    }

    public partial class FirstAccessByPrescriptionActivation_Item_3779Feature : FeatureBase
    {
        public FirstAccessByPrescriptionActivation_Item_3779Feature(Platform platform) : base(platform)
        { }
    }

    public partial class AllPatientsDailyTasks_Item_3783Feature : FeatureBase
    {
        public AllPatientsDailyTasks_Item_3783Feature(Platform platform) : base(platform)
        { }
    }

    public partial class StrutAdjustmentReminder_Item_3788Feature : FeatureBase
    {
        public StrutAdjustmentReminder_Item_3788Feature(Platform platform) : base(platform)
        { }
    }

    public partial class SelfTestSurvey_Item_3793Feature : FeatureBase
    {
        public SelfTestSurvey_Item_3793Feature(Platform platform) : base(platform)
        { }
    }

    public partial class StrutAdjustmentConfirmation_Item_3795Feature : FeatureBase
    {
        public StrutAdjustmentConfirmation_Item_3795Feature(Platform platform) : base(platform)
        { }
    }

    public partial class SetUserProfileNormal_Item_4196Feature : FeatureBase
    {
        public SetUserProfileNormal_Item_4196Feature(Platform platform) : base(platform)
        { }
    }

    public partial class MyContacts_Item_4948Feature : FeatureBase
    {
        public MyContacts_Item_4948Feature(Platform platform) : base(platform)
        { }
    }
    public partial class AllTextMyHEXPlanAppFeature : FeatureBase
    {
        public AllTextMyHEXPlanAppFeature(Platform platform) : base(platform)
        { }
    }
}

namespace PatientApp.UITest.Steps
{

    [Binding]
    public class PappSteps
    {
        readonly IApp app;
        private bool isAnonymous, isNormal;
        readonly bool[] ItemIsSelected = { false, false, false, false, false, false };
        string FrameSeen;
        string BadgeSeen;
        string FrameIDTapped;
        string FrameSeenBackup;
        string ToDoList, DoneList;
        private bool resetView;

        public PappSteps()
        {
            app = FeatureContext.Current.Get<IApp>("App");
            isAnonymous = false;
            isNormal = false;
            resetView = false;
            FrameSeen = "";
            BadgeSeen = "";
            FrameIDTapped = "";
            ToDoList = "";
            DoneList = "";
        }

        [Given(@"User is '(.*)'")]
        public void GivenUserIs(string p0)
        {
            Utils.ExecuteBackdoor(Backdoors.SetTestMode, "true");
            Utils.ExecuteBackdoor(Backdoors.SetSendNotifications, "false");
            if (p0 == "anonymous")
            {
                isAnonymous = true;
                isNormal = false;
            }
            if (p0 == "normal")
            {
                isAnonymous = false;
                isNormal = true;
                Utils.ExecuteBackdoor(Backdoors.SetTestScannedQrCode, new Networking.PrescriptionsMockedIndices().QrCode1);
            }
            //app.Repl();
        }

        [Given(@"The user access with QrCode '(.*)' at time '(.*)'")]
        public void GivenTheUserAccessWithQrCodeAtTime(string QrCodeName, string Time)
        {
            /* 
             * Prescription1 = QrCode1
             * 
             * Qr000 = QrCodeSingleADJ0
             * Qr001 = QrCodeSingleADJ2
             * Qr002 = QrCodeSingleADJ1
             * Qr003 = QrCodeSingleADJ3
             */
            isAnonymous = false;
            isNormal = true;
            Utils.ExecuteBackdoor(Backdoors.SetTestMode, "true");
            Utils.ExecuteBackdoor(Backdoors.SetSendNotifications, "false");
            switch (QrCodeName)
            {
                case "Prescription1":
                    Utils.ExecuteBackdoor(Backdoors.SetTestScannedQrCode, new Networking.PrescriptionsMockedIndices().QrCode1);
                    break;
                case "Qr000":
                    Utils.ExecuteBackdoor(Backdoors.SetTestScannedQrCode, new Networking.PrescriptionsMockedIndices().QrCodeSingleADJ0);
                    break;
                case "Qr001":
                    Utils.ExecuteBackdoor(Backdoors.SetTestScannedQrCode, new Networking.PrescriptionsMockedIndices().QrCodeSingleADJ2);
                    break;
                case "Qr002":
                    Utils.ExecuteBackdoor(Backdoors.SetTestScannedQrCode, new Networking.PrescriptionsMockedIndices().QrCodeSingleADJ1);
                    break;
                case "Qr003":
                    Utils.ExecuteBackdoor(Backdoors.SetTestScannedQrCode, new Networking.PrescriptionsMockedIndices().QrCodeSingleADJ3);
                    break;
            }
            GivenDeviceTimeIs(Time);
        }

        [Given(@"User navigate to '(.*)' page")]
        public void GivenUserNavigateToPage(string PagName)
        {
            switch (PagName)
            {
                case "Home Anonymous":
                    break;
                case "Pin site care":
                    //pagina da home page ==> tap esagono
                    WhenUserTapOnOption("Pin site care");
                    break;
                case "Home Normal":
                    GivenUserNavigateToPage("Start your treatment");
                    Utils.ExecuteBackdoor(Backdoors.SimulateScannedQrCode);
                    Utils.WaitElement("hexagon_prescriptionalert.png");
                    break;
                case "All my daily tasks":
                    GivenUserNavigateToPage("Home Normal");
                    Utils.WaitAndTapElement("BtnAllMyDailyTaskButton"); //we can tap without waiting actually
                    Utils.WaitElement("ico_calendar"); //todo insert this kind of pause everywhere in this method?
                    break;
                case "How do you feel today?":
                    GivenUserNavigateToPage("All my daily tasks");
                    Utils.WaitAndTapElement("LblDayMood");
                    break;
                case "Start your treatment":
                    Utils.WaitAndTapElement("BtnStartPrescriptionButton");
                    break;
                case "Welcome User Anonymous":
                    GivenUserNavigateToPage("Start your treatment");
                    Utils.WaitAndTapElement("BtnWithoutPrescription");
                    break;
                case "Welcome User Normal":
                    GivenUserNavigateToPage("Start your treatment");
                    PassByQRScan();
                    break;
                case "User Profile":
                    if (isAnonymous)
                    {
                        GivenUserNavigateToPage("Welcome User Anonymous");
                    }
                    if (isNormal)
                    {
                        GivenUserNavigateToPage("Welcome User Normal");
                    }
                    WhenUserTapOnOption("Next");
                    break;
                case "Pin Site Care":
                    if (isAnonymous)
                    {
                        GivenUserNavigateToPage("Welcome User Anonymous");
                    }
                    if (isNormal)
                    {
                        GivenUserNavigateToPage("Welcome User Normal");
                    }
                    WhenUserTapOnOption("Next");
                    WhenUserTapOnOption("Next");
                    break;
                case "Personal Goal":
                    GivenUserNavigateToPage("Welcome User Normal");
                    WhenUserTapOnOption("Next");
                    WhenUserTapOnOption("Next");
                    WhenUserTapOnOption("Next");
                    break;
                case "Insights Messages":
                    GivenUserNavigateToPage("Welcome User Normal");
                    WhenUserTapOnOption("Next");
                    WhenUserTapOnOption("Next");
                    WhenUserTapOnOption("Next");
                    Utils.WaitAndTapElement(Utils.GetClassName(PlatformClassName.Switch), QueryType.Class); //fare con l'invoke set on non viene impostato veramente il valore, per disattivare tappo una volta perchè all'inizio è sempre attivo
                    WhenUserTapOnOption("Next");
                    break;
                case "Shopping List":
                    if (isNormal)
                    {
                        GivenUserNavigateToPage("Home Normal");
                    }
                    WhenUserTapOnOption("Pin site care");
                    Utils.WaitAndTapElement("GrdAmIGetInfection");// il caccia ha invertito id sulle due scritte XD
                    break;
                case "Your digital prescription":
                    Utils.ExecuteBackdoor(Backdoors.SetTestMode, "false");
                    GivenUserNavigateToPage("Start your treatment");
                    Utils.WaitAndTapElement("BtnScanCode");
                    //Utils.WaitAndTapElement("OK"); //accettazione permessi fotocamera
                    break;
                case "Strut Adjustment":
                    GivenUserNavigateToPage("Home Normal");
                    WhenUserTapOnOption("Strut Adjust");
                    break;
                case "User Details":
                    if (isNormal)
                    {
                        GivenUserNavigateToPage("Home Normal");
                    }
                    Utils.WaitAndTapElement("toolbar_user");
                    break;
                case "Settings":
                    if (isNormal)
                    {
                        GivenUserNavigateToPage("Home Normal");
                    }
                    Utils.WaitAndTapElement("toolbar_setting");
                    break;
            }
        }

        [When(@"User tap on '(.*)' option")]
        public void WhenUserTapOnOption(string OptName)
        {
            switch (OptName)
            {
                //hexagons
                case "Strut Adjust":
                    Utils.WaitElement("hexagons_full.png");
                    app.Tap("BtnStrutAdjustButton");
                    break;
                case "Pin site care":
                    if (isNormal)
                    {
                        Utils.WaitElement("hexagons_full.png");
                    }
                    if (isAnonymous)
                    {
                        Utils.WaitElement("hexagons_half.png");
                    }
                    app.Tap("BtnPinSiteCareButton");
                    break;
                case "Prescription":
                    Utils.WaitElement("hexagons_full.png");
                    app.Tap("BtnPrescriptionButton");
                    break;
                case "Support": //cambiato con support dopo eliminazione time-lapse
                    if (isNormal)
                    {
                        Utils.WaitElement("hexagons_full.png");
                    }
                    if (isAnonymous)
                    {
                        Utils.WaitElement("hexagons_half.png");
                    }
                    app.Tap("BtnMySurgeonButton");
                    break;
                case "My Diary":
                    Utils.WaitElement("hexagons_full.png");
                    app.Tap("BtnSupportButton");
                    break;
                case "an area without":
                    if (Utils.IsVisible("BtnPrescriptionButton") || Utils.IsVisible("BtnSupportButton"))
                    {
                        Assert.Fail("BtnPrescriptionButton and BtnSupportButton must be hidden");
                    }
                    break;

                //buttons & switch
                case "Tap here to get started":
                    Utils.WaitAndTapElement("BtnStartPrescriptionButton");
                    break;
                case "All my daily tasks":
                    Utils.WaitAndTapElement("BtnAllMyDailyTaskButton");
                    break;
                case "How do you feel today?":
                    if (Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitleAllMyDailyTask"))))
                    {
                        Utils.WaitAndTapElement("LblDayMood");
                    }
                    else
                    {
                        Utils.WaitAndTapElement("LblHowDoYouFeel");
                    }
                    break;
                case "Goal":
                case "Insights":
                    Utils.WaitAndTapElement(Utils.GetClassName(PlatformClassName.Switch), QueryType.Class);
                    break;
                case "Settings":
                    Utils.WaitAndTapElement("toolbar_setting");
                    break;
                case "Profile":
                    Utils.WaitAndTapElement("toolbar_user");
                    break;
                case "Confirm this mood":
                    Utils.WaitAndTapElement("BtnConfirm");
                    break;
                case "i":
                    Utils.WaitAndTapElement("ImgCleansingSolutionInfo");
                    break;
                case "Share":
                    Utils.OnAndroid(() =>
                    {
                        Assert.Ignore("This function is not implemented yet on Android");
                    });
                    Utils.OnIOS(() =>
                    {
                        Utils.WaitAndTapElement("ico_share");
                    });
                    break;
                case "Today I feel":
                    if (Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitleAllMyDailyTask"))))
                    {
                        Utils.WaitAndTapElement("LblDayMood");
                    }
                    else
                    {
                        Utils.WaitAndTapElement("LblHowDoYouFeel");
                    }
                    break;

                //General Button
                case "<":
                    Utils.OnIOS(() =>
                    {
                        Utils.WaitAndTapElement("back");
                    });
                    Utils.GoBackOnAndroid();
                    break;
                case "Exit":
                    Utils.WaitAndTapElement("LblSkip");
                    break;
                case "Back":
                    Utils.WaitAndTapElement("LblPrev");
                    break;
                case "Next":
                case "Done":
                case "Done Adjustment":
                    Utils.WaitAndTapElement("LblNext");
                    break;
                case "a char":
                    Utils.Wait(1000);
                    if (!Utils.IsVisible("BtnChangeNickName"))
                    {
                        if (!Utils.IsVisible("EntryNick"))
                        {
                            Utils.WaitAndTapElement("EdtPersonalGoal");
                        }
                        else
                        {
                            Utils.WaitAndTapElement("EntryNick");
                        }
                        Utils.OnIOS(() =>
                        {
                            app.TapCoordinates(app.Query(x => x.Class("UIKeyboardLayoutStar"))[0].Rect.CenterX, app.Query(x => x.Class("UIKeyboardLayoutStar"))[0].Rect.CenterY);
                        });
                        Utils.OnAndroid(() =>
                        {
                            Assert.Ignore("This function is not implemented yet on Android");
                        });
                        app.DismissKeyboard();
                    }
                    else
                    {
                        app.Tap("BtnChangeNickName");
                        app.EnterText("g");
                        app.DismissKeyboard();
                        app.Tap(x => x.Text("OK"));
                    }
                    break;
                case "X":
                    Utils.WaitAndTapElement("close");
                    break;
                case "Close":
                    Utils.OnIOS(() =>
                    {
                        Utils.WaitAndTapElement("BtnClose");
                    });
                    Utils.OnAndroid(() =>
                    {
                        Utils.WaitAndTapElement("ImgClose");
                    });
                    break;
                case "Cancel":
                case "Not Confirm Postpone":
                    Utils.WaitAndTapElement("Cancel", QueryType.Text);
                    break;
                case "Cancel Mood":
                    Utils.WaitAndTapElement("BtnCancel");
                    break;
                case "Scan your access code":
                    Utils.ExecuteBackdoor(Backdoors.SetTestMode, "false");
                    Utils.WaitAndTapElement("BtnScanCode");
                    break;
                case "Do it later":
                    Utils.WaitAndTapElement("BtnDoItLater");
                    break;
                case "Start Adjustment":
                    Utils.WaitAndTapElement("BtnStartAdjustment");
                    break;
                case "Confirm Postpone":
                case "OK":
                    Utils.WaitAndTapElement("OK", QueryType.Text);
                    break;
                case "How will I know if I get an infection?":
                    Utils.WaitAndTapElement("GrdWhatDoINeed");
                    break;
                case "What do I need?":
                    Utils.WaitAndTapElement("GrdAmIGetInfection");
                    break;
                case "Confirm pin site care":
                    Utils.WaitAndTapElement("BtnPinSiteCareDoneButton");
                    break;
                case "Cancel pin site care":
                    Utils.WaitAndTapElement("BtnMaybeLaterButton");
                    break;
            }
        }

        [Then(@"'(.*)' is correct")]
        public void ThenIsCorrect(string p0)
        {
            /* quello che si vuole fare è:
             * ottenere il culture info del telefono che esegue il test it-IT oppure en-US ecc...
             * a questo punto prendere dal pc data e mese attuali e fare il tap del text sulla schermata home
             * 
             * in realtà al momento si trova tutto in en-US
             */
            DateTime dt = DateTime.Now;
            string monthAndDate = Utils.CultureInfo.DateTimeFormat.GetMonthName(dt.Month) + " ";
            monthAndDate = monthAndDate + dt.Day;
            if (!Utils.IsVisible("hexagons_half.png") && !Utils.IsVisible("hexagons_full.png"))
            {
                //controllo data pagina all my daily task
                if (!Utils.IsVisible(x => x.Text(monthAndDate)))
                {
                    Assert.Fail("Day number or Month name is/are wrong, should be: " + monthAndDate);
                }
            }
            else
            {
                //controllo data Home page
                if (!Utils.IsVisible(x => x.Text(Utils.CultureInfo.DateTimeFormat.GetAbbreviatedMonthName(dt.Month).ToUpper()))) //check month name
                {
                    Assert.Fail("Month name is wrong, should be: " + Utils.CultureInfo.DateTimeFormat.GetAbbreviatedMonthName(dt.Month).ToUpper());
                }
                if (dt.Day < 10 && !Utils.IsVisible(x => x.Text("0" + dt.Day.ToString())))//controllo numero giorno
                {
                    Assert.Fail("Day number is wrong, should be: 0" + dt.Day);
                }
                if (dt.Day >= 10 && !Utils.IsVisible(x => x.Text(dt.Day.ToString()))) //controllo numero giorno
                {
                    Assert.Fail("Day number is wrong, should be: " + dt.Day);
                }
            }
            if (!Utils.IsVisible(x => x.Text(Utils.CultureInfo.DateTimeFormat.GetDayName(dt.DayOfWeek)))) //controllo nome giorno
            {
                Assert.Fail("Day name is wrong, should be:" + Utils.CultureInfo.DateTimeFormat.GetDayName(dt.DayOfWeek));
            }
        }

        [Then(@"'(.*)' page is visualized")]
        public void ThenPageIsVisualized(string page)
        {
            Utils.Wait(2000);
            switch (page)
            {
                case "All my daily tasks":
                    if (!Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitleAllMyDailyTask"))))
                    {
                        Assert.Fail("Wrong page is visualized, should be:" + Utils.GetTranslatedResx("LblTitleAllMyDailyTask"));
                    }
                    break;
                case "Strut Adjust":
                    if (!Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitleStrutAdjust"))))
                    {
                        Assert.Fail("Wrong page is visualized, should be:" + Utils.GetTranslatedResx("LblTitleStrutAdjust"));
                    }
                    break;
                case "Pin site care": //pagina che si ottiene da home page
                    //ho messo un [1] e non [0] perchè ci sono 2 LblTitle
                    if (!Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitlePinSiteCare"))))
                    {
                        Assert.Fail("Wrong page is visualized, should be:" + Utils.GetTranslatedResx("LblTitlePinSiteCare"));
                    }
                    break;
                case "Prescription":
                    if (!Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitlePrescription"))))
                    {
                        Assert.Fail("Wrong page is visualized, should be:" + Utils.GetTranslatedResx("LblTitlePrescription"));
                    }
                    break;
                case "My Diary":
                    if (!Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitleMyDiary"))))
                    {
                        Assert.Fail("Wrong page is visualized, should be:" + Utils.GetTranslatedResx("LblTitleMyDiary"));
                    }
                    break;
                case "Time Lapse":
                    if (!Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitleTimeLapse"))))
                    {
                        Assert.Fail("Wrong page is visualized, should be:" + Utils.GetTranslatedResx("LblTitleTimeLapse"));
                    }
                    break;
                case "Support":
                    if (!Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitleSupport"))))
                    {
                        Assert.Fail("Wrong page is visualized, should be:" + Utils.GetTranslatedResx("LblTitleSupport"));
                    }
                    break;
                case "How do you feel today?":
                    if (!Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitleHowDoYouFeelToday"))))//ci sono più LblTitle, quello corretto è il secondo
                    {
                        Assert.Fail("Wrong page is visualized, should be:" + Utils.GetTranslatedResx("LblTitleHowDoYouFeelToday"));
                    }
                    break;
                case "User profile customization":
                    if (!Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitleUserProfileCustomization"))))
                    {
                        Assert.Fail("Wrong page is visualized, should be:" + Utils.GetTranslatedResx("LblTitleUserProfileCustomization"));
                    }
                    break;
                case "Profile":
                    if (!Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitleProfile"))))
                    {
                        Assert.Fail("Wrong page is visualized, should be:" + Utils.GetTranslatedResx("LblTitleProfile"));
                    }
                    break;
                case "Settings":
                    if (!Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitleSettings"))))
                    {
                        Assert.Fail("Wrong page is visualized, should be:" + Utils.GetTranslatedResx("LblTitleSettings"));
                    }
                    break;
                case "Home Anonymous":
                    if (!Utils.IsVisible("hexagons_half.png"))
                    {
                        Assert.Fail("Wrong page is visualized, should be Home Anonymous");
                    }
                    break;
                case "Home Normal":
                    if (!Utils.IsVisible("hexagons_full.png"))
                    {
                        Assert.Fail("Wrong page is visualized, should be Home Normal");
                    }
                    break;
                case "Welcome User Anonymous":
                    if (Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitleWelcomeUserAnonymous"))))
                    {
                        app.Tap("welcome_anonymous"); //immagine della welcome user anonymous
                    }
                    else
                    {
                        Assert.Fail("Wrong page is visualized, should be:" + Utils.GetTranslatedResx("LblTitleWelcomeUserAnonymous"));
                    }
                    break;
                case "Welcome User Normal":
                    if (Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitleWelcomeUserNormal"))))
                    {
                        app.Tap("welcome_user"); //immagine della welcome user normal
                    }
                    else
                    {
                        Assert.Fail("Wrong page is visualized, should be:" + Utils.GetTranslatedResx("LblTitleWelcomeUserNormal"));
                    }
                    break;
                case "User Profile":
                    if (!Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitleUserProfile"))))
                    {
                        Assert.Fail("Wrong page is visualized, should be:" + Utils.GetTranslatedResx("LblTitleUserProfile"));
                    }
                    break;
                case "Pin Site Care": //pagina che si ottiene dagli step per il set up dell'utente
                    if (!Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitlePinSiteCareSetProfile"))))
                    {
                        Assert.Fail("Wrong page is visualized, should be:" + Utils.GetTranslatedResx("LblTitlePinSiteCareSetProfile"));
                    }
                    break;
                case "Personal Goal":
                    if (!Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitlePersonalGoal"))))
                    {
                        Assert.Fail("Wrong page is visualized, should be:" + Utils.GetTranslatedResx("LblTitlePersonalGoal"));
                    }
                    break;
                case "Insights Messages":
                    if (!Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitleInsightsMessages"))))
                    {
                        Assert.Fail("Wrong page is visualized, should be:" + Utils.GetTranslatedResx("LblTitleInsightsMessages"));
                    }
                    break;
                case "Shopping List":
                    if (!Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitleShoppingList"))))
                    {
                        Assert.Fail("Wrong page is visualized, should be:" + Utils.GetTranslatedResx("LblTitleShoppingList"));
                    }
                    break;
                case "Your digital prescription":
                    //controllo testi:
                    //  light on/off
                    //  Hold over the QR code ecc.
                    //  Quadrato per QR code
                    Utils.WaitElement("LblTitle");
                    if (!Utils.IsVisible(x => x.Text(Utils.GetTranslatedResx("LedLightState"))) ||
                        !Utils.IsVisible(x => x.Text(Utils.GetTranslatedResx("HowToScanText"))) ||
                        !Utils.IsVisible("scanbrackets"))
                    {
                        Assert.Fail("Wrong page is visualized or Wrong text!");
                    }
                    break;
                case "Start your treatment":
                    if (!Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitleStartYourTreatment"))))
                    {
                        Assert.Fail("Wrong page is visualized, should be:" + Utils.GetTranslatedResx("LblTitleStartYourTreatment"));
                    }
                    break;
                case "FrameIDInfo":
                    if (!Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitleStrutAdjust"))) &&
                        !Utils.IsVisible(x => x.Marked("BtnStartAdjustment")))
                    {
                        Assert.Fail("Wrong page is visualized!");
                    }
                    break;
                case "Am I getting an infection?":
                    if (!Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitleInfection"))))
                    {
                        Assert.Fail("Wrong page is visualized, should be:" + Utils.GetTranslatedResx("LblTitleInfection"));
                    }
                    break;
                case "User Details":
                    if (!Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitleUserDetails"))))
                    {
                        Assert.Fail("Wrong page is visualized, should be:" + Utils.GetTranslatedResx("LblTitleUserDetails"));
                    }
                    break;
            }
        }

        [When(@"User enter '(.*)' with '(.*)'")]
        public void WhenUserEnterWith(string field, string myvalue)
        {
            switch (field)
            {
                case "nickname":
                    if (myvalue != "empty")
                    {
                        Utils.Wait(1000);
                        if (!Utils.IsVisible("BtnChangeNickName"))
                        {
                            app.ClearText("EntryNick");
                            app.DismissKeyboard();
                            app.EnterText("EntryNick", myvalue);
                            app.DismissKeyboard();
                        }
                        else
                        {
                            app.Tap("BtnChangeNickName");
                            Utils.OnAndroid(() =>
                            {
                                Utils.WaitElement("Enter a new nickname");
                            });
                            app.ClearText();
                            if (Utils.isAndroid && myvalue == "ciao<>%#&?")
                            {
                                Assert.Ignore("Bug on Android doesn't allow test to enter special chars in an entry");
                            }
                            app.EnterText(myvalue);
                            app.DismissKeyboard();
                            app.Tap(x => x.Text("OK"));
                        }
                    }
                    break;
                case "set daily time":
                    if (myvalue != "empty")
                    {
                        string[] DailyTime = myvalue.Split(':');
                        if (!Utils.IsVisible(x => x.Text(Utils.GetTranslatedResx("LblTitleInsightsMessages"))))
                        {
                            Utils.WaitAndTapElement("TimePickerEntryDailyTime"); // pin site care page
                        }
                        else
                        {
                            Utils.WaitAndTapElement("TpInsightTimeValue");
                        }
                        if (Utils.IsVisible(x => x.Class(Utils.GetClassName(PlatformClassName.TimePicker))))
                        {
                            Utils.OnIOS(() =>
                            {
                                //INSERIMENTO ORA
                                app.Query(x => x.Class(Utils.GetClassName(PlatformClassName.TimePicker)).Invoke("selectRow", Int32.Parse(DailyTime[0]) + 24, "inComponent", 0, "animated", true));
                                app.Tap(x => x.Property("text").EndsWith(DailyTime[0]));

                                //INSERIMENTO MINUTI
                                app.Query(x => x.Class(Utils.GetClassName(PlatformClassName.TimePicker)).Invoke("selectRow", Int32.Parse(DailyTime[1]) + 1, "inComponent", 1, "animated", true));
                                if (app.Query(x => x.Property("text").EndsWith(DailyTime[1])).Length >= 2)
                                {
                                    app.Tap(x => x.Property("text").EndsWith(DailyTime[1]).Index(1));
                                }
                                else
                                {
                                    app.Tap(x => x.Property("text").EndsWith(DailyTime[1]));
                                }
                                app.Tap(x => x.Class("UIButtonLabel")); //tasto done del settaggio ora // todo indicizzare
                            });
                            Utils.OnAndroid(() =>
                            {
                                app.Query(x => x.Class(Utils.GetClassName(PlatformClassName.TimePicker)).Invoke("setCurrentHour", int.Parse(DailyTime[0])));
                                app.Query(x => x.Class(Utils.GetClassName(PlatformClassName.TimePicker)).Invoke("setCurrentMinute", int.Parse(DailyTime[1])));
                                app.Tap(x => x.Text("OK")); //tasto done del settaggio ora //todo indicizzare
                            });
                        }
                    }
                    break;
                case "set your personal goal":
                    if (app.Query("EdtPersonalGoal")[0].Enabled)
                    {
                        app.ClearText("EdtPersonalGoal");
                        app.DismissKeyboard();
                        if (myvalue != "empty" && myvalue != "a 200 chars string")
                        {
                            app.EnterText("EdtPersonalGoal", myvalue);
                        }
                        if (myvalue == "a 200 chars string")
                        {
                            for (int i = 0; i < 20; i++)
                            {
                                app.EnterText("EdtPersonalGoal", "0123456789");
                            }
                        }
                        app.DismissKeyboard();
                    }
                    break;
                case "Mood":
                    Utils.WaitElement("mood_male.jpg");
                    if (Utils.IsVisible("UnselectedIcon"))
                    {
                        app.Tap("UnselectedIcon");
                    }
                    else
                    {
                        app.Tap("ImgSetMoodEmoticon");
                    }
                    Utils.WaitElement(Utils.GetClassName(PlatformClassName.Slider), QueryType.Class);
                    string[] MoodTypes = new string[] { "Frustrated", "Nervous", "Sad", "Bored", "I am OK", "Calm", "Optimistic", "Happy", "Encouraged" };
                    Utils.OnIOS(() =>
                    {
                        app.Tap(x => x.Class(Utils.GetClassName(PlatformClassName.Slider)).Child().Index(2));//non appaiono più le faccine al primo colpo se non tappi sul pallino --> bug segnalato
                    });
                    for (int i = 0; i < 9; i++)
                    {
                        if (MoodTypes[i] == myvalue)
                        {
                            Utils.OnIOS(() =>
                            {
                                app.Query(x => x.Class(Utils.GetClassName(PlatformClassName.Slider)).Invoke("setValue", i, "animated", true));
                                app.Tap(x => x.Class(Utils.GetClassName(PlatformClassName.Slider)).Child().Index(2));
                            });
                            Utils.OnAndroid(() =>
                            {
                                app.SetSliderValue(x => x.Class(Utils.GetClassName(PlatformClassName.Slider)), 124 * i); //this could be correct also for IOS
                            });
                            i = 10;
                        }
                    }
                    break;
                case "list":
                    // Possibili valori di numeroCasuale: 0, 1, 2, 3, 4, 5
                    int Value;
                    Random random = new Random();
                    String[] substring = myvalue.Split(' ');

                    for (int i = 0; i < Int32.Parse(substring[0]); i++)
                    {
                        do
                        {
                            Value = random.Next(0, 6);
                        } while (ItemIsSelected[Value]);
                        ItemIsSelected[Value] = true;
                        app.Tap(x => x.Marked("ImgStateImage").Index(Value));
                    }
                    break;
            }
        }

        [When(@"User navigate to '(.*)' page from '(.*)' page")]
        public void WhenUserNavigateToPageFromPage(string target, string start)
        {
            string[] pages = new string[] { "Home Anonymous", "Start your treatment", "Welcome User Anonymous", "User Profile", "Pin Site Care" };
            if (isNormal)
            {
                pages = new string[] { "Home Normal", "Start your treatment", "Welcome User Normal", "User Profile", "Pin Site Care", "Personal Goal", "Insights Messages" };
            }
            int j = Array.IndexOf(pages, target);
            for (int i = Array.IndexOf(pages, start); i < j; i++)
            {
                if (i == 0)
                {
                    Utils.WaitAndTapElement("BtnStartPrescriptionButton");
                }
                else if (i == 1)
                {
                    if (isAnonymous)
                    {
                        Utils.WaitAndTapElement("BtnWithoutPrescription");
                    }
                    else if (isNormal)
                    {
                        PassByQRScan();
                    }
                }
                else if (i >= 2 && i <= 5)
                {
                    WhenUserTapOnOption("Next");
                }
            }
        }

        [Then(@"The '(.*)' should be '(.*)'")]
        public void ThenTheShouldBe(string field, string myvalue)
        {
            Utils.Wait(2000);
            switch (field)
            {
                case "nickname":
                    if (!Utils.IsVisible("BtnChangeNickName"))
                    {
                        if (myvalue == "empty")
                        {
                            Utils.OnIOS(() => //on Android this can't be checked
                            {
                                if (!Utils.IsVisible(x => x.Text("Nickname")))
                                {
                                    Assert.Fail("Nickname value not as default");
                                }
                            });
                        }
                        else
                        {
                            if (!Utils.IsVisible(x => x.Text(myvalue)))
                            {
                                Assert.Fail("Nickname value not equals to:" + myvalue);
                            }
                        }
                    }
                    else
                    {
                        if (myvalue == "empty")
                        {
                            if (Utils.GetText("LblNickNameValue") != "Nickname")
                            {
                                Assert.Fail("Nickname value not as default");
                            }
                        }
                        else
                        {
                            if (Utils.GetText("LblNickNameValue") != myvalue)
                            {
                                Assert.Fail("Nickname value not equals to:" + myvalue);
                            }
                        }
                    }
                    break;
                case "photo":
                    if (myvalue == "empty" && !Utils.IsVisible(x => x.Text(Utils.GetTranslatedResx("LblSelectPhoto_0"))))
                    {
                        Assert.Fail("Photo is not empty");
                    }
                    break;
                case "set daily time":
                    //con il cambio della lingua basta fare un serie di istruzioni
                    //se la lingua non prevede AM e PM myvalue viene modificato nell'orario consono alla lingua
                    if (myvalue == "AM" || myvalue == "PM")
                    {
                        if (!Utils.GetText("TimePickerEntryDailyTime").Contains(myvalue))
                        {
                            Assert.Fail(myvalue + " string not founded!");
                        }
                    }
                    else
                    {
                        app.Tap(x => x.Text(myvalue)); //secondo me non serve l'id
                    }
                    break;
                case "Goal":
                case "Insights":
                    /* NOTA
                     * personal goal è:
                     * - abilitato se switch su off (disabilito la disabilitazione)
                     * - disabilitato se switch su on (abilito la disabilitazione)
                     */
                    if (myvalue == "enabled")
                    {
                        myvalue = Utils.GetValueName(PlatformValueName.Switch_CheckedValue);
                    }
                    if (myvalue == "disabled")
                    {
                        myvalue = Utils.GetValueName(PlatformValueName.Switch_NotCheckedValue);
                    }
                    if (Utils.GetSwitchValue() != myvalue)
                    {
                        Assert.Fail("Lo stato dello switch è errato!");
                    }
                    break;
                case "set your personal goal":
                    if (myvalue == "empty")
                    {
                        if (Utils.GetText("EdtPersonalGoal") != "")
                        {
                            Assert.Fail("Text Field is not empty!");
                        }
                    }
                    else
                    {
                        if (myvalue == "a 200 chars string")
                        {
                            if (Utils.GetText("EdtPersonalGoal").Length > 200)
                            {
                                Assert.Fail("Text Field cannot contain a string with more than 200 chars!");
                            }
                        }
                        else
                        {
                            app.Tap(x => x.Text(myvalue));
                        }
                    }
                    break;
                case "Frame":
                    if (myvalue == "disappear" && Utils.IsVisible("BtnCancel") || Utils.IsVisible("BtnConfirm") ||
                        Utils.IsVisible(x => x.Class(Utils.GetClassName(PlatformClassName.Slider))))
                    {
                        Assert.Fail("The Frame must disappear!");
                    }
                    break;
                case "Mood":
                    if (myvalue != "empty")
                    {
                        ThenTextWithIdIsCorrect("LblMood" + myvalue);
                    }
                    else
                    {
                        if (!Utils.IsVisible("UnselectedIcon")) //old selector mood_00_unselected
                        {
                            Assert.Fail("Mood is not empty!");
                        }
                    }
                    break;
                case "Today I feel":
                    string[] MoodFace = new string[] { "😠", "😖", "😟", "😕", "😐", "😌", "☺", "🙂", "😃" };
                    string[] MoodName = new string[] { "Frustrated", "Nervous", "Sad", "Bored", "IamOK", "Calm", "Optimistic", "Happy", "Encouraged" };
                    for (int i = 0; i < 9; i++)
                    {
                        if (myvalue == MoodName[i])
                        {
                            myvalue = MoodFace[i];
                            i = 10;
                        }
                    }
                    if (!Utils.IsVisible(x => x.Text("Today I feel " + myvalue)))
                    {
                        Assert.Fail("Today I feel text wrong!!");
                    }
                    break;
                case "list":
                    string[] ListShop = Utils.ExecuteBackdoor(Backdoors.GetShoppingListCheckBoxStatus).ToString().Split(',');
                    for (int i = 0; i < 6; i++)
                    {
                        if (ListShop[i] != ItemIsSelected[i].ToString())
                        {
                            Assert.Fail("Item " + i + " has changed state!");
                        }
                    }
                    break;
                case "QR Scanner":
                    if (!Utils.IsVisible("zxingScannerView"))
                    {
                        Assert.Fail("zxingScannerView area not founded!");
                    }
                    break;
                case "Prescription downloaded":
                    //DownloadedPrescriptions
                    if (!Utils.IsVisible(x => x.Text(Utils.GetTranslatedResx("DownloadedPrescriptions") + " " + myvalue)))
                    {
                        Assert.Fail("Numero atteso: " + myvalue);
                    }
                    break;
                case "Video": //video strut adjustment
                    if (!Utils.IsVisible("Orthofix_logo") && myvalue == "visualized")
                    {
                        Assert.Fail("Video player of pin site care page not visualized!");
                    }
                    break;
                case "Frame IDs":
                    if (resetView)
                    {
                        WhenUserTapOnOption("<");
                        WhenUserTapOnOption("Strut Adjust");
                        Utils.Wait(2000);
                    }
                    string FrameNameList = "";

                    if (myvalue == "Qr000" || myvalue == "Qr001")
                    {
                        FrameNameList = "ABCDEFGHI";
                    }
                    if (myvalue == "Qr002" || myvalue == "Qr003")
                    {
                        FrameNameList = "A";
                    }
                    if (FrameSeen == "" && BadgeSeen == "")
                    {
                        getFrameAndCounterList();
                    }
                    if (FrameNameList.Length != FrameSeen.Length)
                    {
                        Assert.Fail("Non sono visualizzati tutti i Frame ID" + FrameSeen);
                    }
                    if (FrameNameList != FrameSeen)
                    {
                        Assert.Fail("Frame Id aren't ordered alphabetically" + FrameSeen);
                    }

                    FrameSeenBackup = FrameSeen;
                    FrameSeen = "";
                    break;
                case "FrameIdInfo":
                    checkDateTimeAndFrameIDNameOfAdjustment();

                    //controllo che non ci sono troppi riquadri
                    if (app.Query("bkg_strut_cell").Length < 18)
                    {
                        Assert.Fail("Troppi o troppo pochi rettangolini in tot ne appaiono:" + app.Query("bkg_strut_cell").Length.ToString()); // 13 e 14 sovrapposti??
                    }
                    app.Screenshot("Numero riquadri corretto");

                    //controllo valori della tabella e immagini esagono
                    for (int i = 1; i <= 6; i++)
                    {
                        if (!Utils.IsVisible("Exagon_strut_" + i.ToString()))
                        {
                            Assert.Fail("Non è visualizzato l'esagono colorato dello strut numero:" + i.ToString());
                        }
                        if (Utils.GetText("LblStr" + i.ToString() + "Length") != "35")
                        {
                            Assert.Fail("Il campo Length dello strut " + i.ToString() + " non è corretto (35)");
                        }
                        if (i != 4)
                        {
                            if (Utils.GetText("LblStr" + i.ToString() + "Click") != "1")
                            {
                                Assert.Fail("Il campo Length dello strut " + i.ToString() + " non è corretto (1)");
                            }
                        }
                        else
                        {
                            if (Utils.GetText("LblStr" + i.ToString() + "Click") != "-1")
                            {
                                Assert.Fail("Il campo Length dello strut " + i.ToString() + " non è corretto (-1)");
                            }
                        }
                    }
                    app.Screenshot("Valori e immagini corretti");

                    ThenOptionIs("start adjustment", "shown");
                    ThenOptionIs("postpone adjustment", "shown");
                    app.Screenshot("Start and Do it later button visualized");

                    break;
                case "Alert icon":
                    Utils.WaitOnAndroid(2000);
                    CustomAssert.IsVisibilityCorrect("hexagon_strutadjalert.png", myvalue, "visualized", "not visualized");
                    break;
                case "pin site care video":
                    if (!Utils.IsVisible("Orthofix_logo") && myvalue == "visualized")
                    {
                        Assert.Fail("Video player of pin site care page not visualized!");
                    }
                    break;
                case "pin site care alert":
                    CustomAssert.IsVisibilityCorrect("hexagon_pinsitealert.png", myvalue, "visualized", "not visualized");
                    break;
                case "Goal Text":
                    if (!Utils.IsVisible(x => x.Text(myvalue)))
                    {
                        Assert.Fail("Goal Text not equals to the one setted previously");
                    }
                    break;
            }
        }

        [When(@"User swipe from '(.*)' to '(.*)'")]
        public void WhenUserSwipeFromTo(string start, string end)
        {
            switch (start)
            {
                case "right":
                    app.SwipeRightToLeft();
                    break;
                case "left":
                    app.SwipeLeftToRight();
                    break;
            }
        }

        [When(@"User '(.*)' profile with")]
        public void WhenUserProfileWith(string p0, Table table)
        {
            AnonymousTable givenAnonymousTable;
            NormalTable givenNormalTable;
            if (isNormal)
            {
                givenNormalTable = table.CreateInstance<NormalTable>();
                GivenUserNavigateToPage("User Profile");
                WhenUserEnterWith("nickname", givenNormalTable.Nickname);
                app.Screenshot("Screen after inserting Nickname");
                WhenUserTapOnOption("Next");
                if (givenNormalTable.TimePinSite != "empty")
                {
                    WhenUserEnterWith("set daily time", givenNormalTable.TimePinSite);
                }
                app.Screenshot("Screen after inserting Time in set daily time field");
                WhenUserTapOnOption("Next");
                GivenOptionIs("Goal", givenNormalTable.GoalStat);
                if (givenNormalTable.GoalStat == "enabled")
                {
                    WhenUserEnterWith("set your personal goal", givenNormalTable.MyText);
                }
                app.Screenshot("Screen after set up personal goal");
                WhenUserTapOnOption("Next");
                GivenOptionIs("Insights", givenNormalTable.InsightsStat);
                if (givenNormalTable.InsightsStat == "enabled")
                {
                    WhenUserEnterWith("set daily time", givenNormalTable.TimeInsigth);
                }
                app.Screenshot("Screen after set up insights messages");
                WhenUserTapOnOption("Done");
            }
            if (isAnonymous)
            {
                givenAnonymousTable = table.CreateInstance<AnonymousTable>();
                GivenUserNavigateToPage("User Profile");
                WhenUserEnterWith("nickname", givenAnonymousTable.Nickname);
                app.Screenshot("Screen after inserting Nickname");
                WhenUserTapOnOption("Next");
                if (givenAnonymousTable.Time != "empty")
                {
                    WhenUserEnterWith("set daily time", givenAnonymousTable.Time);
                }
                app.Screenshot("Screen after inserting Time in set daily time field");
                WhenUserTapOnOption("Done");
            }
        }

        [Then(@"Anonymous Profile settings are successfully saved")]
        public void ThenAnonymousProfileSettingsAreSuccessfullySaved(Table table)
        {
            var givenTable = table.CreateInstance<AnonymousTable>();
            GivenUserNavigateToPage("User Profile");
            ThenTheShouldBe("nickname", givenTable.Nickname);
            app.Screenshot("Screen after checking nickname field correctness");
            WhenUserTapOnOption("Next");
            ThenTheShouldBe("set daily time", givenTable.Time);
            app.Screenshot("Screen after checking Time field correctness");
        }

        [Then(@"Normal Profile setting are succcessfully saved")]
        public void ThenNormalProfileSettingAreSucccessfullySaved(Table table)
        {
            var givenTable = table.CreateInstance<NormalTable>();
            GivenUserNavigateToPage("User Profile");
            ThenTheShouldBe("nickname", givenTable.Nickname);
            app.Screenshot("Screen after checking nickname field correctness");
            WhenUserNavigateToPageFromPage("Pin Site Care", "User Profile");
            ThenTheShouldBe("set daily time", givenTable.TimePinSite);
            app.Screenshot("Screen after checking Time field correctness");
            WhenUserNavigateToPageFromPage("Personal Goal", "User Profile");
            ThenTheShouldBe("Goal", givenTable.GoalStat);
            ThenTheShouldBe("set your personal goal", givenTable.MyText);
            app.Screenshot("Screen after checking Personal Goal fields correctness");
            WhenUserNavigateToPageFromPage("Personal Goal", "User Profile");
            ThenTheShouldBe("Insights", givenTable.InsightsStat);
            ThenTheShouldBe("set daily time", givenTable.TimeInsigth);
            app.Screenshot("Screen after checking Insights fields correctness");
        }

        [When(@"User '(.*)' to Exit")]
        public void WhenUserToExit(string action)
        {
            if (action == "confirm")
            {
                Utils.WaitAndTapElement("Yes", QueryType.Text);
            }
            if (action == "not confirm")
            {
                Utils.WaitAndTapElement("No", QueryType.Text);
            }
        }

        [Given(@"'(.*)' option is '(.*)'")]
        public void GivenOptionIs(string optionName, string Status)
        {
            switch (optionName)
            {
                //l'uso dell'invoke cambiava stato graficamente ma non veramente
                case "Goal":
                case "Insights":
                    Utils.WaitElement(Utils.GetClassName(PlatformClassName.Switch), QueryType.Class);
                    if (Status == "enabled" && !Utils.Switch_IsChecked()) //enable goal ==> switch on
                    {
                        app.Tap(x => x.Class(Utils.GetClassName(PlatformClassName.Switch)));
                    }
                    if (Status == "disabled" && Utils.Switch_IsChecked()) //disable goal ==> switch off
                    {
                        app.Tap(x => x.Class(Utils.GetClassName(PlatformClassName.Switch)));
                    }
                    break;
            }
        }

        [Then(@"'(.*)' option is '(.*)'")]
        public void ThenOptionIs(string element, string status)
        {
            switch (element)
            {
                case "How do you feel today?":
                    //today i feel e how do you feel hanno lo stesso id
                    if (Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitleAllMyDailyTask"))))
                    {
                        if (!Utils.IsVisible("LblDayMood") && status == "shown")
                        {
                            Assert.Fail("How are you today? button not founded!");
                        }
                        if (Utils.IsVisible("LblDayMood") && status == "not shown" && Utils.GetText("LblDayMood") == "How are you today?")
                        {
                            Assert.Fail("How are you today? button founded!");
                        }
                    }
                    else
                    {
                        if (!Utils.IsVisible("LblHowDoYouFeel") && status == "shown")
                        {
                            Assert.Fail("How are you today? button not founded!");
                        }
                        if (Utils.IsVisible("LblHowDoYouFeel") && status == "not shown" && Utils.GetText("LblHowDoYouFeel") == "How are you today?")
                        {
                            Assert.Fail("How are you today? button founded!");
                        }
                    }
                    break;
                case "Today I feel":
                    if (Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitleAllMyDailyTask"))))
                    {
                        if (!Utils.IsVisible("LblDayMood") && status == "shown")
                        {
                            Assert.Fail("Today I feel button not founded!");
                        }
                        if (Utils.IsVisible("LblDayMoood") && status == "not shown" && Utils.GetText("LblDayMood").Contains("Today I feel"))
                        {
                            Assert.Fail("Today I feel button founded!");
                        }
                    }
                    else
                    {
                        if (!Utils.IsVisible("LblHowDoYouFeel") && status == "shown")
                        {
                            Assert.Fail("Today I feel button not founded!");
                        }
                        if (Utils.IsVisible("LblHowDoYouFeel") && status == "not shown" && Utils.GetText("LblHowDoYouFeel").Contains("Today I feel"))
                        {
                            Assert.Fail("Today I feel button founded!");
                        }
                    }
                    CustomAssert.IsVisibilityCorrect("LblDayMood", status, "shown", "not shown");
                    break;
                case "Mood set":
                    CustomAssert.IsVisibilityCorrect("ImgSetMoodEmoticon", status, "shown", "not shown");
                    break;
                case "Cancel":
                    CustomAssert.IsVisibilityCorrect("BtnCancel", status, "shown", "not shown");
                    break;
                case "Confirm this mood":
                    CustomAssert.IsVisibilityCorrect("BtnConfirm", status, "shown", "not shown");
                    break;
                case "Select Mood":
                    CustomAssert.IsVisibilityCorrect(x => x.Class(Utils.GetClassName(PlatformClassName.Slider)), status, "shown", "not shown", "Mood slider");
                    break;
                case "Done Mood":
                    CustomAssert.IsVisibilityCorrect("BtnDone", status, "shown", "not shown");
                    break;
                case "Share":
                    CustomAssert.IsVisibilityCorrect("ico_share", status, "shown", "not shown");
                    break;
                case "<":
                    Utils.OnIOS(() =>
                    {
                        CustomAssert.IsVisibilityCorrect("back", status, "shown", "not shown");
                    });
                    // Useless check it on Android because the button is not present
                    break;
                case "X":
                    CustomAssert.IsVisibilityCorrect("close", status, "shown", "not shown");
                    break;
                case "Close":
                    CustomAssert.IsVisibilityCorrect("BtnClose", status, "shown", "not shown");
                    break;
                case "start adjustment":
                    if (!Utils.IsVisible("BtnStartAdjustment") && status == "shown")
                    {
                        Assert.Fail("Start button not founded!");
                    }
                    break;
                case "postpone adjustment":
                    if (!Utils.IsVisible("BtnDoItLater") && status == "shown")
                    {
                        Assert.Fail("Do it later button not founded!");
                    }
                    break;
                case "Confirm Postpone":
                    //MANCA ID
                    CustomAssert.IsVisibilityCorrect(x => x.Text("OK"), status, "shown", "not shown", "OK button");
                    break;
                case "Not Confirm Postpone":
                    //MANCA ID
                    CustomAssert.IsVisibilityCorrect(x => x.Text("Cancel"), status, "shown", "not shown", "Cancel button");
                    break;
                case "How will I know if I get an infection?":
                    CustomAssert.IsVisibilityCorrect("GrdWhatDoINeed", status, "shown", "not shown");
                    break;
                case "What do I need?":
                    CustomAssert.IsVisibilityCorrect("GrdAmIGetInfection", status, "shown", "not shown");
                    break;
                case "Send a photo":
                    CustomAssert.IsVisibilityCorrect("LblSendAPhoto", status, "shown", "not shown");
                    break;
                case "Next":
                    CustomAssert.IsVisibilityCorrect("LblNext", status, "shown", "not shown");
                    break;
                case "Back":
                    CustomAssert.IsVisibilityCorrect("LblPrev", status, "shown", "not shown");
                    break;
                case "Confirm pin site care":
                    CustomAssert.IsVisibilityCorrect("BtnPinSiteCareDoneButton", status, "shown", "not shown");
                    break;
                case "Cancel pin site care":
                    CustomAssert.IsVisibilityCorrect("BtnMaybeLaterButton", status, "shown", "not shown");
                    break;
            }
        }

        [Given(@"'(.*)' is '(.*)' in '(.*)' section")]
        public void GivenIsInSection(string action, string status, string section)
        {
            /*
             * QUESTO GIVEN CREDO VADA VUOTO
             * perchè l'appartenenza del mood self assessment alle varie sezioni dipende 
             * dalla prescription creata
             * (forse per l'appartenenza alle sezione Done devo fare io)
             */

            if (section == "To do")
            {
                switch (action)
                {
                    case "Mood Self Assessment":
                        break;
                }
            }
            if (section == "Done")
            {
                switch (action)
                {
                    case "Mood Self Assessment":
                        break;
                }
            }
        }

        [Then(@"Text with id '(.*)' is correct")]
        public void ThenTextWithIdIsCorrect(string TextId)
        {
            string TextCulture = Utils.GetTranslatedResx(TextId);
            if (TextId == "LblClickOnEmoticons" && Utils.isAndroid) //this is a bug of tree. Not give visualized label, on Android.
            {
                //Assert.Ignore(); //TODO create a different step
                return;
            }
            if (TextCulture != "")
            {
                Utils.WaitElement(TextCulture, QueryType.Text);
            }
        }

        [Then(@"'(.*)' request '(.*)'")]
        public void ThenRequest(string item, string action)
        {
            switch (item)
            {
                case "legal terms":
                    if (action == "appear")
                    {
                        app.Tap(x => x.Text("Legal Terms Acceptance"));//va inserito l'ID lbltile
                    }
                    if (action == "not appear" && Utils.IsVisible(x => x.Text("Legal Terms Acceptance"))) //va inserito l'ID lbltile
                    {
                        Assert.Fail("Legal Terms should not visualized!");
                    }
                    break;
            }
        }

        [Given(@"User '(.*)' the '(.*)'")]
        public void GivenUserThe(string action, string item)
        {
            switch (item)
            {
                case "legal terms":
                    if (action == "accept")
                    {
                        app.Tap("BtnAcceptLegalTerms");
                    }
                    break;
            }
        }

        [Then(@"Shopping List '(.*)' is '(.*)'")]
        public void ThenShoppingListIs(string Item, string Action)
        {
            switch (Item)
            {
                case "image":
                    if (Action == "visualized" && !Utils.IsVisible("img_pinsitecare"))
                    {
                        Assert.Fail("Shopping List Image not visualized!");
                    }
                    break;
                case "list":
                    if (app.Query("ImgStateImage").Length < 6)
                    {
                        Assert.Fail("There are not enough dot!");
                    }
                    else
                    {
                        for (int i = 1; i < 7; i++)
                        {
                            //Gli Id contenenti le stringhe sono Item1_ShoppingList (ecc con il numero)
                            string TextCulture = Utils.GetTranslatedResx("Item" + i.ToString() + "ShoppingList");
                            if (!Utils.IsVisible(x => x.Text(TextCulture)))
                            {
                                Assert.Fail("Text" + TextCulture + " not founded or wrong!");
                            }
                        }
                    }
                    break;
                case "Frame":
                    Utils.WaitOnAndroid(1000);
                    CustomAssert.IsVisibilityCorrect("img_cleansing", Action, "visualized", "not visualized");
                    break;
            }
        }

        [When(@"User '(.*)' mood with '(.*)'")]
        public void WhenUserMoodWith(string Action, string MoodName)
        {
            switch (Action)
            {
                case "set and save":
                    if (Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitleAllMyDailyTask"))))
                    {
                        Utils.WaitAndTapElement("LblDayMood");
                    }
                    else
                    {
                        Utils.WaitAndTapElement("LblHowDoYouFeel");
                    }
                    app.Screenshot("Screen after tap on How are you today?");
                    WhenUserEnterWith("Mood", MoodName);
                    app.Screenshot("Screen after selection of mood " + MoodName);
                    WhenUserTapOnOption("Confirm this mood");
                    app.Screenshot("Screen after confirmation of mood " + MoodName);
                    WhenUserTapOnOption("<");
                    break;
                case "update and save":
                    if (Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitleAllMyDailyTask"))))
                    {
                        Utils.WaitAndTapElement("LblDayMood");
                    }
                    else
                    {
                        Utils.WaitAndTapElement("LblHowDoYouFeel");
                    }
                    app.Screenshot("Screen after tap on Today I feel");
                    WhenUserEnterWith("Mood", MoodName);
                    app.Screenshot("Screen after selection of mood " + MoodName);
                    WhenUserTapOnOption("Confirm this mood");
                    app.Screenshot("Screen after confirmation of mood " + MoodName);
                    WhenUserTapOnOption("<");
                    break;
                case "update and not save":
                    if (Utils.IsVisible(x => x.Marked("LblTitle").Text(Utils.GetTranslatedResx("LblTitleAllMyDailyTask"))))
                    {
                        Utils.WaitAndTapElement("LblDayMood");
                    }
                    else
                    {
                        Utils.WaitAndTapElement("LblHowDoYouFeel");
                    }
                    app.Screenshot("Screen after tap on Today I feel");
                    WhenUserEnterWith("Mood", MoodName);
                    app.Screenshot("Screen after selection of mood " + MoodName);
                    WhenUserTapOnOption("Cancel Mood");
                    app.Screenshot("Screen after cancellation of mood " + MoodName);
                    WhenUserTapOnOption("<");
                    break;
                case "update":
                    WhenUserEnterWith("Mood", MoodName);
                    app.Screenshot("Screen after selection of mood " + MoodName);
                    WhenUserTapOnOption("Confirm this mood");
                    break;
                case "not update":
                    WhenUserEnterWith("Mood", MoodName);
                    app.Screenshot("Screen after selection of mood " + MoodName);
                    WhenUserTapOnOption("Cancel Mood");
                    break;
            }
        }

        [Given(@"Device time is '(.*)'")]
        public void GivenDeviceTimeIs(string date)
        {
            Utils.ExecuteBackdoor(Backdoors.SetTimeNow_SystemUtility, date);
        }

        [Given(@"All items are '(.*)'")]
        public void GivenAllItemsAre(string Status)
        {
            string[] ListShop = Utils.ExecuteBackdoor(Backdoors.GetShoppingListCheckBoxStatus).ToString().Split(',');
            for (int i = 0; i < 6; i++)
            {
                if (Status == "unselected" && ListShop[i] == "true")
                {
                    app.Tap(x => x.Marked("ImgStateImage").Index(i)); //allora tappo quell'elemento per deselezionarlo
                }
                if (Status == "selected" && ListShop[i] == "false")
                {
                    app.Tap(x => x.Marked("ImgStateImage").Index(i)); //allora tappo quell'elemento per selezionarlo
                }
            }
        }

        [When(@"User '(.*)' all items")]
        public void WhenUserAllItems(string Action)
        {
            //eseguo il tap su tutti gli elementi
            for (int i = 0; i < 6; i++)
            {
                app.Tap(x => x.Marked("ImgStateImage").Index(i));
            }
        }

        [Then(@"All item are '(.*)'")]
        public void ThenAllItemAre(string Status)
        {
            string[] ListShop = Utils.ExecuteBackdoor(Backdoors.GetShoppingListCheckBoxStatus).ToString().Split(',');
            for (int i = 0; i < 6; i++)
            {
                if (Status == "unselected" && ListShop[i] == "true")
                {
                    Assert.Fail("L'elemento " + i + " della lista è selezionato!");
                }
                if (Status == "selected" && ListShop[i] == "false")
                {
                    Assert.Fail("L'elemento " + i + " della lista è deselezionato!");
                }
            }
        }

        [When(@"User scan a '(.*)' QR code")]
        public void WhenUserScanAQRCode(string howIs)
        {//poi clicco read??
            switch (howIs)
            {
                case "valid":
                    Utils.ExecuteBackdoor(Backdoors.SetTestScannedQrCode, new Networking.PrescriptionsMockedIndices().QrCode1);
                    break;
                case "invalid":
                    Utils.ExecuteBackdoor(Backdoors.SetTestScannedQrCode, new Networking.PrescriptionsMockedIndices().QrCodeInvalidSign);
                    break;
                case "random":
                    Utils.ExecuteBackdoor(Backdoors.SetTestScannedQrCode, "QR code non generato da myHEXplan");
                    break;
            }
            Utils.WaitAndTapElement("BtnScanCode");
            //Utils.WaitAndTapElement("OK", QueryType.Text); givePermission removed
            Utils.WaitAndTapElement("Read mocked code", QueryType.Text);
        }

        [Then(@"A '(.*)' message is shown")]
        public void ThenAMessageIsShown(string whatType)
        {
            switch (whatType)
            {
                case "success":
                    Utils.WaitElement(x => x.Text("OK"), "OK button");
                    if (!Utils.IsVisible(x => x.Text(Utils.GetTranslatedResx("LblTitlePrescriptionMessage"))))
                    {
                        Assert.Fail("Prescription scanned message not visualized!");
                    }
                    break;
                case "error QRE01": //collegato a QR code non di myHEXplan
                    Utils.WaitElement("ImgClose");
                    if (!Utils.IsVisible(x => x.Text(Utils.GetTranslatedResx("ErrorMessage01"))))
                    {
                        Assert.Fail("Error Message is not visualized or is Wrong!");
                    }
                    break;
                case "error QRE02": //collegato al QR code invalido
                    Utils.WaitElement("ImgClose");
                    if (!Utils.IsVisible(x => x.Text(Utils.GetTranslatedResx("ErrorMessage02"))))
                    {
                        Assert.Fail("Error Message is not visualized or is Wrong!");
                    }
                    break;
            }
        }

        [Then(@"'(.*)' appears")]
        public void ThenAppears(string p0)
        {
            switch (p0)
            {
                case "Add your photo":
                    if (!Utils.IsVisible(x => x.Text(Utils.GetTranslatedResx("LblSelectPhoto_0"))))
                    {
                        Assert.Fail("Wrong text under photo field!");
                    }
                    break;
                case "Change your photo":
                    if (!Utils.IsVisible(x => x.Text(Utils.GetTranslatedResx("LblSelectPhoto_1"))))
                    {
                        Assert.Fail("Wrong text under photo field!");
                    }
                    break;
                case "Alert icon":
                    if (!Utils.IsVisible("hexagon_strutadjalert.png"))
                    {
                        Assert.Fail("The images necessary for alert are not founded!");
                    }
                    break;
                case "Alert message":
                    app.Screenshot("AlertMessage");
                    app.Tap(x => x.Text("OK"));
                    break;
                case "Adjustment push notification":
                    if (!Utils.IsVisible(x => x.Text("👌 It's time for your Struts Adjustment 11 AM")))
                    {
                        Assert.Fail("Adjustment push notification not appear");
                    }
                    break;
            }
        }

        [When(@"User '(.*)' strut adjustment of '(.*)'")]
        public void WhenUserStrutAdjustmentOf(string action, string frame)
        {
            switch (action)
            {
                case "complete":
                    if (frame == "all Frame ID")
                    {
                        //#2xA, 1x(B,C,D,E,F,G,H,I)
                        WhenUserStrutAdjustmentOf("complete", "frame ID A");
                        WhenUserStrutAdjustmentOf("complete", "frame ID A");
                        WhenUserStrutAdjustmentOf("complete", "frame ID B");
                        WhenUserStrutAdjustmentOf("complete", "frame ID C");
                        WhenUserStrutAdjustmentOf("complete", "frame ID D");
                        WhenUserStrutAdjustmentOf("complete", "frame ID E");
                        WhenUserStrutAdjustmentOf("complete", "frame ID F");
                        WhenUserStrutAdjustmentOf("complete", "frame ID G");
                        WhenUserStrutAdjustmentOf("complete", "frame ID H");
                        WhenUserStrutAdjustmentOf("complete", "frame ID I");
                        resetView = true;
                    }
                    else
                    {
                        WhenUserTapOnFrameID(frame.Split(' ')[2]);
                        app.Screenshot("Tap on " + frame);
                        WhenUserTapOnOption("Start Adjustment");
                        app.Screenshot("Start of adjustment");
                        WhenUserNavigateTo("Strut 6");
                        WhenUserTapOnOption("Done");
                        Utils.WaitAndTapElement("LblClose");
                    }
                    break;
                case "postpone":
                    WhenUserTapOnFrameID(frame.Split(' ')[2]);
                    WhenUserTapOnOption("Do it later");
                    app.Screenshot("Tap on Do it later");
                    WhenUserTapOnOption("Confirm Postpone");
                    break;
                case "not postpone":
                    WhenUserTapOnFrameID(frame.Split(' ')[2]);
                    WhenUserTapOnOption("Do it later");
                    app.Screenshot("Tap on Do it later");
                    WhenUserTapOnOption("Not Confirm Postpone");
                    break;
                case "execute":
                    WhenUserTapOnOption("Start Adjustment");
                    app.Screenshot("Start of adjustment");
                    WhenUserNavigateTo("Strut 6");
                    WhenUserTapOnOption("Done");
                    Utils.WaitAndTapElement("LblClose");
                    break;
            }
        }

        [When(@"User tap on Frame ID '(.*)'")]
        public void WhenUserTapOnFrameID(string nameFrame)
        {
            Utils.Wait(3000);
            while (!Utils.IsVisible(x => x.Marked("LblFrameIdValue").Text(nameFrame))) //todo put a condition to avoid infinite loop
            {
                ScrollUpByOneElement();
            }
            app.Tap(x => x.Marked("LblFrameIdValue").Text(nameFrame));
            FrameIDTapped = nameFrame;
        }

        [Then(@"'(.*)' page visualized correctly")]
        public void ThenPageVisualizedCorrectly(string strut)
        {
            /*  
                #- current strut number is correct (according to the number of the page)
                #- number of click is correct
                #- direction is correct if the click<0 "<<" else ">>")
                #- gradual  length is correct as in the prescription with a image visualized
                #- image of Ring visualized
                #- an option to go next visualized (except for the last strut that have the done option)
                #- an option to go back visualized (except for the first strut that doesn't have the back option)
            */
            checkDateTimeAndFrameIDNameOfAdjustment();
            //controllo direzione frecce
            if (strut == "Strut 4")
            {
                //if (!Utils.IsVisible("direction_minus.png"))
                if (Utils.ExecuteBackdoor(Backdoors.GetSelectedStrutDirectionImage).ToString() != "direction_minus.png")
                {
                    Assert.Fail("Direction Images wrong");
                }
            }
            else
            {
                //if (!Utils.IsVisible("direction_plus.png"))
                if (Utils.ExecuteBackdoor(Backdoors.GetSelectedStrutDirectionImage).ToString() != "direction_plus.png")
                {
                    Assert.Fail("Direction Images wrong");
                }
            }
            app.Screenshot("Rotation images correct");
            //controllo immagine sfondo
            if (Utils.ExecuteBackdoor(Backdoors.GetSelectedStrutBackgroundImageName).ToString() != "bkg_strut_" + strut.Split(' ').Last().ToString())
            {
                Assert.Fail("Strut's image not correct or not visualized");
            }
            app.Screenshot("Background images correct");
            //controllo valore length
            if (Utils.GetText(x => x.Marked("LblStrAdjLengthVal")) != "35")
            {
                Assert.Fail("Length value wrong, should be 35");
            }
            //controllo valore click
            if (strut == "Strut 4")
            {
                if (Utils.GetText(x => x.Marked("LblStrAdjClickVal")) != "-1")
                {
                    Assert.Fail("Length value wrong, should be -1");
                }
            }
            else
            {
                if (Utils.GetText(x => x.Marked("LblStrAdjClickVal")) != "1")
                {
                    Assert.Fail("Length value wrong, should be 1");
                }
            }
            app.Screenshot("Length and Click value correct");
        }

        [Then(@"Option page of '(.*)' work correctly")]
        public void ThenOptionPageOfWorkCorrectly(string what)
        {
            if (what.Contains("Strut"))
            {
                if (what == "Strut 1")
                {
                    WhenUserTapOnOption("Next");
                    if (Utils.ExecuteBackdoor(Backdoors.GetSelectedStrutBackgroundImageName).ToString() != "bkg_strut_2")
                    {
                        Assert.Fail("Not in strut 2 page!");
                    }
                    app.Screenshot("Tappig next option next strut is visualized");
                    WhenUserTapOnOption("Back");
                }
                if (what == "Strut 6")
                {
                    WhenUserTapOnOption("Back");
                    if (Utils.ExecuteBackdoor(Backdoors.GetSelectedStrutBackgroundImageName).ToString() != "bkg_strut_5")
                    {
                        Assert.Fail("Not in strut 5 page!");
                    }
                    app.Screenshot("Tappig back option previous strut is visualized");
                    WhenUserTapOnOption("Next");
                }
                if (what != "Strut 1" && what != "Strut 6")
                {
                    WhenUserTapOnOption("Next");
                    if (Utils.ExecuteBackdoor(Backdoors.GetSelectedStrutBackgroundImageName).ToString() != "bkg_strut_" + (Int32.Parse(what.Split(' ').Last()) + 1).ToString())
                    {
                        Assert.Fail("Not in strut " + (Int32.Parse(what.Split(' ').Last()) + 1).ToString() + " page!");
                    }
                    app.Screenshot("Tappig next option next strut is visualized");
                    WhenUserTapOnOption("Back");
                    WhenUserTapOnOption("Back");
                    if (Utils.ExecuteBackdoor(Backdoors.GetSelectedStrutBackgroundImageName).ToString() != "bkg_strut_" + (Int32.Parse(what.Split(' ').Last()) - 1).ToString())
                    {
                        Assert.Fail("Not in strut " + (Int32.Parse(what.Split(' ').Last()) - 1).ToString() + " page!");
                    }
                    app.Screenshot("Tappig back option previous strut is visualized");
                    WhenUserTapOnOption("Next");
                }
            }
            switch (what)
            {
                case "End Adjustment":
                    Utils.WaitAndTapElement("LblClose");
                    ThenPageIsVisualized("Strut Adjust");
                    break;
            }
        }

        [When(@"User navigate to '(.*)'")]
        public void WhenUserNavigateTo(string what)
        {
            if (what.Contains("Strut"))
            {
                for (int i = 1; i < Int32.Parse(what.Split(' ')[1]); i++)
                {
                    WhenUserTapOnOption("Next");
                }
            }
        }

        [Then(@"Information about '(.*)' are correct")]
        public void ThenInformationAboutAreCorrect(string what)
        {
            switch (what)
            {
                case "End Adjustment":
                    checkDateTimeAndFrameIDNameOfAdjustment();
                    break;
            }
        }

        [Then(@"Counter of Frame ID '(.*)' should be '(.*)'")]
        public void ThenCounterOfFrameIDShouldBe(string frame, string status)
        {
            string AdjustmentList = "";
            if (BadgeSeen == "" && FrameSeen == "")
            {
                getFrameAndCounterList();
            }
            switch (status)
            { //setto i valori dei contatori da controllare
                case "Qr000":
                    AdjustmentList = "000000000";
                    break;
                case "Qr001":
                    AdjustmentList = "211111111";
                    break;
                case "Qr002":
                    AdjustmentList = "1";
                    break;
                case "Qr003":
                    AdjustmentList = "2";
                    break;
                default:
                    AdjustmentList = status;
                    break;
            }
            switch (frame)
            {
                case "all":
                    if (status == "done")
                    {
                        if (Int32.Parse(BadgeSeen) != 0)
                        {
                            Assert.Fail("Frame Id Badge counter is/are wrong because not all are done " + BadgeSeen);
                        }
                    }
                    else
                    {
                        if (AdjustmentList != BadgeSeen)
                        {
                            Assert.Fail("Frame Id Badge counter is/are wrong " + BadgeSeen);
                        }
                    }
                    break;
                default:
                    int indexFrame = 0;
                    if (FrameSeen == "")
                    {
                        for (int i = 0; i < FrameSeenBackup.Length; i++)
                        {
                            if (FrameSeenBackup[i].ToString() == frame)
                            {
                                indexFrame = i;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < FrameSeen.Length; i++)
                        {
                            if (FrameSeen[i].ToString() == frame)
                            {
                                indexFrame = i;
                            }
                        }
                    }
                    if (status == "done")
                    {
                        if (Int32.Parse(BadgeSeen[indexFrame].ToString()) != 0)
                        {
                            Assert.Fail("Frame Id Badge counter is wrong because not done " + BadgeSeen);
                        }
                    }
                    else
                    {
                        if (AdjustmentList != BadgeSeen[indexFrame].ToString())
                            Assert.Fail("Frame Id Badge counter is wrong " + BadgeSeen);
                    }
                    break;
            }
            BadgeSeen = "";
        }

        [Given(@"'(.*)' activity '(.*)'")]
        public void GivenActivity(string which, string status)
        {
            switch (which)
            {
                case "Pin site care":
                    DateTime dt = DateTime.Now;
                    if (status == "to do")
                    {
                        GivenUserNavigateToPage("Pin Site Care");
                        if (dt.Hour == 0 && dt.Minute == 0)
                        {
                            Utils.Wait(60001 - dt.Second); //wait change of minute
                        }
                        WhenUserEnterWith("set daily time", "00:01");

                        app.Screenshot("Daily time setted.");
                        if (isAnonymous)
                        {
                            WhenUserTapOnOption("Done");
                        }
                        if (isNormal)
                        {
                            WhenUserTapOnOption("Next");
                            WhenUserTapOnOption("Goal");
                            WhenUserTapOnOption("Next");
                            WhenUserTapOnOption("Done");
                        }
                    }
                    if (status == "not to do")
                    {
                        GivenUserNavigateToPage("Pin Site Care");
                        if (dt.Hour == 23 && dt.Minute >= 57)
                        {
                            Utils.Wait((2 * 60 * 1000) + ((60 - dt.Second) * 1000)); //wait change of hour
                        }
                        WhenUserEnterWith("set daily time", "23:59");
                        app.Screenshot("Daily time setted.");
                        if (isAnonymous)
                        {
                            WhenUserTapOnOption("Done");
                        }
                        if (isNormal)
                        {
                            WhenUserTapOnOption("Next");
                            WhenUserTapOnOption("Goal");
                            WhenUserTapOnOption("Next");
                            WhenUserTapOnOption("Done");
                        }
                    }
                    break;
            }
        }

        [Then(@"'(.*)' section counter is correct")]
        public void ThenSectionCounterIsCorrect(string sectionName)
        {
            if (ToDoList == "" && DoneList == "")
            {
                GetToDoList();
                GetDoneList();
            }

            if (sectionName == "To do" && ToDoList.Length != Int32.Parse(Utils.GetText(x => x.Marked("LblToDoCount"))))
            {
                Assert.Fail("Visulized rows and section counter are different!");
            }
            if (sectionName == "Done" && DoneList.Length != Int32.Parse(Utils.GetText(x => x.Marked("LblDoneCount"))))
            {
                Assert.Fail("Visulized rows and section counter are different!");
            }
        }

        [Then(@"'(.*)' appears '(.*)' in '(.*)' section")]
        public void ThenAppearsInSection(string activityName, string numberOfTimes, string sectionName)
        {
            string frameIdName = "";
            if (activityName.Contains("Struts Adjustment"))
            {
                frameIdName = activityName.Split(' ')[2];
                activityName = "Struts Adjustment";
            }

            int counter = Int32.Parse(numberOfTimes.Split(' ')[0]);
            if (ToDoList == "" && DoneList == "")
            {
                GetToDoList();
                GetDoneList();
            }
            //To do section
            if (sectionName == "To do")
            {
                switch (activityName)
                {
                    case "Struts Adjustment":
                        for (int i = 0; i < ToDoList.Length; i++)
                        {
                            if (ToDoList[i].ToString() == frameIdName)
                            {
                                counter--;
                            }
                        }
                        if (counter != 0)
                        {
                            Assert.Fail(activityName + frameIdName + " should appear in To do section " + numberOfTimes);
                        }
                        break;
                    case "Mood Self Assestment":
                        for (int i = 0; i < ToDoList.Length; i++)
                        {
                            if (ToDoList[i].ToString() == "M")
                            {
                                counter--;
                            }
                        }
                        if (counter != 0)
                        {
                            Assert.Fail(activityName + " should appear in To do section " + numberOfTimes);
                        }
                        break;
                }
            }
            //Done section
            if (sectionName == "Done")
            {
                switch (activityName)
                {
                    case "Struts Adjustment":
                        for (int i = 0; i < DoneList.Length; i++)
                        {
                            if (DoneList[i].ToString() == frameIdName)
                            {
                                counter--;
                            }
                        }
                        if (counter != 0)
                        {
                            Assert.Fail(activityName + frameIdName + " should appear in Done section " + numberOfTimes);
                        }
                        break;
                    case "Mood Self Assestment":
                        for (int i = 0; i < DoneList.Length; i++)
                        {
                            if (DoneList[i].ToString() == "M")
                            {
                                counter--;
                            }
                        }
                        if (counter != 0)
                        {
                            Assert.Fail(activityName + " should appear in Done section " + numberOfTimes);
                        }
                        break;
                }
            }
        }

        [When(@"User do activities that can be done")]
        public void WhenUserDoActivitiesThatCanBeDone()
        {
            //# mood self assestment
            //parto da All my daily tasks page
            WhenUserMoodWith("set and save", "Happy"); //setto il Mood e torno in All my daily tasks
            WhenUserTapOnOption("<"); //vado in Home Normal

            //# Struts adjustment
            WhenUserTapOnOption("Strut Adjust"); //Strut adjustment page
            //# Time: 01.00 AM FrameID: A Activities Type: Struts Adjustment
            //# Time: 02.00 AM FrameID: A Activities Type: Struts Adjustment
            //# Time: 04.00 AM FrameID: A Activities Type: Struts Adjustment
            //# Time: 08.00 AM FrameID: C Activities Type: Struts Adjustment
            //When User 'complete' strut adjustment of 'Frame ID A'
            WhenUserStrutAdjustmentOf("complete", "Frame ID A");
            WhenUserStrutAdjustmentOf("complete", "Frame ID A");
            WhenUserStrutAdjustmentOf("complete", "Frame ID A");
            WhenUserStrutAdjustmentOf("complete", "Frame ID C");
            //mi ritrovo in strut adjustment page
            WhenUserTapOnOption("<");
            WhenUserTapOnOption("All my daily tasks");
        }

        [When(@"User wait '(.*)' minutes")]
        public void WhenUserWaitMinutes(int p0)
        {
            Utils.Wait((p0 - 1) * 60 * 1000);
            GivenDeviceTimeIs("11:00");
            Utils.Wait(60 * 1000);
            GivenDeviceTimeIs("11:01");
        }

        [When(@"User became normal")]
        public void WhenUserBecameNormal()
        {
            GivenUserIs("normal");
            GivenUserNavigateToPage("Home Normal");
        }

        public void GetToDoList()
        {
            ToDoList = Utils.ExecuteBackdoor(Backdoors.GetToDoList).ToString();
        }

        public void GetDoneList()
        {
            DoneList = Utils.ExecuteBackdoor(Backdoors.GetDoneList).ToString();
        }

        public void checkDateTimeAndFrameIDNameOfAdjustment()
        {
            DateTime dt = DateTime.Now;
            string prescriptionDate;
            //aggiungo numero del giorno
            if (dt.Day < 10)
            {
                prescriptionDate = "0" + dt.Day.ToString() + " ";
            }
            else
            {
                prescriptionDate = dt.Day.ToString() + " ";
            }
            //aggiungo mese con maiuscola iniziale
            prescriptionDate = prescriptionDate + Utils.CultureInfo.DateTimeFormat.GetAbbreviatedMonthName(dt.Month)[0].ToString().ToUpper();
            prescriptionDate = prescriptionDate + Utils.CultureInfo.DateTimeFormat.GetAbbreviatedMonthName(dt.Month)[1].ToString().ToLower();
            prescriptionDate = prescriptionDate + Utils.CultureInfo.DateTimeFormat.GetAbbreviatedMonthName(dt.Month)[2].ToString().ToLower() + " - ";
            //controllo data e ora adjustment corretti
            if (!Utils.IsVisible(x => x.Text(prescriptionDate + "11 AM")) && !Utils.IsVisible(x => x.Text(prescriptionDate + "01 AM")))
            {
                Assert.Fail("La data e l'ora dell'adjustment deve essere:" + prescriptionDate + "11 AM oppure 01 AM");
            }
            //controllo del nome del frame ID aperto
            app.Screenshot("Data e ora dell'adjustment corrette");

            if (Utils.GetText("LblFrameId").Split(' ').Last() != FrameIDTapped)
            {
                Assert.Fail("Il frame ID tappato è:" + FrameIDTapped);
            }
            app.Screenshot("Nome del Frame ID associato corretto");
        }

        public void ScrollUpByOneElement()
        {
            Utils.OnIOS(() =>
            {
                int numRow = app.Query(x => x.Class(Utils.GetClassName(PlatformClassName.ListView_Row))).Length;
                var xCoord = app.Query(x => x.Class(Utils.GetClassName(PlatformClassName.ListView_Row)).Index(numRow - 1))[0].Rect.CenterX;
                var lastRow = app.Query(x => x.Class(Utils.GetClassName(PlatformClassName.ListView_Row)).Index(numRow - 1))[0].Rect.CenterY;
                var upLastRow = app.Query(x => x.Class(Utils.GetClassName(PlatformClassName.ListView_Row)).Index(numRow - 2))[0].Rect.CenterY;
                float temp;

                int i = 0;
                do
                {
                    temp = app.Query(x => x.Class(Utils.GetClassName(PlatformClassName.ListView_Row)).Index(numRow - 1))[0].Rect.CenterY;
                    app.DragCoordinates(xCoord, lastRow, xCoord, lastRow - 25);
                    i++;
                }
                while (app.Query(x => x.Class(Utils.GetClassName(PlatformClassName.ListView_Row)).Index(numRow - 1))[0].Rect.CenterY > upLastRow &&
                       app.Query(x => x.Class(Utils.GetClassName(PlatformClassName.ListView_Row)).Index(numRow - 1))[0].Rect.CenterY < temp && i < 6);
            });
            Utils.OnAndroid(() =>
            {
                var rect = app.Query(x => x.Class(Utils.GetClassName(PlatformClassName.ListView_Row)))[1].Rect;
                app.DragCoordinates(rect.X, rect.Y + rect.Height, rect.X, rect.Y);
            });
        }

        public void PassByQRScan()
        {
            Utils.WaitAndTapElement("BtnScanCode");
            Utils.WaitAndTapElement("Read mocked code", QueryType.Text);
            Utils.WaitAndTapElement("OK", QueryType.Text);
        }

        public void getFrameAndCounterList()
        {
            FrameSeen = ""; //todo replace with backdoor
            BadgeSeen = "";
            int nFrameVisible = app.Query("LblFrameIdValue").Length; //Numero lettere visibili
            int nBadgeVisible = app.Query("LblBadgeCount").Length; //Numero contatori visibili (i done non si vedono)
            int countDone;

            for (int i = 0; i < nFrameVisible; i++)
            {
                FrameSeen += app.Query("LblFrameIdValue")[i].Text;
                countDone = 0;
                for (int j = 0; j < nBadgeVisible; j++)
                {
                    //this check is strange. Doens't work on android without considering a small pixel error margin
                    if (app.Query("LblFrameIdValue")[i].Rect.Y <= app.Query("LblBadgeCount")[j].Rect.CenterY &&
                        app.Query("LblFrameIdValue")[i].Rect.Y + app.Query("LblFrameIdValue")[i].Rect.Height >=
                        app.Query("LblBadgeCount")[j].Rect.CenterY)
                    {
                        BadgeSeen += app.Query("LblBadgeCount")[j].Text;
                        countDone++;
                    }
                }
                if (countDone == 0)
                {
                    BadgeSeen += "0";
                }
            }
            if (nFrameVisible > 1)
            {
                ScrollUpByOneElement();
                while (app.Query("LblFrameIdValue")[nFrameVisible - 1].Text != FrameSeen[FrameSeen.Length - 1].ToString())
                {
                    nBadgeVisible = app.Query("LblBadgeCount").Length;
                    FrameSeen += app.Query("LblFrameIdValue")[nFrameVisible - 1].Text;
                    countDone = 0;
                    for (int j = 0; j < nBadgeVisible; j++)
                    {
                        if (app.Query("LblFrameIdValue")[nFrameVisible - 1].Rect.Y <= app.Query("LblBadgeCount")[j].Rect.CenterY &&
                            app.Query("LblFrameIdValue")[nFrameVisible - 1].Rect.Y + app.Query("LblFrameIdValue")[nFrameVisible - 1].Rect.Height >=
                            app.Query("LblBadgeCount")[j].Rect.CenterY)
                        {
                            BadgeSeen += app.Query("LblBadgeCount")[j].Text;
                            countDone++;
                        }
                    }
                    if (countDone == 0)
                    {
                        BadgeSeen += "0";
                    }
                    ScrollUpByOneElement();
                }
            }
        }
    }
}
