using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aris3._0Fe.Models
{
    public class Server
    {
        [Key]
        public int Id { get; set; }
        [JsonProperty("server_name")]
        public string ServerName { get; set; }

        public int EpisodeId { get; set; }
        public Episode Episode { get; set; }

        public ICollection<ServerData> ServerDataList { get; set; }
    }
}
