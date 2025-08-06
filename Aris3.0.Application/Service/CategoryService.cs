using Aris3._0.Application.DTOs;
using Aris3._0.Application.Interface.Repositories;
using Aris3._0.Application.Interface.Service;
using Aris3._0.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Aris3._0.Application.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _client;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IFilmRepository _filmRepository;

        public CategoryService(HttpClient client,IUnitOfWork unitOfWork,IMapper mapper)
        {
            _client = client;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<List<CategoryDto>> FetchCategoriesFromApiAsync()
        {
            HttpResponseMessage response;
            response = await _client.GetAsync($"https://phimapi.com/the-loai");
            if (!response.IsSuccessStatusCode)
                return null;
            var content = await response.Content.ReadAsStringAsync();
            var jObj = JArray.Parse(content);
            if (jObj == null)
                return null;
            var cateArray = jObj.ToObject<List<CategoryDto>>();
            return cateArray;
        }

        public async Task<string> UpdateCategoriesToDbAsync()
        {
            HttpResponseMessage response;
            var repo = unitOfWork.Categorys;
            response = await _client.GetAsync($"https://phimapi.com/the-loai");
            if (!response.IsSuccessStatusCode)
                return ("Failed to fetch data");
            var content = await response.Content.ReadAsStringAsync();
            var jObj = JArray.Parse(content);
            if (jObj == null) return(string.Empty);
            var cateArray = jObj.ToObject<List<CategoryDto>>();

            foreach (var item in cateArray)
            {
                bool exists = await unitOfWork.Categorys.Any(x=>x.Slug == item.Slug);
                if (!exists)
                {
                    var entity = mapper.Map<Category>(item);
                    unitOfWork.Categorys.AddAsync(entity);
                }
            }
            await unitOfWork.SaveChangesAsync();
            return "Categories updated";
        }
    }
}

