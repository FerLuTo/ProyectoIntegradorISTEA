namespace Common.Helper;

using System.Globalization;

// custom exception class for throwing application specific exceptions 
// that can be caught and handled within the application
public class AppException : Exception
{
    private Exception ex;

    public AppException() : base() { }

    public AppException(string message) : base(message) { }

    public AppException(Exception ex)
    {
        this.ex = ex;
    }

    public AppException(string message, params object[] args)
        : base(String.Format(CultureInfo.CurrentCulture, message, args))
    {
    }
}