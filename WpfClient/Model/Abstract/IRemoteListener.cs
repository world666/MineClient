namespace WpfClient.Model.Abstract
{
    public interface IRemoteListener
    {
        void InitServer(string port);
        void SetDataInserter(IDataInserter dataInserter);
        void RemoveDataInserter(IDataInserter dataInserter);
    }
}
