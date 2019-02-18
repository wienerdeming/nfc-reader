using KhumoReader.entity;
using Newtonsoft.Json;
using NFCTools;
using PCSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace KhumoReader.server
{
    public class NfcBehavior : WebSocketBehavior
    {

        private NFCReader.ActionCardHandler NFCEventHandler;

        public NfcBehavior()
        {
            NFCEventHandler += (string eventName, SCRState state, string readerName) =>
           {
               if (eventName == "CardInserted")
                   ReadFromNfc();
               if (eventName == "Initialized")
                   SendReaderStatus();
           };
            NfcFactory.Instance.AddEventHandler(NFCEventHandler);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            var eventMessage = JsonConvert.DeserializeObject<Message>(e.Data);
            switch (eventMessage.EventName)
            {
                case EventType.READ: ReadFromNfc(); break;
                case EventType.WRITE: WriteToNfc(eventMessage.EventData); break;
            }
        }

        private void WriteToNfc(string data)
        {
            var message = new Message();
            try
            {
                NfcFactory.Instance.WriteToNFC(data);
                message.EventName = EventType.WRITE;
                message.EventData = data;
            }
            catch (Exception ex)
            {
                message.EventName = EventType.ERROR;
                message.EventData = ex.Message;
            }
            finally
            {
                Send(JsonConvert.SerializeObject(message));
            }
        }

        protected override void OnOpen()
        {
            Console.WriteLine("Opened");
            SendReaderStatus();
            base.OnOpen();
        }

        private void SendReaderStatus()
        {
            var message = new Message();
            try
            {
                var ready = NfcFactory.Instance.IsReady();
                message.EventName = EventType.READY;
                message.EventData = ready.ToString();
            }
            catch
            {
                message.EventName = EventType.ERROR;
                message.EventData = "NFC not connected";
            }
            finally
            {
                Send(JsonConvert.SerializeObject(message));
            }
        }

        private void ReadFromNfc()
        {
            var message = new Message();
            try
            {
                var data = NfcFactory.Instance.ReadData();
                message.EventName = EventType.READ;
                message.EventData = data;
            }
            catch (Exception ex)
            {
                message.EventName = EventType.ERROR;
                message.EventData = ex.Message;
            }
            finally
            {
                Send(JsonConvert.SerializeObject(message));
            }
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Console.WriteLine("Closed");
            NfcFactory.Instance.RemoveEventHandler(NFCEventHandler);
            NFCEventHandler = null;
            base.OnClose(e);
        }
    }
}
