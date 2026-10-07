using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class EventsController : Controller
{
    private readonly ApplicationDbContext context;
    private readonly UserManager<ApplicationUser> userManager;

    public EventsController(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        this.context = context;
        this.userManager = userManager;
    }

    public IActionResult All()
    {
        var events = context.Events
            .Where(e => e.AvailableTickets > 0)
            .ToList();

        return View(events);
    }

    public IActionResult My()
    {
        var userId = userManager.GetUserId(User);

        var events = context.Orders
            .Where(o => o.CustomerId == userId)
            .Select(o => o.Event)
            .Distinct()
            .ToList();

        return View(events);
    }
    [HttpPost]
    public IActionResult Order(Guid eventId, int tickets)
    {
        var userId = userManager.GetUserId(User);

        using var transaction = context.Database.BeginTransaction();

        try
        {
            var evnt = context.Events.FirstOrDefault(e => e.Id == eventId);

            if (evnt == null)
            {
                return NotFound();
            }

            if (tickets <= 0)
            {
                ModelState.AddModelError("", "Invalid number of tickets.");
                return RedirectToAction("All");
            }

            if (evnt.AvailableTickets < tickets)
            {
                ModelState.AddModelError("", "Not enough tickets available.");
                return RedirectToAction("All");
            }

            evnt.AvailableTickets -= tickets;

            var order = new Order
            {
                Id = Guid.NewGuid(),
                EventId = eventId,
                CustomerId = userId,
                TicketsCount = tickets,
                OrderedOn = DateTime.UtcNow
            };

            context.Orders.Add(order);
            context.SaveChanges();

            transaction.Commit();

            return RedirectToAction("My");
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
