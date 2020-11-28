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
        bool ChangePassword(string user_id, string password, string new_password);
        bool UpdateUserRole(string userId, string json, string updated_by);
        bool ChangeLanguage(string user_id, char lang);
        bool ResetPasswordDefault(string username, string new_pass);
        bool ChangeActiveStatus(string json_list_id, string updated_by, int status);
        UserViewModel GetUserByUsername(string userName);
        UserViewModel GetUserByLogin(string userName, string passWord);
        UserViewModel GetUserById(string userId);
        List<UserViewModel> Search(int pageIndex, int pageSize, char lang, string user_name, string full_name, string active_flags, out long total);
        List<UserViewModel> SearchOnekey(int pageIndex, int pageSize, char lang, string keyword, string active_flags, out long total);
    }
}
