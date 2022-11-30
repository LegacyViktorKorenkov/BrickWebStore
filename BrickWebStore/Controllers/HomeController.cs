using BrickWebStore.DataAccess.DataContext;
using BrickWebStore.Models;
using BrickWebStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using BrickWebStore.Utility;

namespace BrickWebStore.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _db;

    public HomeController(
        ILogger<HomeController> logger,
        AppDbContext dbContext)
    {
        _logger = logger;
        _db = dbContext;
    }

    public IActionResult Index()
    {
        var homeVm = new HomeViewModel
        {
            Categories = _db.Category,
            Products = _db.Product.Include(x => x.Category).Include(x => x.BrickStore),
        };

        return View(homeVm);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Details(int? id)
    {
        var cartSession = HttpContext.Session.Get<IReadOnlyCollection<ShoppngCart>>(WC.SessionCart);

        var detailsViewModel = new DetailsViewModel
        {
            Product = _db.Product
                        .Include(x => x.Category)
                        .Include(x => x.BrickStore)
                        .FirstOrDefault(x => x.Id == id),
            IsInCard = cartSession != null ? cartSession.Any(x => x.ProductId == id) : false,
        };

        return View(detailsViewModel);
    }

    [HttpPost, ActionName("Details")]
    public IActionResult DetailsPost(int id)
    {
        var cartSession 
            = HttpContext.Session.Get<IReadOnlyCollection<ShoppngCart>>(WC.SessionCart) 
            ?? Enumerable.Empty<ShoppngCart>();

        cartSession = Enumerable.Append(cartSession, new ShoppngCart { ProductId = id });

        HttpContext.Session.Set(WC.SessionCart, cartSession);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet, ActionName("RemoveFromCard")]
    public IActionResult Remove(int id)
    {
        var cartSession = HttpContext.Session.Get<ICollection<ShoppngCart>>(WC.SessionCart);

        if (cartSession != null && cartSession.Count() > 0)
        {
            var obj = cartSession.SingleOrDefault(x => x.ProductId == id);

            cartSession.Remove(obj);

            HttpContext.Session.Set(WC.SessionCart, cartSession);
        }

        return RedirectToAction(nameof(Index));
    }

    private void UsingLocalVariablesInCallbackCode(int numToDo)
    {
        int[] squares = new int[numToDo];

        AutoResetEvent done = new AutoResetEvent(false);

        for (int i = 0; i < squares.Length; i++)
        {
            ThreadPool.QueueUserWorkItem(obj =>
            {
                int num = (int)obj;

                squares[num] *= num;

                if (Interlocked.Decrement(ref numToDo) == 0)
                {
                    done.Set();
                }
            }, i);
        }

        done.WaitOne();

        for (int j = 0; j < squares.Length; j++)
        {
            Console.WriteLine("Index {0}, Square = {1}", j, squares[j]);
        }
    }
}