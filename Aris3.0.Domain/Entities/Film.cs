using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aris3._0.Domain.Entities
{
    public class Film
    {
        [Key]
        [JsonProperty("_id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("slug")]
        public string Slug { get; set; }
        [JsonProperty("origin_name")]
        public string OriginName { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("poster_url")]
        public string PosterUrl { get; set; }
        [JsonProperty("thumb_url")]
        public string ThumbUrl { get; set; }
        [JsonProperty("is_copyright")]
        public bool IsCopyright { get; set; }
        [JsonProperty("sub_docquyen")]
        public bool SubDocquyen { get; set; }
        [JsonProperty("chieurap")]
        public bool ChieuRap { get; set; }
        [JsonProperty("trailer_url")]
        public string TrailerUrl { get; set; }
        [JsonProperty("time")]
        public string Time { get; set; }
        [JsonProperty("episode_current")]
        public string EpisodeCurrent { get; set; }
        [JsonProperty("episode_total")]
        public string EpisodeTotal { get; set; }
        [JsonProperty("quality")]
        public string Quality { get; set; }
        [JsonProperty("lang")]
        public string Lang { get; set; }
        [JsonProperty("notify")]
        public string Notify { get; set; }
        [JsonProperty("showtimes")]
        public string Showtimes { get; set; }
        [JsonProperty("year")]
        public int? Year { get; set; }
        [JsonProperty("view")]
        public int View { get; set; }
        public int Like { get; set; }
        public ICollection<Director> Directors { get; set; } = new List<Director>();
        public ICollection<Actor> Actors { get; set; } = new List<Actor>();

        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<Country> Countries { get; set; } = new List<Country>();
        public ICollection<Episode> Episodes { get; set; } = new List<Episode>();

        [JsonProperty("tmdb")]
        public Tmbd Tmdb { get; set; }

        [JsonProperty("created")]
        public Created Created { get; set; }

        [JsonProperty("modified")]
        public Modified Modified { get; set; }
    }
}
