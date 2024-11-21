﻿using TinderForPets.Data.Entities;

namespace TinderForPets.Data.Interfaces
{
    public interface IMatchRepository : IRepository<Match>
    {
        public Task<List<Match>> GetMatches(Guid firstSwiperId, CancellationToken cancellationToken);
    }
}