using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using TinderForPets.Data;
using TinderForPets.Data.Entities;

namespace TinderForPets.API
{
    public class TinderForPetsDataSeeder
    {
        private readonly TinderForPetsDbContext _context;
        private readonly IWebHostEnvironment _env;
        private const string SEX_DATA_FILE_NAME = "SexData.json";
        private const string ANIMAL_TYPE_DATA_FILE_NAME = "AnimalTypeData.json";
        private const string BREED_CAT_DATA_FILE_NAME = "BreedCatData.json";
        private const string BREED_DOG_DATA_FILE_NAME = "BreedDogData.json";

        public TinderForPetsDataSeeder(TinderForPetsDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task SeedAsync() 
        {
            await SeedSexesAsync();
            await SeedAnimalTypesAsync();
            await SeedBreedsAsync();
        }

        private async Task<string> GetDataAsync(string dataFileName) 
        {
            if (dataFileName.IsNullOrEmpty()) 
            {
                throw new ArgumentNullException(nameof(dataFileName));
            }

            string rootPath = _env.ContentRootPath;
            string filePath = Path.GetFullPath(Path.Combine(rootPath, "Data", dataFileName));
            using (var r = new StreamReader(filePath))
            {
                string json = await r.ReadToEndAsync();
                return json;
            }
        }

        private async Task SeedSexesAsync() 
        {
            if (!await _context.Sexes.AnyAsync())
            {
                var data = await GetDataAsync(SEX_DATA_FILE_NAME);
                var items = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(data);
                foreach (var item in items)
                {
                    var sex = new Sex(int.Parse(item["id"]), item["name"]);
                    await _context.Sexes.AddAsync(sex);
                }

                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedAnimalTypesAsync()
        {
            if (!await _context.AnimalTypes.AnyAsync())
            {
                var data = await GetDataAsync(ANIMAL_TYPE_DATA_FILE_NAME);
                var items = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(data);
                foreach (var item in items)
                {
                    var type = new AnimalType(int.Parse(item["id"]), item["name"]);
                    await _context.AnimalTypes.AddAsync(type);
                }

                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedBreedsAsync()
        {
            if (!await _context.Breeds.AnyAsync())
            {
                var catBreedData = await GetDataAsync(BREED_CAT_DATA_FILE_NAME);
                var dogBreedData = await GetDataAsync(BREED_DOG_DATA_FILE_NAME);
                var catBreedItems = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(catBreedData);
                var dogBreedItems = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(dogBreedData);

                foreach (var item in catBreedItems)
                {
                    var breed = new Breed(int.Parse(item["Id"]), item["Name"], int.Parse(item["AnimalTypeId"]));
                    await _context.Breeds.AddAsync(breed);
                }

                foreach (var item in dogBreedItems)
                {
                    var breed = new Breed(int.Parse(item["id"].ToString()) + catBreedItems.Count, item["name"].ToString(), 1);
                    await _context.Breeds.AddAsync(breed);
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
