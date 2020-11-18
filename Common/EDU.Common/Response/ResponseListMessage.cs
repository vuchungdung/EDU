using System;

namespace EDU.Common.Response
{
    public class ResponseListMessage<T> : ResponseMessage<T>
    {
        /// <summary>
        /// TotalItems
        /// </summary>
        public long TotalItems { get; set; }
        /// <summary>
        /// Page
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// PageSize
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// PageCount
        /// </summary>
        public int PageCount
        {
            get
            {
                if (PageSize == 0) return 0;
                return (int)Math.Ceiling((decimal)TotalItems / PageSize);
            }
            internal set { }
        }

        public static implicit operator ResponseListMessage<T>(string v)
        {
            throw new NotImplementedException();
        }
    }
}
