using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoTrack.Cli
{
    internal class RangeHelper
    {
        public static Range? GetRange(string rangeString)
        {
            // Create the script options with the references and imports needed
            ScriptOptions options = ScriptOptions.Default
                .WithReferences(typeof(Range).Assembly)
                .WithImports("System");

            Range? myRange = null;
            try
            {
                // Evaluate the range expression string as a Range object
                myRange = CSharpScript.EvaluateAsync<Range>(
                        "Range p=" + rangeString + ";return p;", options).Result;
            }
            catch (CompilationErrorException)
            {

            }


            return myRange;
        }
    }
}
