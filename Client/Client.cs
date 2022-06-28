using ConnectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Client
    {
        private Socket socket;
        private IPEndPoint iPEndPoint;

        public Client(string ip, int port)
        {
            try
            {
                iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            catch
            {
                throw;
            }
        }

        public void StartChating()
        {
            socket.Connect(iPEndPoint);

            Console.WriteLine("Connected to the server");

            do
            {
                Console.Write("Msg: ");
                string msg = Console.ReadLine();

                Message.SendMessage(socket, msg);
                Console.WriteLine($"Server: {Message.ReceiveMessage(socket)}");

                if (((socket.Poll(1000, SelectMode.SelectRead) && (socket.Available == 0)) || !socket.Connected))
                    break;

            } while (true);

            Console.WriteLine("Disconnected");
        }
    }
}
