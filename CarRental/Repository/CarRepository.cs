using CarRental.Model;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Repository;

public class CarRepository:ICarRepository
{
    private DbContextOptions<CarContext> _dbContextOptions;
    public CarRepository( DbContextOptions<CarContext>dbContextOptions)
    {
        _dbContextOptions = dbContextOptions;
    }
    public async Task<List<Car>> GetAllCars(PaginationFilter filter)
    {
        using var dbContext = new CarContext(_dbContextOptions);
        
        return await dbContext.Cars
        .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();;
    }

    public async Task UpdateCarStock(int carId)
    {
        using var dbContext = new CarContext(_dbContextOptions);
        var car = await dbContext.Cars.FirstOrDefaultAsync(c => c.Id == carId);
        if (car != null && car.Stock>0)
        {
            car.Stock -= 1;
            if (car.Stock == 0)
            {
                car.Status = RentalStatus.NotAvailable.ToString();
            }
        }
        await dbContext.SaveChangesAsync();
    }

    public async Task<Car> GetCarByName(string vehicleName)
    {
        using var dbContext = new CarContext(_dbContextOptions);
        return await dbContext.Cars.FirstOrDefaultAsync(c => c.Name == vehicleName);
    }

    public async Task<List<Car>> GetCarsByManufacturerName(CarManufacturer manufacturer)
    {
        using var dbContext = new CarContext(_dbContextOptions);
        return await dbContext.Cars.Where(c => c.Manufacturer == manufacturer.ToString()).ToListAsync();
    }
}