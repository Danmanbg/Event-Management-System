using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext context;

    public AdminController(ApplicationDbContext context)
    {
        this.context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult CreateEvent()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateEvent(Event model)
    {
        model.Id = Guid.NewGuid();
        context.Events.Add(model);
        context.SaveChanges();

        return RedirectToAction("Index");
    }

    public IActionResult AllOrders()
    {
        var orders = context.Orders
            .Include(o => o.Event)
            .Include(o => o.Customer)
            .ToList();

        return View(orders);
    }
}
