using Client.View;
using System;
using System.Windows.Forms;

namespace Client
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //try
            //{
            //    Client client = new Client("127.0.0.1", 8008);
            //    client.StartChating();
            //}
            //catch
            //{
            //    Console.WriteLine("Cannot connect to the server");
            //}

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ChatForm());
        }
    }
}
