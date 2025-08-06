using Aris3._0.Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Aris3._0.Application.DTOs
{
    public class FilmDto
    {
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
        public int Year { get; set; }
        [JsonProperty("view")]
        public int View { get; set; }
        [JsonProperty("director")]
        public List<string> DirectorNames { get; set; } = new List<string>();

        [JsonIgnore]
        public ICollection<Director> Directors { get; set; } = new List<Director>();

        public ICollection<ActorDto> Actors { get; set; } = new List<ActorDto>();


        public ICollection<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
        public ICollection<CountryDto> Countries { get; set; } = new List<CountryDto>();
        public ICollection<EpisodeDto> Episodes { get; set; } = new List<EpisodeDto>();

        public int? TmbdId { get; set; }
        [JsonProperty("tmdb")]
        public Tmbd Tmdb { get; set; }

        [JsonProperty("created")]
        public CreatedDto Created { get; set; }

        [JsonProperty("modified")]
        public ModifiedDto Modified { get; set; }

    }
}
