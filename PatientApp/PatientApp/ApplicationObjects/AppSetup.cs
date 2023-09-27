using System;
using Autofac;
using PatientApp.Interfaces;
using PatientApp.Services;
using PatientApp.ViewModels;
using Xamarin.Forms;
using PatientApp.Networking;

namespace PatientApp.ApplicationObjects
{
  public class AppSetup
  {

    public IContainer CreateContainer()
    {
      var containerBuilder = new ContainerBuilder();
     // var appSettings = GetApplicationSettings();
     // RegisterPlatformSpecificObjects(containerBuilder, appSettings);
      RegisterDependencies(containerBuilder);
      return containerBuilder.Build();
    }


    //public IAppSettings AppSettings { get { return _appSettings; } }


    protected virtual void RegisterDependencies(ContainerBuilder cb)
    {
        // Register Services
        cb.RegisterType<LocalDatabaseService>().As<ILocalDatabaseService>();
        ViewModelLocator.RegisterViewModels(cb);
    }


    //private static IAppSettings GetApplicationSettings()
    //{
    //  var platformSpecificSettings = DependencyService.Get<IAppSettings>();
    //  if (platformSpecificSettings == null)
    //  {
    //    throw new InvalidOperationException($"Missing '{typeof(IAppSettings).FullName}' implementation! Implementation is required.");
    //  }
    //  return platformSpecificSettings;
    //}

    //private static void RegisterPlatformSpecificObjects(ContainerBuilder containerBuilder)
    //{
    //  containerBuilder.RegisterInstance(applicationSettings).AsImplementedInterfaces().SingleInstance();

    //  var sqlLiteConnection = applicationSettings.CreateSqLiteConnection();
    //  containerBuilder.RegisterInstance(sqlLiteConnection).AsSelf().SingleInstance();
    //}
  }
}