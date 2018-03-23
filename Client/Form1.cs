using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
namespace Client
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            timer.Tick += Timer_Tick;
        }
        byte[] send()
        {
            Random random = new Random();
            Int32 trashByteStart = Convert.ToInt32(textBox1.Text);
            Int32 trashByteEnd = Convert.ToInt32(textBox2.Text);
            if ((trashByteStart) < 0 || trashByteEnd < 0)
            {
                MessageBox.Show("error ");
                trashByteEnd = 0;
                trashByteStart = 0;
            }
            string dataString;
            StringBuilder stringBuilder = new StringBuilder();
           

            
           stringBuilder.Append('A', 1);
            stringBuilder.Append(' ', 1);
            foreach(char ch in textBox3.Text)
            {
                stringBuilder.Append(ch, 1);
            }
            stringBuilder.Append(' ', 1);
            stringBuilder.Append('B',1);
           
            dataString = stringBuilder.ToString();

            byte[] dataByte = new byte[256];
            byte[] dataTrashStart = new byte[trashByteStart];
            byte[] dataTrashEnd = new byte[trashByteEnd];

            for (int i = 0; i <= dataTrashStart.Length - 1; i++)
            {
                dataTrashStart[i] = Convert.ToByte(random.Next(12, 255));
            }
            for (int i = 0; i <= dataTrashEnd.Length - 1; i++)
            {
                dataTrashEnd[i] = Convert.ToByte(random.Next(12, 255));
            }

            for (int i = 0; i <= dataTrashStart.Length - 1; i++)
            {
                dataByte[i] = dataTrashStart[i];
            }
            int j = 0;
            string[] hexValuesSplit = dataString.Split(' ');
            for (int i = dataTrashStart.Length+1; i < dataTrashStart.Length + hexValuesSplit.Length; i++)
            {
                byte value = Convert.ToByte(hexValuesSplit[j], 16);
                dataByte[i] = value;
                //if (dataString[j] == '0')
                //    dataByte[i] = 0;
                //else if (dataString[j] == '1')
                //    dataByte[i] = 1;
                //else if (dataString[j] == '2')
                //    dataByte[i] = 2;
                //else if (dataString[j] == '3')
                //    dataByte[i] = 3;
                //else if (dataString[j] == '4')
                //    dataByte[i] = 4;
                //else if (dataString[j] == '5')
                //    dataByte[i] = 5;
                //else if (dataString[j] == '6')
                //    dataByte[i] = 6;
                //else if (dataString[j] == '7')
                //    dataByte[i] = 7;
                //else if (dataString[j] == '8')
                //    dataByte[i] = 8;
                //else if (dataString[j] == '9')
                //    dataByte[i] = 9;
                //else if (dataString[j] == 'A')
                //    dataByte[i] = 10;
                //else if (dataString[j] == 'B')
                //    dataByte[i] = 11;
                //else if (dataString[j] == 'C')
                //    dataByte[i] = 12;
                //else if (dataString[j] == 'D')
                //    dataByte[i] = 13;
                //else if (dataString[j] == 'E')
                //    dataByte[i] = 14;
                //else if (dataString[j] == 'F')
                //    dataByte[i] = 15;
                j++;
            }
            bool OUT = false;
            int k = 0;
            for (int i = 0; i <= 255; i++)
            {
                if (OUT)
                {
                    if (dataTrashEnd.Length != 0)
                    {
                        dataByte[i] = dataTrashEnd[k];
                        k++;
                    }
                    if (dataTrashEnd.Length == k) break;
                    
                }
                if (dataByte[i] == 11)
                {
                    OUT = true;
                }

            }
            return dataByte;
        }
        void Connect(byte[] message)
        {
            try
            {
                SendMessageFromSocket(11000,message);
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

         void SendMessageFromSocket(int port, byte[] message)
        {
            // Буфер для входящих данных
            byte[] bytes = new byte[256];

            // Соединяемся с удаленным устройством

            // Устанавливаем удаленную точку для сокета
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

            Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Соединяем сокет с удаленной точкой
            sender.Connect(ipEndPoint);

            // Отправляем данные через сокет
            int bytesSent = sender.Send(message);

            // Получаем ответ от сервера
            sender.Receive(bytes);
            string str = Encoding.UTF8.GetString(bytes);

            ///textBox4.Text

            Int64 i;
            i = Convert.ToInt64(str);
            // Освобождаем сокет
            textBox4.Text =i.ToString("X");
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
        Timer timer = new Timer();
        
        private void button1_Click(object sender, EventArgs e)
        {
            timer.Interval = 1000;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            byte[] snd = send();
            Connect(snd);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer.Stop();
        }

        string printHex(string str)
        {
            
            return "";
        }
    }
}
