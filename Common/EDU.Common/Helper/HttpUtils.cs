using Easy.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EDU.Common.Message;
using EDU.Common.MediaTypes;

namespace EDU.Common.Helper
{
    public static class HttpUtils
    { 
        public static IConfiguration configuration;
        public static async Task<HttpResponseMessage> Post(string url, RequestMessage req, char currentLanguage)
        {
            if (string.IsNullOrEmpty(req.Data))
            { 
                
                throw new ArgumentException(MessageCodes.ArgumentExceptionData);
            }
            //Xu lu TH call API SSL/TLS: https://
            if (url != null && url.Length > 8 && url.Substring(0, 8) == ("https://"))
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol =  SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                if (bool.Parse(configuration["AppSettings:IGNORE_CERT_HTTPS"]))
                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            }
            if (req.ContentType == MediaTypeHeader.Json)
            {
                var jsonContent = new JSONContent(req.Data, Encoding.UTF8);
                var headers = new Dictionary<string, IEnumerable<string>>
                {
                    {HttpRequestHeader.Accept.ToString(), new []{req.AcceptType} }
                };
                if (req.headerKeys.Count > 0)
                {
                    foreach (var item in req.headerKeys)
                        headers.Add(item.Key, new[] { item.Value });
                }
                if (null != req.FacilityId)
                    headers.Add(MediaTypeHeader.Facility, new[] { req.FacilityId.ToString() });
                if (null != req.UserId)
                    headers.Add(MediaTypeHeader.UserId, new[] { req.UserId.ToString() });
                if (!string.IsNullOrEmpty(req.UserName))
                    headers.Add(MediaTypeHeader.UserName, new[] { req.UserName.ToString() });
                if (!string.IsNullOrEmpty(currentLanguage.ToString()) && currentLanguage != ' ')
                    headers.Add(MediaTypeHeader.CurrentLanguage, new[] { currentLanguage.ToString() });
                try
                {
                    double TimeOut = double.Parse(configuration["AppSettings:ApiTimeOut"]);
                    TimeOut = req.TimeOut > 0 ? req.TimeOut : TimeOut;
                    using (var client = new RestClient(headers, timeout: TimeSpan.FromSeconds(TimeOut)))
                    {
                        var result = await client.PostAsync(url, jsonContent);
                        return result;
                    }
                }
                catch (Exception ex)
                { 
                    throw ex;
                }
            }

            throw new NotImplementedException(MessageCodes.NotSupportedContentType);
        }

        public static async Task<HttpResponseMessage> Post(string url, string Data)
        {
            if (string.IsNullOrEmpty(Data))
            {
                throw new ArgumentException(MessageCodes.ArgumentExceptionData);
            }
            var jsonContent = new JSONContent(Data, Encoding.UTF8);
            try
            {
                double TimeOut = double.Parse(configuration["AppSettings:ApiTimeOut"]);
                using (var client = new RestClient(timeout: TimeSpan.FromSeconds(TimeOut)))
                {
                    var result = await client.PostAsync(url, jsonContent);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            throw new NotImplementedException(MessageCodes.NotSupportedContentType);
        }

        public static async Task<HttpResponseMessage> Post(string url, RequestMessage req, UserInfo userInfo, char currentLanguage)
        {
            if (string.IsNullOrEmpty(req.Data))
            { 
                throw new ArgumentException(MessageCodes.ArgumentExceptionData);
            }
            //Xu lu TH call API SSL/TLS: https://
            if (url != null && url.Length > 8 && url.Substring(0, 8) == ("https://"))
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol =  SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                if (bool.Parse(configuration["AppSettings:IGNORE_CERT_HTTPS"]))
                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            }
            if (req.ContentType == MediaTypeHeader.Json)
            {
                var jsonContent = new JSONContent(req.Data, Encoding.UTF8);
                var headers = new Dictionary<string, IEnumerable<string>>
                {
                    {HttpRequestHeader.Accept.ToString(), new[] {string.IsNullOrEmpty(req.AcceptType) ? MediaTypeHeader.Json : req.AcceptType} }
                };
                if (!string.IsNullOrEmpty(currentLanguage.ToString()) && currentLanguage != ' ')
                    headers.Add(MediaTypeHeader.CurrentLanguage, new[] { currentLanguage.ToString() });
                //check user info & add header
                if (null != userInfo)
                {
                    if (null != userInfo.UserId)
                        headers.Add(MediaTypeHeader.UserId, new[] { userInfo.UserId.ToString() });
                    if (null != userInfo.UserName)
                        headers.Add(MediaTypeHeader.UserName, new[] { userInfo.UserName });
                    if (null != userInfo.FacilityId)
                        headers.Add(MediaTypeHeader.Facility, new[] { userInfo.FacilityId.ToString() });
                    if (null != userInfo.DepartmentId)
                        headers.Add(MediaTypeHeader.Department, new[] { userInfo.DepartmentId.ToString() });
                    if (null != userInfo.IpAddress)
                        headers.Add(MediaTypeHeader.Host, new[] { userInfo.IpAddress });
                    if (null != userInfo.BrowserName)
                        headers.Add(MediaTypeHeader.Browser, new[] { userInfo.BrowserName });
                }
                try
                {
                    double TimeOut = double.Parse(configuration["AppSettings:ApiTimeOut"]);
                    TimeOut = req.TimeOut > 0 ? req.TimeOut : TimeOut;
                    using (var client = new RestClient(headers, timeout: TimeSpan.FromSeconds(TimeOut)))
                    { 
                        var result = await client.PostAsync(url, jsonContent);
                        return result; 
                    }
                }
                catch (Exception ex)
                { 
                    throw ex;
                }
            }

            throw new NotImplementedException(MessageCodes.NotSupportedContentType);
        }

        public static async Task<HttpResponseMessage> Get(string url, RequestMessage req, UserInfo userInfo, char currentLanguage)
        {
            //Xu lu TH call API SSL/TLS: https://
            if (url != null && url.Length > 8 && url.Substring(0, 8) == ("https://"))
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol =   SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                if (bool.Parse(configuration["AppSettings:IGNORE_CERT_HTTPS"]))
                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            }
            if (null != req.Data && !string.IsNullOrEmpty(req.Data))
                url = string.Concat(url, req.Data);
            var headers = new Dictionary<string, IEnumerable<string>>
                {
                    {HttpRequestHeader.Accept.ToString(), new[] {string.IsNullOrEmpty(req.AcceptType) ? MediaTypeHeader.Json : req.AcceptType} }
                };
            if (!string.IsNullOrEmpty(currentLanguage.ToString()) && currentLanguage != ' ')
                headers.Add(MediaTypeHeader.CurrentLanguage, new[] { currentLanguage.ToString() });
            if (null != userInfo)
            {
                if (null != userInfo.UserId)
                    headers.Add(MediaTypeHeader.UserId, new[] { userInfo.UserId.ToString() });
                if (null != userInfo.UserName)
                    headers.Add(MediaTypeHeader.UserName, new[] { userInfo.UserName });
                if (null != userInfo.FacilityId)
                    headers.Add(MediaTypeHeader.Facility, new[] { userInfo.FacilityId.ToString() });
                if (null != userInfo.DepartmentId)
                    headers.Add(MediaTypeHeader.Department, new[] { userInfo.DepartmentId.ToString() });
                if (null != userInfo.IpAddress)
                    headers.Add(MediaTypeHeader.Host, new[] { userInfo.IpAddress });
                if (null != userInfo.BrowserName)
                    headers.Add(MediaTypeHeader.Browser, new[] { userInfo.BrowserName });
            }
            try
            {
                double TimeOut = double.Parse(configuration["AppSettings:ApiTimeOut"]);
                TimeOut =  req.TimeOut > 0 ? req.TimeOut : TimeOut;
                using (var client = new RestClient(headers, timeout: TimeSpan.FromSeconds(TimeOut)))
                { 
                    var result = await client.GetAsync(url);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Task<HttpResponseMessage> Delete(string url, RequestMessage req, char currentLanguage)
        {
            //Xu lu TH call API SSL/TLS: https://
            if (url != null && url.Length > 8 && url.Substring(0, 8) == ("https://"))
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol =  SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                if (bool.Parse(configuration["AppSettings:IGNORE_CERT_HTTPS"]))
                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            }
            throw new NotImplementedException();
        }

        public static Task<HttpResponseMessage> Put(string url, RequestMessage req, char currentLanguage)
        {
            //Xu lu TH call API SSL/TLS: https://
            if (url != null && url.Length > 8 && url.Substring(0, 8) == ("https://"))
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol =  SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                if (bool.Parse(configuration["AppSettings:IGNORE_CERT_HTTPS"]))
                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            }
            throw new NotImplementedException();
        }

        public static string IpAddress()
        {
            string IPAddress = "";
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                }
            }
            return IPAddress; 
        }
        public static string BrowserName(IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }
    } 
}
