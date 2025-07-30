using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aris3._0Fe.Models
{
    public class Tmbd
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("season")]
        public int? Season { get; set; }
        [JsonProperty("vote_average")]
        public float Vote_average { get; set; }
        [JsonProperty("vote_count")]
        public int Vote_count { get; set; }
    }
}
