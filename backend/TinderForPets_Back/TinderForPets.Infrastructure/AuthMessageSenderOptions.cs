namespace TinderForPets.Infrastructure
{
    public class AuthMessageSenderOptions
    {
        public string? SendGridKey { get; set; }
        public string? SendGridSenderEmail { get; set; }
        public string? SendGridSenderName { get; set; }
    }
}
