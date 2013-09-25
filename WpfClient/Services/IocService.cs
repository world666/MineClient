using CLTcpServer;
using CLTcpServer.Interfaces;
using Mc.Settings.Model.Abstract;
using WpfClient.Model;
using WpfClient.Model.Abstract;
using WpfClient.Model.Concrete;
using WpfClient.ViewModel;

namespace WpfClient.Services
{
    public class IocService 
    {
        public static void SetBindings()
        {
            IoC.RegisterType<IMsgParser, MsgParser>();
            IoC.RegisterType<IRemoteExchange, TcpServer>();
            IoC.RegisterSingleton<MainVm, MainVm>();
            IoC.RegisterInstance<IRemoteListener, TcpRemoteListener>();
            IoC.RegisterInstance<IDataInserter, DataBaseInserter>();
            IoC.RegisterSingleton<DateTimeVm, DateTimeVm>();
        }
    }
}
