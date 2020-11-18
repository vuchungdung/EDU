namespace EDU.Common.Caching
{
    public class CacheInfo
    {
        public short DbNumber { get; set; }
        public string Key { get; set; }
        public string Method { get; set; }
        public bool IsAsync { get; set; }
    }
}
