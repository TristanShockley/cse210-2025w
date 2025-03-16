using System;
using System.Collections.Generic;
using System.IO;

abstract class Goal
{
    public string Name { get; protected set; }
    public int Points { get; protected set; }
    public bool IsComplete { get; protected set; }

    public Goal(string name, int points)
    {
        Name = name;
        Points = points;
        IsComplete = false;
    }

    public abstract int RecordProgress();
    public abstract string GetProgress();
    public abstract string Serialize();
}

class SimpleGoal : Goal
{
    public SimpleGoal(string name, int points) : base(name, points) { }

    public override int RecordProgress()
    {
        if (!IsComplete)
        {
            IsComplete = true;
            return Points;
        }
        return 0;
    }

    public override string GetProgress()
    {
        return IsComplete ? "[X]" : "[ ]";
    }

    public override string Serialize()
    {
        return $"SimpleGoal,{Name},{Points},{IsComplete}";
    }
}

class EternalGoal : Goal
{
    public EternalGoal(string name, int points) : base(name, points) { }

    public override int RecordProgress()
    {
        return Points;
    }

    public override string GetProgress()
    {
        return "(Ongoing)";
    }

    public override string Serialize()
    {
        return $"EternalGoal,{Name},{Points}";
    }
}

class ChecklistGoal : Goal
{
    public int TargetCount { get; private set; }
    public int CurrentCount { get; private set; }
    public int Bonus { get; private set; }

    public ChecklistGoal(string name, int points, int targetCount, int bonus) : base(name, points)
    {
        TargetCount = targetCount;
        CurrentCount = 0;
        Bonus = bonus;
    }

    public override int RecordProgress()
    {
        if (!IsComplete)
        {
            CurrentCount++;
            if (CurrentCount >= TargetCount)
            {
                IsComplete = true;
                return Points + Bonus;
            }
            return Points;
        }
        return 0;
    }

    public override string GetProgress()
    {
        return IsComplete ? "[X]" : $"[{CurrentCount}/{TargetCount}]";
    }

    public override string Serialize()
    {
        return $"ChecklistGoal,{Name},{Points},{TargetCount},{CurrentCount},{Bonus}";
    }
}

class GoalManager
{
    private List<Goal> goals = new List<Goal>();
    private int score = 0;
    private string saveFile = "goals.txt";

    public void CreateGoal()
    {
        Console.WriteLine("Select Goal Type:");
        Console.WriteLine("1. Simple Goal\n2. Eternal Goal\n3. Checklist Goal");
        string choice = Console.ReadLine();

        Console.Write("Enter goal name: ");
        string name = Console.ReadLine();

        Console.Write("Enter point value: ");
        int points = int.Parse(Console.ReadLine());

        switch (choice)
        {
            case "1":
                goals.Add(new SimpleGoal(name, points));
                break;
            case "2":
                goals.Add(new EternalGoal(name, points));
                break;
            case "3":
                Console.Write("Enter number of times to complete: ");
                int targetCount = int.Parse(Console.ReadLine());
                Console.Write("Enter bonus points: ");
                int bonus = int.Parse(Console.ReadLine());
                goals.Add(new ChecklistGoal(name, points, targetCount, bonus));
                break;
        }
    }

    public void RecordEvent()
    {
        Console.WriteLine("Select goal to record progress:");
        for (int i = 0; i < goals.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {goals[i].Name} {goals[i].GetProgress()}");
        }
        int choice = int.Parse(Console.ReadLine()) - 1;

        if (choice >= 0 && choice < goals.Count)
        {
            int gainedPoints = goals[choice].RecordProgress();
            score += gainedPoints;
            Console.WriteLine($"You gained {gainedPoints} points! Total Score: {score}");
        }
    }

    public void DisplayGoals()
    {
        Console.WriteLine("\nYour Goals:");
        foreach (var goal in goals)
        {
            Console.WriteLine($"{goal.GetProgress()} {goal.Name} ({goal.Points} pts)");
        }
        Console.WriteLine($"Total Score: {score}\n");
    }

    public void SaveGoals()
    {
        using (StreamWriter outputFile = new StreamWriter(saveFile))
        {
            outputFile.WriteLine(score);
            foreach (var goal in goals)
            {
                outputFile.WriteLine(goal.Serialize());
            }
        }
    }

    public void LoadGoals()
    {
        if (File.Exists(saveFile))
        {
            string[] lines = File.ReadAllLines(saveFile);
            score = int.Parse(lines[0]);
            goals.Clear();

            for (int i = 1; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(',');
                switch (parts[0])
                {
                    case "SimpleGoal":
                        goals.Add(new SimpleGoal(parts[1], int.Parse(parts[2])));
                        break;
                    case "EternalGoal":
                        goals.Add(new EternalGoal(parts[1], int.Parse(parts[2])));
                        break;
                    case "ChecklistGoal":
                        goals.Add(new ChecklistGoal(parts[1], int.Parse(parts[2]), int.Parse(parts[3]), int.Parse(parts[5])));
                        break;
                }
            }
        }
    }
}

class Program
{
    static void Main()
    {
        GoalManager manager = new GoalManager();
        manager.LoadGoals();

        while (true)
        {
            Console.WriteLine("1. Create Goal\n2. Record Progress\n3. Show Goals\n4. Save and Exit");
            string choice = Console.ReadLine();

            if (choice == "1") manager.CreateGoal();
            else if (choice == "2") manager.RecordEvent();
            else if (choice == "3") manager.DisplayGoals();
            else if (choice == "4") { manager.SaveGoals(); break; }
        }
    }
}
