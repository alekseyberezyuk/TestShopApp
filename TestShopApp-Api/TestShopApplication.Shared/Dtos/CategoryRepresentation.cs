using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestShopApplication.Shared.Dtos
{
    public sealed class CategoryRepresentation
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public CategoryRepresentation(object c)
        {
            JObject obj = JObject.Parse(JsonConvert.SerializeObject(c));
            this.Id = int.Parse(obj[nameof(Id)].ToString());
            this.Name = obj[nameof(Name)].ToString();
        }
    }
}
