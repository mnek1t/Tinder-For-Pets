using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinderForPets.Core
{
    public static class OperationCancellationErrors
    {
        public static readonly Error OperationCancelled = new(
           "Operation.IsCanceled", "The following operation was canceled");
    }
}
