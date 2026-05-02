namespace Domain.Exceptions
{
    public sealed class UnAuthorizedException : Exception
    {
        public UnAuthorizedException(string message = $"Invalid email or password") 
            :base(message)
        { }
    }
}
