using AutoMapper;
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
        private readonly IMapper _mapper;

        public ImageHandlerService(IAnimalImageRepository animalImageRepository, IMapper mapper)
        {
            _animalImageRepository = animalImageRepository;
            _mapper = mapper;
        }
        public async Task<Result<AnimalImage>> ResizeImages(IFormFile file, Guid animalProfileId, CancellationToken cancellationToken, string description = "main image") 
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
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
                    await image.SaveAsync(outStream, new PngEncoder(), cancellationToken);
                    imageData = outStream.ToArray();
                }

                var animalImageModel = AnimalImageModel.Create(Guid.NewGuid(), animalProfileId, imageData, description, DateOnly.FromDateTime(DateTime.UtcNow), file.ContentType);
                var animalImageEntity = _mapper.Map<AnimalImage>(animalImageModel);
                return Result.Success<AnimalImage>(animalImageEntity);
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<AnimalImage>(new Error("400", "Operation canceled"));
            }
        }

        public async Task<Result<Guid>> SaveImages(AnimalImage animalImage, CancellationToken cancellationToken) 
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var uploadedAnimalImageId = await _animalImageRepository.CreateAsync(animalImage, cancellationToken);
                return Result.Success<Guid>(uploadedAnimalImageId);
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<Guid>(new Error("400", "Operation canceled"));
            }
        }

    }
}
