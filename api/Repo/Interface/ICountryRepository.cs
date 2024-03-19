using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.CountryDto;
using api.Helper;
using api.Models;
using api.Response;

namespace api.Repo.Interface
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        Task<PagedList<Country>> GetCountries(CountryParams model);
        Task<object> GetCountryById(int id, int PageNumber, int PageSize);
    }
}