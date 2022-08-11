using Raven.Client.Documents;

namespace CarRental.RavenDbRepository;

public interface IDocumentStoreHolder
{
    IDocumentStore Store { get; }
}