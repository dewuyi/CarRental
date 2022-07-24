using CarRental.Model;

namespace CarRental.Repository;

public interface ICarRepository
{
    Task<List<Car>> GetAllCars(PaginationFilter filter);

    Task UpdateCarStock(int carId);
    Task<Car> GetCarByName(string vehicleName);

    Task<List<Car>> GetCarsByManufacturerName(CarManufacturer manufacturer);
}