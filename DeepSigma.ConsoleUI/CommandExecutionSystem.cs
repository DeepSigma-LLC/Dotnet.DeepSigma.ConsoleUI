using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepSigma.ConsoleUI;

namespace DeepSigma.ConsoleUI
{
    /// <summary>
    /// Responsible for processing command-line arguments and executing corresponding methods.
    /// </summary>
    public class CommandExecutionSystem
    {
        private ConsoleMethodCollection ConsoleMethodDefinitions { get; init; }
        private string AppName { get; } = string.Empty;
        private string AppVersion { get; } = string.Empty;
        private string CurrentInstallationDirectory { get; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandExecutionSystem"/> class with the specified console method definitions and application details.
        /// </summary>
        /// <param name="console_method_definitions"></param>
        /// <param name="AppName"></param>
        /// <param name="AppVersion"></param>
        /// <param name="CurrentInstallationDirectory"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CommandExecutionSystem(ConsoleMethodCollection console_method_definitions, string AppName, string AppVersion, string CurrentInstallationDirectory)
        {
            this.ConsoleMethodDefinitions = console_method_definitions ?? throw new ArgumentNullException(nameof(console_method_definitions), "Console method definitions cannot be null.");
            this.AppName = AppName;
            this.AppVersion = AppVersion;
            this.CurrentInstallationDirectory = CurrentInstallationDirectory;
            AddArguments();
        }


        /// <summary>
        /// Processes the command-line arguments passed to the application.
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public void ProcessArguments(string[] arguments)
        {
            IEnumerable<ConsoleCommand> commands = CommandLineArgumentParser.ProcessArguments(arguments, ConsoleMethodDefinitions.GetCollection().Keys.ToArray());
            foreach (ConsoleCommand command in commands)
            {
                InvokeCLIArgument(command);
            }
        }

        /// <summary>
        /// Processes the command-line interface request based on the provided argument.
        /// </summary>
        /// <param name="CLIarguement"></param>
        private void InvokeCLIArgument(ConsoleCommand argument)
        {
            string? command = argument.Command;
            if (command is null)
            {
                ConsoleUtilities.Print("Invalid command. No command was provided.", ConsoleColor.Red);
                return;
            }
            else if(ConsoleMethodDefinitions.GetCollection().ContainsKey(command) == false)
            {
                ConsoleUtilities.Print("Invalid command: " + command, ConsoleColor.Red);
                return;
            }

            command = command.Trim().ToLower();
            ConsoleMethodDefinitions.GetCollection()[command].Method.Invoke(argument.Arguments, argument.Flags);
        }

        /// <summary>
        /// Adds a help argument to the console arguments collection.
        /// </summary>
        private void AddArguments()
        {
            ConsoleMethodDefinitions.Add("help", new ConsoleMethod(ShowHelp, "Shows all available arguments."));
            ConsoleMethodDefinitions.Add("info", new ConsoleMethod(ShowInfo, "Shows app information."));
        }

        /// <summary>
        /// Generates a help message for the console application.
        /// </summary>
        /// <param name="AppName"></param>
        private void ShowHelp(HashSet<ArgumentValuePair> arguments, HashSet<char> flags)
        {
            string all_commands = string.Join(" | ", ConsoleMethodDefinitions.GetCollection().Keys);
            Console.WriteLine($"Usage: {AppName} [{all_commands}| No Command]");

            Console.WriteLine();
            foreach (var argument in ConsoleMethodDefinitions.GetCollection())
            {

                ConsoleUtilities.Print($"Command: {argument.Key}: {argument.Value.Description}", ConsoleColor.Green);

                foreach (var arg in argument.Value.ValidArguments)
                {
                    Console.WriteLine($"Argument: --{arg.Argument}");
                }

                foreach (var flag in argument.Value.ValidFlags)
                {
                    Console.WriteLine($"Flag: -{flag}");
                }

                Console.WriteLine();
            }
        }

        /// <summary>
        /// Generates a info message for the console application.
        /// </summary>
        /// <param name="AppVersion"></param>
        /// <param name="CurrentInstallationDirectory"></param>
        private void ShowInfo(HashSet<ArgumentValuePair> selected_arguments, HashSet<char> selected_flags)
        {
            ConsoleUtilities.Print($"{AppName}", ConsoleColor.Green);
            Console.WriteLine($"Version: {AppVersion}");
            Console.WriteLine("Current Directory: " + CurrentInstallationDirectory);
            Console.WriteLine($"This is a command-line interface for the {AppName} application.");
            Console.WriteLine("For more information, visit the official documentation.");
            Environment.Exit(0);
        }

    }
}
