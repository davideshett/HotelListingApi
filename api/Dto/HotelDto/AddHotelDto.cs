using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dto.HotelDto
{
    public class AddHotelDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        public double Rating { get; set; }
        [Required]
        public int CountryId { get; set; }
    }
}