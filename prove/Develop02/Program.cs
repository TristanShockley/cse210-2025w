using System;
using System.Collections.Generic;
using System.IO;

class JournalProgram
{
    static void Main()
    {
        JournalEntryManager journalEntryManager = new JournalEntryManager();
        string userSelection = "";

        while (userSelection != "5")
        {
            Console.WriteLine("\nJournal Program Menu:");
            Console.WriteLine("1. Write a new entry");
            Console.WriteLine("2. Display journal");
            Console.WriteLine("3. Save journal to file");
            Console.WriteLine("4. Load journal from file");
            Console.WriteLine("5. Exit");
            Console.Write("Select an option: ");
            userSelection = Console.ReadLine();

            switch (userSelection)
            {
                case "1":
                    journalEntryManager.AddNewJournalEntry();
                    break;
                case "2":
                    journalEntryManager.ShowAllJournalEntries();
                    break;
                case "3":
                    Console.Write("Enter filename to save: ");
                    string saveFilePath = Console.ReadLine();
                    journalEntryManager.PersistJournalToFile(saveFilePath);
                    break;
                case "4":
                    Console.Write("Enter filename to load: ");
                    string loadFilePath = Console.ReadLine();
                    journalEntryManager.RetrieveJournalFromFile(loadFilePath);
                    break;
                case "5":
                    Console.WriteLine("Exiting the program.");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    break;
            }
        }
    }
}

class JournalEntryManager
{
    private List<JournalEntry> _journalEntries = new List<JournalEntry>();
    private List<string> _journalPrompts = new List<string>
    {
        "Who was the most interesting person I interacted with today?",
        "What was the best part of my day?",
        "How did I see the hand of the Lord in my life today?",
        "What was the strongest emotion I felt today?",
        "If I had one thing I could do over today, what would it be?"
    };

    public void AddNewJournalEntry()
    {
        Random randomizer = new Random();
        string selectedPrompt = _journalPrompts[randomizer.Next(_journalPrompts.Count)];
        Console.WriteLine($"Prompt: {selectedPrompt}");
        Console.Write("Your response: ");
        string userResponse = Console.ReadLine();
        string currentDate = DateTime.Now.ToShortDateString();

        JournalEntry newJournalEntry = new JournalEntry(currentDate, selectedPrompt, userResponse);
        _journalEntries.Add(newJournalEntry);
        Console.WriteLine("Journal entry recorded successfully.");
    }

    public void ShowAllJournalEntries()
    {
        if (_journalEntries.Count == 0)
        {
            Console.WriteLine("No journal entries available.");
            return;
        }

        foreach (JournalEntry journalEntry in _journalEntries)
        {
            Console.WriteLine($"Date: {journalEntry.EntryDate}\nPrompt: {journalEntry.EntryPrompt}\nResponse: {journalEntry.EntryResponse}\n");
        }
    }

    public void PersistJournalToFile(string fileName)
    {
        using (StreamWriter outputFile = new StreamWriter(fileName))
        {
            foreach (JournalEntry journalEntry in _journalEntries)
            {
                outputFile.WriteLine($"{journalEntry.EntryDate}|{journalEntry.EntryPrompt}|{journalEntry.EntryResponse}");
            }
        }
        Console.WriteLine("Journal successfully saved to file.");
    }

    public void RetrieveJournalFromFile(string fileName)
    {
        if (!File.Exists(fileName))
        {
            Console.WriteLine("File not found.");
            return;
        }

        _journalEntries.Clear();
        string[] fileLines = File.ReadAllLines(fileName);
        foreach (string line in fileLines)
        {
            string[] parts = line.Split('|');
            if (parts.Length == 3)
            {
                JournalEntry loadedJournalEntry = new JournalEntry(parts[0], parts[1], parts[2]);
                _journalEntries.Add(loadedJournalEntry);
            }
        }
        Console.WriteLine("Journal successfully loaded from file.");
    }
}

class JournalEntry
{
    public string EntryDate { get; }
    public string EntryPrompt { get; }
    public string EntryResponse { get; }

    public JournalEntry(string date, string prompt, string response)
    {
        EntryDate = date;
        EntryPrompt = prompt;
        EntryResponse = response;
    }
}
