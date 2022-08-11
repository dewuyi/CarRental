using CarRental.Model;
using CarRental.RavenModel;
using Raven.Client.Documents.Session;
using User = CarRental.RavenModel.User;

namespace CarRental.RavenDbRepository;

public class DocumentFactory:IDocumentFactory
{
    private readonly IDocumentStoreHolder _documentStoreHolder;
    
    public DocumentFactory(IDocumentStoreHolder documentStoreHolder)
    {
        _documentStoreHolder = documentStoreHolder;
    }
    public void SeedCarData()
    {
        try
        {
            using (IDocumentSession session = _documentStoreHolder.Store.OpenSession())
            {
                foreach (var car in GetCars())
                {
                    if(session.Advanced.Exists(car.Id))
                        continue;
                    session.Store(car);
                }

                foreach (var category in GetCarCategories())
                {
                    if(session.Advanced.Exists(category.Id))
                        continue;
                    session.Store(category);
                }

                foreach (var user in GetUsers())
                {
                    if(session.Advanced.Exists(user.Id))
                        continue;
                    session.Store(user);
                }

                var rental = new RentalRavenDb
                {
                    Id = "Rental/1",
                    RentalStart = DateTime.Now,
                    RentalEnd = DateTime.Now.AddDays(7),
                    CarId = 1,
                    CustomerEmail = "Banjioyawoye@gmail.com",
                    CustomerName = "Tommy"
                };
                if (!session.Advanced.Exists(rental.Id))
                {
                    session.Store(rental);
                }
                session.SaveChanges();
            }
        }
        catch (Exception e)
        {
            throw e;
        }
       
    }

    private IEnumerable<CarRavenDb> GetCars()
    {
        return new List<CarRavenDb>{
            new()
            {
                Id = "Car/1",
                Category = CarCategoryEnum.Small.ToString(),
                Manufacturer = CarManufacturer.Bmw.ToString(),
                Name = "BMW X5",
                Status = RentalStatus.Available.ToString(),
                Stock = 20,
                Price = 50
            },
            new()
            {
                Id = "Car/2",
                Category = CarCategoryEnum.Medium.ToString(),
                Manufacturer = CarManufacturer.Toyota.ToString(),
                Name = "Land Cruiser",
                Status = RentalStatus.Available.ToString(),
                Stock = 10,
                Price = 40
            }
        };
    }

    private IEnumerable<CarCategoryRavenDb> GetCarCategories()
    {
        return new List<CarCategoryRavenDb>
        {
            new()
            {
                Id = "CarCategory/1",
                Name = CarCategoryEnum.Small.ToString()
            },
            new()
            {
                Id = "CarCategory/2",
                Name = CarCategoryEnum.Medium.ToString()
            },
            new()
            {
                Id = "CarCategory/3",
                Name = CarCategoryEnum.Large.ToString()
            }
        };
    }

    private IEnumerable<User> GetUsers()
    {
        return new List<User>
        {
            new()
            {
                Id = "User/1",
                Name = "user1",
                Password = "password1"
            },
            new()
            {
                Id = "User/2",
                Name = "user2",
                Password = "password2"
            },
            new()
            {
                Id = "User/3",
                Name = "user3",
                Password = "password3"
            }
        };
    }
    
    
}