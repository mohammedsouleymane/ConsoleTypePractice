using ConsoleTypePractice;

while (true)
{
    var center = (Console.WindowWidth - 80) / 2;
    var height = Console.WindowHeight / 4;
    Console.Clear();
    Console.ResetColor();
    var randomWords = RandomWordGen.RandomWords();
    var typed = "";
    var currentLine = 0;


    foreach (var line in randomWords)
    {
        Console.SetCursorPosition(center, height + currentLine++);
        Console.Write(line);
    }

    currentLine = 0;
    var restart = false;
    while (typed.Length < randomWords[currentLine].Length)
    {
        Console.SetCursorPosition(center + typed.Length, height + currentLine);
        var key = Console.ReadKey(true);
        if (key is { Key: ConsoleKey.R, Modifiers: ConsoleModifiers.Control })
        {
            restart = true;
            break;
        }


        if (key.Key == ConsoleKey.Backspace)
        {
            if (typed.Length > 0)
            {
                Console.ResetColor();
                Console.SetCursorPosition(center + typed.Length - 1, height + currentLine);
                Console.Write(randomWords[currentLine][typed.Length - 1]);
                typed = typed[..^1];
            }

            continue;
        }
        else
        {
            typed += key.KeyChar;
        }

        var i = typed.Length - 1;
        if (i < 0) continue;
        if (randomWords[currentLine][i] != '\n')
        {
            Console.ForegroundColor = typed[i] == randomWords[currentLine][i] ? ConsoleColor.Green : ConsoleColor.Red;
            if (typed[i] != randomWords[currentLine][i] && randomWords[currentLine][i] == ' ')
                Console.Write("_");
            else
                Console.Write(randomWords[currentLine][i]);
            if (typed.Length < randomWords[currentLine].Length && randomWords[currentLine][i + 1] == '\n')
            {
                typed = "";
                currentLine++;
            }
        }
    }

    if (restart) continue;
    Console.Clear();
    Console.ResetColor();
    Console.SetCursorPosition((Console.WindowWidth - 25) / 2, 4 + Console.CursorTop);
    Console.WriteLine("enter or space to restart");
    Console.SetCursorPosition((Console.WindowWidth - 16) / 2, Console.CursorTop);
    Console.WriteLine("ctrl + c to exit");
    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
    if (Console.ReadKey().Key is ConsoleKey.Enter or ConsoleKey.Spacebar) continue;
    break;
}