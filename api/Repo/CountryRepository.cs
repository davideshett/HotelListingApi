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
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        public CountryRepository(DataContext dataContext, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
        }
        public async Task<object> AddCountry(AddCountryDto dto)
        {
            if(CountryExists(dto.Name))
            {
                return new GenericResponse{
                    StatusCode = 400,
                    IsSuccessful = false,
                    Message = "Country already exist"
                };
            }


            var data = new Country
            {
                Name = dto.Name,
                ShortName = dto.ShortName
            };

            await dataContext.Countries.AddAsync(data);
            await dataContext.SaveChangesAsync();
            
            return new GenericResponse
            {
                StatusCode = 201,
                IsSuccessful = true,
                Message = "Country added successfully"
            };
        }

        public bool CountryExists(string name)
        {
            var data = dataContext.Countries.FirstOrDefault(x => x.Name.ToLower().Equals(name.ToLower()));
            if (data == null)
            {
                return false;
            }

            return true;
        }

        public async Task<object> DeleteCountry(int id)
        {
            var data = await dataContext.Countries.FirstOrDefaultAsync(x=> x.Id == id);
            if(data == null)
            {
                return new GenericResponse
                {
                    StatusCode = 400,
                    IsSuccessful = false,
                    Message = "Country does not exist"
                };
            }

            dataContext.Countries.Remove(data);
            return new GenericResponse
            {
                StatusCode = 204,
                IsSuccessful = false,
                Message = "Country deleted successfully"
            };

        }

        public async Task<PagedList<Country>> GetCountries(CountryParams model)
        {
            var data = dataContext.Countries.AsQueryable();

            if(model.Id > 0)
            {
                data = data.Where(x=> x.Id == model.Id);
            }

            if(! string.IsNullOrEmpty(model.Name))
            {
                data = data.Where(x=> x.Name.ToLower().Contains(model.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(model.ShortName))
            {
                data = data.Where(x => x.ShortName.ToLower().Contains(model.ShortName.ToLower()));
            }

            return await PagedList<Country>.CreateAsync(data,model.PageNumber,model.PageSize);
        }

        public async Task<Country> GetCountry(int Id)
        {
            var data = await dataContext.Countries
            .Include(x=> x.Hotels)
            .FirstOrDefaultAsync(x=> x.Id == Id);
            return data;
        }

        public async Task<object> GetCountryById(int Id, int PageNumber, int PageSize)
        {
            if(PageNumber < 1)
            {
                PageNumber = 1;
            }

            if(PageSize < 1)
            {
                PageSize = 10;
            }
            
            var data = await dataContext.Countries.FirstOrDefaultAsync(x=> x.Id == Id);
            var hotelData = dataContext.Hotels.Where(x=> x.CountryId == data.Id).AsQueryable();

            var hotelList = mapper.Map<ICollection<GetHotelDto>>(await PagedList<Hotel>.CreateAsync(hotelData, PageNumber, PageSize));

            var returnObject = new SingleCountryDto
            {
                Id = data.Id,
                Name = data.Name,
                ShortName = data.ShortName,
                Hotels = hotelList
            };

            return returnObject;
        }

        public async Task<object> UpdateCountry(UpdateCountryDto country)
        {
            var data = await dataContext.Countries.FirstOrDefaultAsync(x => x.Id == country.Id);

            data.Name = country.Name;
            data.ShortName = country.ShortName;
            dataContext.Update(data);
            await dataContext.SaveChangesAsync();
            return new GenericResponse
            {
                Message = "Country updated successfully",
                IsSuccessful = true,
                StatusCode = 200
            };
        }
    }
}