using System;
using System.Collections.Generic;

namespace TinderForPets.Data.Entities;

public partial class AnimalType
{
    public int Id { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Animal> Animals { get; set; } = new List<Animal>();
}
