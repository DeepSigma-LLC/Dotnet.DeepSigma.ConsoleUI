using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.ConsoleUI
{
    /// <summary>
    /// Represents a pair of argument name and its corresponding value.
    /// </summary>
    public record ArgumentValuePair
    {
        /// <summary>
        /// The name of the argument, which is typically prefixed with a double hyphen (e.g., "--arg"). Under this framework, single hyphen arguments are reserved for flags.
        /// </summary>
        public string Argument { get; set; }

        /// <summary>
        /// The value associated with the argument. This can be null if the argument does not require a value.
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentValuePair"/> class with the specified argument name and value.
        /// </summary>
        /// <param name="arguement"></param>
        /// <param name="value"></param>
        public ArgumentValuePair(string arguement, string? value)
        {
            this.Argument = arguement;
            this.Value = value;
        }
    }
}
