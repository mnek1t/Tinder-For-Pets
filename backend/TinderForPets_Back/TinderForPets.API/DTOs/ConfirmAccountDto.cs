namespace TinderForPets.API.DTOs
{
    public class ConfirmAccountDto    
    {
        public string EmailAddress { get; set; } = string.Empty;
        public string ConfirmAccountLink { get; set; } = string.Empty;
    }
}
