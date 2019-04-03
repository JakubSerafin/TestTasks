using System.Collections.Generic;
using task2.Models.ModelEntites;

namespace task2.Models.Services.Contracts
{
    public interface IPagedEventGeter
    {
        IList<EventObject> GetPage(int pageNum, int pageSize);
    }
}
