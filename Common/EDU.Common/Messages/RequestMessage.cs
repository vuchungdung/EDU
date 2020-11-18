using System;
using System.Collections.Generic;
using System.Net.Http;

namespace EDU.Common.Message
{ 
    public class RequestMessage
    {
        public string Module { get; set; }
        public string Url { get; set; }
        public HttpMethod Method { get; set; }
        public string ContentType { get; set; }
        public string AcceptType { get; set; }
        public string Data { get; set; }
        public double TimeOut { get; set; }
        public Guid? FacilityId { get; set; }
        public Guid? UserId { get; set; }
        public string UserName { get; set; }
        public Dictionary<string, string> headerKeys { get; set; }
        //public RequestMessage(string module, string url, HttpMethod method, string contentType, string acceptType, string data, int timeOut, Guid? facilityId, Guid? userId, string userName)
        //{
        //    Module = module;
        //    Url = url;
        //    Method = method;
        //    ContentType = contentType;
        //    AcceptType = acceptType;
        //    Data = data;
        //    TimeOut = timeOut;
        //    FacilityId = facilityId;
        //    UserId = userId;
        //    UserName = userName;
        //    headerKeys = new Dictionary<string, string>();
        //} 
    }
}