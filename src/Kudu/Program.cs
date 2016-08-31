using System;
using System.Collections.Generic;
using System.Linq;
using Kudu.Interactive;
using Kudu.Logging;
using Kudu.Meta;

namespace Kudu
{
    internal class Program
    {
        private static readonly AppController Controller;
        private static readonly InputHistory History;
        private static readonly InputReader Reader;

        static Program()
        {
            Controller = new AppController(typeof(App));
            History = new InputHistory(50);
            Reader = new InputReader(Controller);
        }

        public static void Main(string[] args)
        {
            Console.Title = ProgramInfo.Name;
            Console.WriteLine(ProgramInfo.FullName);
            Console.WriteLine();
            Console.CancelKeyPress += Quit;

            try
            {
                Configure(args);

                while (true)
                {
                    try
                    {
                        History.Reset();
                        var input = Reader.ReadInput(History);
                        Console.WriteLine();
                        if (string.IsNullOrWhiteSpace(input))
                        {
                            continue;
                        }
                        if (string.Equals(input, "quit", StringComparison.InvariantCultureIgnoreCase))
                        {
                            break;
                        }
                        History.Add(input);
                        var result = Controller.Execute(input);
                        Console.WriteLine(result);
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ex.ToString());
                        Console.ResetColor();
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Quit();
        }

        private static void Quit()
        {
            Console.WriteLine("Bye!");
        }

        private static void Quit(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine();
            Quit();
        }

        private static void Configure(IList<string> args)
        {
            if (args.Any())
            {
                App.Site = args[0];
            }

            if (args.Count > 1)
            {
                LogLevel minLevel;
                Log.Level = Enum.TryParse(args[1], true, out minLevel) ? minLevel : LogLevel.Normal;
            }
        }
    }
}
