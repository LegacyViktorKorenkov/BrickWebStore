using BrickWebStore.DataAccess.DataContext;
using BrickWebStore.Models;
using BrickWebStore.Models.ViewModels;
using BrickWebStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace BrickWebStore.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;

        [BindProperty]
        public ProductUserViewModel ProductUserViewModel { get; set; }

        public CartController(
            AppDbContext appDbContext,
            IWebHostEnvironment webHostEnvironment,
            IEmailSender emailSender)
        {
            _db = appDbContext;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> prodList;

            var prodInCart = HttpContext.Session.Get<IEnumerable<ShoppngCart>>(WC.SessionCart);

            if (prodInCart != null && prodInCart.Count() > 0)
            {
                prodList = _db.Product.Where(x => prodInCart.Select(x => x.ProductId).Contains(x.Id));
            }
            else
            {
                prodList = Enumerable.Empty<Product>();
            }

            return View(prodList);
        }

        public IActionResult Remove(int? id)
        {
            var prodInCart = HttpContext.Session.Get<ICollection<ShoppngCart>>(WC.SessionCart);

            prodInCart.Remove(prodInCart.SingleOrDefault(x => x.ProductId == id));

            HttpContext.Session.Set(WC.SessionCart, prodInCart);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Summary));
        }


        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //var userId = User.FindFirstValue(ClaimTypes.Name);

            var prodInCart = HttpContext.Session.Get<IEnumerable<ShoppngCart>>(WC.SessionCart);

            ProductUserViewModel = new ProductUserViewModel
            {
                User = _db.ShopUser.FirstOrDefault(x => x.Id == claim.Value),
                Products = prodInCart == null ? new List<Product>() : _db.Product.Where(x => prodInCart.Select(x => x.ProductId).Contains(x.Id)).ToList(),
            };

            return View(ProductUserViewModel);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(ProductUserViewModel productUserViewModel)
        {
            var pathToTemplate = $"{_webHostEnvironment.WebRootPath}{Path.DirectorySeparatorChar.ToString()}templates{Path.DirectorySeparatorChar.ToString()}Inquiry.html";

            var subject = "New inquery";
            var htmlBody = string.Empty;

            using (StreamReader sr = System.IO.File.OpenText(pathToTemplate))
            {
                htmlBody = sr.ReadToEnd();
            }
            var productListSb = new StringBuilder();

            foreach (var p in productUserViewModel.Products)
            {
                productListSb.Append($@" - Name: {p.ProductName} <span style=""font-size:14px""> (ID: {p.Id})</span><br/>");
            }

            var messageBody = string.Format(htmlBody,
                productUserViewModel.User.FullName,
                productUserViewModel.User.Email,
                productUserViewModel.User.PhoneNumber,
                productListSb.ToString());

            await _emailSender.SendEmailAsync(WC.EmailAdmin, subject, messageBody);

            return RedirectToAction(nameof(InqueryConfirmation));
        }

        public IActionResult InqueryConfirmation()
        {
            HttpContext.Session.Clear();

            return View();
        }
    }
}
