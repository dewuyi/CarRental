using CarRental.Model;

namespace CarRental.Repository;

public interface IUserRepository
{
    Task<bool> CreateUser(User user);
}