﻿using System.Collections.Generic;
using System.Linq;
using task2.Models.ModelEntites;

namespace task2.Models.Services.Contracts
{

    public interface IEventGetter
    {
        IList<EventObject> GetEventNotifications();
    }
}
