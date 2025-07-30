using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aris3._0.Domain.Entities
{
    public class Actor
    {
        [Key]
        public int id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        public ICollection<Film> Films { get; set; } = new List<Film>();
    }
}
