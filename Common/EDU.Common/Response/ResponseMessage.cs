using System.Collections.Generic;
using EDU.Common;

namespace EDU.Common.Response
{
    /// <summary>
    /// Api Response original
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseMessage<T>
    {
        /// <summary>
        /// MessageCode
        /// </summary>
        public string MessageCode { get; set; }
        /// <summary>
        /// Data object
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// List broken rule(s) when model is invalid
        /// </summary>
        public List<ValidationRule> BrokenRules { get; set; }
        /// <summary>
        /// Json extra info
        /// </summary>
        public string ExtraInfo { get; set; }
    }
}
