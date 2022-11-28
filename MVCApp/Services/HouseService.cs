using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MVCAppData;
using System.Linq;

namespace MVCApp.Services
{
    public class HouseService
    {
        private HouseDataContext context;
        public IMemoryCache cache;

        private int lastId;

        public HouseService(HouseDataContext context, IMemoryCache cache)
        {
            this.context = context;
            this.cache = cache;

            lastId = context.Houses1.Count();
        }

        ///////////////////////////////////////////////////////////
        public async Task<IEnumerable<House>> GetForStats()
        {
            return await context.Houses1.ToListAsync();
        }
        public async Task<IEnumerable<House>> GetAllHouses()
        {
            return await context.Houses1.Where(p => p.status == 1).ToListAsync();
        }

        ///////////////////////////////////////////////////////////
        public async Task AddHouse(House house)
        {
            context.Houses1.Add(house);
            int n = await context.SaveChangesAsync();
            if (n > 0)
            {
                cache.Set(lastId += 1, house, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            }
        }

        ///////////////////////////////////////////////////////////
        public async Task<House> GetHouse(int id)
        {

            House house = null;
            if (!cache.TryGetValue(id, out house))
            {

                house = await context.Houses1.FirstOrDefaultAsync(p => p.Id == id);
                if (house != null)
                {
                    cache.Set(house.Id, house,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return house;
        }

        ///////////////////////////////////////////////////////////
        public async Task<bool> DropHouse(int id)
        {
            if (cache.TryGetValue(id, out House _house))
            {
                _house.status = 0;
                return true;
            }

            var house = await context.Houses1.FirstOrDefaultAsync(p => p.Id == id);
            if (house == null)
            {
                return false;
            }

            house.status = 0;
            await context.SaveChangesAsync();
            return true;
        }

        ///////////////////////////////////////////////////////////
        public async Task EditUser(House house)
        {
            House _house = await GetHouse(house.Id);
            _house.Title = house.Title;
            _house.Price = house.Price;
            _house.PublicationDate = house.PublicationDate;
            _house.Geo_Lat = house.Geo_Lat;
            _house.Geo_Lon = house.Geo_Lon;
            _house.status = 1;
            _house.file = house.file;

            context.Houses1.Update(_house);
            await context.SaveChangesAsync();
        }

        ///////////////////////////////////////////////////////////

        public async Task<HouseStatistiks?> Statistics()
        {
            var houses = await GetForStats();
            if (houses.Count() == 0)
            {
                return null;
            }

            List<DateTime> vals = DateStat(houses);

            var stats = new HouseStatistiks()
            {
                CountHouses = houses.Count(),
                AdAgeStats = new()
                {
                    Max = vals[1],
                    Min = vals[0]
                },
                PriceStats = new()
                {
                    Max = houses.Where(p => p.status == 1).Max(x => x.Price),
                    Min = houses.Where(p => p.status == 1).Min(x => x.Price),
                    Ave = (int)houses.Where(p => p.status == 1).Average(x => x.Price)
                },
                NumberOfActiveHouses = houses.Where(x => x.status == 1).Count(),
                NumberOfInactiveHouses = houses.Where(x => x.status == 0).Count()
            };
 

            return stats;
        }

        public List<DateTime> DateStat(IEnumerable<House> houses)
        {
            List<DateTime> days = new();
            int x = 0;

            foreach(var house in houses.Where(p =>p.status==1))
            {
                days.Add(house.PublicationDate);
                x++;
            }
            List<DateTime> result = new();

            result.Add(days.AsQueryable().Min());
            result.Add(days.AsQueryable().Max());


            return result;
        }

    }
}