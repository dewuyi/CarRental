using CarRental.RavenModel;

namespace CarRental.RavenDbRepository;

public interface ICarCategoryRepositoryRaven
{
    Task<List<CarCategoryRavenDb>> GetAllCategories();
}