using CarRental.Model;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Repository;

public class CarCategoryRepository : ICarCategoryRepository
{
    private DbContextOptions<CarContext> _dbContextOptions;

    public CarCategoryRepository(DbContextOptions<CarContext> dbContextOptions)
    {
        _dbContextOptions = dbContextOptions;
    }

    public async Task<List<CarCategory>> GetAllCategories()
    {
        using var dbContext = new CarContext(_dbContextOptions);
        return await dbContext.CarCategories.ToListAsync();
    }
}