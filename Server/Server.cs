using ConnectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    internal class Server
    {
        private Socket socket;
        IPEndPoint iPEndPoint;

        public Server(string ip, int port)
        {
            InitializeServer(ip, port);
        }

        private void InitializeServer(string ip, int port)
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

        public void Work()
        {
            socket.Bind(iPEndPoint);
            socket.Listen();

            Console.WriteLine("Server started");

            do
            {
                try
                {
                    Socket socketClient = socket.Accept();
                    Task.Factory.StartNew((obj) => ChatToClient(obj), socketClient);
                }
                catch
                {
                    throw;
                }
            } while (true);
        }

        private void ChatToClient(object socket)
        {
            Socket socketClient = socket as Socket;
            bool isEnd = false;

            Console.WriteLine($"Client {Thread.CurrentThread.ManagedThreadId} join");

            try
            {
                do
                {
                    if (isEnd)
                        break;


                    string msg = Message.ReceiveMessage(socketClient);
                    Console.WriteLine($"Client {Thread.CurrentThread.ManagedThreadId}: {msg}");

                    string response = String.Empty;

                    if (msg.StartsWith("/"))
                    {
                        response = HandleCommand(socketClient, msg, out isEnd);
                    }

                    else
                    {
                        response = "Message receive";
                    }

                    SendResponse(socketClient, response);

                } while (true);
            }
            catch
            {
                throw;
            }
            finally
            {
                socketClient.Shutdown(SocketShutdown.Both);
                socketClient.Close();

                Console.WriteLine($"Client {Thread.CurrentThread.ManagedThreadId} leave");
            }
        }

        private string HandleCommand(Socket clientSocket, string command, out bool isEnd)
        {
            string response = String.Empty;
            isEnd = false;

            if (command.Equals(Message.StopCode))
            {
                response = "Session end";
                isEnd = true;
            }
            else if (command.Equals(Message.DateCode))
            {
                response = DateTime.Now.ToLongDateString();
            }
            else if (command.Equals(Message.TimeCode))
            {
                response = DateTime.Now.ToShortTimeString();
            }
            else if(command.StartsWith(Message.UploadCode))
            {
                string[] cols = command.Split(' ');

                try
                {
                    Message.ReceiveFile(clientSocket);
                    response = "Uploaded";
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                response = "Incorrect command";
            }

            return response;
        }

        private void SendResponse(Socket socket, string message)
        {
            Message.SendMessage(socket, $"[{DateTime.Now.ToShortTimeString()}]: {message}");
        }
    }
}
