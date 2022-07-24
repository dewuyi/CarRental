using CarRental.Model;
using Microsoft.EntityFrameworkCore;

namespace CarRental;

public class CarContext: DbContext
{
    public DbSet<Car> Cars { get; set; }
    public DbSet<CarCategory> CarCategories { get; set; }
    public DbSet<Rental> CarRentals { get; set; }
    public DbSet<User> Users { get; set; }


    public CarContext(DbContextOptions options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>().Property(c => c.Price).HasPrecision(5, 2);
        modelBuilder.Entity<Car>().HasData(
            new Car
            {
               Id = 1,
               Category = CarCategoryEnum.Small,
               Manufacturer = CarManufacturer.Bmw,
               Name = "BMW X5",
               Status = RentalStatus.Available,
               Stock = 20,
               Price = 50
            },
            new Car
            {
                Id = 2,
                Category = CarCategoryEnum.Medium,
                Manufacturer = CarManufacturer.Toyota,
                Name = "Land Cruiser",
                Status = RentalStatus.Available,
                Stock = 10,
                Price = 40
            }
        );
        
        modelBuilder.Entity<CarCategory>().HasData(
           new CarCategory
           {
               Id = 1,
               Name = CarCategoryEnum.Small.ToString()
           },
           new CarCategory
           {
               Id = 2,
               Name = CarCategoryEnum.Medium.ToString()
           },
           new CarCategory
           {
               Id = 3,
               Name = CarCategoryEnum.Large.ToString()
           }
        );

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Name = "user1",
                Password = "password1"
            },
            new User
            {
                Name = "user2",
                Password = "password2"
            },
            new User
            {
                Name = "user3",
                Password = "password3"
            }
        );

    }
}