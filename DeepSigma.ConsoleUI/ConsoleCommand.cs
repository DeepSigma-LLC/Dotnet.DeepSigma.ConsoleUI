using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.ConsoleUI
{
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
        public HashSet<ArgumentValuePair> Arguments { get; set; } = [];

        /// <summary>
        /// Represents a set of flags that can be used with the command. Flags are typically single characters (e.g., '-h' for help).
        /// </summary>
        public HashSet<char> Flags { get; set; } = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleCommand"/> class.
        /// </summary>
        public ConsoleCommand(){}
    }
}
