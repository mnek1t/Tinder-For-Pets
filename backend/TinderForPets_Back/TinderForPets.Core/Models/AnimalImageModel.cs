using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinderForPets.Core.Models
{
    public class AnimalImageModel
    {
        public AnimalImageModel()
        {
        }

        public AnimalImageModel(Guid id, Guid animalProfileId, byte[] imageData, string description, DateOnly uploadDate, string imageFormat)
        {
            Id = id;
            AnimalProfileId = animalProfileId;
            ImageData = imageData;
            Description = description;
            ImageFormat = imageFormat;
            UploadDate = uploadDate;
        }

        public Guid Id { get; set; }
        public Guid AnimalProfileId { get; set; }

        public byte[] ImageData { get; set; }
        public string ImageFormat { get; set; } = string.Empty;
        public DateOnly UploadDate { get; set; }
        public string Description { get; set; } = string.Empty; 
        public static AnimalImageModel Create(Guid id, Guid animalProfileId, byte[] imageData, string description, DateOnly uploadDate, string imageFormat)
        {
            return new AnimalImageModel(id, animalProfileId, imageData, description, uploadDate, imageFormat);
        }
    }
}
