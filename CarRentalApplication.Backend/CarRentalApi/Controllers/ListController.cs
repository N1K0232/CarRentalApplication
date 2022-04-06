using CarRentalApi.BusinessLayer.Services;
using CarRentalApi.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ListController : ControllerBase
    {
        private readonly IListService listService;

        public ListController(IListService listService)
        {
            this.listService = listService;
        }

        [HttpGet("GetPeople")]
        [ProducesResponseType(200, Type = typeof(List<Person>))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> GetPeople()
        {
            var people = await listService.GetPeopleAsync();
            if (people == null)
            {
                return NotFound("No rows found");
            }

            return Ok(people);
        }

        [HttpGet("GetReservations")]
        [ProducesResponseType(200, Type = typeof(List<Reservation>))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> GetReservations()
        {
            var reservations = await listService.GetReservationsAsync();
            if (reservations == null)
            {
                return NotFound("No rows found");
            }

            return Ok(reservations);
        }

        [HttpGet("GetVehicles")]
        [ProducesResponseType(200, Type = typeof(List<Vehicle>))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> GetVehicles()
        {
            var vehicles = await listService.GetVehiclesAsync();
            if (vehicles == null)
            {
                return NotFound("No rows found");
            }

            return Ok(vehicles);
        }
    }
}