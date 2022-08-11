using CarRental.RavenModel;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace CarRental.RavenDbRepository;

public class CarCategoryRepositoryRaven:ICarCategoryRepositoryRaven
{
    private readonly IDocumentStoreHolder _documentStoreHolder;

    public CarCategoryRepositoryRaven(IDocumentStoreHolder documentStoreHolder)
    {
        _documentStoreHolder = documentStoreHolder;
    }

    public async Task<List<CarCategoryRavenDb>> GetAllCategories()
    {
        using IAsyncDocumentSession session = _documentStoreHolder.Store.OpenAsyncSession();
        return await session.Query<CarCategoryRavenDb>().ToListAsync();
    }
}