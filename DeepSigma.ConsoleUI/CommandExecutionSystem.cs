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
        /// <param name="app_name"></param>
        /// <param name="app_version"></param>
        /// <param name="current_installation_directory"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CommandExecutionSystem(ConsoleMethodCollection console_method_definitions, string app_name, string app_version, string current_installation_directory)
        {
            this.AppName = AppName;
            this.AppVersion = AppVersion;
            this.CurrentInstallationDirectory = CurrentInstallationDirectory;
            this.ConsoleMethodDefinitions = console_method_definitions ?? throw new ArgumentNullException(nameof(console_method_definitions), "Console method definitions cannot be null.");
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
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Invokes the specified console command with its arguments and flags.
        /// </summary>
        /// <param name="argument"></param>
        private void InvokeCLIArgument(ConsoleCommand argument)
        {
            string? command = argument.Command;
            if (string.IsNullOrEmpty(command))
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
        /// Displays help information for the console application, including available commands, arguments, and flags.
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="flags"></param>
        private void ShowHelp(Dictionary<string, string?> arguments, Dictionary<char, bool> flags)
        {
            string all_commands = string.Join(" | ", ConsoleMethodDefinitions.GetCollection().Keys);
            Console.WriteLine($"Usage: {AppName} [{all_commands}| No Command]");

            Console.WriteLine();
            foreach (KeyValuePair<string, ConsoleMethod> argument in ConsoleMethodDefinitions.GetCollection())
            {

                ConsoleUtilities.Print($"Command: {argument.Key}: {argument.Value.Description}", ConsoleColor.Green);

                foreach (KeyValuePair<string, string?> arg in argument.Value.ValidArguments)
                {
                    ConsoleUtilities.Print($"Argument: --{arg.Key}");
                }

                foreach (KeyValuePair<char, bool>  flag in argument.Value.ValidFlags)
                {
                    ConsoleUtilities.Print($"Flag: -{flag.Key}");
                }
            }
        }

        /// <summary>
        /// Displays information about the application, including its name, version, and current installation directory.
        /// </summary>
        /// <param name="selected_arguments"></param>
        /// <param name="selected_flags"></param>
        private void ShowInfo(Dictionary<string, string?> selected_arguments, Dictionary<char, bool> selected_flags)
        {
            ConsoleUtilities.Print($"{AppName}", ConsoleColor.Green);
            Console.WriteLine($"Version: {AppVersion}");
            Console.WriteLine("Current Directory: " + CurrentInstallationDirectory);
            Console.WriteLine($"This is a command-line interface for the {AppName} application.");
            Console.WriteLine("For more information, visit the official documentation.");
        }

        /// <summary>
        /// Exits the application.
        /// </summary>
        /// <param name="selected_arguments"></param>
        /// <param name="selected_flags"></param>
        private void Exit(Dictionary<string, string?> selected_arguments, Dictionary<char, bool> selected_flags)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Adds a help argument to the console arguments collection.
        /// </summary>
        private void AddArguments()
        {
            ConsoleMethodDefinitions.Add("help", new ConsoleMethod(ShowHelp, "Shows all available arguments."));
            ConsoleMethodDefinitions.Add("info", new ConsoleMethod(ShowInfo, "Shows app information."));
            ConsoleMethodDefinitions.Add("exit", new ConsoleMethod(Exit, "Exit the application."));
        }
    }
}
