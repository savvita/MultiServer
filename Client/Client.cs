using ConnectLibrary;
using System;
using System.Net;
using System.Net.Sockets;
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

        public void ConnectToServer()
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(iPEndPoint);
            }
            catch
            {
                
            }
        }

        public void Disconnect()
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch
            {

            }
        }

        public bool IsConnected
        {
            get => socket.Connected;
        }

        public bool IsSocketAvailable
        {
            get
            {
                try
                {
                    return !((socket.Poll(1000, SelectMode.SelectRead) && (socket.Available == 0)) || !socket.Connected);
                }
                catch
                {
                    return false;
                }
            }
        }

        public string SendMessage(string message)
        {
            string response = String.Empty;
            try
            {
                if (message != String.Empty)
                {
                    Message.SendMessage(socket, message);
                    response = Message.ReceiveMessage(socket);
                }
            }
            catch
            {
                response = "Connection lost";
                Disconnect();
            }

            return response;
        }

        public string Upload(string path)
        {
            try
            {
                Message.SendMessage(socket, Message.UploadCode);
                Task.Factory.StartNew(() => Message.SendFile(socket, path));
            }
            catch
            {
            }

            return Message.ReceiveMessage(socket);
        }

        //public void StartChating()
        //{
        //    socket.Connect(iPEndPoint);

        //    Console.WriteLine("Connected to the server");

        //    do
        //    {
        //        Console.Write("Msg: ");
        //        string msg = Console.ReadLine();
        //        Message.SendMessage(socket, msg);

        //        if(msg.StartsWith(Message.UploadCode))
        //        {
        //            Task.Factory.StartNew(() =>
        //            Message.SendFile(socket, msg.Substring(Message.UploadCode.Length).Trim()));
        //        }

        //        Console.WriteLine($"Server: {Message.ReceiveMessage(socket)}");

        //        if (((socket.Poll(1000, SelectMode.SelectRead) && (socket.Available == 0)) || !socket.Connected))
        //            break;

        //    } while (true);

        //    Console.WriteLine("Disconnected");
        //}
    }
}
