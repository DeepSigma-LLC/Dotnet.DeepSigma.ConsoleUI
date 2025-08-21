using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.ConsoleUI
{
    /// <summary>
    /// Responsible for parsing command-line arguments and organizing them into a structured format.
    /// </summary>
    public static class CommandLineArgumentParser
    {
        private static string null_command = "null";

        /// <summary>
        /// Processes the command-line arguments and organizes them into a structured format.
        /// </summary>
        /// <param name="all_arguments"></param>
        /// <param name="known_commands"></param>
        /// <returns></returns>
        public static List<ConsoleCommand> ProcessArguments(string[] all_arguments, string[] known_commands)
        {
            List<ConsoleCommand> commands = [];
            string[] joinedArguments = JoinQuotedArgs(all_arguments);
            Dictionary<string, string[]> command_collection = GetCommands(joinedArguments, known_commands);

            foreach(KeyValuePair<string, string[]> command_values in command_collection)
            {
                (string command, string[] argument_values) = (command_values.Key, command_values.Value);
                ConsoleCommand commandArgs = ParseSingleConsoleCommand(argument_values, known_commands);
                if (command == null_command)
                {
                    commandArgs.Command = null;
                }
                commands.Add(commandArgs);
            }
            return commands;
        }

        /// <summary>
        /// Parses a single console command from the provided arguments, extracting flags and argument values.
        /// </summary>
        /// <param name="all_arguments"></param>
        /// <param name="known_commands"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">Exception will be thrown if a command is not found in the known commands.</exception>
        private static ConsoleCommand ParseSingleConsoleCommand(string[] all_arguments, string[] known_commands)
        {
            ArgumentType? lastArgumentType = null;
            List<string> past_arguments = [];
            ConsoleCommand commandArgs = new();

            for (int i = 0; i < all_arguments.Length; i++)
            {
                string value = all_arguments[i].Trim();

                if (lastArgumentType == ArgumentType.ArgumentName && value == "=") // Argument Value 
                {
                    // Do nothing
                }
                else if(lastArgumentType == ArgumentType.ArgumentName) // Stand alone argument value
                {
                    lastArgumentType = ArgumentType.ArgumentValue;

                    string targeted_argument_name = past_arguments.Last().TrimStart('-');
                    if (past_arguments.Last() == "=")
                    {
                        targeted_argument_name = past_arguments.ElementAt(past_arguments.Count - 2).TrimStart('-'); // minus 2 becuase -1 to convert to index and -1 to take the prior value
                    }
                    commandArgs.Arguments.Add(targeted_argument_name, value);
                }
                else if (value.StartsWith("--") && value.Contains("=")) // Full argument
                {
                    SaveFullArgumentValue(commandArgs, value);
                }
                else if (value.StartsWith("--") && value.Contains("=") == false) //Argument key
                {
                    lastArgumentType = ArgumentType.ArgumentName;
                }
                else if (value.StartsWith("-")) //Flag
                {
                    lastArgumentType = ArgumentType.Flag;
                    SaveFlags(commandArgs, value);
                }
                else if (known_commands.Contains(value)) //Command
                {
                    commandArgs.Command = value;
                }
                else
                {
                    ConsoleUtilities.Print("Unknown command", ConsoleColor.Red);
                }

                past_arguments.Add(value);
            }
            return commandArgs;
        }

        private static void SaveFullArgumentValue(ConsoleCommand command, string value)
        {
            string[] equals_split = value.Split("=");
            if (equals_split.Count() >= 3)
            {
                throw new ArgumentException("Unknown argument");
            }
            command.Arguments.Add(equals_split[0].TrimStart('-'), equals_split[1]);
        }

        /// <summary>
        /// Saves flags from the command-line arguments into the ConsoleCommandArgs object.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="value"></param>
        private static void SaveFlags(ConsoleCommand command, string value)
        {
            string trimed_value = value.TrimStart('-');
            char[] flags = trimed_value.ToCharArray();
            foreach (char flag in flags)
            {
                if (command.Flags.ContainsKey(flag) == true)
                {
                    ConsoleUtilities.Print($"Flag '{flag}' was already defined.", ConsoleColor.Red);
                    continue;
                }
                command.Flags.Add(flag, true);
            }
        }

        /// <summary>
        /// Processes the command-line arguments and organizes them into a dictionary of commands and their associated arguments.
        /// </summary>
        /// <param name="all_arguments"></param>
        /// <param name="known_commands"></param>
        /// <returns>Dictionary containing commands as keys and the arguments as an array of strings.</returns>
        private static Dictionary<string, string[]> GetCommands(string[] all_arguments, string[] known_commands)
        {
            Dictionary<string, string[]> command_dict = [];
            List<int> command_indexes = GetCommandIndexes(all_arguments, known_commands);

            // If no commands are found, add the default command with all arguments
            if (command_indexes.Count == 0)
            {
                command_dict.Add(null_command, all_arguments);
                return command_dict;
            }

            int iteration = 0;
            // If commands are found, process them
            foreach (int command_index in command_indexes)
            {
                // If the first command is not at index 0, we need to store the initial arguments
                if (iteration == 0 && command_index != 0)
                {
                    command_dict.Add(null_command, all_arguments.Take(command_index).ToArray());
                }

                if (command_index == command_indexes.Last())
                {
                    // If this is the last command, take all remaining arguments
                    command_dict.Add(all_arguments[command_index], all_arguments.Skip(command_index).ToArray());
                }
                else
                {
                    // If there is a next command, take arguments until the next command index
                    string command = all_arguments[command_index];

                    int next_command_index = command_indexes[iteration + 1];
                    int aruments_to_take = next_command_index - command_index;

                    string[] arguments_to_add = all_arguments.Skip(command_index).Take(aruments_to_take).ToArray();
                    command_dict.Add(command, arguments_to_add);
                }
                iteration++;
            }
            return command_dict;
        }


        /// <summary>
        /// Returns a list of indexes where the specified commands are found in the arguments.
        /// </summary>
        /// <param name="all_arguments"></param>
        /// <param name="known_commands"></param>
        /// <returns></returns>
        private static List<int> GetCommandIndexes(string[] all_arguments, string[] known_commands)
        {
            List<int> command_indexes = [];
            for (int i = 0; i < all_arguments.Length; i++)
            {
                string arg = all_arguments[i].Trim().ToLower();
                if (known_commands.Contains(arg) == true)
                {
                    command_indexes.Add(i);
                }
            }
            return command_indexes;
        }

        /// <summary>
        /// Joins arguments that are enclosed in quotes, treating them as a single argument.
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        private static string[] JoinQuotedArgs(string[] arguments)
        {
            List<string> results = [];
            bool inQuotes = false;
            StringBuilder current = new StringBuilder();

            foreach (string arg in arguments)
            {
                if (inQuotes == false)
                {
                    // Look for opening quote
                    if (arg.Contains("\"") == true)
                    {
                        inQuotes = true;
                        current.Append(arg);
                        continue;
                    }
                    else
                    {
                        results.Add(arg);
                        continue;
                    }
                }

                // Accumulate arguments until closing quote is found
                current.Append(" ").Append(arg);
                if (arg.EndsWith("\""))
                {
                    string result = current.ToString().Replace("\"", string.Empty);
                    results.Add(result);
                    current.Clear();
                    inQuotes = false;
                }
            }

            // Handle unclosed quote
            if (inQuotes)
            {
                results.Add(current.ToString());
            }
            return results.ToArray();
        }
    }
}
