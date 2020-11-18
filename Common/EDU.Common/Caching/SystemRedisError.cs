using System;

namespace EDU.Common.Caching
{
    public partial class SystemRedisError
    {
        public string field { get; set; }
        public string data { get; set; }
        public short? db_number { get; set; }
        public string key { get; set; }
        public Guid id { get; set; }
        public int? expires_after { get; set; }
        public string method { get; set; }
        public bool? is_async { get; set; }
        public string assembly_qualified_name { get; set; }
        public short? priority { get; set; }
        public short? active_flag { get; set; }
        public DateTime? created_date { get; set; }
        public string created_by { get; set; }
        public DateTime? modified_date { get; set; }
        public string modified_by { get; set; }
    }

}
