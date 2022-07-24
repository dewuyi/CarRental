using CarRental.Model;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Repository;

public class RentalRepository:IRentalRepository
{
    private DbContextOptions<CarContext> _dbContextOptions;

    public RentalRepository(DbContextOptions<CarContext> dbContextOptions)
    {
        _dbContextOptions = dbContextOptions;
    }

    public async Task<int> CreateRental(Rental rental)
    {
        using var dbContext = new CarContext(_dbContextOptions);
        await dbContext.CarRentals.AddAsync(rental);
        return await dbContext.SaveChangesAsync();
    }

    public async Task<List<Rental>> GetExpiringRental()
    {
        using var dbContext = new CarContext(_dbContextOptions);
        return await dbContext.CarRentals.Where(c => DateTime.Now >= c.RentalEnd.AddHours(-6)).ToListAsync();
    }
}