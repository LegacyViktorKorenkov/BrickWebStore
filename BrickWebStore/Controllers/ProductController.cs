using BrickWebStore.DataAccess.DataContext;
using BrickWebStore.Models;
using BrickWebStore.Utility;
using BrickWebStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BrickWebStore.DataAccess.Repositories.Abstractions;
using BrickWebStore.DataAccess.Repositories;

namespace BrickWebStore.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _env;

        public ProductController(IProductRepository productRepository,
            IWebHostEnvironment env)
        {
            _productRepository = productRepository;
            _env = env;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _productRepository.GetAll(includProperties: "Category,BrickStore");

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
                CategorySelectList = _productRepository.GetAllDropdownList(WC.CategoryName),
                StoreAddressList = _productRepository.GetAllDropdownList(WC.BrickStoreName),
            };

            if (id != null)
            {
                var product = _productRepository.FirstOrDefault(x => x.Id == id);

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

                    _productRepository.Add(productViewModel.Product);
                }
                else
                {
                    var productObj = _productRepository.FirstOrDefault(x => x.Id == productViewModel.Product.Id, isTracking: false);

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

                    _productRepository.Update(productViewModel.Product);
                }

                try
                {
                    _productRepository.Save();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return RedirectToAction("Index");
            }

            productViewModel.CategorySelectList = _productRepository.GetAllDropdownList(WC.CategoryName);
            productViewModel.StoreAddressList = _productRepository.GetAllDropdownList(WC.BrickStoreName);

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
            var product = _productRepository.FirstOrDefault(x => x.Id == id, includProperties: "Category,BrickStore");

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var product = _productRepository.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var imagePath = $"{_env.WebRootPath}{WC.ImagePath}{product.ProductImage}";

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _productRepository.Remove(product);

            try
            {
                _productRepository.Save();
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
