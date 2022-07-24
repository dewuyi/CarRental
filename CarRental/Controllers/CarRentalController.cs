using CarRental.Model;
using CarRental.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers;

[Authorize]
[ApiController]
[Route("api/rental")]

public class CarRentalController :ControllerBase
{
    private readonly IJwtManagerRepository _jwtManager;
    private readonly ICarRepository _carRepository;
    private readonly ICarCategoryRepository _carCategoryRepository;
    private readonly IRentalRepository _rentalRepository;

    public CarRentalController(IJwtManagerRepository jwtManager,
        ICarRepository carRepository,
        ICarCategoryRepository carCategoryRepository,
        IRentalRepository rentalRepository)
    {
        _jwtManager = jwtManager;
        _carRepository = carRepository;
        _carCategoryRepository = carCategoryRepository;
        _rentalRepository = rentalRepository;
    }
    
    // Get all cars categories
    [HttpGet("categories")]
    public async Task<IActionResult> GetCarCategories()
    {
        var categories = await _carCategoryRepository.GetAllCategories();
        return Ok(categories);
    }

    // Get all cars from a manufacturer
    [HttpGet("cars/{manufacturerName}")]
    public async Task<IActionResult> GetCars(string manufacturerName)
    {
        Enum.TryParse("Active", out CarManufacturer manufacturer);
        var cars = await _carRepository.GetCarsByManufacturerName(manufacturer);
        return Ok(cars);
    }
    
    // Get all vehicles by page
    [HttpGet("cars")]
    public async Task<IActionResult> GetAllCars([FromQuery] PaginationFilter filter)
    {
        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
        var cars = await _carRepository.GetAllCars(validFilter);
        return Ok(cars);
    }

    // Search the car by name
    [HttpGet("car/{vehicleName}")]
    public async Task<IActionResult> GetCar(string vehicleName)
    {
        var car = await _carRepository.GetCarByName(vehicleName);
        return Ok(car);
    }
    
    // Rent a car
    [HttpPost("rent")]
    public async Task<IActionResult> RentCar(RentalRequest rentalRequest)
    {
        var carId = (await _carRepository.GetCarByName(rentalRequest.VehicleName)).Id;
        var rental = new Rental
        {
            CarId = carId,
            RentalStart = rentalRequest.RentalStartDate,
            RentalEnd = rentalRequest.RentalStartDate.AddDays(rentalRequest.RentalDurationInDays),
            CustomerName = rentalRequest.CustomerName,
            CustomerEmail = rentalRequest.CustomerEmail
        };
        
        var result = await _rentalRepository.CreateRental(rental);
        if (result != 1)
            return Ok("Unable to rent car");
        await _carRepository.UpdateCarStock(carId);
        return Created("", rental);
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate(User usersdata)
    {
        var token = _jwtManager.Authenticate(usersdata);

        if (token == null)
        {
            return Unauthorized();
        }

        return Ok(token);
    }
}