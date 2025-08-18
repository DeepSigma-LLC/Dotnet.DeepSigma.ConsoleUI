using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.ConsoleUI
{
    public class ConsoleCommand
    {
        public string? Command {  get; set; }
        public HashSet<ArgumentValuePair> Arguments { get; set; } = [];
        public HashSet<char> Flags { get; set; } = [];
        public ConsoleCommand(){}
    }
}
