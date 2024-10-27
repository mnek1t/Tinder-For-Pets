namespace TinderForPets.Infrastructure
{
    public class AnimalProfileHandler
    {
        public static int CalculateAge(DateOnly dateOfBirth) 
        {
            var today = DateTime.Today;
            var birthDateTime = dateOfBirth.ToDateTime(TimeOnly.MinValue);

            var age = today.Year - birthDateTime.Year;

            if (today < birthDateTime.AddYears(age)) 
            {
                age--;
            }

            return age;
        } 
    }
}
