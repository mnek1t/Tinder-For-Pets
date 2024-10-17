namespace TinderForPets.Data.Entities
{
    public class AnimalImage
    {
        public Guid Id { get; set; } // using new Ulid dt
        public Guid AnimalProfileId { get; set; }
        public AnimalProfile AnimalProfile { get; set; }

        public byte[] ImageData { get; set; }
        public string ImageFormat { get; set; } = string.Empty;
        public DateOnly UploadDate { get; set; }
        public string Description { get; set; } = string.Empty; // main image or something else
    }
}
