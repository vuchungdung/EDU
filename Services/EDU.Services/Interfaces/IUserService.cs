using EDU.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDU.Services.Interfaces
{
    public interface IUserService
    {
        bool Create(UserViewModel model, bool create_emp);
        bool Update(UserViewModel model);
        List<UserViewModel> SearchUserRole(int pageIndex, int pageSize, string role_id, char lang, string user_name, string full_name, out long total);      
        UserViewModel GetUserByUsername(string userName);
        UserViewModel GetUserByLogin(string userName, string passWord);
        UserViewModel GetUserById(string userId);
        List<UserViewModel> Search(int pageIndex, int pageSize, char lang, string user_name, string full_name, string active_flags, out long total);
        List<UserViewModel> SearchOnekey(int pageIndex, int pageSize, char lang, string keyword, string active_flags, out long total);
    }
}
