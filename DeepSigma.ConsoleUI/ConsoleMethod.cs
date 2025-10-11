

namespace DeepSigma.ConsoleUI;

/// <summary>
/// Represents a method that can be executed from the console, along with its description.
/// </summary>
public class ConsoleMethod
{
    /// <summary>
    /// The method to be executed when the console argument is invoked.
    /// </summary>
    public Action<Dictionary<string, string?>, Dictionary<char, bool>> Method { get; set; }

    private Action<Dictionary<string, string?>, Dictionary<char, bool>> UserDefinedMethod { get; init; }

    /// <summary>
    /// Description of the console argument, which can be used to provide information about what the argument does.
    /// </summary>
    public string Description { get; set; } = String.Empty;

    /// <summary>
    /// A set of valid arguments that can be used with this console method. Each argument is represented as a name-value pair.
    /// </summary>
    internal Dictionary<string, string?> ValidArguments { get; set; } = [];

    /// <summary>
    /// A set of valid flags that can be used with this console method. Flags are typically single characters (e.g., '-h' for help).
    /// </summary>
    internal Dictionary<char, bool> ValidFlags { get; set; } = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleMethod"/> class.
    /// </summary>
    /// <param name="action"></param>
    /// <param name="description"></param>
    /// <param name="valid_arguments"></param>
    /// <param name="valid_flags"></param>
    public ConsoleMethod(Action<Dictionary<string, string?>, Dictionary<char, bool>> action, string description, Dictionary<string, string?>? valid_arguments = null, Dictionary<char, bool>? valid_flags = null)
    {
        this.Description = description;
        this.UserDefinedMethod = action;
        this.Method = CustomMethodWithArgumentValidation;

        if (valid_arguments is not null)
        {
            this.ValidArguments = valid_arguments;
        }

        if (valid_flags is not null)
        {
            this.ValidFlags = valid_flags;
        }
    }

    /// <summary>
    /// Executes the user-defined method with validation of arguments and flags.
    /// </summary>
    /// <param name="selected_arguments"></param>
    /// <param name="selected_flags"></param>
    private void CustomMethodWithArgumentValidation(Dictionary<string, string?> selected_arguments, Dictionary<char, bool> selected_flags)
    {
        bool arguments_are_valid = IsValidArguments(selected_arguments, selected_flags);

        if (arguments_are_valid == false)
        {
            ConsoleUtilities.Print("Invalid arguments or flags provided.", ConsoleColor.Red, false);
            return;
        }

        this.UserDefinedMethod.Invoke(selected_arguments, selected_flags);
    }

    /// <summary>
    /// Validates the provided arguments and flags against the method's valid arguments and flags.
    /// </summary>
    /// <param name="selected_arguments"></param>
    /// <param name="selected_flags"></param>
    private bool IsValidArguments(Dictionary<string, string?> selected_arguments, Dictionary<char, bool> selected_flags)
    {
        bool is_valid = true;
        // Validate the arguments and flags against the method's valid arguments and flags
        foreach (KeyValuePair<string, string?> arg in selected_arguments)
        {
            if (ValidArguments.ContainsKey(arg.Key) == false)
            {
                is_valid = false;
                ConsoleUtilities.Print($"Invalid argument: {arg.Key}", ConsoleColor.Red, false);
            }
        }

        foreach (KeyValuePair<char, bool> flag in selected_flags)
        {
            if (ValidFlags.ContainsKey(flag.Key) == false)
            {
                is_valid = false;
                ConsoleUtilities.Print($"Invalid flag: {flag.Key}", ConsoleColor.DarkRed, false);
            }
        }
        return is_valid;
    }
}
