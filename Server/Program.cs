using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Listener();
        }

       static void Listener()
        {
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

            // Создаем сокет Tcp/Ip
            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Назначаем сокет локальной конечной точке и слушаем входящие сокеты
            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);

                // Начинаем слушать соединения
                while (true)
                {
                    Console.WriteLine("Ожидаем соединение через порт {0}", ipEndPoint);

                    // Программа приостанавливается, ожидая входящее соединение
                    Socket handler = sListener.Accept();
                    // Мы дождались клиента, пытающегося с нами соединиться
                    byte[] bytes = new byte[256];
                    handler.Receive(bytes);
                    Int64 amount = 0;
                    bool IN = false;
                    for (int i = 0; i <=255; i++)
                    {
                        if (bytes[i] == 11)
                        {
                            break;
                        }
                        if (IN)
                        {
                            amount += bytes[i];
                        }
                        if (bytes[i] == 10)
                        {
                            IN = true;
                        } 
                    }
                    // Отправляем ответ клиенту
                    string reply =  amount + "";
                    Console.WriteLine("Cумма :" +amount.ToString());
                    byte[] msg = Encoding.UTF8.GetBytes(reply);
                    handler.Send(msg);

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}
