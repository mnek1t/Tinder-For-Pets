using System.Linq.Expressions;
using TinderForPets.Application.DTOs;
using TinderForPets.Data.Entities;

namespace TinderForPets.Application.Services
{
    public static  class AnimalProfileFilterBuilder
    {
        public static Expression<Func<AnimalProfile, bool>> BuildAnimalProfileFilter(AnimalRecommendationFilter filter)
        {
            var parameter = Expression.Parameter(typeof(AnimalProfile), "AnimalProfile");
            Expression? expression = null;

            // Filter by Opposite Sex
            if (filter.OppositeSexId.HasValue)
            {
                var sexProperty = Expression.Property(parameter, nameof(AnimalProfile.SexId));
                var sexValue = Expression.Constant(filter.OppositeSexId.Value);
                var sexCondition = Expression.Equal(sexProperty, sexValue);

                expression = sexCondition;
            }

            // Filter by Nearby S2 Cell IDs
            if (filter.NearbyS2CellIds.Count != 0)
            {
                var cellIdProperty = Expression.Property(parameter, nameof(AnimalProfile.S2CellId));
                var cellIds = Expression.Constant(filter.NearbyS2CellIds);
                var containsMethod = typeof(List<ulong>).GetMethod("Contains", new[] { typeof(ulong) });
                var cellIdCondition = Expression.Call(cellIds, containsMethod!, cellIdProperty);

                expression = expression == null ? cellIdCondition : Expression.AndAlso(expression, cellIdCondition);
            }

            // Exclude swiped profiles
            if (filter.SwipedProfileIds.Count != 0)
            {
                var profileIdProperty = Expression.Property(parameter, nameof(AnimalProfile.Id));
                var profileIds = Expression.Constant(filter.SwipedProfileIds);
                var containsMethod = typeof(List<Guid>).GetMethod("Contains", new[] { typeof(Guid) });
                var containsCondition = Expression.Call(profileIds, containsMethod!, profileIdProperty);
                var notContainsCondition = Expression.Not(containsCondition);
                expression = expression == null ? notContainsCondition : Expression.AndAlso(expression, notContainsCondition);
            }

            // Filter by Breed
            if (filter.BreedId.HasValue)
            {
                var animalProperty = Expression.Property(parameter, nameof(AnimalProfile.Animal));
                var breedProperty = Expression.Property(animalProperty, nameof(Animal.BreedId));
                var breedValue = Expression.Constant(filter.BreedId);
                var breedCondition = Expression.Equal(breedProperty, breedValue);

                expression = expression == null ? breedCondition : Expression.AndAlso(expression, breedCondition);
            }

            // Filter by Animal Type
            if (filter.AnimalTypeId.HasValue)
            {
                var animalProperty = Expression.Property(parameter, nameof(AnimalProfile.Animal));
                var typeProperty = Expression.Property(animalProperty, nameof(Animal.AnimalTypeId));
                var typeValue = Expression.Constant(filter.AnimalTypeId);
                var typeCondition = Expression.Equal(typeProperty, typeValue);

                expression = expression == null ? typeCondition : Expression.AndAlso(expression, typeCondition);
            }

            // exclude those profile who are already in matches
            if (filter.MatchesIds.Count != 0) 
            {
                var profileIdProperty = Expression.Property(parameter, nameof(AnimalProfile.Id));
                var matchProfileIds = Expression.Constant(filter.MatchesIds);
                var containsMethod = typeof(List<Guid>).GetMethod("Contains", new[] { typeof(Guid) });
                var containsCondition = Expression.Call(matchProfileIds, containsMethod!, profileIdProperty);
                var notContainsCondition = Expression.Not(containsCondition);
                expression = expression == null ? notContainsCondition : Expression.AndAlso(expression, notContainsCondition);
            }

            var lambda = Expression.Lambda<Func<AnimalProfile, bool>>(expression, parameter);
            return lambda;
        }
    }
}
