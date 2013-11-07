using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace CLTcpServer
{
    public class TcpServer : Interfaces.IRemoteExchange
    {

        public event Interfaces.ReceiveHandler ReceiveEvent;
        // Счетчик подключенных клиентов
        public int CountClient
        {
            get { return clients.Count; }
        }

        private Thread _acceptThread;
        // Запуск сервера и вспомогательного потока акцептирования клиентских подключений
        // т.е. назначения сокетов ответственных за обмен сообщениями 
        // с соответствующим клиентским приложением
        public TcpServer()
        {
            clients = new List<TcpClient>();
        }


        public bool StartServer(string strPort)
        {
            // Предотвратим повторный запуск сервера
            if (_server == null)
            {
                // Блок перехвата исключений на случай запуска одновременно
                // двух серверных приложений с одинаковым портом.
                try
                {
                    _stopNetwork = false;
                    int port = int.Parse(strPort);
                    _server = new TcpListener(IPAddress.Any, port);
                    _server.Start();


                    _acceptThread = new Thread(AcceptClients);
                    _acceptThread.IsBackground = true;
                    _acceptThread.Start();

                    // оповещение, что сервер запущен
                    return true;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            return true;
        }


        // Принудительная остановка сервера и запущенных потоков.
        public bool StopServer()
        {
            if (_server != null)
            {
                _server.Stop();
                _server = null;
                _stopNetwork = true;

                foreach (TcpClient client in clients)
                {
                    if (client != null) client.Close();
                }

                // оповещаем, что сервер остановлен.
                return true;
            }
            return false;
        }


        /// <summary>
        /// Отправка сообщений клиентам
        /// </summary>
        /// <param name="text">текст сообщения</param>
        /// <param name="skipindex">индекс клиента которому не посылается сообщение</param>
        public void SendToClient(string text, TcpClient client)
        {
            if (client != null)
            {
                // Подготовка и запуск асинхронной отправки сообщения.
                NetworkStream ns = client.GetStream();
                byte[] myReadBuffer = Encoding.Default.GetBytes(text);
                ns.BeginWrite(myReadBuffer, 0, myReadBuffer.Length,
                                                             new AsyncCallback(AsyncSendCompleted), ns);
            }
        }

        /// <summary>
        /// Вернуть полученную строку по номеру клиента
        /// </summary>
        // Принимаем запросы клиентов на подключение и
        // привязываем к каждому подключившемуся клиенту 
        // сокет (в данном случае объект класса TcpClient)
        // для обменом сообщений.
        private void AcceptClients()
        {
            while (true)
            {
                try
                {
                    /*while (_server.Pending() == false)
                    {
                        Thread.Sleep(1);
                    }//this is the part with the lag*/
                    TcpClient client = _server.AcceptTcpClient();
                    clients.Add(client);
                    Thread readThread = new Thread(ReceiveRun);
                    readThread.IsBackground = true;
                    readThread.Start(client);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }


        // Асинхронная отправка сообщения клиенту.
        private void AsyncSendCompleted(IAsyncResult ar)
        {
            NetworkStream ns = (NetworkStream)ar.AsyncState;
            ns.EndWrite(ar);
        }


        // Извлечение сообщения от клиента
        private void ReceiveRun(object _client)
        {
            TcpClient client = _client as TcpClient;
            bool closeConnection = false;
            int repeatNum = 20;
            string s = null;
            while (true)
            {
                try
                {  
                    int i = 0;
                    while (i<2&&!closeConnection)
                    {
                        NetworkStream ns = client.GetStream();
                        // Раскомментировав строчку ниже, тем самым уменьшив размер приемного буфера, можно убедиться,
                        // что прием данных будет все равно осуществляться полностью.
                        while (ns.DataAvailable == true)
                        {
                            // Определить точный размер буфера приема позволяет свойство класса TcpClient - Available
                            byte[] buffer = new byte[client.Available];

                            ns.Read(buffer, 0, buffer.Length);
                            s += Encoding.Default.GetString(buffer);
                            if(i==0)
                              Thread.Sleep(1800);
                        }
                        i++;
                        
                    }
                    if (s != null && !closeConnection)
                    {

                        if (s.IndexOf("+++") == 0 || s.IndexOf("SISC") >= 0) //close connection
                        {
                            client.Close();
                            clients.Remove(client);
                            break;
                        }
                        if (ReceiveEvent != null && s.IndexOf("SISC")<0)
                        {
                            ReceiveEvent(s, client);
                            closeConnection = true;
                        }
                        // Вынужденная строчка для экономия ресурсов процессора.
                        // Неизящный способ.
                    }
                    else if (closeConnection)
                    {
                        client.Close();
                        clients.Remove(client);
                        break;
                    }
                    Thread.Sleep(500);
                    repeatNum--;
                    if (repeatNum == 0)
                        closeConnection = true;
                }
                catch (Exception ex)
                {
                    client.Close();
                    if (clients.Contains(client))
                        clients.Remove(client);
                    break;
                }


                if (_stopNetwork == true) break;

            }
        }

        // Высокоуровневая надстройка для прослушивающего сокета
        private TcpListener _server;
        // Высокоуровневая надстройка для сокетов обмена сообщений с 
        // клиентскими приложениями.
        private List<TcpClient> clients;
        // Флаг мягкой остановки циклов и дополнительных потоков
        private bool _stopNetwork;
    }
}
