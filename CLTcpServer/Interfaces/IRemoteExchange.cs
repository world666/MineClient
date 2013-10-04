using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace CLTcpServer.Interfaces
{
    public delegate void ReceiveHandler(string msg, TcpClient client);
    public interface IRemoteExchange
    {
        //server initialization
        bool StartServer(string strPort);
        //close all connection
        bool StopServer();
        //send data to client with index = clientIndex
        void SendToClient(string text, TcpClient client);

        event ReceiveHandler ReceiveEvent;
        int CountClient { get; }
    }
}
