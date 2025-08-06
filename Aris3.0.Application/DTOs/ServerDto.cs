using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Aris3._0.Application.DTOs.EpisodeDto;

namespace Aris3._0.Application.DTOs
{
    public class ServerDto
    {
        public int Id { get; set; }
        [JsonProperty("server_name")]
        public string ServerName { get; set; }

        public int EpisodeId { get; set; }
        public EpisodeDto Episode { get; set; }

        public ICollection<ServerDataDto> ServerDataList { get; set; }
    }
}
