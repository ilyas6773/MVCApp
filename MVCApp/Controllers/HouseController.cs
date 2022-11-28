using Microsoft.AspNetCore.Mvc;
using MVCAppData;
using MVCApp.Services;

namespace MVCApp.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class HouseController : ControllerBase
    {
        HouseService houseService;
        private readonly HouseDataContext context;
        public HouseController(HouseDataContext context, HouseService service)
        {
            houseService = service;
            this.context = context;
        }

        /////////////////////////GetAll//////////////////////////////
        [HttpGet("GetAll")]
        public async Task<IEnumerable<House>> GetAll()
        {
            
            return await houseService.GetAllHouses();
        }

        /////////////////////////Add//////////////////////////////
        [HttpPost("Add")]
        public async Task<IActionResult> AddHouse(House house)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await houseService.AddHouse(house);
            return Ok();
        }

        /////////////////////////GetById//////////////////////////////
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            House house = await houseService.GetHouse(id);
            if (house == null)
            {
                return NotFound();
            }
            return Ok(house);
        }

        /////////////////////////Delete//////////////////////////////
        [HttpDelete("Delete")]
        public async Task<IActionResult> DropHouse(int id)
        {
            bool isExists = await houseService.DropHouse(id);
            if (isExists == false)
                return NotFound();

            return Ok();
        }

        /////////////////////////Edit//////////////////////////////
        [HttpPut("Edit")]
        public async Task<IActionResult> EditUser(House house)
        {
            await houseService.EditUser(house);
            return Ok();
        }

        /////////////////////////Statistika//////////////////////////////
        [HttpGet("GetStats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = await houseService.Statistics();

            return Ok(stats);
        }
    }
}