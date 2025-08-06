using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aris3._0.Application.DTOs
{
    public class EpisodeDto
    {
        [JsonProperty("server_name")]
        public string ServerName { get; set; }

        [JsonProperty("server_data")]
        public List<ServerDataDto> ServerDataList { get; set; }

    }
}
