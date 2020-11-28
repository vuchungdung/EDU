using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace EDU.DataModel
{
    public class PermissionViewModel
    {
        [DataMember]
        public string role_permission_id { get; set; }
        [DataMember]
        public string page_id { get; set; }
        [DataMember]
        public string role_id { get; set; }
        [DataMember]
        public string page_name { get; set; }
        [DataMember]
        public string page_url { get; set; }
        [DataMember]
        public int page_seq_num { get; set; }
        [DataMember]
        public bool public_flag { get; set; }
        [DataMember]
        public string page_name_e { get; set; }
        [DataMember]
        public string page_name_l { get; set; }
        [DataMember]
        public string page_group_id { get; set; }
        [DataMember]
        public string page_group_name { get; set; }
        [DataMember]
        public string page_group_name_e { get; set; }
        [DataMember]
        public string page_group_name_l { get; set; }
        [DataMember]
        public string css_class { get; set; }
        [DataMember]
        public int ord { get; set; }
        [DataMember]
        public bool is_public { get; set; }
        [DataMember]
        public string action_code { get; set; }
        [DataMember]
        public string action_name { get; set; }
        [DataMember]
        public string action_name_e { get; set; }
        [DataMember]
        public string action_name_l { get; set; }
        [DataMember]
        public string action_description_e { get; set; }
        [DataMember]
        public string action_description_l { get; set; }
        [DataMember]
        public string facility_id { get; set; }
        [DataMember]
        public string api_url { get; set; }
        [DataMember]
        public string action_url { get; set; }
        [DataMember]
        public string created_by { get; set; }
        [DataMember]
        public int active_flag { get; set; }
    }
}
