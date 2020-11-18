using System;
using System.Collections.Generic;
using System.Text;

namespace EDU.Common
{
    public class UserInfo
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FacilityId { get; set; }
        public string DepartmentId { get; set; }
        public string IpAddress { get; set; }
        public string BrowserName { get; set; }
        public UserInfo(string userId, string userName, string facilityId, string departmentId, string ipAddress, string browserName)
        {
            UserId = userId;
            UserName = userName;
            FacilityId = facilityId;
            DepartmentId = departmentId;
            IpAddress = ipAddress;
            BrowserName = BrowserName;
        }
    }
}
