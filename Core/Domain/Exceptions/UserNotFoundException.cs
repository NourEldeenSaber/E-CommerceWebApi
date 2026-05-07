namespace Domain.Exceptions
{
    public sealed class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(string userEmail) : base($"User With Email : {userEmail} Not Found!")
        {
        }
    }
}
