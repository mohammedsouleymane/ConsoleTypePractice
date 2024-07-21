using System.Text.Json;

namespace ConsoleTypePractice;

public class RandomWordGen
{
    static string RandomWord(string[] wordsArray)
    {
        var random = new Random();
        return wordsArray[random.Next(0, wordsArray.Length)]; 
    }

    public static List<string> RandomWords()
    {
        //https://www.ef.com/wwen/english-resources/english-vocabulary/top-1000-words/
        var path = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent + "/words.json";
        var topThousand = JsonSerializer.Deserialize<string[]>(File.ReadAllText(path));
        var randomWords = new List<string>();
        for (var i = 0; i < 4; i++)
        {
            var line = "";
            while (line.Length < 80)
                line += RandomWord(topThousand) + " ";
            if (i < 3)
                randomWords.Add(line + "\n");
            else
                randomWords.Add(line[..^1]);
        }
        return randomWords;
    }
}