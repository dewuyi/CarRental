using CarRental.Model;

namespace CarRental.Repository;

public interface IRentalRepository
{
    Task<int> CreateRental(Rental rental);
    Task<List<Rental>> GetExpiringRental();
}