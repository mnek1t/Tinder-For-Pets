﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinderForPets.Data.Exceptions
{
    public class AnimalNotFoundException : Exception
    {
        public AnimalNotFoundException()
        : base($"Animal was not found. (null)") { }
    }
}
