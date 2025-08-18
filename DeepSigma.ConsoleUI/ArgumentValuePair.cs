using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.ConsoleUI
{
    public record ArgumentValuePair
    {
        public string Argument { get; set; } 
        public string? Value { get; set; }
        public ArgumentValuePair(string arguement, string? value)
        {
            this.Argument = arguement;
            this.Value = value;
        }
    }
}
