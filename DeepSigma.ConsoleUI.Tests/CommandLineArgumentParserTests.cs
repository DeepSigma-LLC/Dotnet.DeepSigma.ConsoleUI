using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeepSigma.ConsoleUI.Tests
{
    public class CommandLineArgumentParserTests
    {
        [Fact]
        public void ParseCommandLineArguments_ValidInput_ReturnsCorrectCommand()
        {
            // Arrange
            string[] args = [ "command", "--arg1=value1", "-f" ];
            var expectedCommand = new ConsoleCommand
            {
                Command = "command",
                Arguments = new HashSet<ArgumentValuePair>
                {
                    new ArgumentValuePair("arg1", "value1")
                },
                Flags = new HashSet<char> { 'f' }
            };

            // Act
            IEnumerable<ConsoleCommand> results = CommandLineArgumentParser.ProcessArguments(args, ["command"]);
            ConsoleCommand result = results.First();

            // Assert
            Assert.Equal(expectedCommand.Command, result.Command);
            Assert.Equal(expectedCommand.Arguments, result.Arguments);
            Assert.Equal(expectedCommand.Flags, result.Flags);
        }

        [Fact]
        public void ParseCommandLineArguments_ValidInputWithNoCommand()
        {
            // Arrange
            string[] args = ["--arg1=value1", "-few", "--arg2=value2", "-top"];
            var expectedCommand = new ConsoleCommand
            {
                Command = null,
                Arguments = new HashSet<ArgumentValuePair>
                {
                    new ArgumentValuePair("arg1", "value1"),
                    new ArgumentValuePair("arg2", "value2")
                },
                Flags = new HashSet<char> { 'f', 'e', 'w', 't', 'o' ,'p' }
            };

            // Act
            IEnumerable<ConsoleCommand> results = CommandLineArgumentParser.ProcessArguments(args, ["command"]);
            ConsoleCommand result = results.First();

            // Assert
            Assert.Equal(expectedCommand.Command, result.Command);
            Assert.Equal(expectedCommand.Arguments, result.Arguments);
            Assert.Equal(expectedCommand.Flags, result.Flags);
        }


        [Fact]
        public void ParseCommandLineArguments_ValidInputWithNoStartingCommand()
        {
            // Arrange
            string[] args = ["--arg1=value1", "-few", "--arg2=value2", "-top", "test"];
            var expectedCommand = new ConsoleCommand
            {
                Command = null,
                Arguments = new HashSet<ArgumentValuePair>
                {
                    new ArgumentValuePair("arg1", "value1"),
                    new ArgumentValuePair("arg2", "value2")
                },
                Flags = new HashSet<char> { 'f', 'e', 'w', 't', 'o', 'p' }
            };

            var expectedCommand2 = new ConsoleCommand
            {
                Command = "test"
            };

            // Act
            List<ConsoleCommand> results = CommandLineArgumentParser.ProcessArguments(args, ["command", "test"]);
            ConsoleCommand result = results.First();
            ConsoleCommand result2 = results[1];

            // Assert
            Assert.Equal(expectedCommand.Command, result.Command);
            Assert.Equal(expectedCommand.Arguments, result.Arguments);
            Assert.Equal(expectedCommand.Flags, result.Flags);

            // Assert
            Assert.Equal(expectedCommand2.Command, result2.Command);
            Assert.Equal(expectedCommand2.Arguments, result2.Arguments);
            Assert.Equal(expectedCommand2.Flags, result2.Flags);
        }




        [Fact]
        public void ParseCommandLineArguments_ValidInputWithQuotes()
        {
            // Arrange
            string[] args = ["command", "--arg1=\"Test" , "value\"", "-fg"];
            var expectedCommand = new ConsoleCommand
            {
                Command = "command",
                Arguments = new HashSet<ArgumentValuePair>
                {
                    new ArgumentValuePair("arg1", "Test value")
                },
                Flags = new HashSet<char> { 'f', 'g' }
            };

            // Act
            IEnumerable<ConsoleCommand> results = CommandLineArgumentParser.ProcessArguments(args, ["command"]);
            ConsoleCommand result = results.First();

            // Assert
            Assert.Equal(expectedCommand.Command, result.Command);
            Assert.Equal(expectedCommand.Arguments, result.Arguments);
            Assert.Equal(expectedCommand.Flags, result.Flags);
        }



        [Fact]
        public void ParseCommandLineArguments_ValididateMultipleInputs()
        {
            // Arrange
            string[] args = ["command", "--arg1=value1", "--arg2=value2", "-fast", "--arg3=value3", "-not", "--arg4=value4",];
            var expectedCommand = new ConsoleCommand
            {
                Command = "command",
                Arguments = new HashSet<ArgumentValuePair>
                {
                    new ArgumentValuePair("arg1", "value1"),
                    new ArgumentValuePair("arg2", "value2"),
                    new ArgumentValuePair("arg3", "value3"),
                    new ArgumentValuePair("arg4", "value4"),
                },
                Flags = new HashSet<char> { 'f', 'a', 's', 't', 'n', 'o', 't'}
            };

            // Act
            IEnumerable<ConsoleCommand> results = CommandLineArgumentParser.ProcessArguments(args, ["command"]);
            ConsoleCommand result = results.First();

            // Assert
            Assert.Equal(expectedCommand.Command, result.Command);
            Assert.Equal(expectedCommand.Arguments, result.Arguments);
            Assert.Equal(expectedCommand.Flags, result.Flags);
        }

        [Fact]
        public void ParseCommandLineArguments_ValididateMultipleCommands()
        {
            // Arrange
            string[] args = ["command", "--arg1=value1", "--arg2=value2", "-fast", "break", "-not", "--arg4=value4", "send", "--arg4=value4", "-nut"];
            var expectedCommand = new ConsoleCommand
            {
                Command = "command",
                Arguments = new HashSet<ArgumentValuePair>
                {
                    new ArgumentValuePair("arg1", "value1"),
                    new ArgumentValuePair("arg2", "value2"),
                },
                Flags = new HashSet<char> { 'f', 'a', 's', 't', }
            };

            var expectedCommand2 = new ConsoleCommand
            {
                Command = "break",
                Arguments = new HashSet<ArgumentValuePair>
                {
                    new ArgumentValuePair("arg4", "value4"),
                },
                Flags = new HashSet<char> { 'n', 'o', 't' }
            };

            var expectedCommand3 = new ConsoleCommand
            {
                Command = "send",
                Arguments = new HashSet<ArgumentValuePair>
                {
                    new ArgumentValuePair("arg4", "value4"),
                },
                Flags = new HashSet<char> { 'n', 'u', 't' }
            };

            // Act
            List<ConsoleCommand> results = CommandLineArgumentParser.ProcessArguments(args, ["command", "break", "send"]).ToList();
            ConsoleCommand result = results[0]; 
            ConsoleCommand result2 = results[1];
            ConsoleCommand result3= results[2];

            // Assert validate first command
            Assert.Equal(expectedCommand.Command, result.Command);
            Assert.Equal(expectedCommand.Arguments, result.Arguments);
            Assert.Equal(expectedCommand.Flags, result.Flags);

            Assert.Equal(expectedCommand2.Command, result2.Command);
            Assert.Equal(expectedCommand2.Arguments, result2.Arguments);
            Assert.Equal(expectedCommand2.Flags, result2.Flags);

            Assert.Equal(expectedCommand3.Command, result3.Command);
            Assert.Equal(expectedCommand3.Arguments, result3.Arguments);
            Assert.Equal(expectedCommand3.Flags, result3.Flags);
        }


        [Fact]
        public void ParseCommandLineArguments_NotSupportedOnInvalidCommand()
        {
            // Arrange
            string[] args = ["command", "--arg1=value1", "-f"];
            var expectedCommand = new ConsoleCommand
            {
                Command = "command",
                Arguments = new HashSet<ArgumentValuePair>
                {
                    new ArgumentValuePair("arg1", "value1")
                },
                Flags = new HashSet<char> { 'f' }
            };

            // Assert
            Assert.Throws<NotSupportedException>(() => CommandLineArgumentParser.ProcessArguments(args, ["different_command"]));
        
        }

    }
}
