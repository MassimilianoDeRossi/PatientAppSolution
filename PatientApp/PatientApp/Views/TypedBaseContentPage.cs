using Autofac;
using PatientApp.ApplicationObjects;
using PatientApp.Interfaces;
using Xamarin.Forms;

namespace PatientApp.Views
{
    //public class BaseContentPage : ContentPage
    //{
    //    public BaseContentPage()
    //    {
    //    }

    //    protected override void OnAppearing()
    //    {
    //        base.OnAppearing();
    //    }
    //}

    public class TypedBaseContentPage<T> : ContentPage where T : IViewModel
    {
        readonly T _viewModel;
        readonly IAppSettings _appSettings;
        public T ViewModel { get { return _viewModel; } }

        public IAppSettings AppSettings {get {return _appSettings;} }


        public TypedBaseContentPage()
        {
            using (var scope = AppContainer.Container.BeginLifetimeScope())
            {
                _viewModel = AppContainer.Container.Resolve<T>();
                _appSettings = AppContainer.Container.Resolve<IAppSettings>();
            }
            BindingContext = _viewModel;
        }
    }
}
