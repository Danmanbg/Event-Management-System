public class Order
{
    public Guid Id { get; set; }

    public DateTime OrderedOn { get; set; }

    public Guid EventId { get; set; }
    public Event Event { get; set; }

    public string CustomerId { get; set; }
    public ApplicationUser Customer { get; set; }

    public int TicketsCount { get; set; }
}
