using EDU.Common.Helper;
using EDU.Common.Helper.Interfaces;
using EDU.DataModel;
using EDU.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDU.Services
{
    public class UserService : IUserService
    {
        private IDatabaseHelper _dbHelper;
        public UserService(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public bool Create(UserViewModel model, bool create_emp)
        {
            string msgError = "";
            try
            {
                var xxx = MessageConvert.SerializeObject(model.objectjson_person);
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_user_create",
                    "@create_employee", create_emp,
                    "@user_id", model.user_id,
                    "@username", model.username,
                    "@password", model.password,
                    "@language_code", model.language_code,
                    "@domain_name", model.domain_name,
                    "@description_l", model.description_l,
                    "@description_e", model.description_e,
                    "@access_token", model.access_token,
                    "@created_by_user_id", model.created_by_user_id,
                    "@objectjson_employee", MessageConvert.SerializeObject(model.objectjson_employee),
                    "@objectjson_person", MessageConvert.SerializeObject(model.objectjson_person));
                if ((result != null && !string.IsNullOrEmpty(result.ToString())) || !string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(Convert.ToString(result) + msgError);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Update(UserViewModel model)
        {
            string msgError = "";
            try
            {
                var vv = MessageConvert.SerializeObject(model.objectjson_person);
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_user_update",
                    "@user_id", model.user_id,
                    "@username", model.username,
                    "@language_code", model.language_code,
                    "@domain_name", model.domain_name,
                    "@description_l", model.description_l,
                    "@description_e", model.description_e,
                    "@lu_user_id", model.lu_user_id,
                    "@objectjson_person", MessageConvert.SerializeObject(model.objectjson_person));
                if ((result != null && !string.IsNullOrEmpty(result.ToString())) || !string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(Convert.ToString(result) + msgError);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public UserViewModel GetUserByUsername(string userName)
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_user_get_by_username", "@username", userName);
                if (!string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(msgError);
                }
                return dt != null ? dt.ConvertTo<UserViewModel>().ToList().FirstOrDefault() : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public UserViewModel GetUserByLogin(string userName, string passWord)
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_user_get_by_login",
                    "@username", userName,
                    "@password", passWord
                    );
                if (!string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(msgError);
                }
                return dt != null ? dt.ConvertTo<UserViewModel>().ToList().FirstOrDefault() : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<UserViewModel> SearchUserRole(int pageIndex, int pageSize, string role_id, char lang, string user_name, string full_name, out long total)
        {
            string msgError = "";
            try
            {
                total = 0;
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_user_role_search",
                    "@page_index", pageIndex,
                    "@page_size", pageSize,
                    "@lang", lang,
                    "@user_name", user_name,
                    "@role_id", role_id);
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);
                if (dt.Rows.Count > 0) total = (long)dt.Rows[0]["RecordCount"];
                return dt.ConvertTo<UserViewModel>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<UserViewModel> Search(int pageIndex, int pageSize, char lang, string user_name, string full_name, string active_flags, out long total)
        {
            string msgError = "";
            try
            {
                total = 0;
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_user_search",
                    "@page_index", pageIndex,
                    "@page_size", pageSize,
                    "@lang", lang,
                    "@user_name", user_name,
                    "@full_name", full_name,
                    "@active_flag", active_flags);
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);
                if (dt.Rows.Count > 0) total = (long)dt.Rows[0]["RecordCount"];
                return dt.ConvertTo<UserViewModel>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<UserViewModel> SearchOnekey(int pageIndex, int pageSize, char lang, string keyword, string active_flags, out long total)
        {
            string msgError = "";
            try
            {
                total = 0;
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_user_search_one_keyword",
                    "@page_index", pageIndex,
                    "@page_size", pageSize,
                    "@lang", lang,
                    "@key", keyword,
                    "@active_flag", active_flags);
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);
                if (dt.Rows.Count > 0) total = (long)dt.Rows[0]["RecordCount"];
                return dt.ConvertTo<UserViewModel>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public UserViewModel GetUserById(string userId)
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_user_get_by_id", "@user_id", userId);
                if (!string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(msgError);
                }
                return dt != null ? dt.ConvertTo<UserViewModel>().ToList().FirstOrDefault() : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
