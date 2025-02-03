using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinderForPets.Application.DTOs
{
    public class MatchCardDto
    {
        public Guid MatchId { get; set; }
        public string ProfileName { get; set; } = string.Empty;
        public int Age { get; set; }
        public bool IsVaccinated { get; set; }
        public bool IsSterilized { get; set; }

        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } 

        public List<AnimalImageDto> Images { get; set; } = new List<AnimalImageDto>();
    }
}
