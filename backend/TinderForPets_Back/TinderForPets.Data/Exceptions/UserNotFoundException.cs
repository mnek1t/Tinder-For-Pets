namespace TinderForPets.Data.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException()
        : base($"User was not found.") { }

        public UserNotFoundException(string email)
        : base($"User with email '{email}' was not found.") { }
    }
}
