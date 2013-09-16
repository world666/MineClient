using System;
using System.Text;
using System.Linq;
using CLTcpServer.Interfaces;
using WpfClient.Model.Abstract;
using WpfClient.Services;

namespace WpfClient.Model.Concrete
{
    public class TcpRemoteListener : IRemoteListener
    {
        private readonly IRemoteExchange _remoteExchange;

        public TcpRemoteListener(IRemoteExchange remoteExchange, IDataInserter dataInserter) {
            _remoteExchange = remoteExchange;
            _remoteExchange.ReceiveEvent += (msg, num) => RemoteService.onRecieve(msg);//for signal quality
            _remoteExchange.ReceiveEvent += AnswerToClient; //remote control
            SetDataInserter(dataInserter);
        }
        private void AnswerToClient(string msg, int numClient)
        {
            byte[] buffer = { 0x52, 0x56, 0x20, 0x00, 0x0D };
            var parameters = IoC.Resolve<IMsgParser>().Parse(msg);
            buffer[3] =
                (byte) RemouteFanControlService.DataForSending.FirstOrDefault(f => f.FanNum == parameters["fn"]).Data;
            buffer[3] += 0x30;
            string send = Encoding.Default.GetString(buffer);
            _remoteExchange.SendToClient(send,numClient);
            RemouteFanControlService.DataForSending.FirstOrDefault(f => f.FanNum == parameters["fn"]).Data = RemouteFanState.Init;
        }
        public void InitServer(string port)
        {
            _remoteExchange.StartServer(port);
        }

        public void SetDataInserter(IDataInserter dataInserter)
        {
            if (dataInserter == null) throw new ArgumentNullException("dataInserter");
            _remoteExchange.ReceiveEvent += (msg,num) => dataInserter.InsertData(msg);
        }

        public void RemoveDataInserter(IDataInserter dataInserter)
        {
            if (dataInserter == null) throw new ArgumentNullException("dataInserter");
            //_remoteExchange.ReceiveEvent -= dataInserter.InsertData;
        }
    }
}
