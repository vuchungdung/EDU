using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDU.Common.Caching
{
    public interface ICachingErrorHandler
    {
        Task Remove(CachingFunc method, int dbNumber, string key, bool isAsync);
        Task Remove(CachingFunc method, int dbNumber, string key, string field, bool isAsync);
    }
}
