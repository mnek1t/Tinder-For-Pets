using System;
using System.Collections.Generic;

namespace TinderForPets.Data.Entities;

public partial class Animal
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public int TypeId { get; set; }

    public int? Age { get; set; }

    public virtual AnimalType Type { get; set; } = null!;

    public virtual UserAccount User { get; set; } = null!;
}
