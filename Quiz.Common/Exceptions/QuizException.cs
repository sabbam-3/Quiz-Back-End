using Quiz.Common.Results;

namespace Quiz.Common.Exceptions;

public class QuizException : Exception
{
    public QuizException(string requestName, Error? error = default, Exception? innerException = default)
        : base("Application exception", innerException)
    {
        RequestName = requestName;
        Error = error;
    }

    public string RequestName { get; }

    public Error? Error { get; }
}