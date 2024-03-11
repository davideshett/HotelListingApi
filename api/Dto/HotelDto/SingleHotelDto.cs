using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.CountryDto;

namespace api.Dto.HotelDto
{
    public class SingleHotelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }

        public UpdateCountryDto Country { get; set; }
    }
}