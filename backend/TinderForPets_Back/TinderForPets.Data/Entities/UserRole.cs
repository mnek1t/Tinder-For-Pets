using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinderForPets.Data.Entities
{
    public class UserRole
    {
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public Guid UserId { get; set; }

        public UserAccount User { get; set; }
        
    }
}
