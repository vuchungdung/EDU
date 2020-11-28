using System;
using System.Collections.Generic;
using System.Text;

namespace EDU.DataModel
{
    public class RoleViewModel
    {
        public string role_id { get; set; }
        public string facility_id { get; set; }
        public string role_name_l { get; set; }
        public string role_name_e { get; set; }
        public string role_name { get; set; }
        public string role_code { get; set; }
        public string description { get; set; }
        public string description_e { get; set; }
        public string description_l { get; set; }
        public bool? use_context_security { get; set; }
        public string context_id { get; set; }
        public long? user_number { get; set; }
        public long? total { get; set; }
        public string created_by { get; set; }
        public int? active_flag { get; set; }
    }
}
