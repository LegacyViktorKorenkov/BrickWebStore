using BrickWebStore.DataContext;
using BrickWebStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrickWebStore.Controllers
{
    public class BrickWebStoreController : Controller
    {
        private readonly AppDbContext _db;

        public BrickWebStoreController(AppDbContext dbContext)
        {
            _db = dbContext;
        }

        public IActionResult Index()
        {
            IEnumerable<BrickWebStoreModel> stores = _db.BrickWebStoreModel;

            return View(stores);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(BrickWebStoreModel brickWebStoreModel)
        {
            _db.BrickWebStoreModel.Add(brickWebStoreModel);

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

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.BrickWebStoreModel.FirstOrDefault(x => x.Id == id);
            
            if(obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditPost(BrickWebStoreModel brickModel)
        {
            if (ModelState.IsValid)
            {
                _db.BrickWebStoreModel.Update(brickModel);

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
            else
            {
                return View(brickModel);
            }
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.BrickWebStoreModel.FirstOrDefault(x => x.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.BrickWebStoreModel.FirstOrDefault(x => x.Id == id);
            if(obj == null)
            {
                return NotFound();
            }

            _db.BrickWebStoreModel.Remove(obj);

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
    }
}
