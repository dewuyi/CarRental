using CarRental.Model;

namespace CarRental.Repository;

public interface ICarCategoryRepository
{
    Task<List<CarCategory>> GetAllCategories();
}