namespace Domain.Exceptions
{
    public class NonFoundException : AppException
    {
        public NonFoundException(string message) : base(message,404)
        {
        }
    }
}
