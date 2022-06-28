using System;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Client client = new Client("127.0.0.1", 8008);
                client.StartChating();
            }
            catch
            {
                Console.WriteLine("Cannot connect to the server");
            }
        }
    }
}
