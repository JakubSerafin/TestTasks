using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using task2.Models.ModelEntites;
using task2.Models.Services.Contracts;

namespace task2.Models.Services.Implementations
{
    public class DummyEventGetter : IEventGetter
    {
        public ICollection<EventObject> GetEventNotifications()
        {
            return new List<EventObject>
            {
                new EventObject(),
                new EventObject(),
                new EventObject(),
                new EventObject()
            };
        }
    }
}
