using System.Collections.Generic;

namespace EDU.Common.Caching
{
    public class HashedDic<T> : CacheInfo
    {
        public IDictionary<string, T> Value { get; set; }
    }
}
