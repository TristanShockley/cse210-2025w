using System;
using System.Collections.Generic;
using System.Threading;

namespace MindfulnessApp
{
    class Program
    {
        // This dictionary logs how many times each activity is performed (above & beyond requirement).
        static Dictionary<string, int> _activityLog = new Dictionary<string, int>()
        {
            {"Breathing", 0},
            {"Reflection", 0},
            {"Listing", 0}
        };

        static void Main(string[] args)
        {
            // *** Above & Beyond: We track how many times each activity was performed. ***
            // (See _activityLog usage in the code below.)

            // Main program loop
            int option = 0;
            while (option != 4)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the Mindfulness Program!\n");
                Console.WriteLine("Menu Options:");
                Console.WriteLine("  1. Breathing Activity");
                Console.WriteLine("  2. Reflection Activity");
                Console.WriteLine("  3. Listing Activity");
                Console.WriteLine("  4. Quit");
                Console.Write("Select an option (1-4): ");

                if (int.TryParse(Console.ReadLine(), out option))
                {
                    switch (option)
                    {
                        case 1:
                            RunBreathingActivity();
                            break;
                        case 2:
                            RunReflectionActivity();
                            break;
                        case 3:
                            RunListingActivity();
                            break;
                        case 4:
                            Console.WriteLine("\nThank you for using the Mindfulness Program!");
                            PrintActivityLog();
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Press any key to continue...");
                            Console.ReadKey();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice. Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        // Display how many times each activity was done.
        private static void PrintActivityLog()
        {
            Console.WriteLine("\n*** Activity Log ***");
            foreach (var entry in _activityLog)
            {
                Console.WriteLine($"{entry.Key} Activity performed {entry.Value} time(s).");
            }
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        private static void RunBreathingActivity()
        {
            Breathing breathing = new Breathing();
            breathing.DisplayIntro();
            breathing.SetTime();     // prompts user for how many seconds
            breathing.Preparation(); // standard "prepare to begin" pause with spinner
            breathing.LoopBreath();  // do the breathing in/out cycle
            breathing.DisplayExtro();
            _activityLog["Breathing"]++; // log usage
        }

        private static void RunReflectionActivity()
        {
            Reflection reflection = new Reflection();
            reflection.DisplayIntro();
            reflection.SetTime();
            reflection.Preparation();
            reflection.DisplayPrompt();
            reflection.DisplayQuestions(); // keep showing questions until time is up
            reflection.DisplayExtro();
            _activityLog["Reflection"]++; // log usage
        }

        private static void RunListingActivity()
        {
            Listing listing = new Listing();
            listing.DisplayIntro();
            listing.SetTime();
            listing.Preparation();
            listing.DisplayPrompt();
            listing.CollectItems();  // user lists items until time is up
            listing.DisplayExtro();
            _activityLog["Listing"]++; // log usage
        }
    }

    // -------------------------------------------------------
    // Base Class: Activities
    // -------------------------------------------------------
    abstract class Activities
    {
        // Common (shared) attributes
        protected string _intro;
        protected string _extro;
        protected int _time;

        public Activities()
        {
            // These can be overridden or set by child classes if desired
            _intro = "Default Intro Message";
            _extro = "Default Extro Message";
            _time = 0;
        }

        // Common behaviors
        public void DisplayIntro()
        {
            Console.Clear();
            Console.WriteLine($"Welcome to the {_intro}.");
            Console.WriteLine();
        }

        public void DisplayExtro()
        {
            Console.WriteLine();
            Console.WriteLine($"Well done! You have completed the {_intro} for {_time} seconds.");
            Spinner(3); // short spinner before finishing
        }

        public void SetTime()
        {
            Console.Write("Enter the duration in seconds: ");
            string input = Console.ReadLine();
            if (int.TryParse(input, out int seconds))
            {
                _time = seconds;
            }
            else
            {
                _time = 10; // default
            }
        }

        // A short "prepare to begin" pause with spinner
        public void Preparation()
        {
            Console.WriteLine("\nGet ready to begin...");
            Spinner(3);
        }

        // Spinner for a given number of seconds
        public void Spinner(int numSeconds)
        {
            DateTime endTime = DateTime.Now.AddSeconds(numSeconds);
            while (DateTime.Now < endTime)
            {
                Console.Write("|");
                Thread.Sleep(200);
                Console.Write("\b \b");

                Console.Write("/");
                Thread.Sleep(200);
                Console.Write("\b \b");

                Console.Write("-");
                Thread.Sleep(200);
                Console.Write("\b \b");

                Console.Write("\\");
                Thread.Sleep(200);
                Console.Write("\b \b");
            }
        }

        // Simple countdown timer
        public void Timer(int numSeconds)
        {
            for (int i = numSeconds; i > 0; i--)
            {
                Console.Write(i);
                Thread.Sleep(1000);
                Console.Write("\b \b");
            }
        }
    }

    // -------------------------------------------------------
    // Derived Class: Breathing
    // -------------------------------------------------------
    class Breathing : Activities
    {
        private string _in = "Breathe in...";
        private string _out = "Breathe out...";

        public Breathing()
        {
            // Set intro and extro to something more meaningful
            _intro = "Breathing Activity";
            _extro = "Breathing Activity";
        }

        public void LoopBreath()
        {
            // We will keep track of time by computing an end time
            DateTime endTime = DateTime.Now.AddSeconds(_time);
            while (DateTime.Now < endTime)
            {
                Console.WriteLine(_in);
                Timer(4); // Pause for 4 seconds
                Console.WriteLine(_out);
                Timer(4); // Pause for 4 seconds
                Console.WriteLine(); // blank line for spacing
            }
        }
    }

    // -------------------------------------------------------
    // Derived Class: Reflection
    // -------------------------------------------------------
    class Reflection : Activities
    {
        private List<string> _prompts = new List<string>()
        {
            "Think of a time when you stood up for someone else.",
            "Think of a time when you did something really difficult.",
            "Think of a time when you helped someone in need.",
            "Think of a time when you did something truly selfless."
        };

        private List<string> _questions = new List<string>()
        {
            "Why was this experience meaningful to you?",
            "Have you ever done anything like this before?",
            "How did you get started?",
            "How did you feel when it was complete?",
            "What made this time different than other times?",
            "What is your favorite thing about this experience?",
            "What could you learn from this experience?",
            "What did you learn about yourself through this?",
            "How can you keep this experience in mind in the future?"
        };

        public Reflection()
        {
            _intro = "Reflection Activity";
            _extro = "Reflection Activity";
        }

        public void DisplayPrompt()
        {
            // Pick a random prompt
            Random rand = new Random();
            int index = rand.Next(_prompts.Count);
            Console.WriteLine("Consider the following prompt:");
            Console.WriteLine($"\n>>> {_prompts[index]} <<<\n");
            Console.WriteLine("When you have something in mind, press enter to continue...");
            Console.ReadLine();
            Console.WriteLine("Now ponder on each of the following questions as they relate to this experience.");
            Console.WriteLine("You may begin in...");
            Timer(5); // countdown before starting the reflection
            Console.WriteLine();
        }

        public void DisplayQuestions()
        {
            DateTime endTime = DateTime.Now.AddSeconds(_time);
            Random rand = new Random();
            while (DateTime.Now < endTime)
            {
                int index = rand.Next(_questions.Count);
                Console.WriteLine(_questions[index]);
                Spinner(3); // short pause/spinner
                Console.WriteLine();
            }
        }
    }

    // -------------------------------------------------------
    // Derived Class: Listing
    // -------------------------------------------------------
    class Listing : Activities
    {
        private List<string> _prompts = new List<string>()
        {
            "Who are people that you appreciate?",
            "What are personal strengths of yours?",
            "Who are people that you have helped this week?",
            "Who are some of your personal heroes?",
            "What are some acts of kindness you’ve seen recently?"
        };

        private List<string> _userItems = new List<string>();

        public Listing()
        {
            _intro = "Listing Activity";
            _extro = "Listing Activity";
        }

        public void DisplayPrompt()
        {
            // Pick a random prompt
            Random rand = new Random();
            int index = rand.Next(_prompts.Count);
            Console.WriteLine("List as many responses as you can to the following prompt:");
            Console.WriteLine($"\n>>> {_prompts[index]} <<<\n");
            Console.WriteLine("You have a few seconds to consider your prompt...");
            Timer(5); // countdown before user starts listing
            Console.WriteLine("Begin listing items! (Press Enter after each item)");
        }

        public void CollectItems()
        {
            DateTime endTime = DateTime.Now.AddSeconds(_time);
            while (DateTime.Now < endTime)
            {
                Console.Write("> ");
                // Use "ReadLine()" to capture user entries
                string item = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(item))
                {
                    _userItems.Add(item);
                }
            }
            Console.WriteLine($"\nYou listed {_userItems.Count} items!");
        }
    }
}
