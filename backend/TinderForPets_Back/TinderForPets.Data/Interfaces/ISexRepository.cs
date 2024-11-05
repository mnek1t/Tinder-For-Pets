using TinderForPets.Data.Entities;

namespace TinderForPets.Data.Interfaces
{
    public interface ISexRepository
    {
        Task<List<Sex>> GetSexes(CancellationToken cancellationToken);
    }
}
