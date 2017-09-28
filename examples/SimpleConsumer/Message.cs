using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Confluent.Kafka.Examples.SimpleConsumer
{
    public class Message
    {
        //public string Id { get; set; }
        //public string Timestamp { get; set; }
        public LocationCheckinData Content { get; set; }
        //public string CallbackName { get; set; }
    }
}
