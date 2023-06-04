using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoTrack.Contracts;

namespace TodoTrack.Cli
{
    internal class RangeHelper
    {
        public static List<string> GetMatchedStringList(string[] command, IEnumerable<IEntity> indexedTodoItems)
        {
            var todoIdToDelete = command;
            var strList = new List<string>();
            if (todoIdToDelete.Length == 1 && !int.TryParse(todoIdToDelete[0], out var _))
            {
                var range = GetRange(command[0]);
                if (range != null)
                    strList = indexedTodoItems.ToArray()[range.Value].Select(w => w.Id).ToList();
                else
                {
                    Console.WriteLine("Invalid range string.");
                }
            }
            else
            {
                foreach (var item in todoIdToDelete)
                {
                    if (int.TryParse(item, out var result))
                    {
                        if (result >= indexedTodoItems.Count()) continue;
                        strList.Add(indexedTodoItems.ToArray()[result].Id);
                    }
                }
            }
            return strList;
        }

        private static Range? GetRange(string rangeString)
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
