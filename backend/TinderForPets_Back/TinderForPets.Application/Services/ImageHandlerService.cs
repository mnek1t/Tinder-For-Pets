using Microsoft.AspNetCore.Http;
using SharedKernel;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using TinderForPets.Core.Models;
using TinderForPets.Data.Entities;
using TinderForPets.Data.Interfaces;

namespace TinderForPets.Application.Services
{
    public class ImageHandlerService
    {
        private const int MAX_WIDTH = 500;
        private const int MAX_HEIGHT = 800;
        private readonly IAnimalImageRepository _animalImageRepository;

        public ImageHandlerService(IAnimalImageRepository animalImageRepository)
        {
            _animalImageRepository = animalImageRepository;
        }
        public async Task<Result<List<AnimalImageModel>>> ResizeImages(List<IFormFile> files, Guid animalProfileId, string description) 
        {
            var animalImageModels = new List<AnimalImageModel>();
            foreach (var file in files)
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                byte[] imageData = memoryStream.ToArray();

                if (file.ContentType.StartsWith("image")) 
                {
                    using var image = Image.Load(imageData);
                    image.Mutate(i => i.Resize(new ResizeOptions
                    {
                        Size = new Size(MAX_WIDTH, MAX_HEIGHT),
                        Mode = ResizeMode.Max
                    }));

                    using var outStream = new MemoryStream();
                    await image.SaveAsync(outStream, new PngEncoder());
                    imageData = outStream.ToArray();
                }

                var animalImageModel = AnimalImageModel.Create(Guid.NewGuid(), animalProfileId, imageData, description, DateOnly.FromDateTime(DateTime.UtcNow), file.ContentType);
                animalImageModels.Add(animalImageModel);
            }

            return Result.Success<List<AnimalImageModel>>(animalImageModels);
            
        }

        public async Task<Result<List<AnimalImage>>> SaveImages(List<AnimalImageModel> animalImageModels) 
        {
            var uploadedAnimalImages = await _animalImageRepository.SaveAnimalMediaAsync(animalImageModels);
            return Result.Success<List<AnimalImage>>(uploadedAnimalImages);
        }

    }
}
