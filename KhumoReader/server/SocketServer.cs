using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace KhumoReader.server
{
    public class SocketServer
    {
        private readonly WebSocketServer webServer;
        public SocketServer(int port = 4949)
        {
            webServer = new WebSocketServer(port);
            webServer.WaitTime = TimeSpan.FromSeconds(10);
            webServer.AddWebSocketService("/nfc", () => new NfcBehavior());
        }

        public void Stop()
        {
            webServer.Stop();
        }

        public void Start()
        {
            Console.WriteLine("Start server");
            webServer.Start();
            Console.WriteLine("Start started successfully");
        }
    }
}
