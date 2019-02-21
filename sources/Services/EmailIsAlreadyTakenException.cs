namespace Services
{
    public class EmailIsAlreadyTakenException : BaseServiceException
    {
        public EmailIsAlreadyTakenException(string email) : base($"Email \"{email}\" is already taken")
        {
        }
    }
}