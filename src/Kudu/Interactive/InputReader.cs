using System;
using System.Linq;
using System.Text;

namespace Kudu.Interactive
{
    public class InputReader
    {
        private const string Prompt = "> ";

        private readonly AppController _controller;

        public InputReader(AppController controller)
        {
            _controller = controller;
        }

        private static void PrintPrompt()
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(Prompt);
            Console.ResetColor();
        }

        public string ReadInput(InputHistory history)
        {
            PrintPrompt();
            var current = new StringBuilder();
            while (true)
            {
                var read = Console.ReadKey(true);

                switch (read.Key)
                {
                    case ConsoleKey.Enter:
                        var input = current.ToString();
                        return input;
                    case ConsoleKey.UpArrow:
                        var previous = history.Previous();
                        if (previous != null)
                        {
                            PrintPrompt();
                            Console.Write(previous);
                            current.Clear();
                            current.Append(previous);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        var next = history.Next();
                        if (next != null)
                        {
                            PrintPrompt();
                            Console.Write(next);
                            current.Clear();
                            current.Append(next);
                        }
                        break;
                    case ConsoleKey.Tab:
                        var result = _controller.GetCompletions(current.ToString());
                        switch (result.Completions.Length)
                        {
                            case 0:
                                break;
                            case 1:
                                var completion = result.Completions.Single();
                                Console.Write(completion);
                                current.Append(completion);
                                break;
                            default:
                                Console.WriteLine();
                                Console.WriteLine(string.Join(", ", result.GetFullCompletions()));
                                PrintPrompt();
                                Console.Write(current.ToString());
                                break;
                        }
                        break;
                    case ConsoleKey.Backspace:
                        if (Console.CursorLeft > Prompt.Length)
                        {
                            Console.Write("\b \b");
                            current.Remove(current.Length - 1, 1);
                        }
                        break;
                    default:
                        if (ShouldPrint(read))
                        {
                            Console.Write(read.KeyChar);
                            current.Append(read.KeyChar);
                        }
                        break;
                }
            }
        }

        private static bool ShouldPrint(ConsoleKeyInfo read)
        {
            return !char.IsControl(read.KeyChar) && (char.IsLetterOrDigit(read.KeyChar) ||
                                                     char.IsPunctuation(read.KeyChar) ||
                                                     char.IsSeparator(read.KeyChar) ||
                                                     char.IsSymbol(read.KeyChar) ||
                                                     char.IsWhiteSpace(read.KeyChar));
        }
    }
}