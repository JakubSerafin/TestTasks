using System.Collections.Generic;

namespace task2.Models.ModelEntites
{
    public class ResultResponse
    {
        public List<EventObject> notifications { get; set; }
        public string responseDesc { get; set; }
        public string responseCode { get; set; }
    }
}
