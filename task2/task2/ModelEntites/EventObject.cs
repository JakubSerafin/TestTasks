using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace task2.Models.ModelEntites
{

    public class EventObject
    {
        public String Category { get; set; }
        public string City { get; set; }
        public string SubCategory { get; set; }
        public string District { get; set; }
        public string Street2 { get; set; }
        public DateTime createDate { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public EventType NotificationType { get; set; }
        [JsonProperty("event")]
        public string EventDescription { get; set; }
    }
}
