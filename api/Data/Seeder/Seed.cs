using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace api.Data.Seeder
{
    public class Seed
    {
        public static async Task SeedCountry(DataContext _context)
        {
            // var serializer = new JsonSerializer();
            // List<Country> teachers = new();
            // using (var streamReader = new StreamReader("Data/Seeder/CountryTableSeeder.json"))
            // using (var textReader = new JsonTextReader(streamReader))
            // {
            //     teachers = serializer.Deserialize<List<Country>>(textReader);
            // }

            if (await _context.Countries.AnyAsync()) return;
            var CountryData = await File.ReadAllTextAsync("Data/Seeder/CountryTableSeeder.json");
            var Countrys = JsonConvert.DeserializeObject<List<Country>>(CountryData);
            foreach (var Country in Countrys)
            {
                _context.Countries.Add(Country);
            }
            await _context.SaveChangesAsync();
        }
    }

}