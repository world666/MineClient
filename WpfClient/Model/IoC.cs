using Ninject;
using Ninject.Parameters;

namespace WpfClient.Model
{
    public class IoC
    {
        private static readonly IKernel _kernel = new StandardKernel();

        public static void RegisterType<TInterface, TClass>() where TClass : TInterface
        {
            _kernel.Bind<TInterface>().To<TClass>();
        }

        public static void RegisterSingleton<TInterface, TClass>() where TClass : TInterface
        {
            _kernel.Bind<TInterface>().To<TClass>().InSingletonScope();
        }

        public static void RegisterInstance<TInterface, TClass> () where TClass : TInterface
        {
            _kernel.Bind<TInterface>().To<TClass>();
        }

        public static T Resolve<T>(params IParameter[] parameters) {
            return _kernel.Get<T>(parameters);
        }
    } 
}
