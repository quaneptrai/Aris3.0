using Aris3._0.Application.DTOs;
using Aris3._0.Application.Interface.Repositories;
using Aris3._0.Application.Interface.Service;
using Aris3._0.Domain.Entities;
using Aris3._0.Infrastructure.Data.Context;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;

namespace Aris3._0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly HttpClient client;
        private readonly ArisDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ICategoryService categoryRepository;

        public CategoryController(HttpClient client, ArisDbContext dbContext, IMapper mapper,ICategoryService categoryRepository)
        {
            this.client = client;
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.categoryRepository = categoryRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllNewUpdateCategory()
        {
           var cate = await categoryRepository.FetchCategoriesFromApiAsync();
           return Ok(cate);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCategoryToTable()
        {
           var message =  categoryRepository.UpdateCategoriesToDbAsync();
            if (message != null) {
                return Ok(new
                {
                    msg = message
                });
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
