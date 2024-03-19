using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.CountryDto;
using api.Dto.HotelDto;
using api.Helper;
using api.Models;
using api.Response;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api.Repo.Interface
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        public CountryRepository(DataContext dataContext, IMapper mapper) : base(dataContext)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
        }

        public async Task<PagedList<Country>> GetCountries(CountryParams model)
        {
            var data = dataContext.Countries.AsQueryable();

            if (model.Id > 0)
            {
                data = data.Where(x => x.Id == model.Id);
            }

            if (!string.IsNullOrEmpty(model.Name))
            {
                data = data.Where(x => x.Name.ToLower().Contains(model.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(model.ShortName))
            {
                data = data.Where(x => x.ShortName.ToLower().Contains(model.ShortName.ToLower()));
            }

            return await PagedList<Country>.CreateAsync(data, model.PageNumber, model.PageSize);

        }

        public async Task<object> GetCountryById(int id, int PageNumber, int PageSize)
        {
            if(PageNumber < 1)
            {
                PageNumber = 1;
            }

            if (PageSize < 1)
            {
                PageSize = 10;
            }

            var data = await GetAsync(id);
            var hotels = dataContext.Hotels.Where(x=> x.CountryId == data.Id).AsQueryable();

            var hotelList = mapper.Map<ICollection<GetHotelDto>>(await PagedList<Hotel>.CreateAsync(hotels, PageNumber, PageSize));

            return new SingleCountryDto
            {
                Id = data.Id,
                Name = data.Name,
                ShortName = data.ShortName,
                Hotels = hotelList
            };
            
        }
    }
}