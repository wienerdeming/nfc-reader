using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;

namespace WebSocketTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please input address");
            string ipAddress = Console.ReadLine();
            Console.WriteLine("Please input port");
            int port = 4949;
            int.TryParse(Console.ReadLine(), out port);
            var wssv = new WebSocket($"ws://{ipAddress}:{port}/nfc");
            {
                wssv.OnMessage += (sender, e) => OnMessage(e);
                wssv.Origin = "*";
                wssv.OnError += (sd, e) => Console.WriteLine("Socket error {0}", e.Message);
                wssv.Connect();
                wssv.OnClose += (a, e) =>
                {
                    Thread.Sleep(500);
                    wssv.Connect();
                };
                wssv.Send(Message("read", ""));
                var line = Console.ReadLine();

                try
                {
                    wssv.Send(Message("write", line));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private static void OnMessage(MessageEventArgs e)
        {
            var message = JsonConvert.DeserializeObject<Message>(e.Data);
            Console.WriteLine("Message received {0} {1}", message.EventName, message.EventData);
        }
        private static string Message(string eventType, string line)
        {
            var message = new Message()
            {
                EventData = line,
                EventName = "write"
            };
            return JsonConvert.SerializeObject(message);
        }
    }
}