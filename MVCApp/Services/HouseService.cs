using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MVCAppData;

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
        public async Task<IEnumerable<House>> GetAllHouses()
        {
            return await context.Houses1.Where(p=>p.status==1).ToListAsync();
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
    }
}
