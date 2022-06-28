using System;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Server server = new Server("127.0.0.1", 8008);
                server.Work();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
