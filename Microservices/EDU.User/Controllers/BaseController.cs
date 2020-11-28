using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDU.Common.Caching.Interfaces;
using EDU.Common.Helper;
using EDU.Common.Providers;
using EDU.DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EDU.User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private static bool EnabledLanguageCaching;
        private int DefaultDatabase = 0;
        private char DefaultLanguage = 'l';
        protected IHttpContextAccessor httpContextAccessor;
        protected ICacheProvider redisHelper;
        protected IConfiguration configuration;
        public BaseController(ICacheProvider redis, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.redisHelper = redis;
            this.httpContextAccessor = httpContextAccessor;
            this.configuration = configuration;
            EnabledLanguageCaching = bool.Parse(configuration["AppSettings:EnabledLanguageCaching"]);
        }
        protected dynamic CurrentUer
        {
            get
            {
                return httpContextAccessor.HttpContext.User;
            }
        }
        protected string CurrentUserId
        {
            get
            {
                var user = httpContextAccessor.HttpContext.User.FindFirst(CustomClaimTypes.UserId);
                if (user == null)
                    return Guid.Empty.ToString();
                return user.Value;
            }
        }

        protected string CurrentUserName
        {
            get
            {
                var user = httpContextAccessor.HttpContext.User.FindFirst(CustomClaimTypes.UserName);
                if (user == null)
                    return Guid.Empty.ToString();
                return user.Value;
            }
        }

        protected List<PermissionViewModel> Permissions
        {
            get
            {
                var jsonPermissions = redisHelper.Get<string>(0, CurrentUserId + "-permissions");
                if (!string.IsNullOrEmpty(jsonPermissions))
                {
                    var permissions = MessageConvert.DeserializeObject<List<PermissionViewModel>>(jsonPermissions);
                    if (permissions != null && permissions.Count > 0)
                    {
                        return permissions;
                    }
                }
                return null;
            }
        }
        protected string CurrentFacilityId
        {
            get
            {
                var user = httpContextAccessor.HttpContext.User.FindFirst(CustomClaimTypes.FacilityId);
                if (user == null)
                    return Guid.Empty.ToString();
                return user.Value;
            }
        }
        protected string CurrentDepartmentId
        {
            get
            {
                var user = httpContextAccessor.HttpContext.User.FindFirst(CustomClaimTypes.DepartmentId);
                if (user == null)
                    return Guid.Empty.ToString();
                return user.Value;
            }
        }
        protected char CurrentLanguage
        {
            get
            {
                if (EnabledLanguageCaching)
                {
                    var result = redisHelper.Get<char>(0, CurrentUserId + "_" + CustomClaimTypes.Language);
                    if (result == 'e' || result == 'l')
                        return result;
                    else
                        return GetAcceptLanguage();
                }
                return GetAcceptLanguage();
            }
            set
            {
                if (EnabledLanguageCaching)
                    redisHelper.Add(DefaultDatabase, CurrentUserId + "_" + CustomClaimTypes.Language, value);
            }
        }

        private char GetAcceptLanguage()
        {
            try
            {
                var userLangs = httpContextAccessor.HttpContext.Request.Headers["Accept-Language"].ToString();
                var firstLang = userLangs.Split(',').FirstOrDefault();
                var defaultLang = string.IsNullOrEmpty(firstLang) ? "local" : firstLang;
                return defaultLang.ToString().ToLower() == "local" ? DefaultLanguage : 'e';
            }
            catch (Exception ex)
            {
                return DefaultLanguage;
            }
        }
    }
}
