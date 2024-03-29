using System.Collections.Generic;

namespace TestShopApplication.Shared.ApiModels
{
    public sealed class Response<T>
    {
        public T Result { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}