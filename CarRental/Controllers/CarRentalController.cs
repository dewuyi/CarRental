using CarRental.Model;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers;

public class CarRentalController :ControllerBase
{
    // GET
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Rental>>> Get()
    {
        return Ok();
    }
}