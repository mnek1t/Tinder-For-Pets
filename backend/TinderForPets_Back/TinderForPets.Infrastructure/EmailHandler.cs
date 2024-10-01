using System.Text.RegularExpressions;

namespace TinderForPets.Infrastructure
{
    public partial class EmailHandler
    {
        private const string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        public static bool IsValidEmailFormat(string email)
        {
            return MyRegex().IsMatch(email);
        }

        [GeneratedRegex(emailPattern)]
        private static partial Regex MyRegex();
    }
}
