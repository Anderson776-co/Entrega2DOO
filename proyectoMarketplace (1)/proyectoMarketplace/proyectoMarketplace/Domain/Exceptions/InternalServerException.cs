namespace Domain.Exceptions
{
    public class InternalServerException : AppException
    {
        public InternalServerException(string message = "Error interno.")
            : base(message, 500)
        {
        }
    }
}