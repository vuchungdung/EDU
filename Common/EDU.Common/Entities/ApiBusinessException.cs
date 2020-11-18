using System;
using System.Net;
using System.Runtime.Serialization;

namespace EDU.Common.Entities
{
    /// <summary>
    /// Api Business Exception
    /// </summary>
    [Serializable]
    [DataContract]
    public class ApiBusinessException : Exception, IApiExceptions
    {
        #region Public Serializable properties.
        [DataMember]
        public int ErrorCode { get; set; }
        [DataMember]
        public string MessageCode { get; set; }
        [DataMember]
        public string ErrorDescription { get; set; }
        [DataMember]
        public HttpStatusCode HttpStatus { get; set; }

        string reasonPhrase = "ApiBusinessException";

        [DataMember]
        public string ReasonPhrase
        {
            get { return this.reasonPhrase; }

            set { this.reasonPhrase = value; }
        }
        #endregion

        #region Public Constructor.
        /// <summary>
        /// Public constructor for Api Business Exception
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="messageCode"></param>
        /// <param name="errorDescription"></param>
        /// <param name="httpStatus"></param>
        public ApiBusinessException(int errorCode, string messageCode, string errorDescription, HttpStatusCode httpStatus)
        {
            ErrorCode = errorCode;
            MessageCode = messageCode;
            ErrorDescription = errorDescription;
            HttpStatus = httpStatus;
        }
        #endregion

    }
}