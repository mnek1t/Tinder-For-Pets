﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinderForPets.Data.Entities;
using TinderForPets.Data.Interfaces;

namespace TinderForPets.Data.Repositories
{
    public class MatchRepository : TinderForPetsRepository<Match>, IMatchRepository
    {
        public MatchRepository(TinderForPetsDbContext context) : base(context) { }

        public async override Task<Guid> CreateAsync(Match entity, CancellationToken cancellationToken)
        {
            await _context.Matches.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }

        public async override Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async override Task<Match> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Match>> GetMatches(Guid firstSwiperId, CancellationToken cancellationToken)
        {
            var matches = await _context.Matches
                .Where(m => m.FirstSwiperId == firstSwiperId || m.SecondSwiperId == firstSwiperId)
                .ToListAsync(cancellationToken);
            return matches;
        }

        public async Task<List<Guid>> GetMatchesProfilesId(Guid profileId, CancellationToken cancellationToken)
        {
            var profileIds = await _context.Matches
                .Where(m => m.FirstSwiperId == profileId || m.SecondSwiperId == profileId)
                .Select(m => m.FirstSwiperId == profileId ? m.SecondSwiperId : m.FirstSwiperId)
                .Distinct()
                .ToListAsync(cancellationToken);

            return profileIds;
        }

        public async override Task<Match> UpdateAsync(Match entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
