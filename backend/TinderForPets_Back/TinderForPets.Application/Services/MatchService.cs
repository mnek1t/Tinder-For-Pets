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

        public async Task<Result<List<AnimalDetailsDto>>> GetMatchesProfilesData(Guid userId, CancellationToken cancellationToken) 
      {
            AnimalProfile? animalProfile = default;
            try
            {
                animalProfile = await _profileRepository.GetAnimalProfileByOwnerIdAsync(userId, cancellationToken);
            }
            catch (AnimalNotFoundException)
            {
                return Result.Failure<List<AnimalDetailsDto>>(AnimalProfileErrors.NotFound);
            }
            var matches = await _matchRepository.GetMatches(animalProfile.Id, cancellationToken) ?? new List<Match>();
            if (matches.Count == 0) 
            {
                return Result.Success<List<AnimalDetailsDto>>(new List<AnimalDetailsDto>());
            }
            var matchIds = matches.Select(m => m.FirstSwiperId == animalProfile.Id ? m.FirstSwiperId : m.SecondSwiperId).ToList();
            var filter = new AnimalRecommendationFilter
            {
                MatchesIds = matchIds
            };
            var matchedAnimalProfiles = await _profileRepository
                    .GetAnimalProfilesAsync(
                        AnimalProfileFilterBuilder.BuildAnimalProfileFilter(filter),
                        cancellationToken);

            var animalDetailsDtos = matchedAnimalProfiles.Select(a =>
            {
                return new AnimalDetailsDto()
                {
                    Profile = new AnimalProfileDto
                    {
                        Id = a.Id,
                        Name = a.Name
                    },
                    Images = a.Images.Select(i => new AnimalImageDto
                    {
                        ImageData = i.ImageData,
                        ImageFormat = i.ImageFormat
                    }).ToList()
                };
            }).ToList();
            return Result.Success<List<AnimalDetailsDto>>(animalDetailsDtos);
        }
    }
}
