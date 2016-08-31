using System.Collections.Generic;
using System.Linq;

namespace Kudu.Interactive
{
    public class CompletionResult
    {
        public string Input { get; private set; }
        public string Prefix { get; }
        public string[] Completions { get; }

        public IEnumerable<string> GetFullCompletions()
        {
            return Completions.Select(c => string.Concat(Prefix, c));
        }

        public CompletionResult(string input, string prefix, string[] completions)
        {
            Input = input;
            Prefix = prefix;
            Completions = completions ?? new string[0];
        }
    }
}