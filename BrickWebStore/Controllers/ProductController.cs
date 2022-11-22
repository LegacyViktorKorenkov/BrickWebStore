using BrickWebStore.DataContext;
using BrickWebStore.Models;
using BrickWebStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;

namespace BrickWebStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext appDbContext,
            IWebHostEnvironment env)
        {
            _db = appDbContext;
            _env = env;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _db.Product
                .Include(x => x.Category)
                .Include(x => x.BrickStore);

            return View(products);
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            #region old
            //IEnumerable<SelectListItem> categoryDropDown = _db.Category.Select(x => new SelectListItem
            //{
            //    Text = x.CategoryName,
            //    Value = x.Id.ToString(),
            //});

            //ViewBag.CategoryDropDown = categoryDropDown;
            ////ViewData["CategoryDropDown"] = categoryDropDown;

            //Product product;

            //if(id == null)
            //{
            //    product = new Product();
            //}
            //else
            //{
            //    product = _db.Product.FirstOrDefault(x => x.Id == id);

            //    if(product == null)
            //    {
            //        return NotFound();
            //    }
            //}

            //return View(product);
            #endregion

            ProductViewModel productViewModel = new ProductViewModel()
            {
                Product = new Product(),
                CategorySelectList = _db.Category.Select(x => new SelectListItem
                {
                    Text = x.CategoryName,
                    Value = x.Id.ToString(),
                }),
                StoreAddressList = _db.BrickWebStoreModel.Select(x => new SelectListItem
                {
                    Text = $"Store name: {x.ShopName}, address: {x.Address}",
                    Value = x.Id.ToString(),
                })
            };

            if (id != null)
            {
                var product = _db.Product.FirstOrDefault(x => x.Id == id);

                if (product != null)
                {
                    productViewModel.Product = product;
                }
                else
                {
                    return NotFound();
                }
            }

            return View(productViewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductViewModel productViewModel)
        {
            string upload, fileName, extension, combinedPath;

            if (ModelState.IsValid)
            {
                var filesCount = HttpContext.Request.Form.Files.Count;
                var file = HttpContext.Request.Form.Files.FirstOrDefault();

                if (productViewModel.Product.Id == 0)
                {
                    (upload, fileName, extension, combinedPath) = GetPath(file);

                    using (var fileStream = new FileStream(combinedPath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productViewModel.Product.ProductImage = $"{fileName}{extension}";

                    _db.Product.Add(productViewModel.Product);
                }
                else
                {
                    var productObj = _db.Product.AsNoTracking().FirstOrDefault(x => x.Id == productViewModel.Product.Id);

                    if (filesCount > 0)
                    {
                        (upload, fileName, extension, combinedPath) = GetPath(file);

                        var oldFile = Path.Combine(upload, productObj.ProductImage);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(combinedPath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        productViewModel.Product.ProductImage = $"{fileName}{extension}";
                    }
                    else
                    {
                        productViewModel.Product.ProductImage = productObj.ProductImage;
                    }

                    _db.Product.Update(productViewModel.Product);
                }

                try
                {
                    _db.SaveChanges();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return RedirectToAction("Index");
            }

            productViewModel.CategorySelectList = _db.Category.Select(x => new SelectListItem
            {
                Text = x.CategoryName,
                Value = x.Id.ToString(),
            });
            productViewModel.StoreAddressList = _db.BrickWebStoreModel.Select(x => new SelectListItem
            {
                Text = $"Store name: {x.ShopName}, address: {x.Address}",
                Value = x.Id.ToString(),
            });

            return View(productViewModel);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //var obj = _db.Category.FirstOrDefault(x => x.Id == id);
            //product.Category = _db.Category.Find(product.CategoryId);
            var product = _db.Product
                .Include(x => x.Category)
                .Include(s => s.BrickStore)
                .FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var product = _db.Product.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var imagePath = $"{_env.WebRootPath}{WC.ImagePath}{product.ProductImage}";

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _db.Product.Remove(product);

            try
            {
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return RedirectToAction("Index");
        }

        private (string, string, string, string) GetPath(IFormFile file)
        {
            var upload = $"{_env.WebRootPath}{WC.ImagePath}";
            var fileName = Guid.NewGuid().ToString();
            var extension = Path.GetExtension(file.FileName);
            var combinedPath = Path.Combine(upload, $"{fileName}{extension}");

            return (upload, fileName, extension, combinedPath);
        }
    }
}
