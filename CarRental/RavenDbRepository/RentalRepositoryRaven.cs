using CarRental.RavenModel;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace CarRental.RavenDbRepository;

public class RentalRepositoryRaven:IRentalRepositoryRaven
{
    private readonly IDocumentStoreHolder _documentStoreHolder;

    public RentalRepositoryRaven(IDocumentStoreHolder documentStoreHolder)
    {
        _documentStoreHolder = documentStoreHolder;
    }

    public async Task CreateRental(RentalRavenDb rental)
    {
        using IAsyncDocumentSession session = _documentStoreHolder.Store.OpenAsyncSession();
        await session.StoreAsync(rental);
        await session.SaveChangesAsync();
    }

    public async Task<List<RentalRavenDb>> GetExpiringRental()
    {
        using IAsyncDocumentSession session = _documentStoreHolder.Store.OpenAsyncSession();
        return await session.Query<RentalRavenDb>().Where(r => DateTime.Now >= r.RentalEnd.AddHours(-6)).ToListAsync();
    }
}