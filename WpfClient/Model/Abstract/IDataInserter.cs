using DataRepository.Models;

namespace WpfClient.Model.Abstract
{
    public interface IDataInserter
    {
        FanLog InsertData(string data);
    }
}
