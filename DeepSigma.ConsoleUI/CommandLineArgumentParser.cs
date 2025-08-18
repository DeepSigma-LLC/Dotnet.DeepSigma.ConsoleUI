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
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static IEnumerable<ConsoleCommand> ProcessArguments(string[] arguments, string[] commands)
        {
            string[] joinedArguments = JoinQuotedArgs(arguments);
            Dictionary<string, string[]> command_collection = GetCommands(joinedArguments, commands);
            List<ConsoleCommand> consoleCommands = [];

            foreach(KeyValuePair<string, string[]> command_values in command_collection)
            {
                (string command, string[] argument_values) = (command_values.Key, command_values.Value);
                ConsoleCommand commandArgs = ParseSingleConsoleCommand(argument_values);
                if (command == null_command)
                {
                    commandArgs.Command = null;
                }
                yield return commandArgs;
            }
        }

        /// <summary>
        /// Parses a single console command from the provided arguments, extracting flags and argument values.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static ConsoleCommand ParseSingleConsoleCommand(string[] args)
        {
            ArgumentType? lastArgumentType = null;
            List<string> past_arguments = [];
            ConsoleCommand commandArgs = new();

            for (int i = 0; i < args.Length; i++)
            {
                string value = args[i].Trim();

                if (lastArgumentType == ArgumentType.ArgumentName) // Argument Value 
                {
                    lastArgumentType = ArgumentType.ArgumentValue;
                    string trimed_value = past_arguments.Last().TrimStart('-');
                    ArgumentValuePair pair = new(trimed_value, value);
                }
                else if (value.StartsWith("--")) // Argument Name
                {
                    lastArgumentType = ArgumentType.ArgumentName;
                }
                else if (value.StartsWith("-")) //Flag
                {
                    lastArgumentType = ArgumentType.Flag;
                    SaveFlags(commandArgs, value);
                }
                else //Command
                {
                    throw new Exception("Unknown command.");
                }

                past_arguments.Add(value);
            }
            return commandArgs;
        }

        /// <summary>
        /// Saves flags from the command-line arguments into the ConsoleCommandArgs object.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="value"></param>
        private static void SaveFlags(ConsoleCommand args, string value)
        {
            string trimed_value = value.TrimStart('-');
            char[] flags = trimed_value.ToCharArray();
            foreach (char flag in flags)
            {
                args.Flags.Add(flag);
            }
        }

        private enum ArgumentType
        {
            QuotedText,
            Flag,
            ArgumentName,
            ArgumentValue
        }

        /// <summary>
        /// Processes the command-line arguments and organizes them into a dictionary of commands and their associated arguments.
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="commands"></param>
        /// <returns></returns>
        private static Dictionary<string, string[]> GetCommands(string[] arguments, string[] commands)
        {
            Dictionary<string, string[]> command_dict = [];
            List<int> command_indexes = GetCommandIndexes(arguments, commands);

            // If no commands are found, add the default command with all arguments
            if (command_indexes.Count == 0)
            {
                command_dict.Add(null_command, arguments);
                return command_dict;
            }

            // If commands are found, process them
            for (int c = 0; c < command_indexes.Count; c++)
            {
                int command_index = command_indexes[c];

                // If the first command is not at index 0, we need to store the initial arguments
                if (c == 0 && command_index != 0)
                {
                    int total_initial_arguments = command_index + 1;
                    command_dict.Add(null_command, arguments.Take(total_initial_arguments).ToArray());
                }
                else
                {
                    int c_next = c + 1;

                    if (c_next >= command_indexes.Count)
                    {
                        // If this is the last command, take all remaining arguments
                        command_dict.Add(commands[command_index], arguments.Skip(command_index).ToArray());
                    }
                    else
                    {
                        // If there is a next command, take arguments until the next command index
                        int next_command_index = arguments.Count() - 1;

                        if(c_next <= command_indexes.Count - 1)
                        {
                            next_command_index = command_indexes[c_next];
                        }

                        int arguments_to_skip = command_index + 1;
                        int aruments_to_take = next_command_index - arguments_to_skip;
                        command_dict.Add(commands[command_index], arguments.Skip(arguments_to_skip).Take(aruments_to_take).ToArray());
                    }
                }
            }
            return command_dict;
        }


        /// <summary>
        /// Returns a list of indexes where the specified commands are found in the arguments.
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="commands"></param>
        /// <returns></returns>
        private static List<int> GetCommandIndexes(string[] arguments, string[] commands)
        {
            List<int> command_indexes = [];
            for (int i = 0; i < arguments.Length; i++)
            {
                string arg = arguments[i].Trim().ToLower();
                if (commands.Contains(arg) == true)
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
                    if (arg.StartsWith("\"") == true && arg.EndsWith("\"") == false)
                    {
                        inQuotes = true;
                        current.Append(arg);
                    }
                    else
                    {
                        results.Add(arg);
                    }
                }
                else
                {
                    // Accumulate arguments until closing quote is found
                    current.Append(" ").Append(arg);
                    if (arg.EndsWith("\""))
                    {
                        results.Add(current.ToString().Replace("\"", string.Empty));
                        current.Clear();
                        inQuotes = false;
                    }
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
