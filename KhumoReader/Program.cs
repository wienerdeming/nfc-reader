using KhumoReader.server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KhumoReader
{
    class Program
    {

        static void Main(string[] args)
        {
            var server = new SocketServer();
            server.Start();
            if (Console.ReadLine() == "quit")
                server.Stop();
        }

    }
}
