using Microsoft.EntityFrameworkCore;
using TinderForPets.Data.Entities;
using TinderForPets.Data.Interfaces;

namespace TinderForPets.Data.Repositories
{
    public class SexRepository : ISexRepository
    {
        private readonly TinderForPetsDbContext _context;
        public SexRepository(TinderForPetsDbContext context)
        {
            _context = context;
        }
        public Task<List<Sex>> GetSexes(CancellationToken cancellationToken)
        {
            var sexes = _context.Sexes.ToListAsync(cancellationToken);
            return sexes;
        }
    }
}
