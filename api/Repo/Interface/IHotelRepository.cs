using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.CountryDto;
using api.Dto.HotelDto;
using api.Helper;
using api.Models;

namespace api.Repo.Interface
{
    public interface IHotelRepository: IGenericRepository<Hotel> 
    {
       Task<PagedList<Hotel>> GetHotels(HotelParams model);
       Task<Hotel> GetHotel(int id);
    }
}