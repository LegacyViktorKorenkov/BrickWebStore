using BrickWebStore.DataAccess.DataContext;
using BrickWebStore.DataAccess.Repositories.Abstractions;
using BrickWebStore.Models;
using BrickWebStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrickWebStore.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class CategoryController : Controller
    {
        private readonly ICategotyRepository _categotyRepository;

        public CategoryController(ICategotyRepository categotyRepository)
        {
            _categotyRepository = categotyRepository;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categories = _categotyRepository.GetAll();

            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _categotyRepository.Add(category);

                try
                {
                    _categotyRepository.Save();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return RedirectToAction("Index");
            }
            else
            {
                return View(category);
            }
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //var obj = _db.Category.FirstOrDefault(x => x.Id == id);
            var obj = _categotyRepository.Find(id.GetValueOrDefault());

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _categotyRepository.Update(category);

                try
                {
                    _categotyRepository.Save();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return RedirectToAction("Index");
            }
            else
            {
                return View(category);
            }
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //var obj = _db.Category.FirstOrDefault(x => x.Id == id);
            var obj = _categotyRepository.Find(id.GetValueOrDefault());

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _categotyRepository.FirstOrDefault(x => x.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            _categotyRepository.Remove(obj);

            try
            {
                _categotyRepository.Save();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return RedirectToAction("Index");
        }
    }
}
