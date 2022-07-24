using CarRental.Model;

namespace CarRental.Repository;

public interface IJwtManagerRepository
{
    Tokens Authenticate(User users);
}