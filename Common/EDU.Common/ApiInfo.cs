using System;

namespace EDU.Common
{
    public class ApiInfo
    {
        public Guid facility_id { get; set; }
        public string module_code { get; set; }
        public string api_url { get; set; }
        public string action_code { get; set; }
        public string assembly_qualified_name { get; set; }
        public DateTime action_date { get; set; }

        public ApiInfo(Guid facilityId, string moduleCode, string apiUrl, string actionCode, DateTime actionDate)
        {
            this.facility_id = facilityId;
            this.module_code = moduleCode;
            this.api_url = apiUrl;
            this.action_code = actionCode;
            this.action_date = actionDate;
        }
    }
}
