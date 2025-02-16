using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

class Program
{
    static void Main()
    {
        ScriptureLibrary library = new ScriptureLibrary("scriptures.txt");
        Scripture scripture = library.GetRandomScripture();

        Console.Clear();
        Console.WriteLine("Welcome to the Scripture Memorizer!");
        Thread.Sleep(1000);
        Console.Clear();

        while (!scripture.IsFullyHidden())
        {
            Console.Clear();
            scripture.Display();
            Console.WriteLine("\nPress Enter to hide words, type 'hint' for help, or 'quit' to exit.");
            string input = Console.ReadLine().Trim().ToLower();

            if (input == "quit")
                break;
            else if (input == "hint")
                scripture.RevealWord();
            else
                scripture.HideWords(3); // Hide 3 words at a time
        }

        Console.Clear();
        scripture.Display();
        Console.WriteLine("\nCongratulations! You've memorized the scripture.");
    }
}

class ScriptureLibrary
{
    private List<Scripture> scriptures = new List<Scripture>();
    private Random random = new Random();

    public ScriptureLibrary(string filePath)
    {
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length == 2)
                    scriptures.Add(new Scripture(new ScriptureReference(parts[0]), parts[1]));
            }
        }
        else
        {
            scriptures.Add(new Scripture(new ScriptureReference("Proverbs 3:5-6"), "Trust in the LORD with all your heart and lean not on your own understanding; in all your ways submit to him, and he will make your paths straight."));
        }
    }

    public Scripture GetRandomScripture()
    {
        return scriptures[random.Next(scriptures.Count)];
    }
}

class Scripture
{
    private ScriptureReference reference;
    private List<Word> words;
    private Random random = new Random();

    public Scripture(ScriptureReference reference, string text)
    {
        this.reference = reference;
        this.words = text.Split(' ').Select(w => new Word(w)).ToList();
    }

    public void Display()
    {
        Console.WriteLine($"{reference}");
        Console.WriteLine(string.Join(" ", words.Select(w => w.Display())));
    }

    public void HideWords(int count)
    {
        var visibleWords = words.Where(w => !w.IsHidden).ToList();
        for (int i = 0; i < count && visibleWords.Count > 0; i++)
        {
            int index = random.Next(visibleWords.Count);
            visibleWords[index].Hide();
            visibleWords.RemoveAt(index);
        }
    }

    public void RevealWord()
    {
        var hiddenWords = words.Where(w => w.IsHidden).ToList();
        if (hiddenWords.Count > 0)
            hiddenWords[random.Next(hiddenWords.Count)].Reveal();
    }

    public bool IsFullyHidden()
    {
        return words.All(w => w.IsHidden);
    }
}

class ScriptureReference
{
    public string ReferenceText { get; }

    public ScriptureReference(string reference)
    {
        ReferenceText = reference;
    }

    public override string ToString()
    {
        return ReferenceText;
    }
}

class Word
{
    private string text;
    public bool IsHidden { get; private set; }

    public Word(string text)
    {
        this.text = text;
        IsHidden = false;
    }

    public void Hide()
    {
        IsHidden = true;
    }

    public void Reveal()
    {
        IsHidden = false;
    }

    public string Display()
    {
        return IsHidden ? "_____" : text;
    }
}
