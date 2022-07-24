using CarRental.Model;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Repository;

public class UserRepository:IUserRepository
{
    private DbContextOptions<CarContext> _dbContextOptions;

    public UserRepository(DbContextOptions<CarContext> dbContextOptions)
    {
        _dbContextOptions = dbContextOptions;
    }

    public async Task<bool> CreateUser(User user)
    {
        using var dbContext = new CarContext(_dbContextOptions);
        if(dbContext.Users.Any(u => u.Name == user.Name))
        {
            return false;
        }

        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync();
        return true;
    }
}