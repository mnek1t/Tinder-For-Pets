using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinderForPets.Data.Entities
{
    public class Match : IEntity
    {
        public Guid Id { get; set; }
        public Guid FirstSwiperId { get; set; }
        public Guid SecondSwiperId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public AnimalProfile FirstSwiper { get; set; }
        public AnimalProfile SecondSwiper { get; set; }

    }
}
