using System;
using System.Collections.Generic;

namespace TinderForPets.Data.Entities;

public partial class AnimalProfile
{
    public Guid? Id { get; set; }

    public string? Name { get; set; }

    public int? Age { get; set; }

    public int? SexId { get; set; }

    public byte[]? Image { get; set; }
}
