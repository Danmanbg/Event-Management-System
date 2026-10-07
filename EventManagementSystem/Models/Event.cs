using System.ComponentModel.DataAnnotations;

public class Event
{
    public Guid Id { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }

    public DateTime EventDate { get; set; }

    public int AvailableTickets { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; }
}
