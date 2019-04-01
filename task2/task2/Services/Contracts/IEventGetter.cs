using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using task2.Models.ModelEntites;

namespace task2.Models.Services.Contracts
{
    public interface IServiceBlocker
    {
        bool CanProcess();
    }

    public interface IEventGetter
    {
        ICollection<EventObject> GetEventNotifications();
    }

    public interface IPagedEventGeter
    {
        ICollection<EventObject> GetPage(int pageNum, int pageSize);
    }
}
