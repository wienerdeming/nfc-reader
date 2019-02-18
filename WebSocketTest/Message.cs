using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketTest
{
    public class Message
    {
        [JsonProperty("event")]
        public string EventName
        {
            get; set;
        }


        [JsonProperty("data")]
        public string EventData
        {
            get; set;
        }
    }
}
