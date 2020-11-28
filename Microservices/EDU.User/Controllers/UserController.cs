using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EDU.Common.Caching.Interfaces;
using EDU.Common.Entities;
using EDU.Common.Helper;
using EDU.Common.Message;
using EDU.Common.Response;
using EDU.DataModel;
using EDU.Services;
using EDU.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EDU.User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private IWebHostEnvironment _env;
        private IUserService _userService;
        public UserController(ICacheProvider redis, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env, UserService userServices) : base(redis, configuration, httpContextAccessor)
        {
            _env = env ?? throw new ArgumentNullException(nameof(env));
            _userService = userServices;
        }
        [Route("update-user")]
        [HttpPost]
        public ResponseMessage<bool?> UpdateUserAsync([FromBody] UserViewModel model)
        {
            var response = new ResponseMessage<bool?>();
            try
            {
                model.lu_user_id = CurrentUserId;
                model.objectjson_employee.facility_id = CurrentFacilityId;
                response.Data = _userService.Update(model);
                response.MessageCode = MessageCodes.UpdateSuccessfully;
            }
            catch (Exception ex)
            {
                response.MessageCode = ex.Message;
            }
            return response;
        }
        [Route("create-user")]
        [HttpPost]
        public ResponseMessage<string> CreateUser([FromBody] UserViewModel model)
        {
            var response = new ResponseMessage<string>();
            try
            {
                model.created_by_user_id = CurrentUserId;
                model.objectjson_employee.facility_id = CurrentFacilityId;
                model.language_code = CurrentLanguage.ToString();
                model.password = CrytographyHelper.CreateMD5(configuration["AppSettings:DEFAULT_PASSWORD"].ToString());
                if (_userService.Create(model,true))
                {
                    response.Data = model.user_id.ToString();
                    response.MessageCode = MessageCodes.CreateSuccessfully;
                }
            }
            catch (Exception ex)
            {
                response.MessageCode = ex.Message;
            }
            return response;
        }
        [Route("get-by-id/{id}")]
        [HttpGet]
        [AllowAnonymous]
        public ResponseMessage<UserViewModel> GetById(string id)
        {
            var response = new ResponseMessage<UserViewModel>();
            try
            {
                response.Data = _userService.GetUserById(id);
            }
            catch (Exception ex)
            {
                response.MessageCode = ex.Message;
            }
            return response;
        }
        [Route("search")]
        [HttpPost]
        public ResponseListMessage<List<UserViewModel>> Search([FromBody] Dictionary<string, object> formData)
        {
            var response = new ResponseListMessage<List<UserViewModel>>();
            try
            {
                var page = int.Parse(formData["page"].ToString());
                var pageSize = int.Parse(formData["pageSize"].ToString());
                var username = formData.Keys.Contains("username") ? (formData["username"]).ToString().Trim() : "";
                var full_name = formData.Keys.Contains("full_name") ? formData["full_name"].ToString() : "";
                string active_flags = "";
                if (formData.Keys.Contains("active_flag") && !string.IsNullOrEmpty(formData["active_flag"].ToString()))
                {
                    var list = JsonConvert.DeserializeObject<JArray>(formData["active_flag"].ToString()).ToList();
                    foreach (var item in list)
                    {
                        if (string.IsNullOrEmpty(active_flags)) active_flags = item.Value<object>().ToString();
                        else active_flags += "," + item.Value<object>().ToString();
                    }
                }
                if (formData.Keys.Contains("deactive_flag") && !string.IsNullOrEmpty(formData["deactive_flag"].ToString()))
                {
                    var list = JsonConvert.DeserializeObject<JArray>(formData["deactive_flag"].ToString()).ToList();
                    foreach (var item in list)
                    {
                        if (string.IsNullOrEmpty(active_flags)) active_flags = item.Value<object>().ToString();
                        else active_flags += "," + item.Value<object>().ToString();
                    }
                }
                long totalItems = 0;
                var data = new List<UserViewModel>();
                if (formData.Keys.Contains("Key_word"))
                {
                    var keyword = formData.Keys.Contains("Key_word") ? (formData["Key_word"]).ToString().Trim() : "";
                    data = _userService.SearchOnekey(page, pageSize, CurrentLanguage, keyword, active_flags, out totalItems);
                }
                else
                    data = _userService.Search(page, pageSize, CurrentLanguage, username, full_name, active_flags, out totalItems);
                response.TotalItems = totalItems;
                response.Data = data;
                response.Page = page;
                response.PageSize = pageSize;
            }
            catch (Exception ex)
            {
                response.MessageCode = ex.Message;
            }
            return response;
        }
    }
}
