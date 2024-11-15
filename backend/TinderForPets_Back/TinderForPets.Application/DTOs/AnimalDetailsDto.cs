namespace TinderForPets.Application.DTOs
{
    public class AnimalDetailsDto
    {
        public AnimalDto Animal { get; set; }
        public AnimalProfileDto Profile { get; set; }
        public List<AnimalImageDto> Images { get; set; }
    }
}
