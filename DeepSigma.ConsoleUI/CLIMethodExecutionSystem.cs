using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepSigma.ConsoleUI;

namespace DeepSigma.ConsoleUI
{
    public class CLIMethodExecutionSystem
    {
        private ConsoleMethodCollection ConsoleArguments { get; init; }
        private string AppName { get; } = string.Empty;
        private string AppVersion { get; } = string.Empty;
        private string CurrentInstallationDirectory { get; } = string.Empty;
        public CLIMethodExecutionSystem(ConsoleMethodCollection consoleArguments, string AppName, string AppVersion, string CurrentInstallationDirectory)
        {
            this.ConsoleArguments = consoleArguments;
            this.AppName = AppName;
            this.AppVersion = AppVersion;
            this.CurrentInstallationDirectory = CurrentInstallationDirectory;
            AddArguments();
        }

        /// <summary>
        /// Generates a info message for the console application.
        /// </summary>
        /// <param name="AppVersion"></param>
        /// <param name="CurrentInstallationDirectory"></param>
        public void ShowInfo(dynamic parameters)
        {
            ConsoleUtilities.Print($"{AppName}", ConsoleColor.Green);
            Console.WriteLine($"Version: {AppVersion}");
            Console.WriteLine("Current Directory: " + CurrentInstallationDirectory);
            Console.WriteLine($"This is a command-line interface for the {AppName} application.");
            Console.WriteLine("For more information, visit the official documentation.");
            Environment.Exit(0);
        }

        /// <summary>
        /// Processes the command-line arguments passed to the application.
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public void ProcessArguments(string[] arguments)
        {
            IEnumerable<ConsoleCommand> args = CLIArgumentParser.ProcessArguments(arguments, ConsoleArguments.GetCollection().Keys.ToArray());
            foreach (ConsoleCommand arg in args)
            {
                if (arg.Command == null)
                {
                    ConsoleUtilities.Print("Invalid command.", ConsoleColor.Red);
                    continue;
                }
                InvokeCLIArgument(arg);
            }
        }

        /// <summary>
        /// Processes the command-line interface request based on the provided argument.
        /// </summary>
        /// <param name="CLIarguement"></param>
        private void InvokeCLIArgument(ConsoleCommand argument)
        {
            string? command = argument.Command;
            if(command is null || ConsoleArguments.GetCollection().ContainsKey(command))
            {
                ConsoleUtilities.Print("Invalid argument: " + command, ConsoleColor.Red);
                return;
            }

            command = command.Trim().ToLower();
            ConsoleArguments.GetCollection()[command].Method.Invoke(argument.Arguments);
        }

        /// <summary>
        /// Adds a help argument to the console arguments collection.
        /// </summary>
        private void AddArguments()
        {
            ConsoleArguments.Add("help", new ConsoleMethod<dynamic>(ShowHelp, "Shows all available arguments."));
            ConsoleArguments.Add("info", new ConsoleMethod<dynamic>(ShowInfo, "Shows app information."));
        }

        /// <summary>
        /// Generates a help message for the console application.
        /// </summary>
        /// <param name="AppName"></param>
        private void ShowHelp(dynamic parameters)
        {
            string all_arguments = string.Join(" | ", ConsoleArguments.GetCollection().Keys);
            Console.WriteLine($"Usage: {AppName} [{all_arguments}| No Argument]");

            Console.WriteLine();
            foreach (var argument in ConsoleArguments.GetCollection())
            {
                Console.WriteLine($"{argument.Key}: {argument.Value.Description}");
            }
        }
    }
}
