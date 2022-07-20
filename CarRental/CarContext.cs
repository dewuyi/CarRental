using CarRental.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CarRental;

public class CarContext: DbContext
{
    public DbSet<Car> Cars { get; set; }
    public DbSet<CarCategory> CarCategories { get; set; }
    public DbSet<Rental> CarRental { get; set; }

  
    public CarContext(DbContextOptions options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>().HasData(
            new Car
            {
               Id = 1,
               Category = CarCategoryEnum.Small,
               Manufacturer = CarManufacturer.Bmw,
               Name = "BMW X5",
               Status = RentalStatus.Available
            },
            new Car
            {
                Id = 2,
                Category = CarCategoryEnum.Medium,
                Manufacturer = CarManufacturer.Toyota,
                Name = "Land Cruiser",
                Status = RentalStatus.Available
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
    }
}