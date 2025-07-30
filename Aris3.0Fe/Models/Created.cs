using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aris3._0Fe.Models
{
    public class Created
    {
        [Key]
        public int id { get; set; }
        [JsonProperty("time")]
        public string Time { get; set; }
    }
}
