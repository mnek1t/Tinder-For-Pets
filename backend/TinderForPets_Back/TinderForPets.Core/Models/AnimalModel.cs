﻿namespace TinderForPets.Core.Models
{
    public class AnimalModel
    {
        public AnimalModel()
        {
            
        }
        private AnimalModel(Guid id, Guid userId, int typeId, int breedId)
        {
            Id = id;
            UserId = userId;
            TypeId = typeId;
            BreedId = breedId;
        }
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public int TypeId { get; set; }
        public int BreedId { get; set; }


        public static AnimalModel Create(Guid id, Guid userId, int typeId, int breedId)
        {
            return new AnimalModel(id, userId, typeId, breedId);
        }
    }
}