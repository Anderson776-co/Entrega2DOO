namespace Domain.Exceptions
{
    public class UnauthorizedException : AppException
    {
        public UnauthorizedException(string message = "Credenciales incorrectas.")
            : base(message, 401)
        {
        }
    }
}