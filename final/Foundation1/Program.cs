using System;
using System.Collections.Generic;

class Comment
{
    public string CommenterName { get; set; }
    public string Text { get; set; }

    public Comment(string commenterName, string text)
    {
        CommenterName = commenterName;
        Text = text;
    }
}

class Video
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int LengthSeconds { get; set; }

    private List<Comment> _comments = new List<Comment>();

    public Video(string title, string author, int lengthSeconds)
    {
        Title = title;
        Author = author;
        LengthSeconds = lengthSeconds;
    }

    public void AddComment(Comment comment)
    {
        _comments.Add(comment);
    }

    public int GetCommentCount()
    {
        return _comments.Count;
    }

    public List<Comment> GetComments()
    {
        return _comments;
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Create videos
        Video video1 = new Video("The Rise of AI", "TechWorld", 300);
        video1.AddComment(new Comment("Alice", "Amazing explanation!"));
        video1.AddComment(new Comment("Bob", "Very informative."));
        video1.AddComment(new Comment("Charlie", "Loved the graphics!"));

        Video video2 = new Video("Cooking with Pasta", "FoodieChef", 420);
        video2.AddComment(new Comment("Dana", "So delicious!"));
        video2.AddComment(new Comment("Eli", "Trying this tonight."));
        video2.AddComment(new Comment("Fay", "Yummy recipe!"));

        Video video3 = new Video("Workout at Home", "FitLife", 600);
        video3.AddComment(new Comment("George", "Burned so many calories!"));
        video3.AddComment(new Comment("Hannah", "I feel energized."));
        video3.AddComment(new Comment("Ian", "More videos like this!"));

        List<Video> videos = new List<Video> { video1, video2, video3 };

        // Display video details
        foreach (Video video in videos)
        {
            Console.WriteLine($"Title: {video.Title}");
            Console.WriteLine($"Author: {video.Author}");
            Console.WriteLine($"Length: {video.LengthSeconds} seconds");
            Console.WriteLine($"Number of comments: {video.GetCommentCount()}");

            foreach (Comment comment in video.GetComments())
            {
                Console.WriteLine($" - {comment.CommenterName}: {comment.Text}");
            }

            Console.WriteLine();
        }
    }
}
