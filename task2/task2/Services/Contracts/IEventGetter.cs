using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using task2.Models.ModelEntites;

namespace task2.Models.Services.Contracts
{
    public interface IEventGetter
    {
        ICollection<EventObject> GetEventNotifications();
    }
}
