﻿namespace TinderForPets.Application.DTOs
{
    public class AnimalDto
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public int AnimalTypeId { get; set; }
        public int BreedId { get; set; }
    }
}
