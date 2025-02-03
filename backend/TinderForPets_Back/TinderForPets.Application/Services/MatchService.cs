using AutoMapper;
using SharedKernel;
using System.Collections.Immutable;
using System.Threading;
using TinderForPets.Application.DTOs;
using TinderForPets.Core;
using TinderForPets.Core.Models;
using TinderForPets.Data.Entities;
using TinderForPets.Data.Exceptions;
using TinderForPets.Data.Interfaces;

namespace TinderForPets.Application.Services
{
    public class MatchService
    {
        private readonly IMatchRepository _matchRepository;
        private readonly IAnimalProfileRepository _profileRepository;
        public MatchService(IMatchRepository matchRepository, IAnimalProfileRepository profileRepository)
        {
            _matchRepository = matchRepository;
            _profileRepository = profileRepository;
        }

        public async Task<Result<List<Guid>>> GetMatchesByProfileId(Guid profileId, CancellationToken cancellationToken) 
        {
            var profileIds = await _matchRepository.GetMatchesProfilesId(profileId, cancellationToken);
            return Result.Success(profileIds);
        }

        public async Task<Result<List<MatchCardDto>>> GetMatchesProfilesData(Guid userId, CancellationToken cancellationToken) 
        {
            // get animal profile - current user who is watching his matches
            AnimalProfile? currentAnimalProfile = default;
            try
            {
                currentAnimalProfile = await _profileRepository.GetAnimalProfileByOwnerIdAsync(userId, cancellationToken);
            }
            catch (AnimalNotFoundException)
            {
                return Result.Failure<List<MatchCardDto>>(AnimalProfileErrors.NotFound);
            }

            // get his matches - all entries from Match table where id of current user is appearing 
            var matchEntries = await _matchRepository.GetMatches(currentAnimalProfile.Id, cancellationToken) ?? new List<Match>();
            if (matchEntries.Count == 0) 
            {
                return Result.Success<List<MatchCardDto>>(new List<MatchCardDto>());
            }
            // extract all id fields of of matched animals (the other swiper)
            var otherSwipersIds = matchEntries.Select(m => m.FirstSwiperId == currentAnimalProfile.Id ? m.SecondSwiperId : m.FirstSwiperId).ToList();

            //var filter = new AnimalRecommendationFilter
            //{
            //    MatchesIds = matchIds
            //};
            var otherSwipersAnimalProfiles = await _profileRepository.GetAnimalProfilesFromIdListAsync(otherSwipersIds, cancellationToken);

            var matchDetailsDtos = otherSwipersAnimalProfiles.Select(otherSwiperProfile =>
            {
                var correspondingMatch = matchEntries.Find(m =>
                    m.FirstSwiperId == otherSwiperProfile.Id || m.SecondSwiperId == otherSwiperProfile.Id);

                return new MatchCardDto()
                {
                    MatchId = correspondingMatch?.Id ?? Guid.Empty,
                    ProfileName = otherSwiperProfile.Name,
                    Description = otherSwiperProfile.Description,
                    Age = otherSwiperProfile.Age,
                    IsVaccinated = otherSwiperProfile.IsVaccinated,
                    IsSterilized = otherSwiperProfile.IsSterilized,
                    CreatedAt = correspondingMatch?.CreatedAt ?? DateTime.UtcNow,
                    Images = otherSwiperProfile.Images.Select(i => new AnimalImageDto
                    {
                        ImageData = i.ImageData,
                        ImageFormat = i.ImageFormat
                    }).ToList()
                };
            }).ToList();
            return Result.Success<List<MatchCardDto>>(matchDetailsDtos);
        }
    }
}
