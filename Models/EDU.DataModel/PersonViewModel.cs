using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace EDU.DataModel
{
    public class PersonViewModel
    {
        public string person_id { get; set; }
        public string person_code { get; set; }
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public string last_name { get; set; }
        public string full_name { get; set; }
        public int? gender { get; set; }
        public DateTime? date_of_birth { get; set; }
        public string place_of_birth { get; set; }
        public string place_of_birth_e { get; set; }
        public string place_of_birth_l { get; set; }
        public string place_of_work { get; set; }
        public string place_of_work_e { get; set; }
        public string place_of_work_l { get; set; }
        public string country_of_nationalty_l { get; set; }
        public string country_of_nationalty { get; set; }
        public string country_of_nationalty_e { get; set; }
        public string country_of_residence_l { get; set; }
        public string country_of_residence { get; set; }
        public string country_of_residence_e { get; set; }
        public string home_phone { get; set; }
        public string mobile_phone { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public string home_address_l { get; set; }
        public string home_address { get; set; }
        public string home_address_e { get; set; }
        public string short_home_address_l { get; set; }
        public string short_home_address { get; set; }
        public string short_home_address_e { get; set; }
        public string residence_address_l { get; set; }
        public string residence_address { get; set; }
        public string residence_address_e { get; set; }
        public string identification { get; set; }
        public DateTime? date_of_issue { get; set; }
        public string place_of_issue { get; set; }
        public string place_of_issue_e { get; set; }
        public string place_of_issue_l { get; set; }
        public string ethnic_group_l { get; set; }
        public string ethnic_group { get; set; }
        public string ethnic_group_e { get; set; }
        public string religion_l { get; set; }
        public string religion { get; set; }
        public string religion_e { get; set; }
        public string marital_status_l { get; set; }
        public string marital_status { get; set; }
        public string marital_status_e { get; set; }
        public string primary_language_l { get; set; }
        public string primary_language { get; set; }
        public string primary_language_e { get; set; }
        public string nationality_country_rcd { get; set; }
        public string residence_country_rcd { get; set; }
        public string id_card_no { get; set; }
        public string ethnic_group_rcd { get; set; }
        public string religion_rcd { get; set; }
        public string marital_status_rcd { get; set; }
        public string primary_language_rcd { get; set; }
        public string description { get; set; }
        public string description_e { get; set; }
        public string description_l { get; set; }
        public string created_by_user_id { get; set; }
        public DateTime? created_date_time { get; set; }
        public string lu_user_id { get; set; }
        public DateTime? lu_updated { get; set; }
        public int? active_flag { get; set; }

    }
}
