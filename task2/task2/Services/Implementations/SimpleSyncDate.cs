﻿using System;
using task2.Models.Services.Contracts;

namespace task2.Models.Services.Implementations
{
    public class SimpleSyncDate : ISyncDate
    {
        public DateTimeOffset? SyncDate
        {
            get;
            set;
        }
    }


    //public class RealEventGetter: 
}
