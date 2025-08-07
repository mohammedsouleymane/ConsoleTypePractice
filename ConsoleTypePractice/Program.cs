using System.Runtime.InteropServices;
using ConsoleTypePractice;

Console.CancelKeyPress += delegate
{
    Console.Clear();
};
while (true)
{
    var center = (Console.WindowWidth - RandomWordGen.LINELENGTH) / 2;
    var height = Console.WindowHeight / 4;
    Console.Clear();
    Console.ResetColor();
    var randomWords = RandomWordGen.RandomWords();
    var typed = "";
    var currentLine = 0;
    var errors = 0;

    var spaces = new Stack<int>();
    foreach (var line in randomWords)
    {
        Console.SetCursorPosition(center, height + currentLine++);
        Console.Write(line);
    }

    currentLine = 0;
    var restart = false;
    
    DateTime? startTime = null;
    while (typed.Length < randomWords[currentLine].Length)
    {
        
        Console.SetCursorPosition(center + typed.Length, height + currentLine);
        var key = Console.ReadKey(true);
        startTime ??= DateTime.Now;
        if (key is { Key: ConsoleKey.R, Modifiers: ConsoleModifiers.Control })
        {
            restart = true;
            break;
        }
        if (key is { Key: ConsoleKey.Backspace, Modifiers: ConsoleModifiers.Control  })
        {
            var lastSpace = spaces.Count != 0 ?  spaces.Peek() : 0;
            if (lastSpace >= typed.Length  && lastSpace != 0)
            {
                spaces.Pop();
                lastSpace = spaces.Count != 0 ?  spaces.Peek() : 0;
            }
                
            if (typed.Length > 0)
            {
                Console.ResetColor();
                Console.SetCursorPosition(center + lastSpace, height + currentLine);
                Console.Write(randomWords[currentLine][lastSpace..typed.Length]);
                typed = typed[..lastSpace];
            }

            continue;
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

            if (spaces.Count != 0)
            {
                if (spaces.Peek() >= typed.Length)
                    spaces.Pop();
            }
            continue;
        }
        typed += key.KeyChar;
        if (key.KeyChar == ' ')
            spaces.Push(typed.Length);
        
        
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
                errors += randomWords[currentLine][..^1].Where((c, index) => typed == "" || c != typed[index]).Count();
                typed = "";
                currentLine++;
                spaces.Clear(); 
            }
        }
    }

    var time = (DateTime.Now - startTime)!.Value.TotalMinutes;
    var uncorrectedErrors = Math.Floor(errors * (1 / time));
    var wpm = Math.Floor((randomWords.Sum(x => x.Length) - 3.0) / 5  / time) - uncorrectedErrors;
    wpm = wpm < 0 ? 0 : wpm;
    if (restart) continue;
    Console.Clear();
    Console.ResetColor();
    Console.SetCursorPosition((Console.WindowWidth - 25) / 2, 4 + Console.CursorTop);
    Console.WriteLine("enter or space to restart");
    Console.SetCursorPosition((Console.WindowWidth - 16) / 2, Console.CursorTop);
    Console.WriteLine("ctrl + c to exit");
    Console.SetCursorPosition((Console.WindowWidth - 8) / 2, Console.CursorTop);
    Console.WriteLine($"wpm: {wpm}");
    Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
    var eoc = Console.ReadKey(true);

    while (true)
    {
        switch (eoc.Key)
        {
            case ConsoleKey.Enter:
            case ConsoleKey.Spacebar:
                restart = true;
                break;
            default:
                eoc = Console.ReadKey(true);
                break;
        }
        if (restart) break;
    }
    if (restart) continue;
}

