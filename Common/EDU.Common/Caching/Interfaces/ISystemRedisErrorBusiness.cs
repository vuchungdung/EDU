using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EDU.Common.Caching
{
    public interface ISystemRedisErrorBusiness
    {
        //store error message
        Task<bool> Remove(CacheInfo cacheInfo);
        Task<bool> Remove(FieldRemove fieldRemove);
        Task<bool> Create<T>(HashedDic<T> data);
        Task<bool> Create<T>(HashedField<T> data);
        Task<bool> Update(Guid id, short activeFlag);
        Task<bool> Update(Guid id, short priority, short activeFlag);
        List<SystemRedisError> GetAll(short activeFag, short maxPriority, int pageSize);
    }
}
