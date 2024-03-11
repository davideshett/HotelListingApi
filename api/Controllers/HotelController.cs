using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.CountryDto;
using api.Dto.HotelDto;
using api.Helper;
using api.Repo.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    public class HotelController : BaseController
    {
        private readonly IHotelRepository repo;
        private readonly IMapper mapper;

        public HotelController(IHotelRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetHotels([FromQuery] HotelParams model)
        {
            var data = await repo.GetHotels(model);
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
                Data = mapper.Map<ICollection<GetHotelDto>>(data)
            });

        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetHotel(int id)
        {
            var data = await repo.GetHotel(id);
            return Ok(new {
                Message = "Successful",
                IsSuccessful = true,
                StatusCode = 200,
                Data = mapper.Map<SingleHotelDto>(data)
            });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddHotel(AddHotelDto model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            var data = await repo.AddHotel(model);
            return Ok(data);
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateHotel(UpdateHotelDto model)
        {
            var data = await repo.UpdateHotel(model);
            return Ok(data);
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var data = await repo.DeleteHotel(id);
            return Ok(data);
        }
    }
}