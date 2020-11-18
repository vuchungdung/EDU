
namespace EDU.Common.Caching
{
    public class HashedField<T> : CacheInfo
    {
        public string Field { get; set; }
        public T Value { get; set; }
        public int ExpiresAfter { get; set; }
    }
}
