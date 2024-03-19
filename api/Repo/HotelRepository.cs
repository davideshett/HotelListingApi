using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.CountryDto;
using api.Dto.HotelDto;
using api.Helper;
using api.Models;
using api.Repo.Interface;
using api.Response;
using Microsoft.EntityFrameworkCore;

namespace api.Repo
{
    public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
    {
        private readonly DataContext dataContext;

        public HotelRepository(DataContext dataContext): base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public Task<Hotel> GetHotel(int id)
        {
            var data = dataContext.Hotels.Include(x=> x.Country).FirstOrDefaultAsync();
            return data;
        }

        public async Task<PagedList<Hotel>> GetHotels(HotelParams model)
        {
            var data = dataContext.Hotels
            .Include(x=> x.Country)
            .OrderBy(x=> x.Id)
            .AsQueryable();

            if(model.Id > 0)
            {
                data = data.Where(x=> x.Id == model.Id);
            }

            if(!string.IsNullOrEmpty(model.Name))
            {
                data = data.Where(x=> x.Name.ToLower().Contains(model.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(model.Address))
            {
                data = data.Where(x => x.Address.ToLower().Contains(model.Address.ToLower()));
            }

            if (model.Rating > 0)
            {
                data = data.Where(x => x.Rating == model.Rating);
            }

            if (model.CountryId > 0)
            {
                data = data.Where(x => x.CountryId == model.CountryId);
            }

            return await PagedList<Hotel>.CreateAsync(data, model.PageNumber, model.PageSize);


        }
    }
}