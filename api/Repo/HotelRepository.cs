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
    public class HotelRepository : IHotelRepository
    {
        private readonly DataContext dataContext;

        public HotelRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public async Task<object> AddHotel(AddHotelDto dto)
        {
            if (HotelExists(dto.Name))
            {
                return new GenericResponse
                {
                    StatusCode = 400,
                    IsSuccessful = false,
                    Message = "Hotel already exist"
                };
            }


            var data = new Hotel
            {
                Name = dto.Name,
                CountryId = dto.CountryId,
                Address = dto.Address,
                Rating = dto.Rating
            };

            await dataContext.Hotels.AddAsync(data);
            await dataContext.SaveChangesAsync();

            return new GenericResponse
            {
                StatusCode = 201,
                IsSuccessful = true,
                Message = "Hotel added successfully"
            };
        }

        public bool HotelExists(string name)
        {
            var data = dataContext.Hotels.FirstOrDefault(x => x.Name.ToLower().Equals(name.ToLower()));
            if (data == null)
            {
                return false;
            }

            return true;
        }

        public async Task<object> DeleteHotel(int id)
        {
            var data = await dataContext.Hotels.FirstOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                return new GenericResponse
                {
                    StatusCode = 400,
                    IsSuccessful = false,
                    Message = "Hotel does not exist"
                };
            }

            dataContext.Hotels.Remove(data);
            return new GenericResponse
            {
                StatusCode = 204,
                IsSuccessful = false,
                Message = "Hotel deleted successfully"
            };

        }

        public async Task<PagedList<Hotel>> GetHotels(HotelParams model)
        {
            var data = dataContext.Hotels.AsQueryable();

            if (model.Id > 0)
            {
                data = data.Where(x => x.Id == model.Id);
            }

            if (model.Rating > 0)
            {
                data = data.Where(x => x.Rating == model.Rating);
            }

            if (model.CountryId > 0)
            {
                data = data.Where(x => x.CountryId == model.CountryId);
            }

            if (!string.IsNullOrEmpty(model.Name))
            {
                data = data.Where(x => x.Name.ToLower().Contains(model.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(model.Address))
            {
                data = data.Where(x => x.Address.ToLower().Contains(model.Address.ToLower()));
            }

            return await PagedList<Hotel>.CreateAsync(data, model.PageNumber, model.PageSize);
        }

        public async Task<Hotel> GetHotel(int Id)
        {
            var data = await dataContext.Hotels
            .Include(x => x.Country)
            .FirstOrDefaultAsync(x => x.Id == Id);
            return data;
        }

        public async Task<object> UpdateHotel(UpdateHotelDto Hotel)
        {
            var data = await dataContext.Hotels.FirstOrDefaultAsync(x => x.Id == Hotel.Id);

            data.Name = Hotel.Name;
            data.Address = Hotel.Address;
            data.CountryId = Hotel.CountryId;
            data.Rating = Hotel.Rating;
            dataContext.Update(data);
            await dataContext.SaveChangesAsync();
            return new GenericResponse
            {
                Message = "Hotel updated successfully",
                IsSuccessful = true,
                StatusCode = 200
            };
        }
    }
}