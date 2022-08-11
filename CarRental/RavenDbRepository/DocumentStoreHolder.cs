using CarRental.Model;
using Microsoft.Extensions.Options;
using Raven.Client.Documents;

namespace CarRental.RavenDbRepository;

public class DocumentStoreHolder:IDocumentStoreHolder
{
    private readonly RavenSettings _ravenSettings;
    public DocumentStoreHolder(IOptions<RavenSettings> ravenSettings)
    {
        _ravenSettings = ravenSettings.Value;
        Store =  new DocumentStore()
        {
            // Define the cluster node URLs (required)
            Urls = new[] {_ravenSettings.Url},
            // Set conventions as necessary (optional)
            Conventions =
            {
                MaxNumberOfRequestsPerSession = 10,
                UseOptimisticConcurrency = true
            },

            // Define a default database (optional)
            Database = _ravenSettings.Database,
        }.Initialize();
    }
    
    public IDocumentStore Store { get; }
    
    
}