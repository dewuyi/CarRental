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
    private readonly IUserRepository _userRepository;

    public CarRentalController(IJwtManagerRepository jwtManager,
        ICarRepository carRepository,
        ICarCategoryRepository carCategoryRepository,
        IRentalRepository rentalRepository, IUserRepository userRepository)
    {
        _jwtManager = jwtManager;
        _carRepository = carRepository;
        _carCategoryRepository = carCategoryRepository;
        _rentalRepository = rentalRepository;
        _userRepository = userRepository;
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
        Enum.TryParse(manufacturerName, out CarManufacturer manufacturer);
        var cars = await _carRepository.GetCarsByManufacturerName(manufacturer);
        if (cars.Count == 0)
        {
            return NotFound($"No vehicle manufactured by {manufacturerName} can be found in our system");
        }
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
        if (car == null)
        {
            return NotFound("Vehicle does not exist in our system");
        }
        return Ok(car);
    }
    

    [HttpPost("rent")]
    public async Task<IActionResult> RentCar(RentalRequest rentalRequest)
    {
        var car = await _carRepository.GetCarByName(rentalRequest.VehicleName);
        if (car == null)
        {
            return NotFound("Vehicle does not exist in our system");
        }
        
        var rental = new Rental
        {
            CarId = car.Id,
            RentalStart = rentalRequest.RentalStartDate,
            RentalEnd = rentalRequest.RentalStartDate.AddDays(rentalRequest.RentalDurationInDays),
            CustomerName = rentalRequest.CustomerName,
            CustomerEmail = rentalRequest.CustomerEmail
        };
        
        var result = await _rentalRepository.CreateRental(rental);
        if (result != 1)
            return Ok("Unable to rent car");
        await _carRepository.UpdateCarStock(car.Id);
        return Created("", $"{car.Name} rented, Due back on {rental.RentalEnd}");
    }

    [HttpPost("user/register")]
    public async Task<IActionResult> Register(User user)
    {
        var userCreated = await _userRepository.CreateUser(user);
        if (!userCreated)
        {
            return BadRequest("User already exist");
        }
        return Created("", $"user {user.Name} created");
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