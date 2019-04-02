using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using task2.Models.ModelEntites;
using task2.Models.Services.Contracts;

namespace task2.Models.Services.Implementations
{
    public class TimePeriodEventGetter : IAsyncEventGetter
    {
        public Task<IList<EventObject>> GetEventNotifications()
        {
            throw new NotImplementedException();
        }
    }
}
