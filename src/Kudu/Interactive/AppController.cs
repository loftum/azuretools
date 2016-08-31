using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Kudu.Extensions;
using Kudu.Logging;
using Mono.CSharp;

namespace Kudu.Interactive
{
    public class AppController
    {
        private readonly Evaluator _evaluator;
        private readonly IDictionary<string, string> _aliases = new Dictionary<string, string>();
        private readonly string _help;

        public AppController(Type type)
        {
            var help = new StringBuilder();

            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(m => !m.IsSpecialName))
            {
                var parameters = method.GetParameters();
                var parms = string.Join(", ",
                    parameters.Select(p => $"{p.ParameterType.GetFriendlyName()} {p.Name}"));
                var pretty = $"{method.Name}({parms})";
                if (!parameters.Any())
                {
                    var alias = Alias(method.Name);
                    _aliases[alias] = pretty;
                    help.AppendFormat("{0} / ", alias);
                }
                help.Append(pretty).AppendLine();
            }
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Static))
            {
                var alias = Alias(property.Name);
                _aliases[alias] = property.Name;
                help.AppendFormat("{0} / {1}", alias, property.Name).AppendLine();
            }
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var alias = Alias(field.Name);
                _aliases[alias] = field.Name;
                help.AppendFormat("{0} / {1}", alias, field.Name).AppendLine();
            }
            _help = help.ToString();

            _evaluator = InitEvaluator(type);
        }

        private static Evaluator InitEvaluator(Type type)
        {
            var settings = new CompilerSettings();
            var printer = new ConsoleReportPrinter();
            var context = new CompilerContext(settings, printer);
            var evaluator = new Evaluator(context)
            {
                InteractiveBaseClass = type
            };
            evaluator.ReferenceAssembly(type.Assembly);
            evaluator.ReferenceAssembly(typeof(Log).Assembly);
            evaluator.Run("using System;");
            evaluator.Run("using System.Linq;");
            evaluator.Run("using Kudu.Logging;");

            return evaluator;
        }

        private static string Alias(string name)
        {
            return string.Join(" ", Parts(name));
        }

        private static IEnumerable<string> Parts(string name)
        {
            var builder = new StringBuilder();
            foreach (var c in name)
            {
                if (char.IsUpper(c))
                {
                    if (builder.Length > 0)
                    {
                        yield return builder.ToString();
                        builder.Clear();
                    }
                }
                builder.Append(char.ToLower(c));
            }
            if (builder.Length > 0)
            {
                yield return builder.ToString();
            }
        }

        private string GetCommand(string alias)
        {
            string command;
            return _aliases.TryGetValue(alias, out command) ? command : alias;
        }

        public CompletionResult GetCompletions(string input)
        {
            string prefix;
            var completions = _evaluator.GetCompletions(input, out prefix) ?? new string[0];
            if (completions.Any())
            {
                return new CompletionResult(input, prefix, completions);
            }
            var aliases = _aliases.Keys.Concat(new[] { "help" }).Where(k => k.StartsWith(input)).Select(k => k.Substring(input.Length)).ToArray();
            return new CompletionResult(input, input, aliases);
        }

        public object Execute(string alias)
        {
            if (alias.In(StringComparison.InvariantCultureIgnoreCase, "help", "?"))
            {
                return _help;
            }
            var command = GetCommand(alias);
            bool resultSet;
            object result;
            var output = _evaluator.Evaluate(command, out result, out resultSet);
            return new AppCommandResult(output, result, resultSet);
        }
    }
}