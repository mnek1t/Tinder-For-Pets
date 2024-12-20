using SharedKernel;

namespace TinderForPets.Core
{
    public class UserErrors
    {
        public static Error NotFound(Guid userId) => new(
            "Users.NotFound", $"The user with the Id = {userId} was not found");
        public static Error NotFoundByEmail(string email) => new(
            "Users.NotFoundByEmail", $"The user does not exist");
        public static Error DuplicateUser(string email) => new(
            "Users.DuplicateUser", $"The user with {email} email already exists");

        public static readonly Error EmailNotUnique = new(
        "Users.EmailNotUnique", "The provided email is not unique");

        public static readonly Error InvalidPassword = new(
        "Users.InvalidPassword", "The provided password is not correct");

        public static readonly Error InvalidEmailFormat = new(
        "Users.InvalidEmailFormat", "The provided email is invalid format");

        public static readonly Error NotCreated = new(
        "Users.NotCreated", "The provided user was not created (null)");

        public static readonly Error NotMatchPassword = new(
        "User.NotMatchPassword", "New Password and Confirm Password do not match");

        public static readonly Error NotAuthorized = new(
        "User.NotAuthorized", "Authorization Header was not attached to request");

    }
}
