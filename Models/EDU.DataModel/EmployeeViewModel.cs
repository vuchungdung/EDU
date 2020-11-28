using System;
using System.Collections.Generic;
using System.Text;

namespace EDU.DataModel
{
    public class EmployeeViewModel
    {
        public string employee_id { get; set; }
        public string facility_id { get; set; }
        public string department_id { get; set; }
        public string employee_code { get; set; }
        public string avatar { get; set; }
        public string job_type_rcd { get; set; }
        public string print_name_e { get; set; }
        public string print_name_l { get; set; }
        public string print_name { get; set; }
        public string created_by_user_id { get; set; }
        public DateTime? created_date_time { get; set; }
        public string lu_user_id { get; set; }
        public DateTime? lu_updated { get; set; }
        public int? active_flag { get; set; }
    }
}
