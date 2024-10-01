using SharedKernel;

namespace TinderForPets.Core
{
    public class UserErrors
    {
        public static Error NotFound(Guid userId) => new(
            "Users.NotFound", $"The user with the Id = {userId} was not found");
        public static Error NotFoundByEmail(string email) => new(
            "Users.NotFoundByEmail", $"The user with the Email = {email} was not found");

        public static readonly Error EmailNotUnique = new(
        "Users.EmailNotUnique", "The provided email is not unique");

        public static readonly Error InvalidPassword = new(
        "Users.InvalidPassword", "The provided password is not correct");

        public static readonly Error InvalidEmailFormat = new(
        "Users.InvalidEmailFormat", "The provided email is invalid format");
    }
}
