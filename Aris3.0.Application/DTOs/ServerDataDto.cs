using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aris3._0.Application.DTOs
{
    public class ServerDataDto
    {
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("slug")]
        public string Slug { get; set; }
        [JsonProperty("filename")]
        public string Filename { get; set; }
        [JsonProperty("link_embed")]
        public string LinkEmbed { get; set; }
        [JsonProperty("link_m3u8")]
        public string LinkM3u8 { get; set; }

        public int ServerId { get; set; }
        public ServerDto Server { get; set; }
    }
}
