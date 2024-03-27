using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.CountryDto;
using api.Dto.HotelDto;
using api.Helper;
using api.Models;
using api.Repo.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace api.Controllers
{
    public class HotelController : BaseController
    {
        private readonly IHotelRepository repo;
        private readonly IMapper mapper;
        private readonly ICountryRepository countryRepository;

        public HotelController(IHotelRepository repo, IMapper mapper,ICountryRepository countryRepository)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.countryRepository = countryRepository;
        }
        [HttpGet]
        [AllowAnonymous]
        [EnableQuery]
        public async Task <IActionResult> GetAll()
        {
            var hotelList = await repo.GetAllAsync();
            return Ok(hotelList);
        }

        // [HttpGet]
        // [AllowAnonymous]
        // [EnableQuery]
        // public async Task<IActionResult> GetHotels([FromQuery] HotelParams model)
        // {
        //     var data = await repo.GetHotels(model);
        //     if(data == null)
        //     {
        //         return BadRequest(new {
        //             Message = "Error",
        //             StatusCode = 401,
        //             IsSuccessful = false
        //         });
        //     }

        //     Response.AddPagination(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);

        //     return Ok(new {
        //         data.CurrentPage,
        //         data.PageSize,
        //         data.TotalCount,
        //         data.TotalPages,
        //         Message = "Success",
        //         StatusCode = 200,
        //         IsSuccessful = true,
        //         Data = mapper.Map<ICollection<GetHotelDto>>(data)
        //     });

        // }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetHotel(int id)
        {
            if(! await repo.Exists(id))
            {
                return NotFound();
            }


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

            if(! await countryRepository.Exists(model.CountryId))
            {
                return NotFound(new {
                    Message = "Country does not exist",
                    IsSuccessful = false,
                    StatusCode = 404
                });
            }

            var hotel = mapper.Map<Hotel>(model);

            var data = await repo.AddAsync(hotel);
            return Ok(data);
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateHotel(UpdateHotelDto model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!await countryRepository.Exists(model.CountryId))
            {
                return NotFound(new
                {
                    Message = "Country does not exist",
                    IsSuccessful = false,
                    StatusCode = 404
                });
            }

            var hotel = mapper.Map<Hotel>(model);
            await repo.UpdateAsync(hotel);
            return Ok(new{
                Message = "Success",
                IsSuccessful = true,
                StatusCode = 201
            });
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            if(! await repo.Exists(id))
            {
                return NotFound(new
                {
                    Message = "Hotel does not exist",
                    IsSuccessful = false,
                    StatusCode = 404
                });
            }
            var data = await repo.DeleteAsync(id);
            return Ok(data);
        }
    }
}