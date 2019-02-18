using NFCTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace KhumoReader
{
    public sealed class NfcFactory : IDisposable
    {
        public static NfcFactory Instance { get { return lazy.Value; } }
        private static readonly Lazy<NfcFactory> lazy = new Lazy<NfcFactory>(() => new NfcFactory());

        private List<NFCReader.ActionCardHandler> delegates = new List<NFCReader.ActionCardHandler>();
        private NFCReader reader;
        private const int BLOCK = 8;
        private Timer timer;

        private NfcFactory()
        {
            StartConnectTimer();
            ConnectReader();
        }

        private void StartConnectTimer()
        {
            timer = new Timer();
            timer.Interval = 10 * 1000;
            timer.Elapsed += (_, ds) => CheckReader();
            timer.Start();
        }

        public void AddEventHandler(NFCReader.ActionCardHandler EventHandler)
        {
            if (reader != null)
                reader.EventCardAction += EventHandler;
            else
                delegates.Add(EventHandler);
        }

        public void RemoveEventHandler(NFCReader.ActionCardHandler EventHandler)
        {
            if (reader != null)
                reader.EventCardAction -= EventHandler;
            delegates.Remove(EventHandler);
        }

        public bool IsReady()
        {
            return reader.ToString() != null;
        }

        private void CheckReader()
        {
            if (reader == null)
            {
                Console.WriteLine("Reconnect nfc...");
                ConnectReader();
            }
        }

        public void ConnectReader()
        {
            try
            {
                reader = new NFCReader(BLOCK);
                delegates.ForEach(x =>
                {
                    reader.EventCardAction += x;
                });


            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine("Nfc connect exception: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Nfc connect exception: {0}", ex.Message);
            }
        }

        public Task<string> ReadDataAsync()
        {
            return Task.FromResult(reader.GetRancherId());
        }

        public string ReadData()
        {
            return reader.GetRancherId();
        }

        public string WriteToNFC(string data)
        {
            var blockId = Guid.Parse(data).ToByteArray();
            Array.Reverse(blockId, 0, 4);
            Array.Reverse(blockId, 4, 2);
            Array.Reverse(blockId, 6, 2);
            try
            {
                var updated = reader.UpdateRancherId(blockId);
                return data;
            }
            catch
            {
                throw new Exception("Произошла ошибка при написание данных");
            }
        }

        public void Dispose()
        {
            timer?.Dispose();
            reader = null;
        }
    }
}
