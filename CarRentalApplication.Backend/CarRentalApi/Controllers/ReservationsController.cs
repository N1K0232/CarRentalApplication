using CarRentalApi.BusinessLayer.Services;
using CarRentalApi.Shared.Models;
using CarRentalApi.Shared.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalApi.Controllers
{
    [ApiController]
    [Route("api/controller")]
    [Produces("application/json")]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationsService reservationsService;

        public ReservationsController(IReservationsService reservationsService)
        {
            this.reservationsService = reservationsService;
        }

        [HttpGet("GetReservation")]
        [ProducesResponseType(200, Type = typeof(Reservation))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> GetPerson(Guid id)
        {
            var reservation = await reservationsService.GetAsync(id);
            if (reservation != null)
            {
                return Ok(reservation);
            }

            return NotFound("the reservation with the specified id wasn't found");
        }

        [HttpPost("SaveReservation")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> SavePerson([FromBody] SaveReservationRequest request)
        {
            request.Id = null;
            var reservation = await reservationsService.SaveAsync(request);
            if (reservation != null)
            {
                return Ok("the reservation was successfully registrated");
            }

            return BadRequest("problem occurred during registration");
        }

        [HttpPut("UpdateReservation")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> UpdatePerson([FromBody] SaveReservationRequest request)
        {
            var reservation = await reservationsService.SaveAsync(request);
            if (reservation != null)
            {
                return Ok("the reservation was successfully updated");
            }

            return BadRequest("problem occurred during updating");
        }
    }
}