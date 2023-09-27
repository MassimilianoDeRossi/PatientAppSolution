using Autofac;
using PatientApp.Droid.Services;
using PatientApp.Interfaces;
using PatientApp.ApplicationObjects;
using PatientApp.Networking;
using PatientApp.Utilities;

namespace PatientApp.Droid
{
    /// <summary>
    /// 
    /// </summary>
    public class Setup : AppSetup
    {     

        protected override void RegisterDependencies(ContainerBuilder cb)
        {            
            base.RegisterDependencies(cb);
            
            cb.RegisterType<DroidHelloFormsService>().As<IHelloFormsService>();

#if ENABLE_TEST_CLOUD
            cb.RegisterType<ApiClientFake>().As<IApiClient>();
            cb.RegisterType<SystemUtilityFake>().As<ISystemUtility>();
#else
            cb.RegisterType<ApiClient>().As<IApiClient>();
            cb.RegisterType<SystemUtility>().As<ISystemUtility>();
#endif

        }
    }
}