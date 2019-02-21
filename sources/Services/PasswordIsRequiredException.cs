namespace Services
{
    public class PasswordIsRequiredException : BaseServiceException
    {
        public PasswordIsRequiredException() : base("Password is required")
        {
        }
    }
}