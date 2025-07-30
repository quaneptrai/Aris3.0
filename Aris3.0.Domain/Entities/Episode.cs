using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aris3._0.Domain.Entities
{
    public class Episode
    {
        [Key]
        public int Id { get; set; }

        public string ServerName { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public string Filename { get; set; }

        public string LinkEmbed { get; set; }

        public string LinkM3U8 { get; set; }

        public string FilmId { get; set; } 
        public Film Film { get; set; }
    }
}
