using System.Text;
using Kudu.Extensions;

namespace Kudu.Interactive
{
    public class AppCommandResult
    {
        public AppCommandResult(string output, object result, bool resultSet)
        {
            Output = output;
            Result = result;
            ResultSet = resultSet;
        }

        public string Output { get; }
        public bool ResultSet { get; }
        public object Result { get; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            if (Output != null)
            {
                builder.AppendLine(Output);
            }
            if (ResultSet)
            {
                builder.Append(Format(Result));
            }
            return builder.ToString();
        }

        private static string Format(object result)
        {
            if (result == null)
            {
                return "null";
            }
            var s = result as string;
            return s ?? result.ToJson(true);
        }
    }
}