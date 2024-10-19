using SharedKernel;

namespace TinderForPets.Core
{
    public class ApiErrors
    {
        public static readonly Error InvalidRequestUrl = new(
            "ApiErrors.InvalidRequestUrl", "Provided Url was not initialized or consisted of incorrect signature");

        public static readonly Error ResponseWereNotReadCorrect = new(
            "ApiErrors.ResponseWereNotReadCorrect", "Error in reading JSON reponse");

        public static Error FailedResponse(string code, string message) => new(code, message);
    }
}
