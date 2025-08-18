using System.Drawing;

namespace DeepSigma.ConsoleUI
{
    /// <summary>
    /// A utility class for printing messages to the console with options for color and formatting.
    /// </summary>
    public class ConsoleUtilities
    {
        /// <summary>
        /// Prints a message to the console with optional new line and color.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="new_line"></param>
        /// <param name="color"></param>
        public static void Print(string message, ConsoleColor color = ConsoleColor.White, bool new_line = true)
        {
            Console.ForegroundColor = color;
            if (new_line)
            {
                Console.WriteLine(message);
                Console.ResetColor();
                return;
            }
            Console.Write(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Overwrites the current line in the console with a new message. This is useful for updating progress or status messages without cluttering the console with multiple lines.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="color"></param>
        public static void PrintOverwriteCurrentLine(string message, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.Write("\r" + message);
            Console.ResetColor();
        }
    }
}
