using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.CountryDto;
using api.Dto.HotelDto;
using api.Models;
using AutoMapper;

namespace api.Helper
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Country, UpdateCountryDto>().ReverseMap();
            CreateMap<Hotel, GetHotelDto>().ReverseMap();
        }
    }
}