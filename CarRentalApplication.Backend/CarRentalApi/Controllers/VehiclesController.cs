using CarRentalApi.BusinessLayer.Services;
using CarRentalApi.Shared.Models;
using CarRentalApi.Shared.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class VehiclesController : ControllerBase
{
    private readonly IVehiclesService vehiclesService;

    public VehiclesController(IVehiclesService vehiclesService)
    {
        this.vehiclesService = vehiclesService;
    }

    [HttpDelete("DeleteVehicles")]
    [ProducesResponseType(200, Type = typeof(string))]
    public async Task<IActionResult> DeleteVehicles()
    {
        await vehiclesService.DeleteAsync();
        return Ok("Vehicles successfully deleted");
    }

    [HttpDelete("DeleteVehicle")]
    [ProducesResponseType(200, Type = typeof(string))]
    public async Task<IActionResult> DeleteVehicle(Guid id)
    {
        await vehiclesService.DeleteAsync(id);
        return Ok("Vehicle successfully deleted");
    }

    [HttpGet("GetVehicle")]
    [ProducesResponseType(200, Type = typeof(Vehicle))]
    [ProducesResponseType(404, Type = typeof(string))]
    public async Task<IActionResult> GetVehicle(Guid id)
    {
        var vehicle = await vehiclesService.GetAsync(id);
        if (vehicle != null)
        {
            return Ok(vehicle);
        }

        return NotFound("can't find a vehicle with the specified id");
    }

    [HttpPost("SaveVehicle")]
    [ProducesResponseType(200, Type = typeof(string))]
    [ProducesResponseType(400, Type = typeof(string))]
    public async Task<IActionResult> SaveVehicle([FromBody] SaveVehicleRequest request)
    {
        request.Id = null;
        var vehicle = await vehiclesService.SaveAsync(request);
        if (vehicle != null)
        {
            return Ok($"the vehicle {vehicle.Brand} {vehicle.Model} with plate: {vehicle.Plate} was successfully registrated in the database");
        }

        return BadRequest("problem occurred during registration");
    }

    [HttpPut("UpdateVehicle")]
    [ProducesResponseType(200, Type = typeof(string))]
    [ProducesResponseType(400, Type = typeof(string))]
    public async Task<IActionResult> UpdatePerson([FromBody] SaveVehicleRequest request)
    {
        var vehicle = await vehiclesService.SaveAsync(request);
        if (vehicle != null)
        {
            return Ok($"the vehicle {vehicle.Brand} {vehicle.Model} with plate: {vehicle.Plate} was successfully updated");
        }

        return BadRequest("problem occurred during updating");
    }
}