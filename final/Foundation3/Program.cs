using System;

public class Address
{
    private string _street;
    private string _city;
    private string _state;
    private string _country;

    public Address(string street, string city, string state, string country)
    {
        _street = street;
        _city = city;
        _state = state;
        _country = country;
    }

    public string GetFullAddress()
    {
        return $"{_street}, {_city}, {_state}, {_country}";
    }
}

public class Event
{
    private string _title;
    private string _description;
    private string _date;
    private string _time;
    private Address _address;

    public Event(string title, string description, string date, string time, Address address)
    {
        _title = title;
        _description = description;
        _date = date;
        _time = time;
        _address = address;
    }

    public virtual string GetStandardDetails()
    {
        return $"Title: {_title}\nDescription: {_description}\nDate: {_date}\nTime: {_time}\nAddress: {_address.GetFullAddress()}";
    }

    public virtual string GetFullDetails()
    {
        return GetStandardDetails();
    }

    public virtual string GetShortDescription()
    {
        return $"Event Type: General\nTitle: {_title}\nDate: {_date}";
    }

    // Protected accessors for derived classes to use
    protected string GetTitle() => _title;
    protected string GetDate() => _date;
}

public class Lecture : Event
{
    private string _speaker;
    private int _capacity;

    public Lecture(string title, string description, string date, string time, Address address, string speaker, int capacity)
        : base(title, description, date, time, address)
    {
        _speaker = speaker;
        _capacity = capacity;
    }

    public override string GetFullDetails()
    {
        return $"{base.GetStandardDetails()}\nType: Lecture\nSpeaker: {_speaker}\nCapacity: {_capacity}";
    }

    public override string GetShortDescription()
    {
        return $"Event Type: Lecture\nTitle: {GetTitle()}\nDate: {GetDate()}";
    }
}

public class Reception : Event
{
    private string _rsvpEmail;

    public Reception(string title, string description, string date, string time, Address address, string rsvpEmail)
        : base(title, description, date, time, address)
    {
        _rsvpEmail = rsvpEmail;
    }

    public override string GetFullDetails()
    {
        return $"{base.GetStandardDetails()}\nType: Reception\nRSVP Email: {_rsvpEmail}";
    }

    public override string GetShortDescription()
    {
        return $"Event Type: Reception\nTitle: {GetTitle()}\nDate: {GetDate()}";
    }
}

public class OutdoorGathering : Event
{
    private string _weatherForecast;

    public OutdoorGathering(string title, string description, string date, string time, Address address, string weatherForecast)
        : base(title, description, date, time, address)
    {
        _weatherForecast = weatherForecast;
    }

    public override string GetFullDetails()
    {
        return $"{base.GetStandardDetails()}\nType: Outdoor Gathering\nWeather Forecast: {_weatherForecast}";
    }

    public override string GetShortDescription()
    {
        return $"Event Type: Outdoor Gathering\nTitle: {GetTitle()}\nDate: {GetDate()}";
    }
}

class Program
{
    static void Main()
    {
        Address lectureAddress = new Address("123 Main St", "New York", "NY", "USA");
        Event lecture = new Lecture("Tech Talk", "New Ai Glasses", "April 10, 2025", "2:00 PM", lectureAddress, "Dr. Smith", 150);

        Address receptionAddress = new Address("456 Garden Rd", "San Diego", "CA", "USA");
        Event reception = new Reception("Company Gala", "Annual networking event", "May 3, 2025", "6:00 PM", receptionAddress, "rsvp@company.com");

        Address outdoorAddress = new Address("789 Park Blvd", "Seattle", "WA", "USA");
        Event outdoor = new OutdoorGathering("Music Festival", "Live bands and food trucks", "June 20, 2025", "12:00 PM", outdoorAddress, "Sunny with a chance of clouds");

        Event[] events = { lecture, reception, outdoor };

        foreach (Event ev in events)
        {
            Console.WriteLine("=== Standard Details ===");
            Console.WriteLine(ev.GetStandardDetails());
            Console.WriteLine("\n=== Full Details ===");
            Console.WriteLine(ev.GetFullDetails());
            Console.WriteLine("\n=== Short Description ===");
            Console.WriteLine(ev.GetShortDescription());
            Console.WriteLine("\n---------------------------\n");
        }
    }
}
