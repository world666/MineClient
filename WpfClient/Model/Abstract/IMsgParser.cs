using System.Collections.Generic;
using WpfClient.Services;

namespace WpfClient.Model.Abstract
{
    public interface IMsgParser
    {
        Dictionary<string, int> Parse(string message);
    }
}
