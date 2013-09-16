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
            clients_string = new List<string>();
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
        public void SendToClient(string text, int clientIndex)
        {
            if (clients[clientIndex] != null)
            {
                // Подготовка и запуск асинхронной отправки сообщения.
                NetworkStream ns = clients[clientIndex].GetStream();
                byte[] myReadBuffer = Encoding.Default.GetBytes(text);
                ns.BeginWrite(myReadBuffer, 0, myReadBuffer.Length,
                                                             new AsyncCallback(AsyncSendCompleted), ns);
            }
        }

        /// <summary>
        /// Вернуть полученную строку по номеру клиента
        /// </summary>
        public string GetClientString(int clientIndex)
        {
            return clients_string[clientIndex];
        }
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
                    clients.Add(_server.AcceptTcpClient());
                    clients_string.Add("");
                    Thread readThread = new Thread(ReceiveRun);
                    readThread.IsBackground = true;
                    readThread.Start(clients.Count - 1);
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
        private void ReceiveRun(object num)
        {
            while (true)
            {
                try
                {
                    string s = null;
                    clients_string[(int)num] = "";
                    NetworkStream ns = clients[(int)num].GetStream();
                    // Раскомментировав строчку ниже, тем самым уменьшив размер приемного буфера, можно убедиться,
                    // что прием данных будет все равно осуществляться полностью.
                    while (ns.DataAvailable == true)
                    {
                        // Определить точный размер буфера приема позволяет свойство класса TcpClient - Available
                        byte[] buffer = new byte[clients[(int)num].Available];

                        ns.Read(buffer, 0, buffer.Length);
                        s += Encoding.Default.GetString(buffer);
                    }
                    if (s != null)
                    {

                        if (s.IndexOf("+++") == 0) //close connection
                        {
                            clients[(int)num].Close();
                            clients.RemoveAt((int)num);
                            break;
                        }
                        if (ReceiveEvent != null)
                        {
                            ReceiveEvent(s,(int)num);
                        }
                        // Вынужденная строчка для экономия ресурсов процессора.
                        // Неизящный способ.
                        Thread.Sleep(100);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
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
        //сообщения клиентов
        private List<string> clients_string;
    }
}
