namespace HRSYS.Application.Exceptions
{
    public class AccountDisabledException : Exception
    {
        public AccountDisabledException(string message) : base(message) { }
    }
}
