using Newtonsoft.Json;

namespace Cover.Neo4J.Domain.Core.Entities
{
    public class UserDto
    {
        [JsonProperty(PropertyName = "login")]
        public string Login { get; set; }

        [JsonProperty(PropertyName = "fullName")]
        public string FullName { get; set; }
    }
}
