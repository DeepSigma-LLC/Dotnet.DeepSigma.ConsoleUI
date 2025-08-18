using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.ConsoleUI
{
    /// <summary>
    /// Represents a method that can be executed from the console, along with its description.
    /// </summary>
    public class ConsoleMethod
    {
        /// <summary>
        /// The method to be executed when the console argument is invoked.
        /// </summary>
        public Action<HashSet<ArgumentValuePair>, HashSet<char>> Method { get; set; }

        private Action<HashSet<ArgumentValuePair>, HashSet<char>> UserDefinedMethod { get; init; }

        /// <summary>
        /// Description of the console argument, which can be used to provide information about what the argument does.
        /// </summary>
        public string Description { get; set; } = String.Empty;

        public HashSet<ArgumentValuePair> ValidArguments { get; set; } = [];
        public HashSet<char> ValidFlags { get; set; } = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleMethod"/> class.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="description"></param>
        /// <param name="valid_arguments"></param>
        /// <param name="valid_flags"></param>
        public ConsoleMethod(Action<HashSet<ArgumentValuePair>, HashSet<char>> action, string description, HashSet<ArgumentValuePair>? valid_arguments = null, HashSet<char>? valid_flags = null)
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
        private void CustomMethodWithArgumentValidation(HashSet<ArgumentValuePair> selected_arguments, HashSet<char> selected_flags)
        {
            ValidateArguments(selected_arguments, selected_flags);

            this.UserDefinedMethod.Invoke(selected_arguments, selected_flags);
        }

        /// <summary>
        /// Validates the provided arguments and flags against the method's valid arguments and flags.
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="flags"></param>
        /// <exception cref="ArgumentException"></exception>
        private void ValidateArguments(HashSet<ArgumentValuePair> arguments, HashSet<char> flags)
        {
            // Validate the arguments and flags against the method's valid arguments and flags
            foreach (var arg in arguments)
            {
                if (!ValidArguments.Contains(arg))
                {
                    throw new ArgumentException($"Invalid argument: {arg.Argument}");
                }
            }

            foreach (var flag in flags)
            {
                if (!ValidFlags.Contains(flag))
                {
                    throw new ArgumentException($"Invalid flag: {flag}");
                }
            }
        }
    }
}
