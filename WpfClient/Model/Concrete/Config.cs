using WpfClient.Model.Abstract;
using WpfClient.Services;

namespace WpfClient.Model.Concrete
{
    public class Config
    {
        private static readonly IConfig _config = IoC.Resolve<IConfig>();

        public static IConfig Instance
        {
            get { return _config; }
        }
    }
}
