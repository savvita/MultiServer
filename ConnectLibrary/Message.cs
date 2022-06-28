using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace ConnectLibrary
{
    public static class Message
    {
        public static int BufferSize { get; set; } = 256;
        public static Encoding Encoding { get; set; } = Encoding.Unicode;
        public static string StopCode { get; set; } = "/end";
        public static string DateCode { get; set; } = "/date";
        public static string TimeCode { get; set; } = "/time";
        public static string ReceiveMessage(Socket socket)
        {
            byte[] buffer = new byte[BufferSize];
            int count = 0;
            StringBuilder sb = new StringBuilder();

            do
            {
                count = socket.Receive(buffer);
                sb.Append(Encoding.GetString(buffer, 0, count));
            } while (socket.Available > 0);

            return sb.ToString();
        }

        public static void SendMessage(Socket socket, string message)
        {
            try
            {
                socket.Send(Encoding.GetBytes(message));
            }
            catch
            {
                throw;
            }
        }

        public static void SendFile(Socket socket, string path)
        {
            try
            {
                socket.Send(File.ReadAllBytes(path));
            }
            catch
            {
                throw;
            }
        }

        public static void ReceiveFile(Socket socket)
        {
            byte[] buffer = new byte[BufferSize];
            int count = 0;

            FileStream file = new FileStream(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "file"),
                FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

            do
            {
                count = socket.Receive(buffer);
                file.Write(buffer, 0, count);
            } while (socket.Available > 0);

            file.Close();
        }
    }
}
