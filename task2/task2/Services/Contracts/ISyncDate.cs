using System;

namespace task2.Models.Services.Contracts
{
    public interface ISyncDate
    {
        DateTimeOffset? SyncDate { get; set; }
    }
}
