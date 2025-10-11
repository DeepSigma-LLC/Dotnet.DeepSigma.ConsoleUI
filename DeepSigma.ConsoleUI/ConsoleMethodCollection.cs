

namespace DeepSigma.ConsoleUI;

/// <summary>
/// Represents a collection of console methods, allowing for the addition and retrieval of arguments by name.
/// </summary>
public class ConsoleMethodCollection
{
    private readonly Dictionary<string, ConsoleMethod> MethodCollection = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleMethodCollection"/> class.
    /// </summary>
    public ConsoleMethodCollection(){}

    /// <summary>
    /// Adds a console argument to the collection with a specified name.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="console_method"></param>
    public void Add(string command, ConsoleMethod console_method)
    {
        ValidateCommandName(command);
        ValidateConsoleMethod(console_method);
        MethodCollection[command] = console_method;
    }

    /// <summary>
    /// Retrieves all console arguments in the collection.
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, ConsoleMethod> GetCollection()
    {
        return MethodCollection;
    }

    /// <summary>
    /// Retrieves a console argument by its name.
    /// </summary>
    /// <param name="command"></param>
    /// <exception cref="ArgumentException"></exception>
    private void ValidateCommandName(string command)
    {
        if (string.IsNullOrWhiteSpace(command))
        {
            throw new ArgumentException("Command name cannot be null or whitespace.", nameof(command));
        }
    }

    /// <summary>
    /// Validates that the console argument is not null.
    /// </summary>
    /// <param name="argument"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private void ValidateConsoleMethod(ConsoleMethod argument)
    {
        if (argument == null)
        {
            throw new ArgumentNullException(nameof(argument), "Argument value cannot be null.");
        }
    }
}
