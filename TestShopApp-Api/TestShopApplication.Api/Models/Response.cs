using System.Collections.Generic;

namespace TestShopApplication.Api.Models
{
    public class Response<T>
    {
        public T Result { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}