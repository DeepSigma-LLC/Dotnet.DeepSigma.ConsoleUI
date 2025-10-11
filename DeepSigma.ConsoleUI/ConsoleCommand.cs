
namespace DeepSigma.ConsoleUI;

/// <summary>
/// Represents a command that can be executed from the console, including its command name, arguments, and flags.
/// </summary>
public class ConsoleCommand
{
    /// <summary>
    /// The command to be executed when the console argument is invoked. This is a string that represents the command name.
    /// </summary>
    public string? Command {  get; set; }

    /// <summary>
    /// Represents a set of arguments that can be used with the command. Each argument is represented as a name-value pair.
    /// </summary>
    public Dictionary<string, string?> Arguments { get; init; } = [];

    /// <summary>
    /// Represents a set of flags that can be used with the command. Flags are typically single characters (e.g., '-h' for help).
    /// </summary>
    public Dictionary<char, bool> Flags { get; init; } = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleCommand"/> class.
    /// </summary>
    public ConsoleCommand(string? Command, Dictionary<string, string?>? Arguments = null, HashSet<char>? Flags = null)
    {
        this.Command = Command?.Trim().ToLower();
        this.Arguments = Arguments ?? [];
        this.Flags = Flags?.ToDictionary(flag => flag, flag => true) ?? [];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleCommand"/> class.
    /// </summary>
    public ConsoleCommand()
    {
        
    }


}
