using CarRentalApi.BusinessLayer.Services;
using CarRentalApi.Shared.Models;
using CarRentalApi.Shared.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PeopleController : ControllerBase
{
    private readonly IPeopleService peopleService;

    public PeopleController(IPeopleService peopleService)
    {
        this.peopleService = peopleService;
    }

    [HttpDelete("DeletePeople")]
    [ProducesResponseType(200, Type = typeof(string))]
    public async Task<IActionResult> DeletePeople()
    {
        await peopleService.DeleteAsync();
        return Ok("people successfully deleted");
    }

    [HttpDelete("DeletePerson")]
    [ProducesResponseType(200, Type = typeof(string))]
    public async Task<IActionResult> DeletePerson(Guid id)
    {
        await peopleService.DeleteAsync(id);
        return Ok("person successfully");
    }

    [HttpGet("GetPerson")]
    [ProducesResponseType(200, Type = typeof(Person))]
    [ProducesResponseType(404, Type = typeof(string))]
    public async Task<IActionResult> GetPerson(Guid id)
    {
        var person = await peopleService.GetAsync(id);
        if (person != null)
        {
            return Ok(person);
        }

        return NotFound("can't find a person with the specified id");
    }

    [HttpPost("SavePerson")]
    [ProducesResponseType(200, Type = typeof(string))]
    [ProducesResponseType(400, Type = typeof(string))]
    public async Task<IActionResult> SavePerson([FromBody] SavePersonRequest request)
    {
        request.Id = null;
        var person = await peopleService.SaveAsync(request);
        if (person != null)
        {
            return Ok($"the person {person.FirstName} {person.LastName} was successfully registrated");
        }

        return BadRequest("problem occurred during registration");
    }

    [HttpPut("UpdatePerson")]
    [ProducesResponseType(200, Type = typeof(string))]
    [ProducesResponseType(400, Type = typeof(string))]
    public async Task<IActionResult> UpdatePerson([FromBody] SavePersonRequest request)
    {
        var person = await peopleService.SaveAsync(request);
        if (person != null)
        {
            return Ok($"the person {person.FirstName} {person.LastName} was successfully updated");
        }

        return BadRequest("problem occurred during updating");
    }
}