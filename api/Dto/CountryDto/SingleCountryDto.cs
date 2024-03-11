using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.HotelDto;
using api.Helper;
using api.Models;

namespace api.Dto.CountryDto
{
    public class SingleCountryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public ICollection<GetHotelDto> Hotels { get; set; }

    }
}