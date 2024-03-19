using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.CountryDto;
using api.Helper;
using api.Models;
using api.Repo.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    public class CountryController : BaseController
    {
        private readonly ICountryRepository repo;
        private readonly IMapper mapper;

        public CountryController(ICountryRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCountries([FromQuery] CountryParams model)
        {
            
            var data = await repo.GetCountries(model);
            if(data == null)
            {
                return BadRequest(new {
                    Message = "Error",
                    StatusCode = 401,
                    IsSuccessful = false
                });
            }

            Response.AddPagination(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);

            return Ok(new {
                data.CurrentPage,
                data.PageSize,
                data.TotalCount,
                data.TotalPages,
                Message = "Success",
                StatusCode = 200,
                IsSuccessful = true,
                Data = mapper.Map<ICollection<UpdateCountryDto>>(data)
            });

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddCountry(AddCountryDto model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            var country = mapper.Map<Country>(model);

            var data = await repo.AddAsync(country);

            return Ok(data);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCountry(int id,int PageNumber, int PageSize)
        {
            var data = await repo.GetCountryById(id,PageNumber,PageSize);
            return Ok(new{
                HotelsCurrentPage = PageNumber,
                HotelsPageSize = PageSize,
                Data = data
            });
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateCountry(UpdateCountryDto model)
        {
            var country = mapper.Map<Country> (model);
            await repo.UpdateAsync(country);
            return Ok(country);
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await repo.Exists(id);
            if(country == false)
            {
                return NotFound(new {
                    Message = "Not found",
                    IsSuccessful = false,
                    StatusCode = 404
                });
            }
            var data = await repo.DeleteAsync(id);
            return Ok(data);
        }
    }
}