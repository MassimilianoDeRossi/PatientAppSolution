using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;

namespace PatientApp.ViewModels
{
    /// <summary>
    /// Class used to bind singleton viewmodel the pages
    /// </summary>
    public class ViewModelLocator
    {
        public AllMyDailyTasksViewModel AllMyDailyTasks => ServiceLocator.Current.GetInstance<AllMyDailyTasksViewModel>();
        public HomeViewModel Home => ServiceLocator.Current.GetInstance<HomeViewModel>();
        public SettingsViewModel Settings => ServiceLocator.Current.GetInstance<SettingsViewModel>();
        public ProfileViewModel Profile => ServiceLocator.Current.GetInstance<ProfileViewModel>();
        public WizardUserSettingsViewModel WizardUserSettings => ServiceLocator.Current.GetInstance<WizardUserSettingsViewModel>();
        public PrescriptionViewModel Prescription => ServiceLocator.Current.GetInstance<PrescriptionViewModel>();
        public PinSiteCareViewModel PinSiteCare => ServiceLocator.Current.GetInstance<PinSiteCareViewModel>();
        public StrutAdjustmentViewModel StrutAdjustment => ServiceLocator.Current.GetInstance<StrutAdjustmentViewModel>();
        public MyPrescriptionsViewModel MyPrescriptions => ServiceLocator.Current.GetInstance<MyPrescriptionsViewModel>();
        public HowDoYouFeelViewModel HowDoYouFeel => ServiceLocator.Current.GetInstance<HowDoYouFeelViewModel>();
        public PersonalGoalViewModel PersonalGoal => ServiceLocator.Current.GetInstance<PersonalGoalViewModel>();
        public MyDiaryViewModel MyDiary => ServiceLocator.Current.GetInstance<MyDiaryViewModel>();
        public ShoppingListViewModel ShoppingList => ServiceLocator.Current.GetInstance<ShoppingListViewModel>();
        public MotivationalMessageViewModel MotivationalMessage => ServiceLocator.Current.GetInstance<MotivationalMessageViewModel>();
        public TestViewModel Test => ServiceLocator.Current.GetInstance<TestViewModel>();
        public TimeLapseViewModel TimeLapse => ServiceLocator.Current.GetInstance<TimeLapseViewModel>();

        public ViewModelLocator()
        {

        }

        public static void RegisterServices(IContainer container)
        {
            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
        }

        public static void RegisterViewModels(ContainerBuilder cb)
        {
            cb.RegisterType<HomeViewModel>().SingleInstance();
            cb.RegisterType<SettingsViewModel>().SingleInstance();
            cb.RegisterType<ProfileViewModel>().SingleInstance();
            cb.RegisterType<WizardUserSettingsViewModel>().SingleInstance();
            cb.RegisterType<PrescriptionViewModel>().SingleInstance();
            cb.RegisterType<PinSiteCareViewModel>().SingleInstance();
            cb.RegisterType<StrutAdjustmentViewModel>().SingleInstance();
            cb.RegisterType<AllMyDailyTasksViewModel>().SingleInstance();
            cb.RegisterType<HowDoYouFeelViewModel>().SingleInstance();
            cb.RegisterType<PersonalGoalViewModel>().SingleInstance();
            cb.RegisterType<MyDiaryViewModel>().SingleInstance();
            cb.RegisterType<ShoppingListViewModel>().SingleInstance();
            cb.RegisterType<MotivationalMessageViewModel>().SingleInstance();
            cb.RegisterType<MyPrescriptionsViewModel>().SingleInstance();
            cb.RegisterType<TestViewModel>().SingleInstance();
            cb.RegisterType<TimeLapseViewModel>().SingleInstance();
        }
    }
}
