using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.CountryDto;
using api.Helper;
using api.Models;

namespace api.Repo.Interface
{
    public interface ICountryRepository
    {
        Task<object> AddCountry(AddCountryDto dto);
        Task<PagedList<Country>> GetCountries(CountryParams model);
        Task<Country> GetCountry(int Id);
        Task<object> GetCountryById(int Id, int PageNumber, int PageSize);
        Task<object> UpdateCountry (UpdateCountryDto country);
        Task<object> DeleteCountry(int id);
        bool CountryExists(string name);
    }
}