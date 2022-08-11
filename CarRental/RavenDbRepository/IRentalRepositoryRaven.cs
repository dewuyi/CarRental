using CarRental.RavenModel;

namespace CarRental.RavenDbRepository;

public interface IRentalRepositoryRaven
{
    Task CreateRental(RentalRavenDb rental);
    Task<List<RentalRavenDb>> GetExpiringRental();
}