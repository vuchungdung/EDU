using System;
using System.Collections.Generic;
using System.Text;

namespace EDU.DataModel
{
    public class UserViewModel
    {
        public string user_id { get; set; }
        public string username { get; set; }
        public string print_name_e { get; set; }
        public string print_name_l { get; set; }
        public string password { get; set; }
        public string language_code { get; set; }
        public string facility_id { get; set; }
        public string department_id { get; set; }
        public string domain_name { get; set; }
        public string type { get; set; }
        public string description_l { get; set; }
        public string description { get; set; }
        public string description_e { get; set; }
        public string access_token { get; set; }
        public string job_type_rcd { get; set; }
        public string full_name { get; set; }
        public string avatar { get; set; }
        public string email { get; set; }
        public DateTime? date_of_birth { get; set; }
        public int? gender { get; set; }
        public string first_name { get; set; }
        public string token_type { get; set; }
        public string middle_name { get; set; }
        public string last_name { get; set; }
        public int? expires_in { get; set; }
        public string refresh_token { get; set; }
        public DateTime? issued { get; set; }
        public DateTime? expires { get; set; }
        public string created_by_user_id { get; set; }
        public DateTime? created_date_time { get; set; }
        public string lu_user_id { get; set; }
        public DateTime? lu_updated { get; set; }
        public int? active_flag { get; set; }
        public List<RoleViewModel> listjson_roles { get; set; }
        public List<PermissionViewModel> listjson_permissions { get; set; }
        public EmployeeViewModel objectjson_employee { get; set; }
        public PersonViewModel objectjson_person { get; set; }
    }
}
