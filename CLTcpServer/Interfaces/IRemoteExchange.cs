using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLTcpServer.Interfaces
{
    public delegate void ReceiveHandler(string msg, int clientNum);
    public interface IRemoteExchange
    {
        //server initialization
        bool StartServer(string strPort);
        //close all connection
        bool StopServer();
        //send data to client with index = clientIndex
        void SendToClient(string text, int clientIndex);
        //get receive String with index = clientIndex
        string GetClientString(int clientIndex);

        event ReceiveHandler ReceiveEvent;
        int CountClient { get; }
    }
}
