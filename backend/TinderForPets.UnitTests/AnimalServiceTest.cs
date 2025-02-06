using AutoMapper;
using FluentAssertions;
using Moq;
using TinderForPets.Application.DTOs;
using TinderForPets.Application.Interfaces;
using TinderForPets.Application.Services;
using TinderForPets.Core;
using TinderForPets.Core.Models;
using TinderForPets.Data.Entities;
using TinderForPets.Data.Exceptions;
using TinderForPets.Data.Interfaces;
using TinderForPets.Data.Repositories;

namespace TinderForPets.UnitTests
{
    public class AnimalServiceTest
    {
        private readonly Mock<IAnimalProfileRepository> _animalProfileRepositoryMock;
        private readonly Mock<IAnimalRepository> _animalRepositoryMock;
        private readonly Mock<IAnimalTypeRepository> _animalTypeRepositoryMock;
        private readonly Mock<IBreedRepository> _breedRepositoryMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<ISexRepository> _sexRepositoryMock;
        private readonly Mock<IAnimalImageRepository> _animalImageRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AnimalService _animalService;
        public AnimalServiceTest()
        {
            _animalProfileRepositoryMock = new Mock<IAnimalProfileRepository>();
            _animalRepositoryMock = new Mock<IAnimalRepository>();
            _animalTypeRepositoryMock = new Mock<IAnimalTypeRepository>();
            _breedRepositoryMock = new Mock<IBreedRepository>();
            _cacheServiceMock = new Mock<ICacheService>();
            _sexRepositoryMock = new Mock<ISexRepository>();
            _animalImageRepositoryMock = new Mock<IAnimalImageRepository>();
            _mapperMock = new Mock<IMapper>();

            _animalService = new AnimalService(
                _animalProfileRepositoryMock.Object,
                _animalRepositoryMock.Object,
                _cacheServiceMock.Object,
                _animalTypeRepositoryMock.Object,
                _breedRepositoryMock.Object,
                _sexRepositoryMock.Object,
                _animalImageRepositoryMock.Object,
                _mapperMock.Object
            );
        }

        #region GetAnimalTypes
        [Fact]
        public async Task GetAnimalTypes_ReturnsOperationCanceledFailure() 
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();
            var result = await _animalService.GetAnimalTypesAsync(cts.Token);
            cts.Dispose();

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(OperationCancellationErrors.OperationCancelled.Code);
            result.Error.Description.Should().Be(OperationCancellationErrors.OperationCancelled.Description);
        }

        [Fact]
        public async Task GetAnimalTypes_ReturnsDataFromCache()
        {
            var cacheKey = "GET_ANIMAL_TYPES";
            var mockCachedData = new List<AnimalTypeDto> 
            {
                new AnimalTypeDto() { Id = 1, TypeName = "Dog" },
                new AnimalTypeDto() { Id = 2, TypeName = "Cat" },
            };

            _cacheServiceMock.Setup(service => service.GetAsync<List<AnimalTypeDto>>(cacheKey))
                .ReturnsAsync(mockCachedData);

            var result = await _animalService.GetAnimalTypesAsync(CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(mockCachedData);

            _animalTypeRepositoryMock.Verify(repo => repo.GetAllAnimalTypesAsync(CancellationToken.None), Times.Never);
        }

        [Fact]
        public async Task GetAnimalTypes_ReturnsDataFromDatabaseWhenCacheIsEmpty()
        {
            var cancellationToken = CancellationToken.None;
            var cacheKey = "GET_ANIMAL_TYPES";
            var mockDatabaseData = new List<AnimalType>
            {
                new AnimalType(1, "Dog"),
                new AnimalType(2, "Cat")
            };

            var mockAnimalTypeDto = mockDatabaseData.Select(a => new AnimalTypeDto
            {
                Id = a.Id,
                TypeName = a.TypeName

            }).ToList();

            _cacheServiceMock.Setup(service => service.GetAsync<List<AnimalTypeDto>?>(cacheKey))
                .ReturnsAsync((List<AnimalTypeDto>?)null);

            _animalTypeRepositoryMock.Setup(repo => repo.GetAllAnimalTypesAsync(cancellationToken))
                .ReturnsAsync(mockDatabaseData);

            _cacheServiceMock.Setup(service => service.SetAsync<List<AnimalTypeDto>>(cacheKey, mockAnimalTypeDto, TimeSpan.FromMinutes(5)))
                .Returns(Task.CompletedTask);

            var result = await _animalService.GetAnimalTypesAsync(cancellationToken);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(mockAnimalTypeDto);

            _cacheServiceMock.Verify(service => service.GetAsync<List<AnimalTypeDto>>(cacheKey), Times.Once);
            _animalTypeRepositoryMock.Verify(repo => repo.GetAllAnimalTypesAsync(cancellationToken), Times.Once);
            _cacheServiceMock.Verify(service => service.SetAsync(
                cacheKey,
                It.Is<List<AnimalTypeDto>>(list =>
                    list.Count == mockAnimalTypeDto.Count &&
                    list[0].Id == mockAnimalTypeDto[0].Id &&
                    list[0].TypeName == mockAnimalTypeDto[0].TypeName),
                TimeSpan.FromMinutes(5)
            ), Times.Once);
        }
        #endregion
        #region GetAnimalBreedById
        [Fact]
        public async Task GetAnimalBreedById_ReturnsOperationCanceledFailure()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();
            var result = await _animalService.GetAnimalBreedByIdAsync(It.IsAny<int>(),cts.Token);
            cts.Dispose();

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(OperationCancellationErrors.OperationCancelled.Code);
            result.Error.Description.Should().Be(OperationCancellationErrors.OperationCancelled.Description);
        }
        [Fact]
        public async Task GetAnimalBreedById_ReturnsDataFromCache()
        {
            int animalTypeId = 1;
            var cacheKey = "GET_ANIMAL_BREEDS_BY_TYPE_ID_" + animalTypeId.ToString();
            var mockCachedData = new List<BreedDto>
            {
                new BreedDto() { Id = 1, BreedName = "American Spiltzer" },
                new BreedDto() { Id = 2, BreedName = "Abyssian" },
            };

            _cacheServiceMock.Setup(service => service.GetAsync<List<BreedDto>>(cacheKey))
                .ReturnsAsync(mockCachedData);

            var result = await _animalService.GetAnimalBreedByIdAsync(animalTypeId, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(mockCachedData);

            _breedRepositoryMock.Verify(repo => repo.GetBreedsByTypeIdAsync(animalTypeId, CancellationToken.None), Times.Never);
        }

        [Fact]
        public async Task GetAnimalBreedById_ReturnsDataNotFoundFromDatabase()
        {
            int animalTypeId = 50;
            var cancellationToken = CancellationToken.None;
            var cacheKey = "GET_ANIMAL_BREEDS_BY_TYPE_ID_" + animalTypeId.ToString();

            var mockDatabaseData = new List<Breed>
            {
                new Breed(1, "American Spiltzer", animalTypeId),
                new Breed(2, "Abyssian", animalTypeId)
            };

            var mockBreedDto = mockDatabaseData.Select(b => new BreedDto
            {
                Id = b.Id,
                BreedName = b.BreedName

            }).ToList();

            _cacheServiceMock.Setup(service => service.GetAsync<List<BreedDto>?>(cacheKey))
                .ReturnsAsync((List<BreedDto>?)null);

            _breedRepositoryMock.Setup(repo => repo.GetBreedsByTypeIdAsync(animalTypeId, cancellationToken))
                .ThrowsAsync(new BreedNotFoundException());

            var result = await _animalService.GetAnimalBreedByIdAsync(animalTypeId, cancellationToken);

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(AnimalProfileErrors.BreedNotFound(animalTypeId).Code);
            result.Error.Description.Should().Be(AnimalProfileErrors.BreedNotFound(animalTypeId).Description);


            _cacheServiceMock.Verify(service => service.GetAsync<List<BreedDto>>(cacheKey), Times.Once);
            _breedRepositoryMock.Verify(repo => repo.GetBreedsByTypeIdAsync(animalTypeId, cancellationToken), Times.Once);
            _cacheServiceMock.Verify(service => service.SetAsync(cacheKey, It.IsAny<List<BreedDto>>(), TimeSpan.FromMinutes(5)), Times.Never);
        }

        [Fact]
        public async Task GetAnimalBreedById_ReturnsDataFromDatabaseWhenCacheIsEmpty()
        {
            int animalTypeId = 1;
            var cancellationToken = CancellationToken.None;
            var cacheKey = "GET_ANIMAL_BREEDS_BY_TYPE_ID_" + animalTypeId.ToString();

            var mockDatabaseData = new List<Breed>
            {
                new Breed(1, "American Spiltzer", animalTypeId),
                new Breed(2, "Abyssian", animalTypeId)
            };

            var mockBreedDto = mockDatabaseData.Select(b => new BreedDto
            {
                Id = b.Id,
                BreedName = b.BreedName

            }).ToList();

            _cacheServiceMock.Setup(service => service.GetAsync<List<BreedDto>?>(cacheKey))
                .ReturnsAsync((List<BreedDto>?)null);

            _breedRepositoryMock.Setup(repo => repo.GetBreedsByTypeIdAsync(animalTypeId, cancellationToken))
                .ReturnsAsync(mockDatabaseData);

            _cacheServiceMock.Setup(service => service.SetAsync<List<BreedDto>>(cacheKey, mockBreedDto, TimeSpan.FromMinutes(5)))
                .Returns(Task.CompletedTask);

            var result = await _animalService.GetAnimalBreedByIdAsync(animalTypeId ,cancellationToken);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(mockBreedDto);

            _cacheServiceMock.Verify(service => service.GetAsync<List<BreedDto>>(cacheKey), Times.Once);
            _breedRepositoryMock.Verify(repo => repo.GetBreedsByTypeIdAsync(animalTypeId, cancellationToken), Times.Once);
            _cacheServiceMock.Verify(service => service.SetAsync(
                cacheKey,
                It.Is<List<BreedDto>>(list =>
                    list.Count == mockBreedDto.Count &&
                    list[0].Id == mockBreedDto[0].Id &&
                    list[0].BreedName == mockBreedDto[0].BreedName),
                TimeSpan.FromMinutes(5)
            ), Times.Once);
        }
        #endregion
        #region GetSexes
        [Fact]
        public async Task GetSexes_ReturnsOperationCanceledFailure()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();
            var result = await _animalService.GetSexesAsync(cts.Token);
            cts.Dispose();

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(OperationCancellationErrors.OperationCancelled.Code);
            result.Error.Description.Should().Be(OperationCancellationErrors.OperationCancelled.Description);
        }

        [Fact]
        public async Task GetSexes_ReturnsDataFromCache()
        {
            var cacheKey = "GET_SEXES";
            var mockCachedData = new List<SexDto>
            {
                new SexDto() { Id = 1, SexName = "Male" },
                new SexDto() { Id = 2, SexName = "Female" },
            };

            _cacheServiceMock.Setup(service => service.GetAsync<List<SexDto>>(cacheKey))
                .ReturnsAsync(mockCachedData);

            var result = await _animalService.GetSexesAsync(CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(mockCachedData);

            _sexRepositoryMock.Verify(repo => repo.GetSexes(CancellationToken.None), Times.Never);
        }

        [Fact]
        public async Task GetSexes_ReturnsDataFromDatabaseWhenCacheIsEmpty()
        {
            var cancellationToken = CancellationToken.None;
            var cacheKey = "GET_SEXES";
            var mockDatabaseData = new List<Sex>
            {
                new Sex(1, "Male"),
                new Sex(2, "Female")
            };

            var mockSexDto = mockDatabaseData.Select(s => new SexDto
            {
                Id = s.Id,
                SexName = s.SexName

            }).ToList();

            _cacheServiceMock.Setup(service => service.GetAsync<List<SexDto>?>(cacheKey))
                .ReturnsAsync((List<SexDto>?)null);

            _sexRepositoryMock.Setup(repo => repo.GetSexes(cancellationToken))
                .ReturnsAsync(mockDatabaseData);

            _cacheServiceMock.Setup(service => service.SetAsync<List<SexDto>>(cacheKey, mockSexDto, TimeSpan.FromMinutes(5)))
                .Returns(Task.CompletedTask);

            var result = await _animalService.GetSexesAsync(cancellationToken);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(mockSexDto);

            _cacheServiceMock.Verify(service => service.GetAsync<List<SexDto>>(cacheKey), Times.Once);
            _sexRepositoryMock.Verify(repo => repo.GetSexes(cancellationToken), Times.Once);
            _cacheServiceMock.Verify(service => service.SetAsync(
                cacheKey,
                It.Is<List<SexDto>>(list =>
                    list.Count == mockSexDto.Count &&
                    list[0].Id == mockSexDto[0].Id &&
                    list[0].SexName == mockSexDto[0].SexName),
                TimeSpan.FromMinutes(5)
            ), Times.Once);
        }
        #endregion
        #region GetAnimalImage
        [Fact]
        public async Task GetAnimalImage_ReturnsOperationCanceledFailure()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();
            var result = await _animalService.GetAnimalImageAsync(It.IsAny<Guid>(), cts.Token);
            cts.Dispose();

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(OperationCancellationErrors.OperationCancelled.Code);
            result.Error.Description.Should().Be(OperationCancellationErrors.OperationCancelled.Description);
        }

        [Fact]
        public async Task GetAnimalImage_ReturnsDataNotFoundFromDatabase()
        {
            var animalProfileId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;
            var cacheKey = $"GET_ANIMAL_IMAGE {animalProfileId}";

            _cacheServiceMock.Setup(service => service.GetAsync<AnimalImageDto?>(cacheKey))
                .ReturnsAsync((AnimalImageDto?)null);

            _animalImageRepositoryMock.Setup(repo => repo.GetAnimalImageAsync(animalProfileId, cancellationToken))
                .ThrowsAsync(new AnimalNotFoundException());

            var result = await _animalService.GetAnimalImageAsync(animalProfileId, cancellationToken);

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(AnimalProfileErrors.ImageIsNotFound.Code);
            result.Error.Description.Should().Be(AnimalProfileErrors.ImageIsNotFound.Description);

            _cacheServiceMock.Verify(service => service.GetAsync<AnimalImageDto>(cacheKey), Times.Once);
            _animalImageRepositoryMock.Verify(repo => repo.GetAnimalImageAsync(animalProfileId, cancellationToken), Times.Once);
            _cacheServiceMock.Verify(service => service.SetAsync(cacheKey, It.IsAny<AnimalImageDto>(), TimeSpan.FromMinutes(5)), Times.Never);
        }

        [Fact]
        public async Task GetAnimalImage_ReturnsDataFromCache()
        {
            var animalProfileId = Guid.NewGuid();
            var cacheKey = $"GET_ANIMAL_IMAGE {animalProfileId}";
            var mockCachedData = new AnimalImageDto() { ImageData = new byte[1], ImageFormat = "imageExtension" };

            _cacheServiceMock.Setup(service => service.GetAsync<AnimalImageDto>(cacheKey))
                .ReturnsAsync(mockCachedData);

            var result = await _animalService.GetAnimalImageAsync(animalProfileId, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(mockCachedData);

            _animalImageRepositoryMock.Verify(repo => repo.GetAnimalImageAsync(animalProfileId, CancellationToken.None), Times.Never);
        }

        [Fact]
        public async Task GetAnimalImage_ReturnsDataFromDatabaseWhenCacheIsEmpty()
        {
            var cancellationToken = CancellationToken.None;
            var animalProfileId = Guid.NewGuid();
            var cacheKey = $"GET_ANIMAL_IMAGE {animalProfileId}";
            
            var mockDatabaseData = new AnimalImage { AnimalProfileId = animalProfileId, ImageData = new byte[1], ImageFormat = "imageExtension" };
            var mockAnimalImageDto =  new AnimalImageDto() { ImageData = mockDatabaseData.ImageData, ImageFormat = mockDatabaseData.ImageFormat };

            _cacheServiceMock.Setup(service => service.GetAsync<AnimalImageDto?>(cacheKey))
                .ReturnsAsync((AnimalImageDto?)null);

            _animalImageRepositoryMock.Setup(repo => repo.GetAnimalImageAsync(animalProfileId, cancellationToken))
                .ReturnsAsync(mockDatabaseData);

            _cacheServiceMock.Setup(service => service.SetAsync<AnimalImageDto>(cacheKey, mockAnimalImageDto, TimeSpan.FromMinutes(5)))
                .Returns(Task.CompletedTask);

            var result = await _animalService.GetAnimalImageAsync(animalProfileId, cancellationToken);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(mockAnimalImageDto);

            _cacheServiceMock.Verify(service => service.GetAsync<AnimalImageDto>(cacheKey), Times.Once);
            _animalImageRepositoryMock.Verify(repo => repo.GetAnimalImageAsync(animalProfileId, cancellationToken), Times.Once);
            _cacheServiceMock.Verify(service => service.SetAsync(
                cacheKey,
                It.Is<AnimalImageDto>(dto => dto.ImageData == mockAnimalImageDto.ImageData && dto.ImageFormat == mockAnimalImageDto.ImageFormat),
                TimeSpan.FromMinutes(5)
            ), Times.Once);
        }
        #endregion
        #region GetAnimalProfileDetails
        [Fact]
        public async Task GetAnimalProfileDetails_ReturnsOperationCanceledFailure()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();
            var result = await _animalService.GetAnimalProfileDetails(It.IsAny<Guid>(), cts.Token);
            cts.Dispose();

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(OperationCancellationErrors.OperationCancelled.Code);
            result.Error.Description.Should().Be(OperationCancellationErrors.OperationCancelled.Description);
        }

        [Fact]
        public async Task GetAnimalProfileDetails_ReturnsDataFromCache()
        {
            var ownerId = Guid.NewGuid();
            var animalProfileId = Guid.NewGuid();
            var cacheKey = "GET_ANIMAL_PROFILE_DETAILS_BY_OWNER" + ownerId.ToString();
            var mockCachedDataDto = new AnimalDetailsDto()
            {
                Animal = new AnimalDto()
                {
                    Id = Guid.NewGuid(),
                    Breed = "test",
                    AnimalType = "test"
                },
                Profile = new AnimalProfileDto
                {
                    Id = Guid.NewGuid(),
                    Name = "test",
                    Description = "test",
                    Age = 20,
                    DateOfBirth = DateOnly.Parse("2004-10-28"),
                    Sex = "Male",
                    IsSterilized = true,
                    IsVaccinated = true,
                    City = "test",
                    Country = "test",
                },
                Images = new List<AnimalImageDto>()
                { new AnimalImageDto
                    {
                        ImageData = new byte[1],
                        ImageFormat = "imageFormat",
                    } 
                },
            };

            _cacheServiceMock.Setup(service => service.GetAsync<AnimalDetailsDto>(cacheKey))
                .ReturnsAsync(mockCachedDataDto);

            var result = await _animalService.GetAnimalProfileDetails(ownerId, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(mockCachedDataDto);

            _cacheServiceMock.Verify(service => service.GetAsync<AnimalDetailsDto>(cacheKey), Times.Once);

            _animalProfileRepositoryMock.Verify(repo => repo.GetAnimalProfileDetails(ownerId, CancellationToken.None), Times.Never);
            _cacheServiceMock.Verify(service => service.SetAsync(cacheKey, It.IsAny<AnimalDetailsDto>(), TimeSpan.FromHours(1)), Times.Never);

        }
        [Fact]
        public async Task GetAnimalProfileDetails_ReturnsDataNotFoundFromDatabase()
        {
            var ownerId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            _animalProfileRepositoryMock.Setup(repo => repo.GetAnimalProfileDetails(ownerId, cancellationToken))
                .ThrowsAsync(new AnimalNotFoundException());

            var result = await _animalService.GetAnimalProfileDetails(ownerId, cancellationToken);

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(AnimalProfileErrors.NotFound.Code);
            result.Error.Description.Should().Be(AnimalProfileErrors.NotFound.Description);

            //_cacheServiceMock.Verify(service => service.GetAsync<AnimalImageDto>(cacheKey), Times.Once);
            _animalProfileRepositoryMock.Verify(repo => repo.GetAnimalProfileDetails(ownerId, cancellationToken), Times.Once);
            //_cacheServiceMock.Verify(service => service.SetAsync(cacheKey, It.IsAny<AnimalImageDto>(), TimeSpan.FromMinutes(5)), Times.Never);
        }

        [Fact]
        public async Task GetAnimalProfileDetails_ReturnsDataFromDatabaseWhenCacheIsEmpty()
        {
            var cancellationToken = CancellationToken.None;
            var ownerId = Guid.NewGuid();
            var cacheKey = "GET_ANIMAL_PROFILE_DETAILS_BY_OWNER" + ownerId.ToString();

            var mockDatabaseData = new AnimalProfile
            {
                Id = Guid.NewGuid(),
                Name = "Test Animal Profile Name",
                Description = "Dummy description",
                Age = 20,
                DateOfBirth = DateOnly.Parse("2004-10-28"),
                Sex = new Sex(1, "Male"),
                IsSterilized = true,
                IsVaccinated = true,
                City = "Riga",
                Country = "Latvia",
                Animal = new Animal() { Id = Guid.NewGuid(), Type = new AnimalType(1, "Dog"), Breed = new Breed(1, "American Foxhind", 1)},
                Images = new List<AnimalImage>() { new AnimalImage() { ImageData = new byte[1], ImageFormat = "imageExtenstion" } }
            };

            var mockAnimalProfileDto = new AnimalDetailsDto() { 
                Animal = new AnimalDto()
                {
                    Id = mockDatabaseData.Animal.Id,
                    Breed = mockDatabaseData.Animal.Breed.BreedName,
                    AnimalType = mockDatabaseData.Animal.Type.TypeName
                },
                Profile = new AnimalProfileDto
                {
                    Id = mockDatabaseData.Id,
                    Name = mockDatabaseData.Name,
                    Description = mockDatabaseData.Description,
                    Age = mockDatabaseData.Age,
                    DateOfBirth = mockDatabaseData.DateOfBirth,
                    Sex = mockDatabaseData.Sex.SexName,
                    IsSterilized = mockDatabaseData.IsSterilized,
                    IsVaccinated = mockDatabaseData.IsVaccinated,
                    City = mockDatabaseData.City,
                    Country = mockDatabaseData.Country,
                },
                Images = mockDatabaseData.Images.Select(i => new AnimalImageDto
                {
                    ImageData = i.ImageData,
                    ImageFormat = i.ImageFormat
                }).ToList(),
            };

            _cacheServiceMock.Setup(service => service.GetAsync<AnimalDetailsDto?>(cacheKey))
                .ReturnsAsync((AnimalDetailsDto?)null);

            _animalProfileRepositoryMock.Setup(repo => repo.GetAnimalProfileDetails(ownerId, cancellationToken))
                .ReturnsAsync(mockDatabaseData);

            _cacheServiceMock.Setup(service => service.SetAsync<AnimalDetailsDto>(cacheKey, mockAnimalProfileDto, TimeSpan.FromHours(1)))
                .Returns(Task.CompletedTask);

            var result = await _animalService.GetAnimalProfileDetails(ownerId, cancellationToken);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(mockAnimalProfileDto);

            _cacheServiceMock.Verify(service => service.GetAsync<AnimalDetailsDto>(cacheKey), Times.Once);
            _animalProfileRepositoryMock.Verify(repo => repo.GetAnimalProfileDetails(ownerId, cancellationToken), Times.Once);
            _cacheServiceMock.Verify(service => service.SetAsync(
                cacheKey,
                It.Is<AnimalDetailsDto>(dto => dto.Animal.Id == mockAnimalProfileDto.Animal.Id),
                TimeSpan.FromHours(1)
            ), Times.Once);
        }
        #endregion
        #region CreateAnimal
        [Fact]
        public async Task CreateAnimal_ReturnsOperationCanceledFailure()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();
            var result = await _animalService.CreateAnimalAsync(It.IsAny<AnimalDto>(), cts.Token);
            cts.Dispose();
    
            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(OperationCancellationErrors.OperationCancelled.Code);
            result.Error.Description.Should().Be(OperationCancellationErrors.OperationCancelled.Description);
        }

        [Fact]
        public async Task CreateAnimal_ReturnAnimalIsNotCreatedFailure() 
        {
            var animalDto = new AnimalDto()
            {
                Id = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                BreedId = 1,
                AnimalTypeId = 1,
            };

            _mapperMock.Setup(mapper => mapper.Map<Animal>(It.IsAny<AnimalModel>()))
                .Returns(It.IsAny<Animal>());

            _animalRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Animal>(), CancellationToken.None)).ReturnsAsync(Guid.Empty);

            var result = await _animalService.CreateAnimalAsync(animalDto, CancellationToken.None);
            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(AnimalProfileErrors.NotCreatedAnimal.Code);
            result.Error.Description.Should().Be(AnimalProfileErrors.NotCreatedAnimal.Description);

            _mapperMock.Verify(mapper => mapper.Map<Animal>(It.IsAny<AnimalModel>()), Times.Once);
            _animalRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Animal>(), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task CreateAnimal_ReturnAnimalCreatedSuccess()
        {
            var animalDto = new AnimalDto()
            {
                Id = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                BreedId = 1,
                AnimalTypeId = 1,
            };
            var createdAnimalId = Guid.NewGuid();
            _mapperMock.Setup(mapper => mapper.Map<Animal>(It.IsAny<AnimalModel>()))
                .Returns(It.IsAny<Animal>());

            _animalRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Animal>(), CancellationToken.None)).ReturnsAsync(createdAnimalId);

            var result = await _animalService.CreateAnimalAsync(animalDto, CancellationToken.None);
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(createdAnimalId);

            _mapperMock.Verify(mapper => mapper.Map<Animal>(It.IsAny<AnimalModel>()), Times.Once);
            _animalRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Animal>(), CancellationToken.None), Times.Once);
        }
        #endregion
        #region CreateAnimalProfile
        [Fact]
        public async Task CreatePetProfile_ReturnsOperationCanceledFailure()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();
            var result = await _animalService.CreatePetProfile(It.IsAny<AnimalProfileDto>(), cts.Token);
            cts.Dispose();

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(OperationCancellationErrors.OperationCancelled.Code);
            result.Error.Description.Should().Be(OperationCancellationErrors.OperationCancelled.Description);
        }

        [Fact]
        public async Task CreatePetProfile_ReturnAnimalIsNotCreatedFailure()
        {
            var animalProfileDto = new AnimalProfileDto()
            {
                Id = Guid.NewGuid()
            };

            _mapperMock.Setup(mapper => mapper.Map<AnimalProfile>(It.IsAny<AnimalProfileModel>()))
                .Returns(It.IsAny<AnimalProfile>());

            _animalProfileRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<AnimalProfile>(), CancellationToken.None))
                .ReturnsAsync(Guid.Empty);

            var result = await _animalService.CreatePetProfile(animalProfileDto, CancellationToken.None);
            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(AnimalProfileErrors.NotCreatedProfile.Code);
            result.Error.Description.Should().Be(AnimalProfileErrors.NotCreatedProfile.Description);

            _mapperMock.Verify(mapper => mapper.Map<AnimalProfile>(It.IsAny<AnimalProfileModel>()), Times.Once);
            _animalProfileRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<AnimalProfile>(), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task CreatePetProfile_ReturnAnimalCreatedSuccess()
        {
            var animalProfileDto = new AnimalProfileDto()
            {
                Id = Guid.NewGuid()
            };
            var createdAnimalProfileId = Guid.NewGuid();
            _mapperMock.Setup(mapper => mapper.Map<AnimalProfile>(It.IsAny<AnimalProfileModel>()))
                .Returns(It.IsAny<AnimalProfile>());

            _animalProfileRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<AnimalProfile>(), CancellationToken.None))
                .ReturnsAsync(createdAnimalProfileId);

            var result = await _animalService.CreatePetProfile(animalProfileDto, CancellationToken.None);
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(createdAnimalProfileId);

            _mapperMock.Verify(mapper => mapper.Map<AnimalProfile>(It.IsAny<AnimalProfileModel>()), Times.Once);
            _animalProfileRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<AnimalProfile>(), CancellationToken.None), Times.Once);
        }
        #endregion
        #region UpdateAnimal
        [Fact]
        public async Task UpdateAnimal_ReturnsOperationCanceledFailure()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();
            var result = await _animalService.UpdateAnimal(It.IsAny<AnimalDto>(), cts.Token);
            cts.Dispose();

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(OperationCancellationErrors.OperationCancelled.Code);
            result.Error.Description.Should().Be(OperationCancellationErrors.OperationCancelled.Description);
        }

        [Fact]
        public async Task UpdateAnimal_ReturnsDataNotFoundFromDatabase()
        {
            var animalDto =new AnimalDto 
            {
                Id = Guid.NewGuid()
            };
            var cancellationToken = CancellationToken.None;

            _mapperMock.Setup(mapper => mapper.Map<Animal>(It.IsAny<AnimalModel>()))
                .Returns(It.IsAny<Animal>());

            _animalRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Animal>(), cancellationToken))
                .ThrowsAsync(new AnimalNotFoundException());

            var result = await _animalService.UpdateAnimal(animalDto, cancellationToken);

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(AnimalProfileErrors.NotUpdated.Code);
            result.Error.Description.Should().Be(AnimalProfileErrors.NotUpdated.Description);

            _mapperMock.Verify(mapper => mapper.Map<Animal>(It.IsAny<AnimalModel>()), Times.Once);
            _animalRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Animal>(), cancellationToken), Times.Once);
        }
        [Fact]
        public async Task UpdateAnimal_ReturnsDataUpdatedSuccess()
        {
            var animalDto = new AnimalDto
            {
                Id = Guid.NewGuid()
            };

            var mockAnimal = new Animal
            {
                Id = Guid.NewGuid(),
                Breed = new Breed(1, "test", 1),
                Type = new AnimalType(1, "test"),
                UserId = Guid.NewGuid(),
            };
            var cancellationToken = CancellationToken.None;

            _mapperMock.Setup(mapper => mapper.Map<Animal>(It.IsAny<AnimalModel>()))
                .Returns(It.IsAny<Animal>());

            _animalRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Animal>(), cancellationToken))
                .ReturnsAsync(mockAnimal);

            var result = await _animalService.UpdateAnimal(animalDto, cancellationToken);

            result.IsSuccess.Should().BeTrue();
            result.Value.Breed.Should().Be(mockAnimal.Breed.BreedName);
            result.Value.AnimalType.Should().Be(mockAnimal.Type.TypeName);


            _mapperMock.Verify(mapper => mapper.Map<Animal>(It.IsAny<AnimalModel>()), Times.Once);
            _animalRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Animal>(), cancellationToken), Times.Once);
        }
        #endregion
        #region UpdatePetProfile
        [Fact]
        public async Task UpdatePetProfile_ReturnsOperationCanceledFailure()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();
            var result = await _animalService.UpdatePetProfile(It.IsAny<AnimalProfileDto>(), cts.Token);
            cts.Dispose();

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(OperationCancellationErrors.OperationCancelled.Code);
            result.Error.Description.Should().Be(OperationCancellationErrors.OperationCancelled.Description);
        }

        [Fact]
        public async Task UpdatePetProfile_ReturnsDataNotFoundFromDatabase()
        {
            var animalProfileDto = new AnimalProfileDto
            {
                Id = Guid.NewGuid()
            };
            var cancellationToken = CancellationToken.None;

            _mapperMock.Setup(mapper => mapper.Map<AnimalProfile>(It.IsAny<AnimalProfileModel>()))
                .Returns(It.IsAny<AnimalProfile>());

            _animalProfileRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<AnimalProfile>(), cancellationToken))
                .ThrowsAsync(new AnimalNotFoundException());

            var result = await _animalService.UpdatePetProfile(animalProfileDto, cancellationToken);

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(AnimalProfileErrors.NotUpdated.Code);
            result.Error.Description.Should().Be(AnimalProfileErrors.NotUpdated.Description);

            _mapperMock.Verify(mapper => mapper.Map<AnimalProfile>(It.IsAny<AnimalProfileModel>()), Times.Once);
            _animalProfileRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<AnimalProfile>(), cancellationToken), Times.Once);
        }
        [Fact]
        public async Task UpdatePetProfile_ReturnsDataUpdatedSuccess()
        {
            var animaProfilelDto = new AnimalProfileDto
            {
                Id = Guid.NewGuid()
            };

            var mockAnimalProfile = new AnimalProfile
            {
                Id = Guid.NewGuid(),
                Sex = new Sex(1, "male")
            };
            var cancellationToken = CancellationToken.None;

            _mapperMock.Setup(mapper => mapper.Map<AnimalProfile>(It.IsAny<AnimalProfileModel>()))
                .Returns(It.IsAny<AnimalProfile>());

            _animalProfileRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<AnimalProfile>(), cancellationToken))
                .ReturnsAsync(mockAnimalProfile);

            var result = await _animalService.UpdatePetProfile(animaProfilelDto, cancellationToken);

            result.IsSuccess.Should().BeTrue();
            result.Value.Sex.Should().Be(mockAnimalProfile.Sex.SexName);

            _mapperMock.Verify(mapper => mapper.Map<AnimalProfile>(It.IsAny<AnimalProfileModel>()), Times.Once);
            _animalProfileRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<AnimalProfile>(), cancellationToken), Times.Once);
        }
        #endregion
    }
}
