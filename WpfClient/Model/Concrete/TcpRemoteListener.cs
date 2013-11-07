using System;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using CLTcpServer.Interfaces;
using DataRepository.Models;
using WpfClient.Model.Abstract;
using WpfClient.Services;
using WpfClient.ViewModel;

namespace WpfClient.Model.Concrete
{
    public class TcpRemoteListener : IRemoteListener
    {
        private readonly IRemoteExchange _remoteExchange;

        public TcpRemoteListener(IRemoteExchange remoteExchange, IDataInserter dataInserter) {
            _remoteExchange = remoteExchange;
            _remoteExchange.ReceiveEvent += AnswerToClient; //remote control
            _remoteExchange.ReceiveEvent += (msg, num) => RemoteService.onRecieve(msg);//for signal quality       
            SetDataInserter(dataInserter);      
        }
        private void AnswerToClient(string msg, TcpClient client)
        {
            byte[] buffer = { 0x52, 0x56, 0x20, 0x00, 0x0D };
            var parameters = IoC.Resolve<IMsgParser>().Parse(msg);
            buffer[3] =
                (byte) RemouteFanControlService.GetData(parameters["fn"]);
            buffer[3] += 0x30;
            string send = Encoding.Default.GetString(buffer);
            _remoteExchange.SendToClient(send,client);
        }
        public void InitServer(string port)
        {
            _remoteExchange.StartServer(port);
        }

        public void SetDataInserter(IDataInserter dataInserter)
        {
            if (dataInserter == null) throw new ArgumentNullException("dataInserter");
            _remoteExchange.ReceiveEvent += (msg, num) =>
                {
                    FanLog fanLog = dataInserter.InsertData(msg);//insert in DB
                    var update = IoC.Resolve<MainVm>().CurrentView as IUpDatable;//update views if possible
                    if(update!=null)
                        update.Update(fanLog);  
                    //HTTPService.UpdateData(fanLog);//update index file for web server
                };
        }

        public void RemoveDataInserter(IDataInserter dataInserter)
        {
            if (dataInserter == null) throw new ArgumentNullException("dataInserter");
            //_remoteExchange.ReceiveEvent -= dataInserter.InsertData;
        }
    }
}
