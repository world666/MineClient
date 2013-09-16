using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mc.HTTPServer
{
    public delegate void RemoteControlHandler(int fanObjectNum,string parameter);
    public class WebServer
    {
        TcpListener Listener; // Объект, принимающий TCP-клиентов
        private RemoteControlHandler _remoteControl;
        // Запуск сервера
        public WebServer(int Port,RemoteControlHandler remoteControl)
        {
            _remoteControl = remoteControl;
            Listener = new TcpListener(IPAddress.Any, Port); // Создаем "слушателя" для указанного порта
            Listener.Start(); // Запускаем его
            Task.Run(() => AcceptClients());

        }

        private void AcceptClients()
        {
            // В бесконечном цикле
            while (true)
            {
                // Принимаем нового клиента
                TcpClient Client = Listener.AcceptTcpClient();
                // Создаем поток
                Thread Thread = new Thread(new ParameterizedThreadStart(ClientThread));
                // И запускаем этот поток, передавая ему принятого клиента
                Thread.Start(Client);
            }
        }

        void ClientThread(Object StateInfo)
        {
            // Просто создаем новый экземпляр класса Client и передаем ему приведенный к классу TcpClient объект StateInfo
            new Client((TcpClient)StateInfo,_remoteControl);
        }

        // Остановка сервера
        ~WebServer()
        {
            // Если "слушатель" был создан
            if (Listener != null)
            {
                // Остановим его
                Listener.Stop();
            }
        }
    }
}
