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
    public class ConsoleMethod<T>
    {
        /// <summary>
        /// The method to be executed when the console argument is invoked.
        /// </summary>
        public Action<T> Method { get; set; }

        /// <summary>
        /// Description of the console argument, which can be used to provide information about what the argument does.
        /// </summary>
        public string Description { get; set; } = String.Empty;

        /// <summary>
        /// Represents an action that can be executed from the console with a description.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="description"></param>
        public ConsoleMethod(Action<T> action, string description)
        {
            this.Description = description;
            this.Method = action;
        }
    }
}
