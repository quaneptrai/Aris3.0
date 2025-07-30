using Aris3._0.Application.DTOs;
using Aris3._0.Domain.Entities;
using Aris3._0.Infrastructure.Data.Context;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Aris3._0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly HttpClient client;
        private readonly ArisDbContext dbContext;
        private readonly IMapper mapper;

        public CategoryController(HttpClient client, ArisDbContext dbContext, IMapper mapper)
        {
            this.client = client;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllNewUpdateCategory()
        {
            HttpResponseMessage response;
            response = await client.GetAsync($"https://phimapi.com/the-loai");
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Failed to fetch data");
            var content = await response.Content.ReadAsStringAsync();
            var jObj = JArray.Parse(content);
            if (jObj == null)
                return BadRequest("No Category");
            var cateArray = jObj.ToObject<List<CategoryDto>>();
            return Ok(cateArray);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCategoryToTable()
        {
            HttpResponseMessage response;
            response = await client.GetAsync($"https://phimapi.com/the-loai");
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Failed to fetch data");
            var content = await response.Content.ReadAsStringAsync();
            var jObj = JArray.Parse(content);
            if (jObj == null) return BadRequest(string.Empty);
            var cateArray = jObj.ToObject<List<CategoryDto>>();

            foreach (var item in cateArray)
            {
                bool exists = dbContext.categories.Any(c => c.Slug == item.Slug);
                if (!exists)
                {
                    var entity = mapper.Map<Category>(item);
                    dbContext.categories.Add(entity);
                }
            }
            await dbContext.SaveChangesAsync();
            return Ok("Categories updated");
        }
    }
}
