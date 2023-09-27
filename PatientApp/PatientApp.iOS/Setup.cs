using Autofac;
using PatientApp.Interfaces;
using IoCDemo.iOS;
using PatientApp.ApplicationObjects;
using PatientApp.Networking;
using PatientApp.Utilities;

namespace PatientApp.iOS
{
    public class Setup : AppSetup
    {
        protected override void RegisterDependencies(ContainerBuilder cb)
        {
            base.RegisterDependencies(cb);
            
            cb.RegisterType<IOSHelloFormsService>().As<IHelloFormsService>();

#if ENABLE_TEST_CLOUD
            cb.RegisterType<ApiClientFake>().As<IApiClient>();
            cb.RegisterType<SystemUtilityFake>().As<ISystemUtility>();
#else
            cb.RegisterType<ApiClient>().As<IApiClient>();

            //if (System.Diagnostics.Debugger.IsAttached)
            //    cb.RegisterType<DebugSystemUtility>().As<ISystemUtility>();
            //else
                cb.RegisterType<SystemUtility>().As<ISystemUtility>();
#endif
        }
    }
}