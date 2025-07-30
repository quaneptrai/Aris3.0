using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Aris3._0.Application.DTOs
{
    public class ActorDto
    {
        public int id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
