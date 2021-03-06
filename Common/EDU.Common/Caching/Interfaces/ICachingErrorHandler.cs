﻿using EDU.Common.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDU.Common.Caching.Interfaces
{
    public interface ICachingErrorHandler
    {
        Task Remove(CachingFunc method, int dbNumber, string key, bool isAsync);
        Task Remove(CachingFunc method, int dbNumber, string key, string field, bool isAsync);
    }
}
